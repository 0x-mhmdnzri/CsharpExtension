# Graph Report - CsharpVscode  (2026-10-12)

## Corpus Check
- 66 files · ~15,667 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 4 file(s) not represented in the graph (top: (none) 2, .props 1, .Config 1)

## Summary
- 526 nodes · 695 edges · 40 communities (32 shown, 8 thin omitted)
- Extraction: 95% EXTRACTED · 5% INFERRED · 0% AMBIGUOUS · INFERRED: 37 edges (avg confidence: 0.89)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `9fc40eb5`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- Contracts.cs
- Shared.csproj
- Symbol
- README.md
- معماری سامانه هوشمندی C# و .NET برای VS Code
- اجزای سامانه
- تصمیم‌های معماری
- BinderScopeKind
- طراحی دقت‌محور Binder و Overload Resolution
- backlog اصلی
- قرارداد LSP و مدل پاسخ پیشنهادها
- SymbolKind
- قراردادهای داخلی سامانه
- طراحی شبیه‌ساز EF Core و Translatability
- Product Backlog
- طراحی Rule Engine
- ماتریس پوشش C# و CLR
- مشارکت و Git Flow
- Schemaهای داده
- Ponytail Help
- طراحی Knowledge Pipeline
- قرارداد دقت پیشنهاد
- طراحی ارزیابی و کیفیت پیشنهادها
- Ponytail, lazy senior dev mode
- ponytail-audit/SKILL.md
- Ponytail Gain
- ponytail-review/SKILL.md
- Ponytail
- سیاست نسخه‌گذاری
- RuntimeModel.cs
- ponytail-debt/SKILL.md
- SemanticGraph/Class1.cs
- Protocol/Class1.cs
- Shared/Class1.cs
- RankingEngine/Class1.cs
- RuleEngine/Class1.cs
- RuntimeModel/Class1.cs
- KnowledgeEngine/Class1.cs
- RoslynHost/Class1.cs
- LookupStatus

## God Nodes (most connected - your core abstractions)
1. `Symbol` - 21 edges
2. `SymbolKind` - 18 edges
3. `Accessibility` - 15 edges
4. `معماری سامانه هوشمندی C# و .NET برای VS Code` - 15 edges
5. `BinderScopeKind` - 13 edges
6. `Binder` - 13 edges
7. `DocumentAnalysis` - 13 edges
8. `TypeSymbol` - 13 edges
9. `GraphSnapshot` - 12 edges
10. `DotnetExpertAdvisor.Shared` - 11 edges

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

## Communities (40 total, 8 thin omitted)

### Community 0 - "Contracts.cs"
Cohesion: 0.10
Nodes (30): DotnetExpertAdvisor.SemanticGraph, ISemanticGraph, SemanticGraph, DocumentAnalysis, DocumentId, DocumentRequest, Evidence, GraphEdge (+22 more)

### Community 1 - "Shared.csproj"
Cohesion: 0.07
Nodes (20): net10.0, Microsoft.NET.Sdk, net10.0, Microsoft.NET.Sdk, net10.0, Microsoft.NET.Sdk, net10.0, Microsoft.NET.Sdk (+12 more)

### Community 2 - "Symbol"
Cohesion: 0.07
Nodes (42): DotnetExpertAdvisor.Shared, DotnetExpertAdvisor.Architecture.Tests, Symbols, Binder, Kind, Parent, LookupOptions, LookupResult (+34 more)

### Community 3 - "README.md"
Cohesion: 0.06
Nodes (26): ADR-001: semantic engine اختصاصی, Consequences, Decision, ADR-002: language server جدا از VS Code client, Consequences, Decision, ADR-003: knowledge pack امضاشده, Consequences (+18 more)

### Community 4 - "معماری سامانه هوشمندی C# و .NET برای VS Code"
Cohesion: 0.08
Nodes (24): pipeline به‌روزرسانی Microsoft Blog, امنیت و حریم خصوصی, تصمیم معماری اصلی, تصمیم پیشنهادی فعلی, دامنه قابلیت‌ها, دانش و به‌روزرسانی, ساختار repository پیشنهادی, قابلیت‌های CLR و حافظه (+16 more)

