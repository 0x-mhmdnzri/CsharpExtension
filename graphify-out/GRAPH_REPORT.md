# Graph Report - CsharpVscode  (2026-10-13)

## Corpus Check
- 97 files · ~25,303 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 5 file(s) not represented in the graph (top: (none) 3, .props 1, .Config 1)

## Summary
- 949 nodes · 1601 edges · 61 communities (49 shown, 12 thin omitted)
- Extraction: 94% EXTRACTED · 6% INFERRED · 0% AMBIGUOUS · INFERRED: 90 edges (avg confidence: 0.87)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `6fa0bfde`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- .ReadAsync
- Shared.csproj
- Contracts.cs
- ConversionContext
- DotnetExpertAdvisor.Shared
- adr/README.md
- TypeSymbol
- معماری سامانه هوشمندی C# و .NET برای VS Code
- Binder
- TypeProvider
- LanguageFeature
- ConversionClassifier
- تصمیم‌های معماری
- .Read
- system_collections_immutable
- قراردادهای داخلی سامانه
- EditorFeatures.cs
- طراحی دقت‌محور Binder و Overload Resolution
- extension.js
- backlog ÃƒËœÃ‚Â§ÃƒËœÃ‚ÂµÃƒâ„¢Ã¢â‚¬Å¾Ãƒâ€ºÃ…â€™
- ContractSmokeTests
- LanguageVersion
- ConversionKind
- LexerDiagnostic
- SymbolKind
- package.json
- .Tokenize
- .Analyze
- طراحی شبیه‌ساز EF Core و Translatability
- Product Backlog
- TokenKind
- SyntaxKind
- طراحی Rule Engine
- ماتریس پوشش C# و CLR
- مشارکت و Git Flow
- Schemaهای داده
- قرارداد LSP و مدل پاسخ پیشنهادها
- Ponytail Help
- طراحی Knowledge Pipeline
- TypeKind
- قرارداد دقت پیشنهاد
- طراحی ارزیابی و کیفیت پیشنهادها
- ConversionQuality
- Ponytail, lazy senior dev mode
- ponytail-audit/SKILL.md
- Ponytail Gain
- ponytail-review/SKILL.md
- Ponytail
- سیاست نسخه‌گذاری
- RecoveryKind
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
1. `TypeSymbol` - 40 edges
2. `DotnetExpertAdvisor.Shared` - 37 edges
3. `ConversionClassifier` - 33 edges
4. `LanguageFeature` - 33 edges
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
- `قرارداد Completion` --references--> `CompletionItem`  [INFERRED]
  docs/lsp-contract.md → src/Protocol/ProtocolModels.cs
- `3. C# Compiler Intelligence Host` --references--> `RoslynHost`  [INFERRED]
  docs/architecture.md → src/RoslynHost/RoslynHost.cs
- `مرزهای ماژول‌ها` --references--> `RoslynHost`  [INFERRED]
  docs/internal-contracts.md → src/RoslynHost/RoslynHost.cs

## Import Cycles
- None detected.

## Communities (61 total, 12 thin omitted)

### Community 0 - ".ReadAsync"
Cohesion: 0.17
Nodes (5): DotnetExpertAdvisor.Protocol, DotnetExpertAdvisor.LanguageServer, StdioLanguageServer, StdioTransport, StdioTransportTests

### Community 1 - "Shared.csproj"
Cohesion: 0.07
Nodes (20): net10.0, Microsoft.NET.Sdk, net10.0, Microsoft.NET.Sdk, net10.0, Microsoft.NET.Sdk, net10.0, Microsoft.NET.Sdk (+12 more)

### Community 2 - "Contracts.cs"
Cohesion: 0.06
Nodes (45): DotnetExpertAdvisor.RuleEngine, DotnetExpertAdvisor.RoslynHost, DotnetExpertAdvisor.RuntimeModel, 3. C# Compiler Intelligence Host, مرزهای ماژول‌ها, CompletionItem, CompletionRequest, CompletionResponse (+37 more)

### Community 3 - "ConversionContext"
Cohesion: 0.36
Nodes (3): ConversionCacheKey, ConversionContext, IConversionClassifier

