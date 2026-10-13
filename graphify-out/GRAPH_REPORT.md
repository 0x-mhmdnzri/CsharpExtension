# Graph Report - CsharpVscode  (2026-10-13)

## Corpus Check
- 93 files · ~24,623 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 5 file(s) not represented in the graph (top: (none) 3, .props 1, .Config 1)

## Summary
- 929 nodes · 1555 edges · 59 communities (48 shown, 11 thin omitted)
- Extraction: 95% EXTRACTED · 5% INFERRED · 0% AMBIGUOUS · INFERRED: 83 edges (avg confidence: 0.87)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `441b52ec`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- قرارداد LSP و مدل پاسخ پیشنهادها
- Shared.csproj
- Contracts.cs
- NamedTypeSymbol
- معماری سامانه هوشمندی C# و .NET برای VS Code
- adr/README.md
- ImmutableArray
- TypeProvider
- LanguageFeature
- تصمیم‌های معماری
- قراردادهای داخلی سامانه
- EditorFeatures.cs
- TypeSymbol
- .Read
- DotnetExpertAdvisor.Shared
- Binder
- LexerDiagnostic
- طراحی دقت‌محور Binder و Overload Resolution
- extension.js
- backlog Ø§ØµÙ„ÛŒ
- LanguageVersion
- ConversionKind
- SymbolKind
- package.json
- .Tokenize
- طراحی شبیه‌ساز EF Core و Translatability
- MemberLookup
- Product Backlog
- TokenKind
- طراحی Rule Engine
- Parser.cs
- ماتریس پوشش C# و CLR
- BinderScopeKind
- مشارکت و Git Flow
- Schemaهای داده
- Ponytail Help
- طراحی Knowledge Pipeline
- TypeKind
- قرارداد دقت پیشنهاد
- طراحی ارزیابی و کیفیت پیشنهادها
- SyntaxKind
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
1. `TypeSymbol` - 40 edges
2. `DotnetExpertAdvisor.Shared` - 33 edges
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
- `3. C# Compiler Intelligence Host` --references--> `RoslynHost`  [INFERRED]
  docs/architecture.md → src/RoslynHost/RoslynHost.cs
- `مرزهای ماژول‌ها` --references--> `RoslynHost`  [INFERRED]
  docs/internal-contracts.md → src/RoslynHost/RoslynHost.cs
- `Symbols` --references--> `NamedTypeSymbol`  [INFERRED]
  docs/design/binder-overload-resolution.md → src/Shared/Symbols.cs

## Import Cycles
- None detected.

## Communities (59 total, 11 thin omitted)

### Community 0 - "قرارداد LSP و مدل پاسخ پیشنهادها"
Cohesion: 0.08
Nodes (18): DotnetExpertAdvisor.Protocol, DotnetExpertAdvisor.LanguageServer, `runtime/explainSuggestion`, زمان‌بندی و سطح تحلیل, قابلیت اختصاصی, قابلیت‌های پایه, قرارداد Completion, قرارداد LSP و مدل پاسخ پیشنهادها (+10 more)

### Community 1 - "Shared.csproj"
Cohesion: 0.07
Nodes (20): net10.0, Microsoft.NET.Sdk, net10.0, Microsoft.NET.Sdk, net10.0, Microsoft.NET.Sdk, net10.0, Microsoft.NET.Sdk (+12 more)

### Community 2 - "Contracts.cs"
Cohesion: 0.08
Nodes (35): DotnetExpertAdvisor.RuleEngine, DotnetExpertAdvisor.RuntimeModel, IRankingEngine, RankingEngine, IRuleEngine, RuleEngine, IRuntimeModel, RuntimeEstimate (+27 more)

### Community 3 - "NamedTypeSymbol"
Cohesion: 0.20
Nodes (8): ConversionCacheKey, ConversionContext, ConversionResult, Identity, IConversionClassifier, NamedTypeSymbol, NullableTypeSymbol, ContractSmokeTests

