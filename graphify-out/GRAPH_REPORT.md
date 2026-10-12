# Graph Report - CsharpVscode  (2026-10-12)

## Corpus Check
- 70 files · ~17,740 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 5 file(s) not represented in the graph (top: (none) 3, .props 1, .Config 1)

## Summary
- 626 nodes · 895 edges · 46 communities (37 shown, 9 thin omitted)
- Extraction: 94% EXTRACTED · 6% INFERRED · 0% AMBIGUOUS · INFERRED: 50 edges (avg confidence: 0.88)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `fe5e8dc7`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- TypeSymbol
- Shared.csproj
- DotnetExpertAdvisor.Shared
- Contracts.cs
- adr/README.md
- Binder
- تصمیم‌های معماری
- قراردادهای داخلی سامانه
- معماری سامانه هوشمندی C# و .NET برای VS Code
- طراحی دقت‌محور Binder و Overload Resolution
- backlog اصلی
- ConversionKind
- قرارداد LSP و مدل پاسخ پیشنهادها
- SymbolKind
- package.json
- طراحی شبیه‌ساز EF Core و Translatability
- Product Backlog
- طراحی Rule Engine
- extension.js
- ماتریس پوشش C# و CLR
- مشارکت و Git Flow
- Schemaهای داده
- Ponytail Help
- طراحی Knowledge Pipeline
- TypeKind
- RuntimeModel.cs
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
- VarianceKind
- SemanticGraph/Class1.cs
- Protocol/Class1.cs
- Shared/Class1.cs
- RankingEngine/Class1.cs
- RuleEngine/Class1.cs
- RuntimeModel/Class1.cs
- KnowledgeEngine/Class1.cs
- RoslynHost/Class1.cs

## God Nodes (most connected - your core abstractions)
1. `TypeSymbol` - 35 edges
2. `ConversionClassifier` - 31 edges
3. `Symbol` - 22 edges
4. `ConversionKind` - 19 edges
5. `SymbolKind` - 18 edges
6. `Accessibility` - 15 edges
7. `معماری سامانه هوشمندی C# و .NET برای VS Code` - 15 edges
8. `NamedTypeSymbol` - 14 edges
9. `BinderScopeKind` - 13 edges
10. `Binder` - 13 edges

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

## Communities (46 total, 9 thin omitted)

### Community 0 - "TypeSymbol"
Cohesion: 0.07
Nodes (39): Symbols, ConversionCacheKey, ConversionClassifier, CachedConversionCount, ConversionContext, ConversionResult, Identity, IConversionClassifier (+31 more)

### Community 1 - "Shared.csproj"
Cohesion: 0.07
Nodes (20): net10.0, Microsoft.NET.Sdk, net10.0, Microsoft.NET.Sdk, net10.0, Microsoft.NET.Sdk, net10.0, Microsoft.NET.Sdk (+12 more)

### Community 2 - "DotnetExpertAdvisor.Shared"
Cohesion: 0.08
Nodes (22): DotnetExpertAdvisor.Shared, DotnetExpertAdvisor.RuleEngine, DotnetExpertAdvisor.RoslynHost, DotnetExpertAdvisor.Architecture.Tests, DotnetExpertAdvisor.SemanticGraph, DotnetExpertAdvisor.RankingEngine, 1. VS Code Client, 2. Custom C# Language Server (+14 more)

### Community 3 - "Contracts.cs"
Cohesion: 0.11
Nodes (29): ISemanticGraph, SemanticGraph, DocumentAnalysis, DocumentId, DocumentRequest, Evidence, GraphEdge, GraphNode (+21 more)

### Community 4 - "adr/README.md"
Cohesion: 0.06
Nodes (26): ADR-001: semantic engine اختصاصی, Consequences, Decision, ADR-002: language server جدا از VS Code client, Consequences, Decision, ADR-003: knowledge pack امضاشده, Consequences (+18 more)

### Community 5 - "Binder"
Cohesion: 0.08
Nodes (24): Binder, Kind, Parent, BinderScopeKind, Block, Global, Imports, Lambda (+16 more)

### Community 6 - "تصمیم‌های معماری"
Cohesion: 0.07
Nodes (26): ADR-001: موتور semantic اختصاصی, ADR-002: language server جدا از VS Code client, ADR-003: knowledge pack به‌جای self-modifying code, ADR-004: snapshot و generation برای consistency, ADR-005: precision مقدم بر recall در ruleهای فعال, ADR-006: عدم وابستگی خارجی در runtime افزونه, ADR-007: پشتیبانی C# 6 تا 15 با capability matrix, ADR-008: NuGet و EF به‌عنوان بخشی از semantic context (+18 more)