### Community 4 - "DotnetExpertAdvisor.Shared"
Cohesion: 0.07
Nodes (19): DotnetExpertAdvisor.Shared, DotnetExpertAdvisor.Architecture.Tests, DotnetExpertAdvisor.SemanticGraph, DotnetExpertAdvisor.RankingEngine, EfPerformanceAnalyzer, EfPerformanceFinding, EfRiskSeverity, Info (+11 more)

### Community 5 - "adr/README.md"
Cohesion: 0.06
Nodes (26): ADR-001: semantic engine اختصاصی, Consequences, Decision, ADR-002: language server جدا از VS Code client, Consequences, Decision, ADR-003: knowledge pack امضاشده, Consequences (+18 more)

### Community 6 - "TypeSymbol"
Cohesion: 0.17
Nodes (30): Symbols, Accessibility, Internal, NotApplicable, Private, PrivateProtected, Protected, ProtectedInternal (+22 more)

### Community 7 - "معماری سامانه هوشمندی C# و .NET برای VS Code"
Cohesion: 0.06
Nodes (32): 1. VS Code Client, 2. Custom C# Language Server, 4. Project Semantic Graph, 5. Runtime and Memory Model, 6. Rule Engine, 7. Knowledge Engine, 8. Ranking Engine, pipeline به‌روزرسانی Microsoft Blog (+24 more)

### Community 8 - "Binder"
Cohesion: 0.08
Nodes (24): Binder, Kind, Parent, BinderScopeKind, Block, Global, Imports, Lambda (+16 more)

### Community 10 - "LanguageFeature"
Cohesion: 0.07
Nodes (29): LanguageFeature, CollectionExpressions, ExtensionMembers, FieldKeyword, FileScopedNamespaces, GenericMath, GlobalUsings, InitOnlySetters (+21 more)

### Community 11 - "ConversionClassifier"
Cohesion: 0.17
Nodes (4): ConversionClassifier, CachedConversionCount, ConversionResult, Identity

### Community 12 - "تصمیم‌های معماری"
Cohesion: 0.07
Nodes (26): ADR-001: موتور semantic اختصاصی, ADR-002: language server جدا از VS Code client, ADR-003: knowledge pack به‌جای self-modifying code, ADR-004: snapshot و generation برای consistency, ADR-005: precision مقدم بر recall در ruleهای فعال, ADR-006: عدم وابستگی خارجی در runtime افزونه, ADR-007: پشتیبانی C# 6 تا 15 با capability matrix, ADR-008: NuGet و EF به‌عنوان بخشی از semantic context (+18 more)

### Community 13 - ".Read"
Cohesion: 0.06
Nodes (27): AssemblyMetadataIndex, AssemblyMetadataReader, MetadataAssemblyReference, MetadataGenericParameter, MetadataMember, MetadataType, MetadataTypeForwarder, EfProviderInfo (+19 more)

### Community 15 - "قراردادهای داخلی سامانه"
Cohesion: 0.10
Nodes (19): DotnetExpertAdvisor.KnowledgeEngine, DocumentContext, Evidence, observability, SemanticContext, SuggestionCandidate, جریان درخواست completion, خطا و cancellation (+11 more)

### Community 16 - "EditorFeatures.cs"
Cohesion: 0.14
Nodes (15): SuggestionSeverity, Error, Hint, Info, Warning, CodeActionInfo, DefinitionLocation, DiagnosticInfo (+7 more)

### Community 17 - "طراحی دقت‌محور Binder و Overload Resolution"
Cohesion: 0.11
Nodes (17): Best-effort و speculative binding, Better function member, Binder chain, Candidate set, Conversion Classifier, Generic Inference, Member Lookup, Nullable flow (+9 more)

### Community 18 - "extension.js"
Cohesion: 0.15
Nodes (10): activate(), cp, fs, handleOutput(), inputBuffer, path, pending, request() (+2 more)

