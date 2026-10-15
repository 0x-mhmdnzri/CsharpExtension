# معماری سامانه هوشمندی C# و .NET برای VS Code

## وضعیت سند

- نسخه: 0.1
- وضعیت: پیش‌نویس معماری برای تأیید
- وضعیت پیاده‌سازی: هنوز هیچ کدی بر اساس این سند ایجاد نشده است
- دامنه نسخه هدف: تحلیل عمیق پروژه، جایگزینی کامل قابلیت‌های زبانی C# و پیشنهادهای مبتنی بر CLR، Roslyn و دانش .NET

## هدف

ساخت یک Language Intelligence Platform برای C# و .NET که completion، parsing، semantic analysis، ساختار واقعی پروژه، روابط نمادها، قراردادهای API، رفتار runtime، هزینه‌های حافظه و اصول طراحی را خودش کنترل کند و پیشنهادهای دقیق، قابل توضیح و قابل آزمون ارائه دهد.

هسته افزونه نباید به language server، موتور IntelliSense، سرویس ابری، مدل خارجی یا package تحلیل کد خارجی وابسته باشد. VS Code فقط بستر editor است؛ parser، semantic engine، index، ranking و protocol adapter در همین repository توسعه داده می‌شوند.

هر پیشنهاد باید به یک علت فنی، بخش مشخصی از پروژه و یک یا چند منبع دانش متصل باشد. پیشنهاد بدون دلیل، منبع یا قابلیت بازتولید در تست پذیرفته نمی‌شود.

## دامنه قابلیت‌ها

### قابلیت‌های زبانی و ویرایشی

- Completion و ranking اختصاصی
- Signature Help، Hover و نمایش توضیح فنی
- Diagnostics و Code Actions
- Refactoring، Rename، Go to Definition، Find References و Navigation
- تحلیل nullable، generic constraint، accessibility و API compatibility
- پیشنهاد بر اساس namespace، پروژه، target framework و dependencyهای واقعی

### قابلیت‌های تحلیل پروژه

- بارگذاری کامل Solution و Project
- ساخت گراف پروژه، reference، package و symbol
- تحلیل چندهدفه برای Target Frameworkهای مختلف
- ردیابی مسیر داده، exception flow، control flow و async flow
- تشخیص قراردادهای معماری پروژه
- تشخیص الگوهای تکراری، coupling، dependency cycle و API misuse

### قابلیت‌های CLR و حافظه

- metadata، assembly، IL و JIT
- type system، boxing/unboxing، value/reference semantics
- GC generations، LOH، POH، finalization و pinning
- allocation، copying و lifetime
- Span/Memory، ArrayPool و object pooling
- reflection، AssemblyLoadContext و loading behavior
- thread، synchronization، Task، async/await و execution context
- exception، stack و اثر آن بر performance

### دانش و به‌روزرسانی

- دانش ساختاریافته از CLR via C# و Pro .NET Memory Management، با رعایت مجوز نشر
- مقالات رسمی Microsoft .NET Blog و DevBlogs
- نسخه‌گذاری و امضای knowledge pack
- crawler جداگانه با رعایت robots.txt، شرایط استفاده و محدودیت نرخ
- آزمون regression قبل از انتشار هر قاعده یا بسته دانش

## تصمیم معماری اصلی

برای کنترل کامل completion و ranking، افزونه به یک language server اختصاصی نیاز دارد. این server parser، symbol table، type system، flow analysis، index و ranking خودش را دارد و از طریق transport اختصاصی با VS Code ارتباط دارد. استفاده از Roslyn یا language server C# به‌عنوان authority مجاز نیست؛ سازگاری رفتاری با قواعد زبان باید با corpus و specification داخلی سنجیده شود.

افزونه VS Code مسئول UI، تنظیمات، مدیریت lifecycle و به‌روزرسانی بسته‌های دانش است. تحلیل سنگین در process جداگانه انجام می‌شود تا crash یا مصرف حافظه آن editor را از کار نیندازد.

## اجزای سامانه

### 1. VS Code Client

مسئولیت‌ها:

- ثبت commandها، settingها و viewها
- اتصال LSP
- نمایش completion، diagnostic، explanation و source
- مدیریت cache و signed knowledge pack
- گزارش feedback کاربر
- کنترل حالت offline و privacy

### 2. Custom C# Language Server

مسئولیت‌ها:

- پیاده‌سازی endpointهای LSP
- هماهنگی با workspace و document lifecycle
- تبدیل نتایج Roslyn و rule engine به LSP
- اعمال ranking واحد روی همه پیشنهادها
- جلوگیری از پیشنهادهای تکراری یا متناقض
- ارائه trace قابل مشاهده برای debug

