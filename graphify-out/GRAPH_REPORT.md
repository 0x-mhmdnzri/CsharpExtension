# Graph Report - CsharpVscode  (2026-10-13)

## Corpus Check
- 89 files · ~24,306 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 5 file(s) not represented in the graph (top: (none) 3, .props 1, .Config 1)

## Summary
- 906 nodes · 1517 edges · 56 communities (42 shown, 14 thin omitted)
- Extraction: 95% EXTRACTED · 5% INFERRED · 0% AMBIGUOUS · INFERRED: 83 edges (avg confidence: 0.87)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `ec1a66a1`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- DotnetExpertAdvisor.Shared
- Shared.csproj
- Contracts.cs
- TypeSymbol
- Binder
- معماری سامانه هوشمندی C# و .NET برای VS Code
- adr/README.md
- LanguageFeature
- تصمیم‌های معماری
- قراردادهای داخلی سامانه
- NuGetProjectIndex.cs
- EditorFeatures.cs
- LexerDiagnostic
- AssemblyMetadataReader.cs
- system_collections_immutable
- TypeProvider
- backlog اصلی
- LanguageVersion
- extension.js
- ConversionKind
- SymbolKind
- package.json
- طراحی شبیه‌ساز EF Core و Translatability
- .Tokenize
- Product Backlog
- .Analyze
- TokenKind
- .Read
- طراحی Rule Engine
- MetadataReader
- ماتریس پوشش C# و CLR
- مشارکت و Git Flow
- Schemaهای داده
- قرارداد LSP و مدل پاسخ پیشنهادها
- Ponytail Help
- طراحی Knowledge Pipeline
- قرارداد دقت پیشنهاد
- طراحی ارزیابی و کیفیت پیشنهادها
- SyntaxKind
- Ponytail, lazy senior dev mode
- ponytail-audit/SKILL.md
- Ponytail Gain
- ponytail-review/SKILL.md
- Ponytail
- سیاست نسخه‌گذاری
- RecoveryKind
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
4. `Symbol` - 30 edges
5. `DotnetExpertAdvisor.Shared` - 29 edges
6. `TypeProvider` - 19 edges
7. `ConversionKind` - 19 edges
8. `CSharpLexer` - 18 edges
9. `SymbolKind` - 18 edges
10. `NamedTypeSymbol` - 16 edges

## Surprising Connections (you probably didn't know these)
- `قرارداد RuleEngine` --references--> `KnowledgeSnapshot`  [INFERRED]
  docs/internal-contracts.md → src/KnowledgeEngine/KnowledgeEngine.cs
- `قرارداد Completion` --references--> `CompletionItem`  [INFERRED]
  docs/lsp-contract.md → src/Protocol/ProtocolModels.cs
- `3. C# Compiler Intelligence Host` --references--> `RoslynHost`  [INFERRED]
  docs/architecture.md → src/RoslynHost/RoslynHost.cs
- `مرزهای ماژول‌ها` --references--> `RoslynHost`  [INFERRED]
  docs/internal-contracts.md → src/RoslynHost/RoslynHost.cs
- `Binder chain` --references--> `LookupResult`  [INFERRED]
  docs/design/binder-overload-resolution.md → src/Shared/Binder.cs

## Import Cycles
- None detected.

## Communities (56 total, 14 thin omitted)

### Community 0 - "DotnetExpertAdvisor.Shared"
Cohesion: 0.06
Nodes (18): DotnetExpertAdvisor.Shared, DotnetExpertAdvisor.RuleEngine, DotnetExpertAdvisor.Protocol, DotnetExpertAdvisor.Architecture.Tests, DotnetExpertAdvisor.RankingEngine, DotnetExpertAdvisor.LanguageServer, StdioLanguageServer, StdioTransport (+10 more)

### Community 1 - "Shared.csproj"
Cohesion: 0.07
Nodes (20): net10.0, Microsoft.NET.Sdk, net10.0, Microsoft.NET.Sdk, net10.0, Microsoft.NET.Sdk, net10.0, Microsoft.NET.Sdk (+12 more)