### Community 19 - "backlog ÃƒËœÃ‚Â§ÃƒËœÃ‚ÂµÃƒâ„¢Ã¢â‚¬Å¾Ãƒâ€ºÃ…â€™"
Cohesion: 0.11
Nodes (17): backlog ÃƒËœÃ‚Â§ÃƒËœÃ‚ÂµÃƒâ„¢Ã¢â‚¬Å¾Ãƒâ€ºÃ…â€™, milestone ÃƒËœÃ‚Â¯Ãƒâ„¢Ã¢â‚¬Å¡ÃƒËœÃ‚ÂªÃƒÂ¢Ã¢â€šÂ¬Ã…â€™Ãƒâ„¢Ã¢â‚¬Â¦ÃƒËœÃ‚Â­Ãƒâ„¢Ã‹â€ ÃƒËœÃ‚Â± Binder, milestone ÃƒËœÃ‚Â´ÃƒËœÃ‚Â¨Ãƒâ€ºÃ…â€™Ãƒâ„¢Ã¢â‚¬Â¡ÃƒÂ¢Ã¢â€šÂ¬Ã…â€™ÃƒËœÃ‚Â³ÃƒËœÃ‚Â§ÃƒËœÃ‚Â² EF Ãƒâ„¢Ã‹â€  translatability, Ãƒâ„¢Ã¢â‚¬Â¦ÃƒËœÃ‚Â³Ãƒâ€ºÃ…â€™ÃƒËœÃ‚Â± ÃƒËœÃ‚Â§ÃƒËœÃ‚Â¬ÃƒËœÃ‚Â±ÃƒËœÃ‚Â§Ãƒâ€ºÃ…â€™ ÃƒËœÃ‚Â¨ÃƒËœÃ‚Â¹ÃƒËœÃ‚Â¯Ãƒâ€ºÃ…â€™, Ãƒâ„¢Ã¢â‚¬Â¦ÃƒËœÃ‚Â§ÃƒËœÃ‚ÂªÃƒËœÃ‚Â±Ãƒâ€ºÃ…â€™ÃƒËœÃ‚Â³ ÃƒËœÃ‚Â¯ÃƒËœÃ‚Â§Ãƒâ„¢Ã¢â‚¬Â ÃƒËœÃ‚Â´ Ãƒâ„¢Ã‹â€  rule coverage, Ãƒâ„¢Ã‹â€ ÃƒËœÃ‚Â¶ÃƒËœÃ‚Â¹Ãƒâ€ºÃ…â€™ÃƒËœÃ‚ÂªÃƒÂ¢Ã¢â€šÂ¬Ã…â€™Ãƒâ„¢Ã¢â‚¬Â¡ÃƒËœÃ‚Â§Ãƒâ€ºÃ…â€™ Ãƒâ„¢Ã¢â‚¬Â¦ÃƒËœÃ‚Â¬ÃƒËœÃ‚Â§ÃƒËœÃ‚Â², Ãƒâ„¢Ã‚ÂÃƒËœÃ‚Â§ÃƒËœÃ‚Â² 1: Ãƒâ„¢Ã¢â‚¬Å¡ÃƒËœÃ‚Â±ÃƒËœÃ‚Â§ÃƒËœÃ‚Â±ÃƒËœÃ‚Â¯ÃƒËœÃ‚Â§ÃƒËœÃ‚Â¯ Ãƒâ„¢Ã‹â€  Ãƒâ„¢Ã¢â‚¬Â¡ÃƒËœÃ‚Â³ÃƒËœÃ‚ÂªÃƒâ„¢Ã¢â‚¬Â¡ ÃƒËœÃ‚Â²ÃƒËœÃ‚Â¨ÃƒËœÃ‚Â§Ãƒâ„¢Ã¢â‚¬Â, Ãƒâ„¢Ã‚ÂÃƒËœÃ‚Â§ÃƒËœÃ‚Â² 2: semantic engine ÃƒËœÃ‚Â§ÃƒËœÃ‚Â®ÃƒËœÃ‚ÂªÃƒËœÃ‚ÂµÃƒËœÃ‚Â§ÃƒËœÃ‚ÂµÃƒâ€ºÃ…â€™ (+9 more)

### Community 20 - "ContractSmokeTests"
Cohesion: 0.18
Nodes (6): MemberLookup, MemberLookupOptions, MemberLookupResult, IsSuccess, SymbolFactory, ContractSmokeTests

### Community 21 - "LanguageVersion"
Cohesion: 0.13
Nodes (12): LanguageCapabilities, LanguageVersion, CSharp10, CSharp11, CSharp12, CSharp13, CSharp14, CSharp15 (+4 more)

### Community 22 - "ConversionKind"
Cohesion: 0.12
Nodes (17): ConversionKind, AnonymousFunction, Boxing, CollectionExpression, ExplicitNumeric, Identity, ImplicitNullable, ImplicitNumeric (+9 more)