### 3. C# Compiler Intelligence Host

مسئولیت‌ها:

- workspace و document lifecycle اختصاصی
- lexer، parser، syntax tree و syntax recovery
- symbol table، type system، overload resolution و nullable flow
- control/data/exception/async flow
- incremental analysis و cancellation
- مدیریت C# 6 تا 15 و چند Target Framework

`RoslynHost` adapter runtime رسمی Roslyn است؛ Roslyn مجموعه‌ی کاندیدهای معتبر را تولید می‌کند و موتور داخلی فقط مسئول ranking و intent است.

### 4. Project Semantic Graph

گره‌ها:

- Solution، Project، Assembly، Package
- Namespace، Type، Member، Parameter و Local
- File، Document، API و Configuration

یال‌ها:

- reference، inheritance، implementation
- calls، reads، writes، constructs
- depends-on، overrides و implements
- allocates، awaits، throws و catches

گراف باید incremental باشد و تغییر یک فایل باعث بازتحلیل کل solution نشود.

### 5. Runtime and Memory Model

این بخش مدل ایستای کد را به رفتار احتمالی runtime نگاشت می‌کند:

- هزینه تخصیص برای هر expression و مسیر اجرایی
- احتمال boxing، copying و escape
- اثر closure و async state machine
- احتمال فشار روی LOH/POH
- lifetime object و rootهای احتمالی
- trade-off بین readability، correctness و throughput

این مدل جایگزین profiler نیست؛ خروجی آن باید به‌صورت «پیشنهاد احتمالی با سطح اطمینان» ارائه شود.

### 6. Rule Engine

هر rule شامل این بخش‌هاست:

```text
Rule
├── id و version
├── trigger
├── preconditions
├── Roslyn query
├── runtime rationale
├── suggested transformation
├── severity و confidence
├── applicable frameworks
├── references
└── test cases
```

قواعد به سه دسته تقسیم می‌شوند:

- Correctness: خطا یا رفتار نادرست
- Design: نقض قرارداد، abstraction یا معماری
- Performance: allocation، latency، throughput یا memory pressure

### 7. Knowledge Engine

دانش خام مستقیماً به کاربر پیشنهاد نمی‌شود. ابتدا به concept، claim، rule، example و counterexample تبدیل می‌شود.

هر claim باید داشته باشد:

- source URI یا شناسه کتاب
- نسخه و تاریخ استخراج
- سطح اعتبار
- مفاهیم مرتبط
- نمونه مثبت و منفی
- ruleهای استفاده‌کننده
- محدودیت‌ها و شرایط کاربرد

### 8. Ranking Engine

امتیاز پیشنهادی بر اساس این عوامل محاسبه می‌شود:

- ارتباط با متن و cursor
- تطابق با semantic graph پروژه
- سازگاری با target framework
- شدت مشکل
- میزان اطمینان rule
- سابقه feedback کاربر
- هزینه تغییر پیشنهادی
- احتمال false positive

Ranking نباید فقط بر اساس شباهت متنی باشد.

## قرارداد LSP

نسخه اول باید endpointهای زیر را پشتیبانی کند:

- initialize و workspace management
- textDocument/completion
- textDocument/hover
- textDocument/signatureHelp
- textDocument/publishDiagnostics
- textDocument/codeAction
- textDocument/definition
- textDocument/references
- textDocument/rename
- workspace/symbol

برای توضیح تخصصی پیشنهادها، یک extension method اختصاصی اضافه می‌شود:

```text
runtime/explainSuggestion
```

خروجی آن شامل rule، evidence، runtime impact، memory impact و source است.

## چرخه تولید پیشنهاد

```text
Document Change
  → Incremental Roslyn Update
  → Semantic Context
  → Candidate Rules
  → Project/Runtime Checks
  → Confidence Calculation
  → Ranking
  → Deduplication
  → LSP Response
```

پیشنهاد باید cancellation-aware باشد. اگر تحلیل عمیق در زمان مناسب تمام نشود، completion سریع با نتایج پایه برگردد و نتایج تحلیلی بعداً به‌روزرسانی شوند.

## pipeline به‌روزرسانی Microsoft Blog

```text
RSS/Sitemap/Official Pages
  → Fetch با rate limit
  → robots/terms validation
  → Canonicalization
  → Content extraction
  → Concept and claim extraction
  → Rule proposal
  → Human or policy review
  → Roslyn regression corpus
  → Package build
  → Package signing
  → Publish manifest
  → Client verification and download
```

