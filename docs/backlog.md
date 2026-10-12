# Product Backlog

## روش اولویت‌بندی

- `P0`: پیش‌نیاز architecture یا correctness؛ بدون آن feature بعدی معتبر نیست.
- `P1`: قابلیت لازم برای MVP دقیق.
- `P2`: قابلیت مهم بعد از MVP.
- `P3`: بهبود یا قابلیت اختیاری.

هر item باید یک branch، acceptance criteria و validation داشته باشد. itemهای P0 و P1 با یک commit بزرگ پیاده‌سازی نمی‌شوند؛ هر بخش باید کوچک و قابل bisect باشد.

## وضعیت فعلی

| ID | عنوان | Priority | Status | Dependency | Branch |
|---|---|---:|---|---|---|
| GOV-001 | معماری و قراردادهای داخلی | P0 | done | - | - |
| GOV-002 | solution skeleton | P0 | done | GOV-001 | - |
| GOV-003 | Gitflow و versioning | P0 | done | GOV-002 | - |
| BIND-001 | Symbol hierarchy و ErrorTypeSymbol | P0 | done | LANG-003, META-001 | feature/binder/symbols |
| LANG-001 | grammar/token model برای C# 6 تا 15 | P0 | ready | GOV-001 | feature/language/token-model |
| EF-005 | provenance/confidence و IsComplete | P1 | ready | EF-001 | feature/ef/provenance |
| KNOW-006 | concept registry برای CLR/C# | P1 | ready | GOV-001 | feature/knowledge/curriculum |

## مسیر MVP دقت‌محور

```text
LANG-001 → LANG-002 → LANG-003
                         ↓
META-001 → BIND-001 → BIND-002 → BIND-003 → BIND-005
                                      ↓
                               IDE-002 + QA-001
                                      ↓
                                  EF-005
```

MVP باید ابتدا subset مشخص C# 10+ و .NET 8 را با compile-valid rate بالا پوشش دهد؛ سپس capability matrix به C# 6 تا 15 گسترش می‌یابد.

## Backlog تفصیلی

### Compiler core

| ID | خروجی | Acceptance criteria | Validation |
|---|---|---|---|
| LANG-001 | feature matrix و token specification | هر feature یک language gate و fixture دارد | specification review |
| LANG-002 | lexer lossless و incremental | round-trip trivia و recovery بدون crash | unit + fuzz |
| LANG-003 | parser و syntax tree | parse/print متن corpus را حفظ کند | golden corpus |
| BIND-001 | symbol hierarchy و error symbols | source/metadata symbol مشترک و candidate حفظ شود | symbol dump |
| BIND-002 | binder chain و lookup result | NotFound/Inaccessible/WrongArity/Ambiguous تفکیک شود | conformance |
| BIND-003 | conversion classifier | kind ساختاریافته و cache version-aware باشد | conversion corpus |
| BIND-005 | overload resolution | candidate rejection reason و best member درست باشد | spec + differential CI |
| BIND-006 | generic inference و LINQ | زنجیره‌های Select/Where نوع درست داشته باشند | LINQ corpus |
| BIND-007 | speculative verification | diagnostic جدید نسبت به baseline تشخیص داده شود | compile-valid benchmark |

### Metadata و project index

| ID | خروجی | Acceptance criteria | Validation |
|---|---|---|---|
| META-001 | metadata reader داخلی با BCL | reference assembly و XML docs ایندکس شوند | assembly corpus |
| NUGET-001 | assets/project watcher | PackageReference و TFM تغییر incremental بدهد | fixture restore |
| NUGET-002 | package API index | direct/transitive compile assets درست باشند | assets comparison |
| NUGET-003 | cache content-addressed | key شامل id/version/tfm/hash باشد | cache tests |

### EF و rule knowledge

| ID | خروجی | Acceptance criteria | Validation |
|---|---|---|---|
| EF-005 | provenance و IsComplete | مدل ناقص هیچ negative rule قطعی تولید نکند | model corpus |
| EF-006 | conventions/annotations | key/FK/navigation با مدل واقعی منطبق باشد | ToDebugString oracle |
| EF-007 | Fluent interpreter | symbol identity و lambda property resolve شود | EF fixture |
| EF-009 | translation matrix | verdict بر اساس provider/version باشد | ToQueryString CI |
| EF-010 | query completion | Include/ThenInclude و IQueryable context معتبر باشند | compile-valid rate |
| KNOW-002 | official crawler | RSS/sitemap، rate limit و canonical URL ثبت شود | crawler fixtures |
| KNOW-004 | signed rule pack | hash/signature، compatibility و rollback کار کند | package tests |

### Editor و کیفیت

| ID | خروجی | Acceptance criteria | Validation |
|---|---|---|---|
| IDE-001 | transport اختصاصی | request cancellation و version guard داشته باشد | protocol integration |
| IDE-002 | context classifier | expected type، lambda، LINQ و async تشخیص داده شوند | context corpus |
| IDE-005 | explainable ranker | score featureها قابل مشاهده باشند | ranking tests |
| QA-001 | corpus واقعی | مجوز، نسخه زبان و TFM ثبت شده باشد | corpus manifest |
| QA-002 | metric harness | Top-k، MRR، compile-valid و latency ثبت شوند | benchmark run |
| REL-002 | alpha release | build، pack، changelog و tag immutable باشند | release checklist |

## Definition of Ready

- contract و dependency مشخص است.
- test fixture یا روش ساخت fixture مشخص است.
- branch name و نوع commit تعیین شده است.
- معلوم است که تغییر external dependency وارد runtime می‌کند یا نه.

## Definition of Done

- acceptance criteria کامل است.
- build و test scope سبز است.
- Graphify update انجام شده است.
- docs/task state به‌روز است.
- commit Conventional و branch قابل review است.
- risk و shortcut احتمالی ثبت شده است.