### Community 7 - "قراردادهای داخلی سامانه"
Cohesion: 0.10
Nodes (19): DotnetExpertAdvisor.KnowledgeEngine, DocumentContext, Evidence, observability, SemanticContext, SuggestionCandidate, جریان درخواست completion, خطا و cancellation (+11 more)

### Community 8 - "معماری سامانه هوشمندی C# و .NET برای VS Code"
Cohesion: 0.08
Nodes (24): pipeline به‌روزرسانی Microsoft Blog, امنیت و حریم خصوصی, تصمیم معماری اصلی, تصمیم پیشنهادی فعلی, دامنه قابلیت‌ها, دانش و به‌روزرسانی, ساختار repository پیشنهادی, قابلیت‌های CLR و حافظه (+16 more)

### Community 9 - "طراحی دقت‌محور Binder و Overload Resolution"
Cohesion: 0.11
Nodes (17): Best-effort و speculative binding, Better function member, Binder chain, Candidate set, Conversion Classifier, Generic Inference, Member Lookup, Nullable flow (+9 more)

### Community 10 - "backlog اصلی"
Cohesion: 0.11
Nodes (17): backlog اصلی, milestone دقت‌محور Binder, milestone شبیه‌ساز EF و translatability, تعریف انجام‌شدن, دفتر کار پروژه, فاز 1: قرارداد و هسته زبان, فاز 2: semantic engine اختصاصی, فاز 3: metadata، assembly و NuGet (+9 more)

### Community 11 - "ConversionKind"
Cohesion: 0.12
Nodes (17): ConversionKind, AnonymousFunction, Boxing, CollectionExpression, ExplicitNumeric, Identity, ImplicitNullable, ImplicitNumeric (+9 more)

### Community 12 - "قرارداد LSP و مدل پاسخ پیشنهادها"
Cohesion: 0.13
Nodes (14): DotnetExpertAdvisor.Protocol, `runtime/explainSuggestion`, زمان‌بندی و سطح تحلیل, قابلیت اختصاصی, قابلیت‌های پایه, قرارداد Completion, قرارداد LSP و مدل پاسخ پیشنهادها, هدف (+6 more)

### Community 13 - "SymbolKind"
Cohesion: 0.12
Nodes (16): SymbolKind, ArrayType, DynamicType, ErrorType, Event, Field, FunctionPointerType, Local (+8 more)

### Community 14 - "package.json"
Cohesion: 0.12
Nodes (15): activationEvents, categories, contributes, commands, description, displayName, engines, vscode (+7 more)

### Community 15 - "طراحی شبیه‌ساز EF Core و Translatability"
Cohesion: 0.14
Nodes (13): cross-checkهای اختیاری, gate پیشنهادی, provenance و confidence, Query provenance, Translation verdict, اصل, ترتیب بازسازی مدل, سطح اول: تحلیل ایستا (+5 more)

### Community 16 - "Product Backlog"
Cohesion: 0.17
Nodes (11): Backlog تفصیلی, Compiler core, Definition of Done, Definition of Ready, Editor و کیفیت, EF و rule knowledge, Metadata و project index, Product Backlog (+3 more)

### Community 17 - "طراحی Rule Engine"
Cohesion: 0.18
Nodes (10): Correctness, Design, Performance, Schema پیشنهادی Rule, اصل اصلی, انواع Rule, سطوح اطمینان, طراحی Rule Engine (+2 more)

### Community 18 - "extension.js"
Cohesion: 0.18
Nodes (4): cp, fs, path, vscode

### Community 19 - "ماتریس پوشش C# و CLR"
Cohesion: 0.20
Nodes (9): 1. اجرای CLR، assembly و deployment, 2. مدل انواع و dispatch, 3. انواع ضروری و امکانات زبان, 4. exception، memory و resource lifetime, 5. hosting، loading، reflection و serialization, 6. threading و asynchronous execution, استفاده در پیشنهادها, قرارداد پوشش (+1 more)

### Community 20 - "مشارکت و Git Flow"
Cohesion: 0.22
Nodes (8): جریان feature, جریان hotfix, جریان release, شاخه‌ها, قواعد تغییر, مشارکت و Git Flow, نام‌گذاری commit, نسخه‌گذاری

### Community 21 - "Schemaهای داده"
Cohesion: 0.22
Nodes (8): Compatibility, GraphEdge, GraphNode, KnowledgeReference, RuleDescriptor, Schemaهای داده, Settings, شناسه‌ها