### Community 2 - "Contracts.cs"
Cohesion: 0.07
Nodes (38): DotnetExpertAdvisor.SemanticGraph, DotnetExpertAdvisor.RuntimeModel, CompletionItem, CompletionRequest, CompletionResponse, ExplainSuggestionRequest, InitializeResponse, IRankingEngine (+30 more)

### Community 3 - "TypeSymbol"
Cohesion: 0.05
Nodes (61): Symbols, ConversionCacheKey, ConversionClassifier, CachedConversionCount, ConversionContext, ConversionQuality, Best, Good (+53 more)

### Community 4 - "Binder"
Cohesion: 0.05
Nodes (41): Best-effort و speculative binding, Better function member, Binder chain, Candidate set, Conversion Classifier, Generic Inference, Member Lookup, Nullable flow (+33 more)

### Community 5 - "معماری سامانه هوشمندی C# و .NET برای VS Code"
Cohesion: 0.05
Nodes (37): DotnetExpertAdvisor.RoslynHost, 1. VS Code Client, 2. Custom C# Language Server, 3. C# Compiler Intelligence Host, 4. Project Semantic Graph, 5. Runtime and Memory Model, 6. Rule Engine, 7. Knowledge Engine (+29 more)

### Community 6 - "adr/README.md"
Cohesion: 0.06
Nodes (26): ADR-001: semantic engine اختصاصی, Consequences, Decision, ADR-002: language server جدا از VS Code client, Consequences, Decision, ADR-003: knowledge pack امضاشده, Consequences (+18 more)

### Community 7 - "LanguageFeature"
Cohesion: 0.07
Nodes (29): LanguageFeature, CollectionExpressions, ExtensionMembers, FieldKeyword, FileScopedNamespaces, GenericMath, GlobalUsings, InitOnlySetters (+21 more)

### Community 8 - "تصمیم‌های معماری"
Cohesion: 0.07
Nodes (26): ADR-001: موتور semantic اختصاصی, ADR-002: language server جدا از VS Code client, ADR-003: knowledge pack به‌جای self-modifying code, ADR-004: snapshot و generation برای consistency, ADR-005: precision مقدم بر recall در ruleهای فعال, ADR-006: عدم وابستگی خارجی در runtime افزونه, ADR-007: پشتیبانی C# 6 تا 15 با capability matrix, ADR-008: NuGet و EF به‌عنوان بخشی از semantic context (+18 more)

### Community 9 - "قراردادهای داخلی سامانه"
Cohesion: 0.10
Nodes (19): DotnetExpertAdvisor.KnowledgeEngine, DocumentContext, Evidence, observability, SemanticContext, SuggestionCandidate, جریان درخواست completion, خطا و cancellation (+11 more)

### Community 10 - "NuGetProjectIndex.cs"
Cohesion: 0.14
Nodes (10): NuGetApiCache, NuGetApiIndex, NuGetAsset, NuGetCachedAssembly, NuGetPackageApi, NuGetPackageReference, NuGetProjectIndex, NuGetProjectIndexReader (+2 more)

### Community 11 - "EditorFeatures.cs"
Cohesion: 0.16
Nodes (15): MetadataMember, SuggestionSeverity, Error, Hint, Info, Warning, CodeActionInfo, DefinitionLocation (+7 more)

### Community 12 - "LexerDiagnostic"
Cohesion: 0.27
Nodes (9): LexerDiagnostic, SyntaxToken, TextSpan, End, CSharpParser, ParseResult, Recoveries, RecoveryEvent (+1 more)

### Community 13 - "AssemblyMetadataReader.cs"
Cohesion: 0.18
Nodes (5): AssemblyMetadataIndex, MetadataAssemblyReference, MetadataGenericParameter, MetadataType, MetadataTypeForwarder

### Community 16 - "backlog اصلی"
Cohesion: 0.11
Nodes (17): backlog اصلی, milestone دقت‌محور Binder, milestone شبیه‌ساز EF و translatability, تعریف انجام‌شدن, دفتر کار پروژه, فاز 1: قرارداد و هسته زبان, فاز 2: semantic engine اختصاصی, فاز 3: metadata، assembly و NuGet (+9 more)