### Community 5 - "اجزای سامانه"
Cohesion: 0.10
Nodes (19): DotnetExpertAdvisor.RuleEngine, DotnetExpertAdvisor.RoslynHost, DotnetExpertAdvisor.RankingEngine, 1. VS Code Client, 2. Custom C# Language Server, 3. C# Compiler Intelligence Host, 4. Project Semantic Graph, 5. Runtime and Memory Model (+11 more)

### Community 6 - "تصمیم‌های معماری"
Cohesion: 0.07
Nodes (26): ADR-001: موتور semantic اختصاصی, ADR-002: language server جدا از VS Code client, ADR-003: knowledge pack به‌جای self-modifying code, ADR-004: snapshot و generation برای consistency, ADR-005: precision مقدم بر recall در ruleهای فعال, ADR-006: عدم وابستگی خارجی در runtime افزونه, ADR-007: پشتیبانی C# 6 تا 15 با capability matrix, ADR-008: NuGet و EF به‌عنوان بخشی از semantic context (+18 more)

### Community 7 - "BinderScopeKind"
Cohesion: 0.18
Nodes (10): BinderScopeKind, Block, Global, Imports, Lambda, Member, Namespace, Pattern (+2 more)

### Community 8 - "طراحی دقت‌محور Binder و Overload Resolution"
Cohesion: 0.11
Nodes (17): Best-effort و speculative binding, Better function member, Binder chain, Candidate set, Conversion Classifier, Generic Inference, Member Lookup, Nullable flow (+9 more)

### Community 9 - "backlog اصلی"
Cohesion: 0.11
Nodes (17): backlog اصلی, milestone دقت‌محور Binder, milestone شبیه‌ساز EF و translatability, تعریف انجام‌شدن, دفتر کار پروژه, فاز 1: قرارداد و هسته زبان, فاز 2: semantic engine اختصاصی, فاز 3: metadata، assembly و NuGet (+9 more)

### Community 10 - "قرارداد LSP و مدل پاسخ پیشنهادها"
Cohesion: 0.13
Nodes (14): DotnetExpertAdvisor.Protocol, `runtime/explainSuggestion`, زمان‌بندی و سطح تحلیل, قابلیت اختصاصی, قابلیت‌های پایه, قرارداد Completion, قرارداد LSP و مدل پاسخ پیشنهادها, هدف (+6 more)

### Community 11 - "SymbolKind"
Cohesion: 0.12
Nodes (16): SymbolKind, ArrayType, DynamicType, ErrorType, Event, Field, FunctionPointerType, Local (+8 more)

### Community 12 - "قراردادهای داخلی سامانه"
Cohesion: 0.10
Nodes (19): DotnetExpertAdvisor.KnowledgeEngine, DocumentContext, Evidence, observability, SemanticContext, SuggestionCandidate, جریان درخواست completion, خطا و cancellation (+11 more)

### Community 13 - "طراحی شبیه‌ساز EF Core و Translatability"
Cohesion: 0.14
Nodes (13): cross-checkهای اختیاری, gate پیشنهادی, provenance و confidence, Query provenance, Translation verdict, اصل, ترتیب بازسازی مدل, سطح اول: تحلیل ایستا (+5 more)

### Community 14 - "Product Backlog"
Cohesion: 0.17
Nodes (11): Backlog تفصیلی, Compiler core, Definition of Done, Definition of Ready, Editor و کیفیت, EF و rule knowledge, Metadata و project index, Product Backlog (+3 more)

### Community 15 - "طراحی Rule Engine"
Cohesion: 0.18
Nodes (10): Correctness, Design, Performance, Schema پیشنهادی Rule, اصل اصلی, انواع Rule, سطوح اطمینان, طراحی Rule Engine (+2 more)

### Community 16 - "ماتریس پوشش C# و CLR"
Cohesion: 0.20
Nodes (9): 1. اجرای CLR، assembly و deployment, 2. مدل انواع و dispatch, 3. انواع ضروری و امکانات زبان, 4. exception، memory و resource lifetime, 5. hosting، loading، reflection و serialization, 6. threading و asynchronous execution, استفاده در پیشنهادها, قرارداد پوشش (+1 more)

### Community 17 - "مشارکت و Git Flow"
Cohesion: 0.22
Nodes (8): جریان feature, جریان hotfix, جریان release, شاخه‌ها, قواعد تغییر, مشارکت و Git Flow, نام‌گذاری commit, نسخه‌گذاری