### Community 22 - "Ponytail Help"
Cohesion: 0.25
Nodes (7): Configure Default Mode, Deactivate, Levels, More, Ponytail Help, Skills, Update

### Community 23 - "طراحی Knowledge Pipeline"
Cohesion: 0.25
Nodes (7): Blog crawler, Manifest, تولید Rule, شناسه و نسخه, طراحی Knowledge Pipeline, مراحل ingestion, منابع

### Community 24 - "TypeKind"
Cohesion: 0.25
Nodes (8): TypeKind, Class, Delegate, Enum, Error, Interface, Record, Struct

### Community 25 - "RuntimeModel.cs"
Cohesion: 0.48
Nodes (4): DotnetExpertAdvisor.RuntimeModel, IRuntimeModel, RuntimeEstimate, RuntimeModel

### Community 26 - "قرارداد دقت پیشنهاد"
Cohesion: 0.29
Nodes (6): Quality gates, SuggestionContract, SuggestionEvidence, قرارداد دقت پیشنهاد, قواعد اجباری, منابع دانشی

### Community 27 - "طراحی ارزیابی و کیفیت پیشنهادها"
Cohesion: 0.29
Nodes (6): Feedback کاربر, آستانه اولیه نسخه آزمایشی, تست‌های لازم, طراحی ارزیابی و کیفیت پیشنهادها, مجموعه آزمون, معیارهای اصلی

### Community 28 - "ConversionQuality"
Cohesion: 0.33
Nodes (6): ConversionQuality, Best, Good, Invalid, RequiresRuntimeCheck, Uncertain

### Community 29 - "Ponytail, lazy senior dev mode"
Cohesion: 0.40
Nodes (4): Before you write, graphify, Ponytail, lazy senior dev mode, The smallest complete change

### Community 30 - "ponytail-audit/SKILL.md"
Cohesion: 0.40
Nodes (4): 1. Map first, 2. Look for, 3. Check before you report, 4. Output

### Community 31 - "Ponytail Gain"
Cohesion: 0.40
Nodes (4): Boundaries, Honesty boundary, Ponytail Gain, Scoreboard

### Community 32 - "ponytail-review/SKILL.md"
Cohesion: 0.40
Nodes (4): 1. Understand first, 2. Look for, 3. Check before you report, 4. Output

### Community 33 - "Ponytail"
Cohesion: 0.40
Nodes (4): Before you write, Levels, Ponytail, The smallest complete change

### Community 34 - "سیاست نسخه‌گذاری"
Cohesion: 0.40
Nodes (4): سیاست نسخه‌گذاری, منبع نسخه, نسخه‌های بعدی پیشنهادی, نگاشت تغییر به نسخه

### Community 35 - "ponytail-debt/SKILL.md"
Cohesion: 0.50
Nodes (3): Boundaries, Output, Scan

### Community 36 - "VarianceKind"
Cohesion: 0.50
Nodes (4): VarianceKind, Contravariant, Covariant, Invariant

## Knowledge Gaps
- **105 isolated node(s):** `net10.0`, `Microsoft.NET.Sdk`, `net10.0`, `Microsoft.NET.Sdk`, `net10.0` (+100 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 384 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **9 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `DotnetExpertAdvisor.Shared` connect `DotnetExpertAdvisor.Shared` to `TypeSymbol`, `Contracts.cs`, `Binder`, `قراردادهای داخلی سامانه`, `قرارداد LSP و مدل پاسخ پیشنهادها`, `RuntimeModel.cs`?**
  _High betweenness centrality (0.153) - this node is a cross-community bridge._
- **What connects `net10.0`, `Microsoft.NET.Sdk`, `net10.0` to the rest of the system?**
  _105 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `TypeSymbol` be split into smaller, more focused modules?**
  _Cohesion score 0.07450444292549556 - nodes in this community are weakly interconnected._
- **Why does `RoslynHost` connect `DotnetExpertAdvisor.Shared` to `Contracts.cs`?**
  _High betweenness centrality (0.066) - this node is a cross-community bridge._
- **Should `Shared.csproj` be split into smaller, more focused modules?**
  _Cohesion score 0.06984126984126984 - nodes in this community are weakly interconnected._
- **Should `DotnetExpertAdvisor.Shared` be split into smaller, more focused modules?**
  _Cohesion score 0.07816091954022988 - nodes in this community are weakly interconnected._
- **Should `Contracts.cs` be split into smaller, more focused modules?**
  _Cohesion score 0.1099290780141844 - nodes in this community are weakly interconnected._