### Community 17 - "LanguageVersion"
Cohesion: 0.12
Nodes (12): LanguageCapabilities, LanguageVersion, CSharp10, CSharp11, CSharp12, CSharp13, CSharp14, CSharp15 (+4 more)

### Community 18 - "extension.js"
Cohesion: 0.15
Nodes (10): activate(), cp, fs, handleOutput(), inputBuffer, path, pending, request() (+2 more)

### Community 19 - "ConversionKind"
Cohesion: 0.12
Nodes (17): ConversionKind, AnonymousFunction, Boxing, CollectionExpression, ExplicitNumeric, Identity, ImplicitNullable, ImplicitNumeric (+9 more)

### Community 20 - "SymbolKind"
Cohesion: 0.12
Nodes (16): SymbolKind, ArrayType, DynamicType, ErrorType, Event, Field, FunctionPointerType, Local (+8 more)

### Community 21 - "package.json"
Cohesion: 0.12
Nodes (15): activationEvents, categories, contributes, commands, description, displayName, engines, vscode (+7 more)

### Community 22 - "طراحی شبیه‌ساز EF Core و Translatability"
Cohesion: 0.14
Nodes (13): cross-checkهای اختیاری, gate پیشنهادی, provenance و confidence, Query provenance, Translation verdict, اصل, ترتیب بازسازی مدل, سطح اول: تحلیل ایستا (+5 more)

### Community 24 - "Product Backlog"
Cohesion: 0.15
Nodes (12): Backlog تفصیلی, Compiler core, Definition of Done, Definition of Ready, Editor و کیفیت, EF و rule knowledge, Metadata و project index, Product Backlog (+4 more)

### Community 25 - ".Analyze"
Cohesion: 0.35
Nodes (4): EfDbContextModel, EfEntityModel, EfModelAnalyzer, EfModelSnapshot

### Community 26 - "TokenKind"
Cohesion: 0.15
Nodes (13): TokenKind, Bad, Character, Comment, Directive, EndOfFile, Identifier, Keyword (+5 more)

### Community 28 - "طراحی Rule Engine"
Cohesion: 0.18
Nodes (10): Correctness, Design, Performance, Schema پیشنهادی Rule, اصل اصلی, انواع Rule, سطوح اطمینان, طراحی Rule Engine (+2 more)

### Community 30 - "ماتریس پوشش C# و CLR"
Cohesion: 0.20
Nodes (9): 1. اجرای CLR، assembly و deployment, 2. مدل انواع و dispatch, 3. انواع ضروری و امکانات زبان, 4. exception، memory و resource lifetime, 5. hosting، loading، reflection و serialization, 6. threading و asynchronous execution, استفاده در پیشنهادها, قرارداد پوشش (+1 more)

### Community 31 - "مشارکت و Git Flow"
Cohesion: 0.22
Nodes (8): جریان feature, جریان hotfix, جریان release, شاخه‌ها, قواعد تغییر, مشارکت و Git Flow, نام‌گذاری commit, نسخه‌گذاری

### Community 32 - "Schemaهای داده"
Cohesion: 0.22
Nodes (8): Compatibility, GraphEdge, GraphNode, KnowledgeReference, RuleDescriptor, Schemaهای داده, Settings, شناسه‌ها

### Community 33 - "قرارداد LSP و مدل پاسخ پیشنهادها"
Cohesion: 0.22
Nodes (8): `runtime/explainSuggestion`, زمان‌بندی و سطح تحلیل, قابلیت اختصاصی, قابلیت‌های پایه, قرارداد Completion, قرارداد LSP و مدل پاسخ پیشنهادها, هدف, چرخه عمر

### Community 34 - "Ponytail Help"
Cohesion: 0.25
Nodes (7): Configure Default Mode, Deactivate, Levels, More, Ponytail Help, Skills, Update

### Community 35 - "طراحی Knowledge Pipeline"
Cohesion: 0.25
Nodes (7): Blog crawler, Manifest, تولید Rule, شناسه و نسخه, طراحی Knowledge Pipeline, مراحل ingestion, منابع

