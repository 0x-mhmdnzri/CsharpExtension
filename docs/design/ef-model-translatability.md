# طراحی شبیه‌ساز EF Core و Translatability

## اصل

مدل EF بدون اجرای کد کاربر بازسازی می‌شود. پیشنهادهای EF فقط وقتی دقیق‌اند که هم مدل مفهومی و هم منشأ query معلوم باشد.

دو خروجی مستقل داریم:

1. `EfModel`: entity، property، navigation، key، foreign key، index، provider و version
2. `TranslationVerdict`: آیا expression برای provider و نسخه EF قابل ترجمه است؟

## provenance و confidence

```csharp
public enum EfProvenance
{
    Convention, DataAnnotation, FluentApi, ModelSnapshot,
    CompiledModel, RuntimeProbe
}

public enum EfConfidence { High, Medium, Low }

public sealed record EfFact<T>(
    T Value,
    EfProvenance Source,
    EfConfidence Confidence,
    DocumentLocation? Origin);
```

`EfModel.IsComplete=false` وقتی بخشی از model با reflection، loop پویا، dynamic یا assembly باینری غیرقابل مشاهده ساخته شده باشد. در این حالت ruleهای منفی حق حذف پیشنهاد را ندارند و فقط پیشنهاد افزودنی یا hint آموزشی مجاز است.

## مدل دامنه

```csharp
public sealed record EfModel(
    ImmutableArray<EfDbContext> Contexts,
    ImmutableArray<EfModelConflict> Conflicts);

public sealed record EfDbContext(
    TypeSymbol Type,
    EfProvider? Provider,
    PackageVersion EfVersion,
    bool IsComplete,
    ImmutableArray<EfEntityType> Entities);

public sealed record EfEntityType(
    TypeSymbol ClrType,
    EfFact<InheritanceStrategy> Inheritance,
    ImmutableArray<EfProperty> Properties,
    ImmutableArray<EfNavigation> Navigations,
    ImmutableArray<EfKey> Keys,
    ImmutableArray<EfForeignKey> ForeignKeys,
    ImmutableArray<EfIndex> Indexes,
    bool IsOwned,
    bool IsComplexType,
    bool IsKeyless,
    string? TableOrView,
    ImmutableArray<QueryFilter> QueryFilters);
```

## ترتیب بازسازی مدل

### سطح اول: تحلیل ایستا

1. کشف `DbSet<T>`، `Entity<T>()`، navigation reachability و `ApplyConfiguration`.
2. اجرای مفهومی conventionهای نسخه EF: key، navigation، inverse، FK، many-to-many، required و pre-convention.
3. اعمال Data Annotation مانند `Key`، `ForeignKey`، `InverseProperty`، `Owned`، `ComplexType`، `NotMapped`، `Column`، `Required`، `MaxLength`، `Index`، `Keyless`، `Timestamp` و `ConcurrencyCheck`.
4. تفسیر ایستای Fluent API بر اساس symbol identity نوع declaring، نام و signature؛ نام متن کافی نیست.
5. حل lambdaهای `x => x.Prop`، `nameof` و رشته‌های ثابت. شاخه‌های provider-specific فقط در صورت provider شناخته‌شده ارزیابی می‌شوند.

### cross-checkهای اختیاری

- `ModelSnapshot`: تشخیص اختلاف مدل یا migration جاافتاده
- compiled model: فقط اگر کاربر آن را تولید کرده و منبعش قابل مشاهده باشد
- entity باینری از NuGet: فقط convention و attribute؛ `IsComplete=false`

تعارض بین منابع silent حذف نمی‌شود؛ به‌صورت `EfModelConflict` ذخیره می‌گردد.

## پیشنهادهای مبتنی بر مدل

- `Include/ThenInclude`: فقط navigation معتبر؛ تشخیص reference در برابر collection و overload درست
- filtered include: فقط `Where`، `OrderBy`، `ThenBy`، `Skip` و `Take`
- مسیر string-based include از گراف navigation
- `Where/Select/OrderBy/GroupBy/Join` با element type درست
- `AsNoTracking`، `AsSplitQuery`، `IgnoreQueryFilters` و `TagWith` فقط برای EF-rooted `IQueryable`
- `ExecuteUpdate/SetProperty` و `ExecuteDelete` با value type درست
- `FromSql` ترجیحاً به‌جای `FromSqlRaw` برای رشته‌ی ساخته‌شده
- Fluent API: `HasOne/WithMany/WithOne`، `HasForeignKey`، `HasColumnType`، provider methods و `HasConversion`

## Query provenance

```text
DbSet<T> / Set<T> / FromSql / Database.SqlQuery
  → EfRooted IQueryable
  → IQueryable operators preserve provenance
  → AsEnumerable / ToList / ToArray / AsAsyncEnumerable
  → ClientSide
```

پارامتر `IQueryable<T>` در repository برابر `Unknown` است، مگر منشأ آن اثبات شود. در `Unknown` هیچ candidate حذف نمی‌شود.

فقط lambda تبدیل‌شده به `Expression<Func<...>>` در عملگر EF-rooted در معرض translation است. projection نهایی `Select` استثنای مشخص و versioned دارد.

## Translation verdict

```csharp
public enum TranslationVerdict { Yes, No, Unknown, Conditional }

public sealed record TranslationResult(
    TranslationVerdict Verdict,
    EfConfidence Confidence,
    string? Reason,
    string? Provider,
    PackageVersion EfVersion,
    string SourceId);
```

دو گروه محدودیت جدا هستند:

1. محدودیت expression tree در compile-time، مانند `?.`، statement lambda، dynamic و بعضی patternها.
2. محدودیت provider در runtime، مانند متدهای بدون translator یا overloadهای پشتیبانی‌نشده.

ماتریس کلیددار است:

```text
(provider, efVersion, method/member, argumentShape)
  → Yes | No | Conditional | Unknown
```

منابع matrix: مستندات رسمی function mapping، metadata translatorهای provider به‌عنوان evidence کمکی، و oracle اجرایی CI با `ToQueryString()`.

## gate پیشنهادی

| نتیجه | رفتار |
|---|---|
| No + confidence بالا + model کامل | حذف candidate و توضیح دقیق |
| No + confidence متوسط | تنزل ranking و نمایش warning |
| Conditional | پیشنهاد با شرط و evidence |
| Unknown یا model ناقص | بدون اثر منفی؛ فقط پیشنهاد افزودنی |

## نسخه و provider

EF version از package resolve‌شده در `project.assets.json` می‌آید، نه آخرین نسخه موجود. capabilityهای EF با version gate ثبت می‌شوند؛ API diff و release notes رسمی منبع تغییرات هستند.

## معیار خروج

```text
E1 context/DbSet/provider/version discovery       = 100% روی corpus
E2 conventions + annotations                      = تطابق Model.ToDebugString
E3 fluent interpreter                             = تطابق corpus یا IsComplete=false
E4 query completion                              = compile-valid >= 99%
E5 translation matrix                            = precision حذف‌ها نزدیک 100%
E6 snapshot cross-check                          = اختلاف قابل توضیح
```

