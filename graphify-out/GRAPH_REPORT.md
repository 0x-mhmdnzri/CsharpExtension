# Graph Report - CsharpVscode  (2026-10-13)

## Corpus Check
- 91 files · ~24,444 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 5 file(s) not represented in the graph (top: (none) 3, .props 1, .Config 1)

## Summary
- 918 nodes · 1537 edges · 49 communities (39 shown, 10 thin omitted)
- Extraction: 95% EXTRACTED · 5% INFERRED · 0% AMBIGUOUS · INFERRED: 83 edges (avg confidence: 0.87)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `7cf66b41`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- قرارداد LSP و مدل پاسخ پیشنهادها
- Shared.csproj
- .Tokenize
- TypeSymbol
- Contracts.cs
- معماری سامانه هوشمندی C# و .NET برای VS Code
- adr/README.md
- TypeProvider
- LanguageFeature
- تصمیم‌های معماری
- قراردادهای داخلی سامانه
- EditorFeatures.cs
- طراحی دقت‌محور Binder و Overload Resolution
- .Read
- DotnetExpertAdvisor.Shared
- backlog اصلی
- ConversionKind
- Binder
- extension.js
- SymbolKind
- package.json
- طراحی شبیه‌ساز EF Core و Translatability
- Product Backlog
- طراحی Rule Engine
- ماتریس پوشش C# و CLR
- .Analyze
- مشارکت و Git Flow
- Schemaهای داده
- Ponytail Help
- طراحی Knowledge Pipeline
- قرارداد دقت پیشنهاد
- طراحی ارزیابی و کیفیت پیشنهادها
- ConversionQuality
- Ponytail, lazy senior dev mode
- ponytail-audit/SKILL.md
- Ponytail Gain
- ponytail-review/SKILL.md
- Ponytail
- سیاست نسخه‌گذاری
- ponytail-debt/SKILL.md
- SemanticGraph/Class1.cs
- Protocol/Class1.cs
- Shared/Class1.cs
- RankingEngine/Class1.cs
- RuleEngine/Class1.cs
- RuntimeModel/Class1.cs
- KnowledgeEngine/Class1.cs
- RoslynHost/Class1.cs

## God Nodes (most connected - your core abstractions)
1. `TypeSymbol` - 40 edges
2. `ConversionClassifier` - 33 edges
3. `LanguageFeature` - 33 edges
4. `DotnetExpertAdvisor.Shared` - 31 edges
5. `Symbol` - 30 edges
6. `TypeProvider` - 19 edges
7. `ConversionKind` - 19 edges
8. `CSharpLexer` - 18 edges
9. `SymbolKind` - 18 edges
10. `NamedTypeSymbol` - 16 edges

## Surprising Connections (you probably didn't know these)
- `Binder chain` --references--> `LookupResult`  [INFERRED]
  docs/design/binder-overload-resolution.md → src/Shared/Binder.cs
- `قرارداد RuleEngine` --references--> `KnowledgeSnapshot`  [INFERRED]
  docs/internal-contracts.md → src/KnowledgeEngine/KnowledgeEngine.cs
- `3. C# Compiler Intelligence Host` --references--> `RoslynHost`  [INFERRED]
  docs/architecture.md → src/RoslynHost/RoslynHost.cs
- `مرزهای ماژول‌ها` --references--> `RoslynHost`  [INFERRED]
  docs/internal-contracts.md → src/RoslynHost/RoslynHost.cs
- `قرارداد Completion` --references--> `CompletionItem`  [INFERRED]
  docs/lsp-contract.md → src/Protocol/ProtocolModels.cs

## Import Cycles
- None detected.

## Communities (49 total, 10 thin omitted)

### Community 0 - "قرارداد LSP و مدل پاسخ پیشنهادها"
Cohesion: 0.07
Nodes (21): DotnetExpertAdvisor.RuleEngine, DotnetExpertAdvisor.Protocol, DotnetExpertAdvisor.LanguageServer, `runtime/explainSuggestion`, زمان‌بندی و سطح تحلیل, قابلیت اختصاصی, قابلیت‌های پایه, قرارداد Completion (+13 more)

