# طراحی دقت‌محور Binder و Overload Resolution

## اصل خروجی

وقتی latency اولویت نیست، هیچ پیشنهاد semantic قبل از بررسی قابل‌کامپایل‌بودن نمایش داده نمی‌شود. ترتیب کیفیت:

1. Soundness: پیشنهاد نباید خطای compile یا nullable warning جدید ایجاد کند.
2. Completeness: همه candidateهای معتبر باید حفظ شوند.
3. Relevance: candidateهای معتبر بر اساس context رتبه‌بندی شوند.

pipeline:

```text
Context Classifier
  → Candidate Generators
  → Semantic Filter
  → Speculative Verification
  → Feature Extraction
  → Explainable Ranking
```

## لایه‌های الزاماً جدا

```text
Lossless Syntax Tree
  → Declarations / Symbols
  → Binder Chain
  → Bound Tree با Conversionهای صریح
```

### Syntax

- lossless و full-fidelity با trivia، comment و directive
- error-tolerant برای کد ناقص حین تایپ
- incremental range update
- preprocessor کامل شامل `#if`، `#nullable` و `#pragma`
- language-version gate از C# 6 تا C# 15

### Symbols

مدل مشترک برای source و metadata:

`NamespaceSymbol`، `NamedTypeSymbol`، `MethodSymbol`، `PropertySymbol`، `FieldSymbol`، `EventSymbol`، `ParameterSymbol`، `LocalSymbol`، `TypeParameterSymbol`، array/pointer/function-pointer/tuple/dynamic type.

`ErrorTypeSymbol` باید `CandidateSymbols` را نگه دارد تا binding شکست‌خورده cascade error ایجاد نکند ولی completion همچنان candidate داشته باشد.

Generic constructed type با `TypeMap` و cache ساخته می‌شود؛ symbolهای metadata lazy هستند.

### Binder chain

```text
Global
  → Imports/Usings
  → Namespace
  → Type
  → Member
  → Block
  → Lambda/Query/Pattern scope
```

`LookupResult` باید دلیل شکست را نگه دارد:

`Viable`، `Ambiguous`، `Inaccessible`، `WrongArity`، `NotAValue`، `NotFound`.

## Member Lookup

1. memberهای نام‌دار در type، base type، effective base/interface و `object` برای interface
2. حذف inaccessibleها با حفظ دلیل
3. اعمال hiding، `new`، override و signature برابر
4. مدیریت conflict بین non-method و method
5. ساخت method group و حذف base definition در صورت وجود member قابل‌اعمال derived

موارد تست اجباری: `Color Color`، `protected` access، `internal` و `InternalsVisibleTo`، file-local type، explicit interface implementation، generic arity، `var` به‌عنوان type، `@identifier`، static abstract member و default interface implementation.

Extension method lookup از نزدیک‌ترین namespace و using شروع می‌شود. receiver فقط identity، implicit reference یا boxing دارد؛ numeric و user-defined conversion برای extension lookup مجاز نیست. C# 14 extension member شامل property، static و operator هم مدل می‌شود.

## Conversion Classifier

امضای مفهومی:

```csharp
ConversionResult ClassifyConversion(TypeSymbol source, TypeSymbol target, ConversionContext context);
```

نتیجه bool نیست و شامل kind، rank، هزینه، implicit/explicit و evidence است. انواع اصلی:

`Identity`، numeric، enum، nullable، null literal، reference، boxing، constant، user-defined، anonymous function، method group، tuple، interpolated string handler، default، collection expression، target-typed conditional/switch و Span/ReadOnlySpan.

نتیجه با کلید `(source, target, context, languageVersion)` cache می‌شود. variance، array conversion و lifted nullable user-defined conversion جداگانه تست می‌شوند.

## Overload Resolution

### Candidate set

- normal و expanded form برای `params`
- positional، named، non-trailing named، `ref/out/in` و optional/default
- generic inference و حذف constraint-violating candidate
- حذف base definition وقتی derived candidate قابل‌اعمال است

### Better function member

برای هر argument، `BetterConversionFromExpression` اجرا می‌شود: identity، target specificity، numeric tie-break، lambda return type، tuple و collection element rules.

tie-breakها به‌ترتیب: non-generic، more-specific parameter، non-expanded، default کمتر، `in` در برابر by-value و `OverloadResolutionPriority` در C# 13.

### نتیجه

```csharp
public sealed record OverloadResolutionResult(
    ImmutableArray<Candidate> Applicable,
    ImmutableArray<RejectedCandidate> Rejected,
    Candidate? Best,
    ResolutionKind Kind);
```

`RejectedCandidate` باید دلیل رد، conversion failure، arity، constraint یا accessibility را نگه دارد. برای completion فقط winner حفظ نمی‌شود؛ کل candidate set برای ranking و explanation لازم است.

### Partial resolution برای completion

در `Foo(a, |` فقط prefix binding انجام می‌شود. expected typeهای پارامتر cursor از اجتماع candidateهای سازگار ساخته می‌شوند و نام parameter هم به‌عنوان hint ارائه می‌شود. همین API باید signature help و argument completion را تغذیه کند.

## Generic Inference

فاز اول: output type inference و explicit parameter inference برای lambda.

فاز دوم: نگهداری exact/lower/upper bounds و fixing تکراری type parameterهایی که به lambda وابسته‌اند. انتخاب نهایی باید یکتا و قابل تبدیل به همه boundها باشد.

Tuple names، collection element type، nullable annotation، Span conversion و LINQ chain باید fixture مستقل داشته باشند.

## Best-effort و speculative binding

- اگر candidateهای مبهم return type یکسان دارند، همان type برای ادامه زنجیره مجاز است.
- receiver از آخرین بخش سالم محاسبه می‌شود.
- اگر type نامعلوم است، نتیجه خالی بهتر از حدس نادرست است.

```csharp
public interface ISpeculativeVerifier
{
    VerificationResult Verify(Document document, int position, string insertText);
}

public sealed record VerificationResult(
    bool Compiles,
    ImmutableArray<Diagnostic> NewErrors,
    ImmutableArray<Diagnostic> NewNullableWarnings);
```

diagnosticها نسبت به insertion مقایسه می‌شوند و خطاهای baseline کد ناقص حذف می‌گردند.

## Nullable flow

بعد از binder پایدار اجرا می‌شود و شامل annotation context، conditional state، definite assignment و attributeهای flow مانند `NotNullWhen`، `MemberNotNull` و `DoesNotReturn` است.

## Verification و oracle

Roslyn فقط در CI به‌عنوان differential oracle استفاده می‌شود. محصول به Roslyn وابسته نیست. Symbol dump مشترک شامل `(span, display, type, conversion, constant)` تولید و با oracle مقایسه می‌شود.

## ترتیب milestone

```text
A symbols + metadata + simple lookup
B generic substitution + constraints + variance
C conversions + overload resolution بدون inference
D inference + lambda + method group + LINQ
E patterns + switch + tuple + record + primary constructor
F extension members + foreach/await/using + handlers
G nullable flow + speculative verification
```

شرط‌های خروج: conformance corpus، round-trip parser، differential symbol dump و compile-valid rate حداقل 99٪ در benchmark قبل از فعال‌سازی ruleهای مربوط.

