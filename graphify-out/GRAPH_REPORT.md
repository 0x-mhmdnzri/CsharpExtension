# Graph Report - CsharpVscode  (2026-10-12)

## Corpus Check
- 52 files · ~12,467 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 4 file(s) not represented in the graph (top: (none) 2, .props 1, .Config 1)

## Summary
- 376 nodes · 473 edges · 35 communities (27 shown, 8 thin omitted)
- Extraction: 95% EXTRACTED · 5% INFERRED · 0% AMBIGUOUS · INFERRED: 22 edges (avg confidence: 0.87)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `3e256092`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- Contracts.cs
- Shared.csproj
- IReadOnlyList
- تصمیم‌های معماری
- معماری سامانه هوشمندی C# و .NET برای VS Code
- DotnetExpertAdvisor.Shared
- طراحی دقت‌محور Binder و Overload Resolution
- backlog اصلی
- قراردادهای داخلی سامانه
- اجزای سامانه
- طراحی Rule Engine
- KnowledgeEngine
- ماتریس پوشش C# و CLR
- مشارکت و Git Flow
- Schemaهای داده
- Ponytail Help
- طراحی Knowledge Pipeline
- RuntimeModel.cs
- قرارداد دقت پیشنهاد
- طراحی ارزیابی و کیفیت پیشنهادها
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
1. `معماری سامانه هوشمندی C# و .NET برای VS Code` - 15 edges
2. `DocumentAnalysis` - 13 edges
3. `GraphSnapshot` - 12 edges
4. `SuggestionCandidate` - 11 edges
5. `طراحی دقت‌محور Binder و Overload Resolution` - 11 edges
6. `قراردادهای داخلی سامانه` - 11 edges
7. `backlog اصلی` - 10 edges
8. `DocumentRequest` - 10 edges
9. `ماتریس پوشش C# و CLR` - 9 edges
10. `DotnetExpertAdvisor.Shared` - 9 edges

## Surprising Connections (you probably didn't know these)
- `قرارداد RuleEngine` --references--> `KnowledgeSnapshot`  [INFERRED]
  docs/internal-contracts.md → src/KnowledgeEngine/KnowledgeEngine.cs
- `3. C# Compiler Intelligence Host` --references--> `RoslynHost`  [INFERRED]
  docs/architecture.md → src/RoslynHost/RoslynHost.cs
- `مرزهای ماژول‌ها` --references--> `RoslynHost`  [INFERRED]
  docs/internal-contracts.md → src/RoslynHost/RoslynHost.cs
- `قرارداد Completion` --references--> `CompletionItem`  [INFERRED]
  docs/lsp-contract.md → src/Protocol/ProtocolModels.cs
- `CompletionRequest` --references--> `DocumentRequest`  [EXTRACTED]
  src/Protocol/ProtocolModels.cs → src/Shared/Contracts.cs

## Import Cycles
- None detected.

## Communities (35 total, 8 thin omitted)

### Community 0 - "Contracts.cs"
Cohesion: 0.18
Nodes (17): DotnetExpertAdvisor.SemanticGraph, ISemanticGraph, SemanticGraph, DocumentAnalysis, DocumentId, DocumentRequest, GraphEdge, GraphNode (+9 more)

### Community 1 - "Shared.csproj"
Cohesion: 0.07
Nodes (20): net10.0, Microsoft.NET.Sdk, net10.0, Microsoft.NET.Sdk, net10.0, Microsoft.NET.Sdk, net10.0, Microsoft.NET.Sdk (+12 more)

### Community 2 - "IReadOnlyList"
Cohesion: 0.11
Nodes (19): DotnetExpertAdvisor.RuleEngine, DotnetExpertAdvisor.RankingEngine, IRankingEngine, RankingEngine, IRuleEngine, RuleEngine, Evidence, RankedSuggestion (+11 more)