### Community 1 - "Shared.csproj"
Cohesion: 0.07
Nodes (20): net10.0, Microsoft.NET.Sdk, net10.0, Microsoft.NET.Sdk, net10.0, Microsoft.NET.Sdk, net10.0, Microsoft.NET.Sdk (+12 more)

### Community 2 - ".Tokenize"
Cohesion: 0.06
Nodes (38): LanguageCapabilities, CSharpLexer, LexerDiagnostic, LexResult, SyntaxToken, TextChangeRange, TextSpan, End (+30 more)

### Community 3 - "TypeSymbol"
Cohesion: 0.05
Nodes (57): Symbols, LookupOptions, LookupResult, HasCandidates, IsSuccess, ConversionCacheKey, ConversionClassifier, CachedConversionCount (+49 more)

### Community 4 - "Contracts.cs"
Cohesion: 0.09
Nodes (33): DotnetExpertAdvisor.SemanticGraph, DotnetExpertAdvisor.RuntimeModel, IRankingEngine, RankingEngine, IRuntimeModel, RuntimeEstimate, RuntimeModel, ISemanticGraph (+25 more)

### Community 5 - "معماری سامانه هوشمندی C# و .NET برای VS Code"
Cohesion: 0.05
Nodes (37): DotnetExpertAdvisor.RoslynHost, 1. VS Code Client, 2. Custom C# Language Server, 3. C# Compiler Intelligence Host, 4. Project Semantic Graph, 5. Runtime and Memory Model, 6. Rule Engine, 7. Knowledge Engine (+29 more)

### Community 6 - "adr/README.md"
Cohesion: 0.06
Nodes (26): ADR-001: semantic engine اختصاصی, Consequences, Decision, ADR-002: language server جدا از VS Code client, Consequences, Decision, ADR-003: knowledge pack امضاشده, Consequences (+18 more)

### Community 8 - "LanguageFeature"
Cohesion: 0.07
Nodes (29): LanguageFeature, CollectionExpressions, ExtensionMembers, FieldKeyword, FileScopedNamespaces, GenericMath, GlobalUsings, InitOnlySetters (+21 more)

### Community 9 - "تصمیم‌های معماری"
Cohesion: 0.07
Nodes (26): ADR-001: موتور semantic اختصاصی, ADR-002: language server جدا از VS Code client, ADR-003: knowledge pack به‌جای self-modifying code, ADR-004: snapshot و generation برای consistency, ADR-005: precision مقدم بر recall در ruleهای فعال, ADR-006: عدم وابستگی خارجی در runtime افزونه, ADR-007: پشتیبانی C# 6 تا 15 با capability matrix, ADR-008: NuGet و EF به‌عنوان بخشی از semantic context (+18 more)

### Community 10 - "قراردادهای داخلی سامانه"
Cohesion: 0.10
Nodes (19): DotnetExpertAdvisor.KnowledgeEngine, DocumentContext, Evidence, observability, SemanticContext, SuggestionCandidate, جریان درخواست completion, خطا و cancellation (+11 more)

### Community 11 - "EditorFeatures.cs"
Cohesion: 0.16
Nodes (14): SuggestionSeverity, Error, Hint, Info, Warning, CodeActionInfo, DefinitionLocation, DiagnosticInfo (+6 more)

### Community 12 - "طراحی دقت‌محور Binder و Overload Resolution"
Cohesion: 0.11
Nodes (17): Best-effort و speculative binding, Better function member, Binder chain, Candidate set, Conversion Classifier, Generic Inference, Member Lookup, Nullable flow (+9 more)

### Community 13 - ".Read"
Cohesion: 0.08
Nodes (16): AssemblyMetadataIndex, AssemblyMetadataReader, MetadataAssemblyReference, MetadataGenericParameter, MetadataMember, MetadataType, MetadataTypeForwarder, NuGetApiCache (+8 more)