### Community 4 - "معماری سامانه هوشمندی C# و .NET برای VS Code"
Cohesion: 0.05
Nodes (37): DotnetExpertAdvisor.RoslynHost, 1. VS Code Client, 2. Custom C# Language Server, 3. C# Compiler Intelligence Host, 4. Project Semantic Graph, 5. Runtime and Memory Model, 6. Rule Engine, 7. Knowledge Engine (+29 more)

### Community 5 - "adr/README.md"
Cohesion: 0.06
Nodes (26): ADR-001: semantic engine اختصاصی, Consequences, Decision, ADR-002: language server جدا از VS Code client, Consequences, Decision, ADR-003: knowledge pack امضاشده, Consequences (+18 more)

### Community 6 - "ImmutableArray"
Cohesion: 0.13
Nodes (28): Symbols, Accessibility, Internal, NotApplicable, Private, PrivateProtected, Protected, ProtectedInternal (+20 more)

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
Nodes (15): MetadataMember, SuggestionSeverity, Error, Hint, Info, Warning, CodeActionInfo, DefinitionLocation (+7 more)

### Community 12 - "TypeSymbol"
Cohesion: 0.24
Nodes (3): ConversionClassifier, CachedConversionCount, TypeSymbol

### Community 13 - ".Read"
Cohesion: 0.07
Nodes (16): AssemblyMetadataIndex, AssemblyMetadataReader, MetadataAssemblyReference, MetadataGenericParameter, MetadataType, MetadataTypeForwarder, NuGetApiCache, NuGetApiIndex (+8 more)

### Community 14 - "DotnetExpertAdvisor.Shared"
Cohesion: 0.05
Nodes (28): DotnetExpertAdvisor.Shared, DotnetExpertAdvisor.Architecture.Tests, DotnetExpertAdvisor.SemanticGraph, DotnetExpertAdvisor.RankingEngine, EfLinqAnalyzer, EfQueryAnalysis, EfTranslationStatus, ClientEvaluationRisk (+20 more)

### Community 15 - "Binder"
Cohesion: 0.12
Nodes (14): Binder, Kind, Parent, LookupOptions, LookupResult, HasCandidates, IsSuccess, LookupStatus (+6 more)

### Community 16 - "LexerDiagnostic"
Cohesion: 0.26
Nodes (7): LexerDiagnostic, LexResult, SyntaxToken, TextSpan, End, CSharpParser, SyntaxNode

### Community 17 - "طراحی دقت‌محور Binder و Overload Resolution"
Cohesion: 0.11
Nodes (17): Best-effort و speculative binding, Better function member, Binder chain, Candidate set, Conversion Classifier, Generic Inference, Member Lookup, Nullable flow (+9 more)

### Community 18 - "extension.js"
Cohesion: 0.15
Nodes (10): activate(), cp, fs, handleOutput(), inputBuffer, path, pending, request() (+2 more)

### Community 19 - "backlog Ø§ØµÙ„ÛŒ"
Cohesion: 0.11
Nodes (17): backlog Ø§ØµÙ„ÛŒ, milestone Ø´Ø¨ÛŒÙ‡â€ŒØ³Ø§Ø² EF Ùˆ translatability, milestone Ø¯Ù‚Øªâ€ŒÙ…Ø­ÙˆØ± Binder, Ø¯ÙØªØ± Ú©Ø§Ø± Ù¾Ø±ÙˆÚ˜Ù‡, ØªØ¹Ø±ÛŒÙ Ø§Ù†Ø¬Ø§Ù…â€ŒØ´Ø¯Ù†, Ù…Ø³ÛŒØ± Ø§Ø¬Ø±Ø§ÛŒ Ø¨Ø¹Ø¯ÛŒ, ÙØ§Ø² 1: Ù‚Ø±Ø§Ø±Ø¯Ø§Ø¯ Ùˆ Ù‡Ø³ØªÙ‡ Ø²Ø¨Ø§Ù†, ÙØ§Ø² 2: semantic engine Ø§Ø®ØªØµØ§ØµÛŒ (+9 more)

### Community 20 - "LanguageVersion"
Cohesion: 0.13
Nodes (12): LanguageCapabilities, LanguageVersion, CSharp10, CSharp11, CSharp12, CSharp13, CSharp14, CSharp15 (+4 more)