### Community 18 - "Schemaهای داده"
Cohesion: 0.22
Nodes (8): Compatibility, GraphEdge, GraphNode, KnowledgeReference, RuleDescriptor, Schemaهای داده, Settings, شناسه‌ها

### Community 19 - "Ponytail Help"
Cohesion: 0.25
Nodes (7): Configure Default Mode, Deactivate, Levels, More, Ponytail Help, Skills, Update

### Community 20 - "طراحی Knowledge Pipeline"
Cohesion: 0.25
Nodes (7): Blog crawler, Manifest, تولید Rule, شناسه و نسخه, طراحی Knowledge Pipeline, مراحل ingestion, منابع

### Community 21 - "قرارداد دقت پیشنهاد"
Cohesion: 0.29
Nodes (6): Quality gates, SuggestionContract, SuggestionEvidence, قرارداد دقت پیشنهاد, قواعد اجباری, منابع دانشی

### Community 22 - "طراحی ارزیابی و کیفیت پیشنهادها"
Cohesion: 0.29
Nodes (6): Feedback کاربر, آستانه اولیه نسخه آزمایشی, تست‌های لازم, طراحی ارزیابی و کیفیت پیشنهادها, مجموعه آزمون, معیارهای اصلی

### Community 23 - "Ponytail, lazy senior dev mode"
Cohesion: 0.40
Nodes (4): Before you write, graphify, Ponytail, lazy senior dev mode, The smallest complete change

### Community 24 - "ponytail-audit/SKILL.md"
Cohesion: 0.40
Nodes (4): 1. Map first, 2. Look for, 3. Check before you report, 4. Output

### Community 25 - "Ponytail Gain"
Cohesion: 0.40
Nodes (4): Boundaries, Honesty boundary, Ponytail Gain, Scoreboard

### Community 26 - "ponytail-review/SKILL.md"
Cohesion: 0.40
Nodes (4): 1. Understand first, 2. Look for, 3. Check before you report, 4. Output

### Community 27 - "Ponytail"
Cohesion: 0.40
Nodes (4): Before you write, Levels, Ponytail, The smallest complete change

### Community 28 - "سیاست نسخه‌گذاری"
Cohesion: 0.40
Nodes (4): سیاست نسخه‌گذاری, منبع نسخه, نسخه‌های بعدی پیشنهادی, نگاشت تغییر به نسخه

### Community 29 - "RuntimeModel.cs"
Cohesion: 0.48
Nodes (4): DotnetExpertAdvisor.RuntimeModel, IRuntimeModel, RuntimeEstimate, RuntimeModel

### Community 30 - "ponytail-debt/SKILL.md"
Cohesion: 0.50
Nodes (3): Boundaries, Output, Scan

### Community 39 - "LookupStatus"
Cohesion: 0.29
Nodes (7): LookupStatus, Ambiguous, Inaccessible, NotAValue, NotFound, Viable, WrongArity

## Knowledge Gaps
- **78 isolated node(s):** `net10.0`, `Microsoft.NET.Sdk`, `net10.0`, `Microsoft.NET.Sdk`, `net10.0` (+73 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 329 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **8 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `DotnetExpertAdvisor.Shared` connect `Symbol` to `Contracts.cs`, `اجزای سامانه`, `قرارداد LSP و مدل پاسخ پیشنهادها`, `قراردادهای داخلی سامانه`, `RuntimeModel.cs`?**
  _High betweenness centrality (0.134) - this node is a cross-community bridge._
- **What connects `net10.0`, `Microsoft.NET.Sdk`, `net10.0` to the rest of the system?**
  _78 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Contracts.cs` be split into smaller, more focused modules?**
  _Cohesion score 0.1036734693877551 - nodes in this community are weakly interconnected._
- **Why does `RoslynHost` connect `اجزای سامانه` to `Contracts.cs`?**
  _High betweenness centrality (0.071) - this node is a cross-community bridge._
- **Should `Shared.csproj` be split into smaller, more focused modules?**
  _Cohesion score 0.06984126984126984 - nodes in this community are weakly interconnected._
- **Why does `اجزای سامانه` connect `اجزای سامانه` to `معماری سامانه هوشمندی C# و .NET برای VS Code`?**
  _High betweenness centrality (0.054) - this node is a cross-community bridge._
- **Should `Symbol` be split into smaller, more focused modules?**
  _Cohesion score 0.07305669199298656 - nodes in this community are weakly interconnected._