### Community 36 - "قرارداد دقت پیشنهاد"
Cohesion: 0.29
Nodes (6): Quality gates, SuggestionContract, SuggestionEvidence, قرارداد دقت پیشنهاد, قواعد اجباری, منابع دانشی

### Community 37 - "طراحی ارزیابی و کیفیت پیشنهادها"
Cohesion: 0.29
Nodes (6): Feedback کاربر, آستانه اولیه نسخه آزمایشی, تست‌های لازم, طراحی ارزیابی و کیفیت پیشنهادها, مجموعه آزمون, معیارهای اصلی

### Community 38 - "SyntaxKind"
Cohesion: 0.29
Nodes (7): SyntaxKind, Block, CompilationUnit, Error, NamespaceDeclaration, Token, TypeDeclaration

### Community 39 - "Ponytail, lazy senior dev mode"
Cohesion: 0.40
Nodes (4): Before you write, graphify, Ponytail, lazy senior dev mode, The smallest complete change

### Community 40 - "ponytail-audit/SKILL.md"
Cohesion: 0.40
Nodes (4): 1. Map first, 2. Look for, 3. Check before you report, 4. Output

### Community 41 - "Ponytail Gain"
Cohesion: 0.40
Nodes (4): Boundaries, Honesty boundary, Ponytail Gain, Scoreboard

### Community 42 - "ponytail-review/SKILL.md"
Cohesion: 0.40
Nodes (4): 1. Understand first, 2. Look for, 3. Check before you report, 4. Output

### Community 43 - "Ponytail"
Cohesion: 0.40
Nodes (4): Before you write, Levels, Ponytail, The smallest complete change

### Community 44 - "سیاست نسخه‌گذاری"
Cohesion: 0.40
Nodes (4): سیاست نسخه‌گذاری, منبع نسخه, نسخه‌های بعدی پیشنهادی, نگاشت تغییر به نسخه

### Community 45 - "RecoveryKind"
Cohesion: 0.40
Nodes (5): RecoveryKind, IncompleteDeclaration, MissingName, UnclosedDelimiter, UnexpectedDelimiter

### Community 46 - "ponytail-debt/SKILL.md"
Cohesion: 0.50
Nodes (3): Boundaries, Output, Scan

## Knowledge Gaps
- **168 isolated node(s):** `net10.0`, `Microsoft.NET.Sdk`, `net10.0`, `Microsoft.NET.Sdk`, `net10.0` (+163 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 475 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **14 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `DotnetExpertAdvisor.Shared` connect `DotnetExpertAdvisor.Shared` to `Contracts.cs`, `TypeSymbol`, `Binder`, `معماری سامانه هوشمندی C# و .NET برای VS Code`, `قراردادهای داخلی سامانه`, `NuGetProjectIndex.cs`, `EditorFeatures.cs`, `LexerDiagnostic`, `AssemblyMetadataReader.cs`, `system_collections_immutable`, `.Analyze`?**
  _High betweenness centrality (0.209) - this node is a cross-community bridge._
- **Are the 3 inferred relationships involving `ConversionClassifier` (e.g. with `MemberLookup` and `.ValidateConversionClassifier()`) actually correct?**
  _`ConversionClassifier` has 3 INFERRED edges - model-reasoned connections that need verification._
- **What connects `net10.0`, `Microsoft.NET.Sdk`, `net10.0` to the rest of the system?**
  _168 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `DotnetExpertAdvisor.Shared` be split into smaller, more focused modules?**
  _Cohesion score 0.06448202959830866 - nodes in this community are weakly interconnected._
- **Why does `RoslynHost` connect `معماری سامانه هوشمندی C# و .NET برای VS Code` to `DotnetExpertAdvisor.Shared`, `Contracts.cs`?**
  _High betweenness centrality (0.061) - this node is a cross-community bridge._
- **Should `Shared.csproj` be split into smaller, more focused modules?**
  _Cohesion score 0.07301587301587302 - nodes in this community are weakly interconnected._
- **Should `Contracts.cs` be split into smaller, more focused modules?**
  _Cohesion score 0.07451923076923077 - nodes in this community are weakly interconnected._