### Community 21 - "ConversionKind"
Cohesion: 0.12
Nodes (17): ConversionKind, AnonymousFunction, Boxing, CollectionExpression, ExplicitNumeric, Identity, ImplicitNullable, ImplicitNumeric (+9 more)

### Community 22 - "SymbolKind"
Cohesion: 0.12
Nodes (16): SymbolKind, ArrayType, DynamicType, ErrorType, Event, Field, FunctionPointerType, Local (+8 more)

### Community 23 - "package.json"
Cohesion: 0.12
Nodes (15): activationEvents, categories, contributes, commands, description, displayName, engines, vscode (+7 more)

### Community 25 - "طراحی شبیه‌ساز EF Core و Translatability"
Cohesion: 0.14
Nodes (13): cross-checkهای اختیاری, gate پیشنهادی, provenance و confidence, Query provenance, Translation verdict, اصل, ترتیب بازسازی مدل, سطح اول: تحلیل ایستا (+5 more)

### Community 26 - "MemberLookup"
Cohesion: 0.27
Nodes (4): MemberLookup, MemberLookupOptions, MemberLookupResult, IsSuccess

### Community 27 - "Product Backlog"
Cohesion: 0.15
Nodes (12): Backlog ØªÙØµÛŒÙ„ÛŒ, Compiler core, Definition of Done, Definition of Ready, Editor Ùˆ Ú©ÛŒÙÛŒØª, EF Ùˆ rule knowledge, Metadata Ùˆ project index, Product Backlog (+4 more)

### Community 28 - "TokenKind"
Cohesion: 0.15
Nodes (13): TokenKind, Bad, Character, Comment, Directive, EndOfFile, Identifier, Keyword (+5 more)

### Community 29 - "طراحی Rule Engine"
Cohesion: 0.18
Nodes (10): Correctness, Design, Performance, Schema پیشنهادی Rule, اصل اصلی, انواع Rule, سطوح اطمینان, طراحی Rule Engine (+2 more)

### Community 30 - "Parser.cs"
Cohesion: 0.22
Nodes (8): ParseResult, Recoveries, RecoveryEvent, RecoveryKind, IncompleteDeclaration, MissingName, UnclosedDelimiter, UnexpectedDelimiter

### Community 31 - "ماتریس پوشش C# و CLR"
Cohesion: 0.20
Nodes (9): 1. اجرای CLR، assembly و deployment, 2. مدل انواع و dispatch, 3. انواع ضروری و امکانات زبان, 4. exception، memory و resource lifetime, 5. hosting، loading، reflection و serialization, 6. threading و asynchronous execution, استفاده در پیشنهادها, قرارداد پوشش (+1 more)

### Community 32 - "BinderScopeKind"
Cohesion: 0.20
Nodes (10): BinderScopeKind, Block, Global, Imports, Lambda, Member, Namespace, Pattern (+2 more)

### Community 33 - "مشارکت و Git Flow"
Cohesion: 0.22
Nodes (8): جریان feature, جریان hotfix, جریان release, شاخه‌ها, قواعد تغییر, مشارکت و Git Flow, نام‌گذاری commit, نسخه‌گذاری

### Community 34 - "Schemaهای داده"
Cohesion: 0.22
Nodes (8): Compatibility, GraphEdge, GraphNode, KnowledgeReference, RuleDescriptor, Schemaهای داده, Settings, شناسه‌ها

### Community 35 - "Ponytail Help"
Cohesion: 0.25
Nodes (7): Configure Default Mode, Deactivate, Levels, More, Ponytail Help, Skills, Update

### Community 36 - "طراحی Knowledge Pipeline"
Cohesion: 0.25
Nodes (7): Blog crawler, Manifest, تولید Rule, شناسه و نسخه, طراحی Knowledge Pipeline, مراحل ingestion, منابع

### Community 37 - "TypeKind"
Cohesion: 0.25
Nodes (8): TypeKind, Class, Delegate, Enum, Error, Interface, Record, Struct

### Community 38 - "قرارداد دقت پیشنهاد"
Cohesion: 0.29
Nodes (6): Quality gates, SuggestionContract, SuggestionEvidence, قرارداد دقت پیشنهاد, قواعد اجباری, منابع دانشی