### Community 23 - "LexerDiagnostic"
Cohesion: 0.35
Nodes (6): LexerDiagnostic, SyntaxToken, TextSpan, End, CSharpParser, RecoveryEvent

### Community 24 - "SymbolKind"
Cohesion: 0.12
Nodes (16): SymbolKind, ArrayType, DynamicType, ErrorType, Event, Field, FunctionPointerType, Local (+8 more)

### Community 25 - "package.json"
Cohesion: 0.12
Nodes (15): activationEvents, categories, contributes, commands, description, displayName, engines, vscode (+7 more)

### Community 27 - ".Analyze"
Cohesion: 0.18
Nodes (10): EfLinqAnalyzer, EfQueryAnalysis, EfTranslationStatus, ClientEvaluationRisk, Translatable, Unknown, EfDbContextModel, EfEntityModel (+2 more)

### Community 28 - "طراحی شبیه‌ساز EF Core و Translatability"
Cohesion: 0.14
Nodes (13): cross-checkهای اختیاری, gate پیشنهادی, provenance و confidence, Query provenance, Translation verdict, اصل, ترتیب بازسازی مدل, سطح اول: تحلیل ایستا (+5 more)

### Community 29 - "Product Backlog"
Cohesion: 0.15
Nodes (12): Backlog ÃƒËœÃ‚ÂªÃƒâ„¢Ã‚ÂÃƒËœÃ‚ÂµÃƒâ€ºÃ…â€™Ãƒâ„¢Ã¢â‚¬Å¾Ãƒâ€ºÃ…â€™, Compiler core, Definition of Done, Definition of Ready, Editor Ãƒâ„¢Ã‹â€  ÃƒÅ¡Ã‚Â©Ãƒâ€ºÃ…â€™Ãƒâ„¢Ã‚ÂÃƒâ€ºÃ…â€™ÃƒËœÃ‚Âª, EF Ãƒâ„¢Ã‹â€  rule knowledge, Metadata Ãƒâ„¢Ã‹â€  project index, Product Backlog (+4 more)

### Community 30 - "TokenKind"
Cohesion: 0.15
Nodes (13): TokenKind, Bad, Character, Comment, Directive, EndOfFile, Identifier, Keyword (+5 more)

### Community 31 - "SyntaxKind"
Cohesion: 0.19
Nodes (10): ParseResult, Recoveries, SyntaxKind, Block, CompilationUnit, Error, NamespaceDeclaration, Token (+2 more)

### Community 32 - "طراحی Rule Engine"
Cohesion: 0.18
Nodes (10): Correctness, Design, Performance, Schema پیشنهادی Rule, اصل اصلی, انواع Rule, سطوح اطمینان, طراحی Rule Engine (+2 more)

### Community 33 - "ماتریس پوشش C# و CLR"
Cohesion: 0.20
Nodes (9): 1. اجرای CLR، assembly و deployment, 2. مدل انواع و dispatch, 3. انواع ضروری و امکانات زبان, 4. exception، memory و resource lifetime, 5. hosting، loading، reflection و serialization, 6. threading و asynchronous execution, استفاده در پیشنهادها, قرارداد پوشش (+1 more)

### Community 34 - "مشارکت و Git Flow"
Cohesion: 0.22
Nodes (8): جریان feature, جریان hotfix, جریان release, شاخه‌ها, قواعد تغییر, مشارکت و Git Flow, نام‌گذاری commit, نسخه‌گذاری

### Community 35 - "Schemaهای داده"
Cohesion: 0.22
Nodes (8): Compatibility, GraphEdge, GraphNode, KnowledgeReference, RuleDescriptor, Schemaهای داده, Settings, شناسه‌ها

### Community 36 - "قرارداد LSP و مدل پاسخ پیشنهادها"
Cohesion: 0.22
Nodes (8): `runtime/explainSuggestion`, زمان‌بندی و سطح تحلیل, قابلیت اختصاصی, قابلیت‌های پایه, قرارداد Completion, قرارداد LSP و مدل پاسخ پیشنهادها, هدف, چرخه عمر

### Community 37 - "Ponytail Help"
Cohesion: 0.25
Nodes (7): Configure Default Mode, Deactivate, Levels, More, Ponytail Help, Skills, Update

### Community 38 - "طراحی Knowledge Pipeline"
Cohesion: 0.25
Nodes (7): Blog crawler, Manifest, تولید Rule, شناسه و نسخه, طراحی Knowledge Pipeline, مراحل ingestion, منابع