افزونه نباید کد اجرایی خود را از محتوای crawl شده تولید و اجرا کند. Blog فقط knowledge و rule metadata تولید می‌کند و ruleهای اجرایی باید در محیط build معتبر تست شوند.

## امنیت و حریم خصوصی

- کد پروژه به‌صورت پیش‌فرض از دستگاه خارج نمی‌شود.
- استفاده از backend یا مدل خارجی باید opt-in باشد.
- knowledge pack با امضای دیجیتال و hash بررسی می‌شود.
- crawler و parser در sandbox اجرا می‌شوند.
- محتوای خارجی هرگز به‌عنوان کد قابل اجرا پذیرفته نمی‌شود.
- تنظیمات شامل حالت offline، مسیر cache و telemetry قابل خاموش‌کردن است.

## ساختار repository پیشنهادی

```text
src/
  VSCode.Client/
  LanguageServer/
  RoslynHost/
  ProjectModel/
  SemanticGraph/
  RuntimeModel/
  RuleEngine/
  KnowledgeEngine/
  RankingEngine/
  Protocol/
  Shared/

tools/
  BlogCrawler/
  KnowledgeCompiler/
  RuleValidator/
  CorpusRunner/

knowledge/
  concepts/
  rules/
  sources/
  schemas/

tests/
  LanguageServer.Tests/
  RoslynHost.Tests/
  RuleEngine.Tests/
  Ranking.Tests/
  Corpus/
  Compatibility/

docs/
  architecture.md
  lsp-contract.md
  rule-authoring.md
  knowledge-pipeline.md
  evaluation.md
```

## معیارهای پذیرش

- completion پایه در پروژه‌های کوچک بدون تأخیر محسوس
- تحلیل incremental در solutionهای بزرگ
- ثبت p50 و p95 زمان پاسخ LSP
- اندازه‌گیری precision و false-positive برای هر rule
- regression test برای هر rule جدید
- نمایش source و rationale برای تمام پیشنهادهای تحلیلی
- عدم نشت کد پروژه در حالت local
- سازگاری جداگانه با هر Target Framework
- قابلیت خاموش‌کردن rule یا category به‌صورت مستقل

## مراحل توسعه

### مرحله صفر: تثبیت قرارداد

- تعیین معنای دقیق CLRS و CIS
- تعیین رابطه با C# Dev Kit
- تعیین نسخه‌های هدف .NET
- تعیین سیاست مجوز منابع
- نهایی‌سازی schemaهای LSP، rule و knowledge

### مرحله یک: هسته language server

- workspace و document lifecycle
- compiler داخلی و semantic model
- completion پایه و diagnostics
- logging، cancellation و cache

### مرحله دو: semantic graph و rule engine

- ساخت graph incremental
- ruleهای correctness و design
- explanation و source tracking
- corpus اولیه و regression framework

### مرحله سه: runtime و memory intelligence

- allocation model
- GC/LOH/POH rules
- async/closure/boxing rules
- performance suggestions با confidence

### مرحله چهار: knowledge pipeline

- crawler رسمی
- compiler برای knowledge pack
- signing، manifest و update client
- تست ingestion و rollback

### مرحله پنج: ranking و جایگزینی کامل

- ranking چندمعیاره
- feedback loop
- پوشش کامل endpointهای LSP
- تصمیم نهایی درباره کنار گذاشتن یا هماهنگی با C# Dev Kit

## مواردی که هنوز باید تأیید شوند

1. معنای دقیق CLRS و CIS.
2. local-only بودن تحلیل یا مجازبودن backend/LLM.
3. نسخه‌های هدف .NET.
4. جایگزینی کامل C# Dev Kit یا اجرای همزمان و هماهنگ.
5. سطح telemetry و feedback.
6. منابع کتابی و محدوده محتوایی مجاز برای استخراج.

## تصمیم پیشنهادی فعلی

تا زمان تأیید موارد بالا، این پروژه با فرض‌های زیر طراحی می‌شود:

- زبان پیاده‌سازی: C#
- ارتباط editor: LSP
- موتور semantic: پیاده‌سازی داخلی؛ Roslyn فقط در صورت تصمیم جداگانه برای ابزار مقایسه‌ای خارج از runtime
- اجرای تحلیل: local-first
- knowledge update: بسته‌های نسخه‌دار و امضاشده
- Blog source: فقط منابع رسمی Microsoft
- ruleها: explainable، testable و versioned
- شروع کار: طراحی و قراردادها، بدون تولید کد اجرایی