### Community 3 - "تصمیم‌های معماری"
Cohesion: 0.07
Nodes (26): ADR-001: موتور semantic اختصاصی, ADR-002: language server جدا از VS Code client, ADR-003: knowledge pack به‌جای self-modifying code, ADR-004: snapshot و generation برای consistency, ADR-005: precision مقدم بر recall در ruleهای فعال, ADR-006: عدم وابستگی خارجی در runtime افزونه, ADR-007: پشتیبانی C# 6 تا 15 با capability matrix, ADR-008: NuGet و EF به‌عنوان بخشی از semantic context (+18 more)

### Community 4 - "معماری سامانه هوشمندی C# و .NET برای VS Code"
Cohesion: 0.08
Nodes (24): pipeline به‌روزرسانی Microsoft Blog, امنیت و حریم خصوصی, تصمیم معماری اصلی, تصمیم پیشنهادی فعلی, دامنه قابلیت‌ها, دانش و به‌روزرسانی, ساختار repository پیشنهادی, قابلیت‌های CLR و حافظه (+16 more)

### Community 5 - "DotnetExpertAdvisor.Shared"
Cohesion: 0.11
Nodes (17): DotnetExpertAdvisor.Shared, DotnetExpertAdvisor.Protocol, DotnetExpertAdvisor.Architecture.Tests, `runtime/explainSuggestion`, زمان‌بندی و سطح تحلیل, قابلیت اختصاصی, قابلیت‌های پایه, قرارداد Completion (+9 more)

### Community 6 - "طراحی دقت‌محور Binder و Overload Resolution"
Cohesion: 0.11
Nodes (18): Best-effort و speculative binding, Better function member, Binder chain, Candidate set, Conversion Classifier, Generic Inference, Member Lookup, Nullable flow (+10 more)

### Community 7 - "backlog اصلی"
Cohesion: 0.12
Nodes (16): backlog اصلی, milestone دقت‌محور Binder, تعریف انجام‌شدن, دفتر کار پروژه, فاز 1: قرارداد و هسته زبان, فاز 2: semantic engine اختصاصی, فاز 3: metadata، assembly و NuGet, فاز 4: IntelliSense و VS Code (+8 more)

### Community 8 - "قراردادهای داخلی سامانه"
Cohesion: 0.13
Nodes (14): DocumentContext, Evidence, observability, SemanticContext, SuggestionCandidate, جریان درخواست completion, خطا و cancellation, قرارداد KnowledgeEngine (+6 more)

### Community 9 - "اجزای سامانه"
Cohesion: 0.15
Nodes (13): DotnetExpertAdvisor.RoslynHost, 1. VS Code Client, 2. Custom C# Language Server, 3. C# Compiler Intelligence Host, 4. Project Semantic Graph, 5. Runtime and Memory Model, 6. Rule Engine, 7. Knowledge Engine (+5 more)

### Community 10 - "طراحی Rule Engine"
Cohesion: 0.18
Nodes (10): Correctness, Design, Performance, Schema پیشنهادی Rule, اصل اصلی, انواع Rule, سطوح اطمینان, طراحی Rule Engine (+2 more)

### Community 11 - "KnowledgeEngine"
Cohesion: 0.36
Nodes (5): DotnetExpertAdvisor.KnowledgeEngine, IKnowledgeEngine, KnowledgeEngine, KnowledgeSnapshot, KnowledgeReference

### Community 12 - "ماتریس پوشش C# و CLR"
Cohesion: 0.20
Nodes (9): 1. اجرای CLR، assembly و deployment, 2. مدل انواع و dispatch, 3. انواع ضروری و امکانات زبان, 4. exception، memory و resource lifetime, 5. hosting، loading، reflection و serialization, 6. threading و asynchronous execution, استفاده در پیشنهادها, قرارداد پوشش (+1 more)

### Community 13 - "مشارکت و Git Flow"
Cohesion: 0.22
Nodes (8): جریان feature, جریان hotfix, جریان release, شاخه‌ها, قواعد تغییر, مشارکت و Git Flow, نام‌گذاری commit, نسخه‌گذاری