### Community 39 - "طراحی ارزیابی و کیفیت پیشنهادها"
Cohesion: 0.29
Nodes (6): Feedback کاربر, آستانه اولیه نسخه آزمایشی, تست‌های لازم, طراحی ارزیابی و کیفیت پیشنهادها, مجموعه آزمون, معیارهای اصلی

### Community 40 - "SyntaxKind"
Cohesion: 0.29
Nodes (7): SyntaxKind, Block, CompilationUnit, Error, NamespaceDeclaration, Token, TypeDeclaration

### Community 41 - "ConversionQuality"
Cohesion: 0.33
Nodes (6): ConversionQuality, Best, Good, Invalid, RequiresRuntimeCheck, Uncertain

### Community 42 - "Ponytail, lazy senior dev mode"
Cohesion: 0.40
Nodes (4): Before you write, graphify, Ponytail, lazy senior dev mode, The smallest complete change

### Community 43 - "ponytail-audit/SKILL.md"
Cohesion: 0.40
Nodes (4): 1. Map first, 2. Look for, 3. Check before you report, 4. Output

### Community 44 - "Ponytail Gain"
Cohesion: 0.40
Nodes (4): Boundaries, Honesty boundary, Ponytail Gain, Scoreboard

### Community 45 - "ponytail-review/SKILL.md"
Cohesion: 0.40
Nodes (4): 1. Understand first, 2. Look for, 3. Check before you report, 4. Output

### Community 46 - "Ponytail"
Cohesion: 0.40
Nodes (4): Before you write, Levels, Ponytail, The smallest complete change

### Community 47 - "سیاست نسخه‌گذاری"
Cohesion: 0.40
Nodes (4): سیاست نسخه‌گذاری, منبع نسخه, نسخه‌های بعدی پیشنهادی, نگاشت تغییر به نسخه

### Community 48 - "ponytail-debt/SKILL.md"
Cohesion: 0.50
Nodes (3): Boundaries, Output, Scan

### Community 49 - "VarianceKind"
Cohesion: 0.50
Nodes (4): VarianceKind, Contravariant, Covariant, Invariant

## Knowledge Gaps
- **173 isolated node(s):** `Info`, `Warning`, `net10.0`, `Microsoft.NET.Sdk`, `net10.0` (+168 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 479 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **11 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `DotnetExpertAdvisor.Shared` connect `DotnetExpertAdvisor.Shared` to `قرارداد LSP و مدل پاسخ پیشنهادها`, `Contracts.cs`, `NamedTypeSymbol`, `معماری سامانه هوشمندی C# و .NET برای VS Code`, `ImmutableArray`, `قراردادهای داخلی سامانه`, `EditorFeatures.cs`, `.Read`, `Binder`, `LexerDiagnostic`, `LanguageVersion`, `MemberLookup`, `Parser.cs`?**
  _High betweenness centrality (0.225) - this node is a cross-community bridge._
- **Are the 3 inferred relationships involving `ConversionClassifier` (e.g. with `MemberLookup` and `.ValidateConversionClassifier()`) actually correct?**
  _`ConversionClassifier` has 3 INFERRED edges - model-reasoned connections that need verification._
- **What connects `Info`, `Warning`, `net10.0` to the rest of the system?**
  _173 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `قرارداد LSP و مدل پاسخ پیشنهادها` be split into smaller, more focused modules?**
  _Cohesion score 0.0766488413547237 - nodes in this community are weakly interconnected._
- **Why does `RoslynHost` connect `معماری سامانه هوشمندی C# و .NET برای VS Code` to `قرارداد LSP و مدل پاسخ پیشنهادها`, `Contracts.cs`?**
  _High betweenness centrality (0.060) - this node is a cross-community bridge._
- **Should `Shared.csproj` be split into smaller, more focused modules?**
  _Cohesion score 0.07301587301587302 - nodes in this community are weakly interconnected._
- **Should `Contracts.cs` be split into smaller, more focused modules?**
  _Cohesion score 0.08361581920903954 - nodes in this community are weakly interconnected._