### Community 14 - "DotnetExpertAdvisor.Shared"
Cohesion: 0.05
Nodes (32): DotnetExpertAdvisor.Shared, DotnetExpertAdvisor.Architecture.Tests, DotnetExpertAdvisor.RankingEngine, LookupStatus, Ambiguous, Inaccessible, NotAValue, NotFound (+24 more)

### Community 15 - "backlog اصلی"
Cohesion: 0.11
Nodes (17): backlog اصلی, milestone دقت‌محور Binder, milestone شبیه‌ساز EF و translatability, تعریف انجام‌شدن, دفتر کار پروژه, فاز 1: قرارداد و هسته زبان, فاز 2: semantic engine اختصاصی, فاز 3: metadata، assembly و NuGet (+9 more)

### Community 16 - "ConversionKind"
Cohesion: 0.12
Nodes (17): ConversionKind, AnonymousFunction, Boxing, CollectionExpression, ExplicitNumeric, Identity, ImplicitNullable, ImplicitNumeric (+9 more)

### Community 17 - "Binder"
Cohesion: 0.14
Nodes (13): Binder, Kind, Parent, BinderScopeKind, Block, Global, Imports, Lambda (+5 more)

### Community 18 - "extension.js"
Cohesion: 0.15
Nodes (10): activate(), cp, fs, handleOutput(), inputBuffer, path, pending, request() (+2 more)

### Community 19 - "SymbolKind"
Cohesion: 0.12
Nodes (16): SymbolKind, ArrayType, DynamicType, ErrorType, Event, Field, FunctionPointerType, Local (+8 more)

### Community 20 - "package.json"
Cohesion: 0.12
Nodes (15): activationEvents, categories, contributes, commands, description, displayName, engines, vscode (+7 more)

### Community 21 - "طراحی شبیه‌ساز EF Core و Translatability"
Cohesion: 0.14
Nodes (13): cross-checkهای اختیاری, gate پیشنهادی, provenance و confidence, Query provenance, Translation verdict, اصل, ترتیب بازسازی مدل, سطح اول: تحلیل ایستا (+5 more)

### Community 22 - "Product Backlog"
Cohesion: 0.15
Nodes (12): Backlog تفصیلی, Compiler core, Definition of Done, Definition of Ready, Editor و کیفیت, EF و rule knowledge, Metadata و project index, Product Backlog (+4 more)

### Community 23 - "طراحی Rule Engine"
Cohesion: 0.18
Nodes (10): Correctness, Design, Performance, Schema پیشنهادی Rule, اصل اصلی, انواع Rule, سطوح اطمینان, طراحی Rule Engine (+2 more)

### Community 24 - "ماتریس پوشش C# و CLR"
Cohesion: 0.20
Nodes (9): 1. اجرای CLR، assembly و deployment, 2. مدل انواع و dispatch, 3. انواع ضروری و امکانات زبان, 4. exception، memory و resource lifetime, 5. hosting، loading، reflection و serialization, 6. threading و asynchronous execution, استفاده در پیشنهادها, قرارداد پوشش (+1 more)

### Community 25 - ".Analyze"
Cohesion: 0.18
Nodes (10): EfLinqAnalyzer, EfQueryAnalysis, EfTranslationStatus, ClientEvaluationRisk, Translatable, Unknown, EfDbContextModel, EfEntityModel (+2 more)

### Community 26 - "مشارکت و Git Flow"
Cohesion: 0.22
Nodes (8): جریان feature, جریان hotfix, جریان release, شاخه‌ها, قواعد تغییر, مشارکت و Git Flow, نام‌گذاری commit, نسخه‌گذاری

### Community 27 - "Schemaهای داده"
Cohesion: 0.22
Nodes (8): Compatibility, GraphEdge, GraphNode, KnowledgeReference, RuleDescriptor, Schemaهای داده, Settings, شناسه‌ها

### Community 28 - "Ponytail Help"
Cohesion: 0.25
Nodes (7): Configure Default Mode, Deactivate, Levels, More, Ponytail Help, Skills, Update