### Community 39 - "TypeKind"
Cohesion: 0.25
Nodes (8): TypeKind, Class, Delegate, Enum, Error, Interface, Record, Struct

### Community 40 - "قرارداد دقت پیشنهاد"
Cohesion: 0.29
Nodes (6): Quality gates, SuggestionContract, SuggestionEvidence, قرارداد دقت پیشنهاد, قواعد اجباری, منابع دانشی

### Community 41 - "طراحی ارزیابی و کیفیت پیشنهادها"
Cohesion: 0.29
Nodes (6): Feedback کاربر, آستانه اولیه نسخه آزمایشی, تست‌های لازم, طراحی ارزیابی و کیفیت پیشنهادها, مجموعه آزمون, معیارهای اصلی

### Community 42 - "ConversionQuality"
Cohesion: 0.33
Nodes (6): ConversionQuality, Best, Good, Invalid, RequiresRuntimeCheck, Uncertain

### Community 43 - "Ponytail, lazy senior dev mode"
Cohesion: 0.40
Nodes (4): Before you write, graphify, Ponytail, lazy senior dev mode, The smallest complete change

### Community 44 - "ponytail-audit/SKILL.md"
Cohesion: 0.40
Nodes (4): 1. Map first, 2. Look for, 3. Check before you report, 4. Output

### Community 45 - "Ponytail Gain"
Cohesion: 0.40
Nodes (4): Boundaries, Honesty boundary, Ponytail Gain, Scoreboard

### Community 46 - "ponytail-review/SKILL.md"
Cohesion: 0.40
Nodes (4): 1. Understand first, 2. Look for, 3. Check before you report, 4. Output

### Community 47 - "Ponytail"
Cohesion: 0.40
Nodes (4): Before you write, Levels, Ponytail, The smallest complete change

### Community 48 - "سیاست نسخه‌گذاری"
Cohesion: 0.40
Nodes (4): سیاست نسخه‌گذاری, منبع نسخه, نسخه‌های بعدی پیشنهادی, نگاشت تغییر به نسخه

### Community 49 - "RecoveryKind"
Cohesion: 0.40
Nodes (5): RecoveryKind, IncompleteDeclaration, MissingName, UnclosedDelimiter, UnexpectedDelimiter

### Community 50 - "ponytail-debt/SKILL.md"
Cohesion: 0.50
Nodes (3): Boundaries, Output, Scan

### Community 51 - "VarianceKind"
Cohesion: 0.50
Nodes (4): VarianceKind, Contravariant, Covariant, Invariant

## Knowledge Gaps
- **176 isolated node(s):** `Concepts`, `Claims`, `Rules`, `net10.0`, `Microsoft.NET.Sdk` (+171 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 482 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **12 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `DotnetExpertAdvisor.Shared` connect `DotnetExpertAdvisor.Shared` to `.ReadAsync`, `Contracts.cs`, `ConversionContext`, `TypeSymbol`, `Binder`, `.Read`, `system_collections_immutable`, `قراردادهای داخلی سامانه`, `EditorFeatures.cs`, `ContractSmokeTests`, `.Analyze`, `SyntaxKind`?**
  _High betweenness centrality (0.228) - this node is a cross-community bridge._
- **Are the 3 inferred relationships involving `ConversionClassifier` (e.g. with `MemberLookup` and `.ValidateConversionClassifier()`) actually correct?**
  _`ConversionClassifier` has 3 INFERRED edges - model-reasoned connections that need verification._
- **What connects `Concepts`, `Claims`, `Rules` to the rest of the system?**
  _176 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Shared.csproj` be split into smaller, more focused modules?**
  _Cohesion score 0.07301587301587302 - nodes in this community are weakly interconnected._
- **Why does `3. C# Compiler Intelligence Host` connect `Contracts.cs` to `معماری سامانه هوشمندی C# و .NET برای VS Code`?**
  _High betweenness centrality (0.045) - this node is a cross-community bridge._
- **Should `Contracts.cs` be split into smaller, more focused modules?**
  _Cohesion score 0.06368011847463902 - nodes in this community are weakly interconnected._
- **Should `DotnetExpertAdvisor.Shared` be split into smaller, more focused modules?**
  _Cohesion score 0.06882591093117409 - nodes in this community are weakly interconnected._