### Community 14 - "Schemaهای داده"
Cohesion: 0.22
Nodes (8): Compatibility, GraphEdge, GraphNode, KnowledgeReference, RuleDescriptor, Schemaهای داده, Settings, شناسه‌ها

### Community 15 - "Ponytail Help"
Cohesion: 0.25
Nodes (7): Configure Default Mode, Deactivate, Levels, More, Ponytail Help, Skills, Update

### Community 16 - "طراحی Knowledge Pipeline"
Cohesion: 0.25
Nodes (7): Blog crawler, Manifest, تولید Rule, شناسه و نسخه, طراحی Knowledge Pipeline, مراحل ingestion, منابع

### Community 17 - "RuntimeModel.cs"
Cohesion: 0.48
Nodes (4): DotnetExpertAdvisor.RuntimeModel, IRuntimeModel, RuntimeEstimate, RuntimeModel

### Community 18 - "قرارداد دقت پیشنهاد"
Cohesion: 0.29
Nodes (6): Quality gates, SuggestionContract, SuggestionEvidence, قرارداد دقت پیشنهاد, قواعد اجباری, منابع دانشی

### Community 19 - "طراحی ارزیابی و کیفیت پیشنهادها"
Cohesion: 0.29
Nodes (6): Feedback کاربر, آستانه اولیه نسخه آزمایشی, تست‌های لازم, طراحی ارزیابی و کیفیت پیشنهادها, مجموعه آزمون, معیارهای اصلی

### Community 20 - "Ponytail, lazy senior dev mode"
Cohesion: 0.40
Nodes (4): Before you write, graphify, Ponytail, lazy senior dev mode, The smallest complete change

### Community 21 - "ponytail-audit/SKILL.md"
Cohesion: 0.40
Nodes (4): 1. Map first, 2. Look for, 3. Check before you report, 4. Output

### Community 22 - "Ponytail Gain"
Cohesion: 0.40
Nodes (4): Boundaries, Honesty boundary, Ponytail Gain, Scoreboard

### Community 23 - "ponytail-review/SKILL.md"
Cohesion: 0.40
Nodes (4): 1. Understand first, 2. Look for, 3. Check before you report, 4. Output

### Community 24 - "Ponytail"
Cohesion: 0.40
Nodes (4): Before you write, Levels, Ponytail, The smallest complete change

### Community 25 - "سیاست نسخه‌گذاری"
Cohesion: 0.40
Nodes (4): سیاست نسخه‌گذاری, منبع نسخه, نسخه‌های بعدی پیشنهادی, نگاشت تغییر به نسخه

### Community 26 - "ponytail-debt/SKILL.md"
Cohesion: 0.50
Nodes (3): Boundaries, Output, Scan

## Knowledge Gaps
- **28 isolated node(s):** `net10.0`, `Microsoft.NET.Sdk`, `net10.0`, `Microsoft.NET.Sdk`, `net10.0` (+23 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 238 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **8 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `RoslynHost` connect `اجزای سامانه` to `Contracts.cs`, `IReadOnlyList`?**
  _High betweenness centrality (0.074) - this node is a cross-community bridge._
- **What connects `net10.0`, `Microsoft.NET.Sdk`, `net10.0` to the rest of the system?**
  _28 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Shared.csproj` be split into smaller, more focused modules?**
  _Cohesion score 0.06984126984126984 - nodes in this community are weakly interconnected._
- **Why does `اجزای سامانه` connect `اجزای سامانه` to `معماری سامانه هوشمندی C# و .NET برای VS Code`?**
  _High betweenness centrality (0.056) - this node is a cross-community bridge._
- **Should `IReadOnlyList` be split into smaller, more focused modules?**
  _Cohesion score 0.1111111111111111 - nodes in this community are weakly interconnected._
- **Should `تصمیم‌های معماری` be split into smaller, more focused modules?**
  _Cohesion score 0.07407407407407407 - nodes in this community are weakly interconnected._
- **Should `معماری سامانه هوشمندی C# و .NET برای VS Code` be split into smaller, more focused modules?**
  _Cohesion score 0.08 - nodes in this community are weakly interconnected._