### Community 29 - "طراحی Knowledge Pipeline"
Cohesion: 0.25
Nodes (7): Blog crawler, Manifest, تولید Rule, شناسه و نسخه, طراحی Knowledge Pipeline, مراحل ingestion, منابع

### Community 30 - "قرارداد دقت پیشنهاد"
Cohesion: 0.29
Nodes (6): Quality gates, SuggestionContract, SuggestionEvidence, قرارداد دقت پیشنهاد, قواعد اجباری, منابع دانشی

### Community 31 - "طراحی ارزیابی و کیفیت پیشنهادها"
Cohesion: 0.29
Nodes (6): Feedback کاربر, آستانه اولیه نسخه آزمایشی, تست‌های لازم, طراحی ارزیابی و کیفیت پیشنهادها, مجموعه آزمون, معیارهای اصلی

### Community 32 - "ConversionQuality"
Cohesion: 0.33
Nodes (6): ConversionQuality, Best, Good, Invalid, RequiresRuntimeCheck, Uncertain

### Community 33 - "Ponytail, lazy senior dev mode"
Cohesion: 0.40
Nodes (4): Before you write, graphify, Ponytail, lazy senior dev mode, The smallest complete change

### Community 34 - "ponytail-audit/SKILL.md"
Cohesion: 0.40
Nodes (4): 1. Map first, 2. Look for, 3. Check before you report, 4. Output

### Community 35 - "Ponytail Gain"
Cohesion: 0.40
Nodes (4): Boundaries, Honesty boundary, Ponytail Gain, Scoreboard

### Community 36 - "ponytail-review/SKILL.md"
Cohesion: 0.40
Nodes (4): 1. Understand first, 2. Look for, 3. Check before you report, 4. Output

### Community 37 - "Ponytail"
Cohesion: 0.40
Nodes (4): Before you write, Levels, Ponytail, The smallest complete change

### Community 38 - "سیاست نسخه‌گذاری"
Cohesion: 0.40
Nodes (4): سیاست نسخه‌گذاری, منبع نسخه, نسخه‌های بعدی پیشنهادی, نگاشت تغییر به نسخه

### Community 39 - "ponytail-debt/SKILL.md"
Cohesion: 0.50
Nodes (3): Boundaries, Output, Scan

## Knowledge Gaps
- **171 isolated node(s):** `Translatable`, `Unknown`, `ClientEvaluationRisk`, `net10.0`, `Microsoft.NET.Sdk` (+166 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 477 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **10 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `DotnetExpertAdvisor.Shared` connect `DotnetExpertAdvisor.Shared` to `قرارداد LSP و مدل پاسخ پیشنهادها`, `.Tokenize`, `TypeSymbol`, `Contracts.cs`, `معماری سامانه هوشمندی C# و .NET برای VS Code`, `قراردادهای داخلی سامانه`, `EditorFeatures.cs`, `.Read`, `.Analyze`?**
  _High betweenness centrality (0.221) - this node is a cross-community bridge._
- **Are the 3 inferred relationships involving `ConversionClassifier` (e.g. with `MemberLookup` and `.ValidateConversionClassifier()`) actually correct?**
  _`ConversionClassifier` has 3 INFERRED edges - model-reasoned connections that need verification._
- **What connects `Translatable`, `Unknown`, `ClientEvaluationRisk` to the rest of the system?**
  _171 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `قرارداد LSP و مدل پاسخ پیشنهادها` be split into smaller, more focused modules?**
  _Cohesion score 0.06827880512091039 - nodes in this community are weakly interconnected._
- **Why does `RoslynHost` connect `معماری سامانه هوشمندی C# و .NET برای VS Code` to `قرارداد LSP و مدل پاسخ پیشنهادها`, `Contracts.cs`?**
  _High betweenness centrality (0.061) - this node is a cross-community bridge._
- **Should `Shared.csproj` be split into smaller, more focused modules?**
  _Cohesion score 0.07301587301587302 - nodes in this community are weakly interconnected._
- **Should `.Tokenize` be split into smaller, more focused modules?**
  _Cohesion score 0.060641627543035995 - nodes in this community are weakly interconnected._