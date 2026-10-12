# دفتر کار پروژه

این فایل مرجع اصلی وضعیت کار است. هر تغییر باید قبل از شروع، یک شناسه task داشته باشد و بعد از تغییر کد، وضعیت، branch، commit و validation آن ثبت شود.

## قواعد استفاده

1. هر task فقط یک خروجی قابل بررسی دارد.
2. taskهای وابسته بدون تکمیل dependency شروع نمی‌شوند.
3. هر task توسعه روی `feature/<scope>-<description>` انجام می‌شود.
4. پس از تغییر کد، build و آزمون مرتبط اجرا می‌شود.
5. پس از تغییر کد، Graphify باید update شود.
6. هر commit باید Conventional Commit و build سبز داشته باشد.
7. منبع اطلاعات این فایل و `state.json` است؛ متن گفت‌وگو منبع وضعیت محسوب نمی‌شود.

## وضعیت‌های مجاز

`backlog`، `ready`، `in-progress`، `blocked`، `done`، `cancelled`

## کارهای تکمیل‌شده

| ID | عنوان | وضعیت | Commit/مدرک |
|---|---|---|---|
| GOV-001 | معماری کلان و قراردادهای داخلی | done | docs/architecture.md و docs/internal-contracts.md |
| GOV-002 | solution skeleton و قراردادهای C# | done | `baadefd` |
| GOV-003 | Gitflow محلی و SemVer اولیه | done | main/develop، `0.1.0-alpha.1` |
| GOV-004 | نصب project-scoped Graphify و hook | done | `.codex/skills/graphify`، `AGENTS.md` |
| GOV-005 | نصب project-scoped Ponytail | done | `.codex/skills/ponytail*`، `AGENTS.md` |

## backlog اصلی

### فاز 1: قرارداد و هسته زبان

| ID | عنوان | وضعیت | وابستگی | شاخه پیشنهادی |
|---|---|---|---|---|
| LANG-001 | تعیین grammar و token model برای C# 6 تا 15 | ready | GOV-001 | feature/language/token-model |
| LANG-002 | lexer اختصاصی با خطاپذیری و incremental ranges | backlog | LANG-001 | feature/language/lexer |
| LANG-003 | parser اختصاصی و syntax tree پایدار | backlog | LANG-002 | feature/language/parser |
| LANG-004 | syntax recovery برای کد ناقص هنگام تایپ | backlog | LANG-003 | feature/language/recovery |
| LANG-005 | capability matrix نسخه‌های C# 6 تا 15 | backlog | LANG-001 | feature/language/version-matrix |

### فاز 2: semantic engine اختصاصی

| ID | عنوان | وضعیت | وابستگی | شاخه پیشنهادی |
|---|---|---|---|---|
| SEM-001 | symbol table و scope resolution | backlog | LANG-003 | feature/semantic/symbol-table |
| SEM-002 | type system، generic constraint و variance | backlog | SEM-001 | feature/semantic/type-system |
| SEM-003 | overload resolution و conversion rules | backlog | SEM-002 | feature/semantic/overload-resolution |
| SEM-004 | nullable flow و definite assignment | backlog | SEM-002 | feature/semantic/nullability |
| SEM-005 | control/data/exception/async flow | backlog | SEM-001 | feature/semantic/flow-analysis |
| SEM-006 | project/solution dependency graph | backlog | SEM-001 | feature/semantic/project-graph |

### فاز 3: metadata، assembly و NuGet

| ID | عنوان | وضعیت | وابستگی | شاخه پیشنهادی |
|---|---|---|---|---|
| META-001 | خواندن metadata و IL بدون کتابخانه تحلیل خارجی | backlog | SEM-002 | feature/metadata/reader |
| META-002 | assembly reference و target framework index | backlog | META-001 | feature/metadata/framework-index |
| NUGET-001 | خواندن PackageReference و assets file | backlog | META-002 | feature/nuget/project-index |
| NUGET-002 | ایندکس package/type/member و API usage | backlog | NUGET-001 | feature/nuget/api-index |
| NUGET-003 | به‌روزرسانی index هنگام اضافه‌شدن package جدید | backlog | NUGET-002 | feature/nuget/incremental-update |

### فاز 4: IntelliSense و VS Code

| ID | عنوان | وضعیت | وابستگی | شاخه پیشنهادی |
|---|---|---|---|---|
| IDE-001 | پیاده‌سازی transport اختصاصی روی stdio | backlog | SEM-001 | feature/ide/stdio-protocol |
| IDE-002 | completion context و edit range | backlog | LANG-003، SEM-001 | feature/ide/completion-context |
| IDE-003 | signature help، hover و go-to definition | backlog | SEM-001 | feature/ide/navigation |
| IDE-004 | diagnostics، code action و rename | backlog | SEM-003، SEM-004 | feature/ide/actions |
| IDE-005 | ranking بر اساس context و project graph | backlog | IDE-002، SEM-006 | feature/ide/context-ranking |
| IDE-006 | VS Code client بدون language-service خارجی | backlog | IDE-001 | feature/vscode/client |

### فاز 5: EF و اکوسیستم .NET

| ID | عنوان | وضعیت | وابستگی | شاخه پیشنهادی |
|---|---|---|---|---|
| EF-001 | تشخیص DbContext، DbSet و entity model | backlog | SEM-002، NUGET-002 | feature/ef/model-analysis |
| EF-002 | تحلیل LINQ و ترجمه احتمالی به SQL | backlog | SEM-005، EF-001 | feature/ef/linq-analysis |
| EF-003 | پیشنهادهای migration، tracking و query performance | backlog | EF-002 | feature/ef/rules |
| EF-004 | پشتیبانی providerهای رایج با metadata قابل به‌روزرسانی | backlog | NUGET-003، EF-001 | feature/ef/providers |

### فاز 6: دانش و به‌روزرسانی

| ID | عنوان | وضعیت | وابستگی | شاخه پیشنهادی |
|---|---|---|---|---|
| KNOW-001 | schema concept/claim/rule/source | backlog | GOV-001 | feature/knowledge/schema |
| KNOW-002 | crawler رسمی Microsoft Blog و RSS/sitemap | backlog | KNOW-001 | feature/knowledge/crawler |
| KNOW-003 | استخراج claim و rule بدون اجرای محتوای خارجی | backlog | KNOW-002 | feature/knowledge/extractor |
| KNOW-004 | knowledge pack نسخه‌دار، hash و signature | backlog | KNOW-003 | feature/knowledge/packaging |
| KNOW-005 | client update، rollback و compatibility check | backlog | KNOW-004 | feature/knowledge/update-client |

### فاز 7: کیفیت و release

| ID | عنوان | وضعیت | وابستگی | شاخه پیشنهادی |
|---|---|---|---|---|
| QA-001 | corpus پروژه‌های واقعی C# 6 تا 15 | backlog | LANG-005، SEM-006 | feature/qa/corpus |
| QA-002 | معیار precision، recall، ranking و latency | backlog | IDE-005 | feature/qa/metrics |
| QA-003 | regression test برای هر rule و version | backlog | KNOW-003 | feature/qa/regression |
| QA-004 | benchmark مقایسه‌ای با سناریوهای Rider | backlog | QA-001، QA-002 | feature/qa/benchmark |
| REL-001 | packaging افزونه و language server | backlog | IDE-006 | feature/release/packaging |
| REL-002 | release `0.1.0-alpha.2` | backlog | REL-001، QA-003 | release/0.1.0-alpha.2 |

## مسیر اجرای بعدی

```text
LANG-001 → LANG-002 → LANG-003 → SEM-001 → IDE-001 → IDE-002
```

مسیر موازی پس از ایجاد schema:

```text
KNOW-001 → KNOW-002 → KNOW-003 → KNOW-004
NUGET-001 → NUGET-002 → NUGET-003
```

## تعریف انجام‌شدن

هر task زمانی `done` است که:

- کد و قراردادهای لازم کامل باشند.
- تست یا self-check مرتبط وجود داشته باشد.
- `dotnet build` و checkهای همان scope موفق باشند.
- graph به‌روز شده باشد.
- مستندات و task state به‌روز شده باشند.
- commit روی branch درست ثبت شده باشد.

