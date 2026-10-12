# Graph Report - CsharpVscode  (2026-10-12)

## Corpus Check
- 76 files · ~21,957 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 5 file(s) not represented in the graph (top: (none) 3, .props 1, .Config 1)

## Summary
- 811 nodes · 1282 edges · 55 communities (46 shown, 9 thin omitted)
- Extraction: 94% EXTRACTED · 6% INFERRED · 0% AMBIGUOUS · INFERRED: 76 edges (avg confidence: 0.87)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `7fe5fc6d`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- TypeSymbol
- Shared.csproj
- .Tokenize
- Contracts.cs
- adr/README.md
- LanguageFeature
- تصمیم‌های معماری
- قراردادهای داخلی سامانه
- معماری سامانه هوشمندی C# و .NET برای VS Code
- طراحی دقت‌محور Binder و Overload Resolution
- TypeProvider
- backlog اصلی
- ConversionKind
- قرارداد LSP و مدل پاسخ پیشنهادها
- SymbolKind
- package.json
- Symbols.cs
- طراحی شبیه‌ساز EF Core و Translatability
- extension.js
- MemberLookup
- Product Backlog
- Symbol
- TokenKind
- TypeKind
- BinderScopeKind
- طراحی Rule Engine
- LanguageVersion
- ماتریس پوشش C# و CLR
- مشارکت و Git Flow
- Schemaهای داده
- NamedTypeSymbol
- Ponytail Help
- طراحی Knowledge Pipeline
- Accessibility
- RuntimeModel.cs
- قرارداد دقت پیشنهاد
- طراحی ارزیابی و کیفیت پیشنهادها
- LookupStatus
- ConstructedTypeSymbol
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
4. `Symbol` - 30 edges
5. `TypeProvider` - 19 edges
6. `ConversionKind` - 19 edges
7. `DotnetExpertAdvisor.Shared` - 18 edges
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

## Communities (55 total, 9 thin omitted)

### Community 0 - "TypeSymbol"
Cohesion: 0.12
Nodes (14): ConversionCacheKey, ConversionClassifier, CachedConversionCount, ConversionContext, ConversionQuality, Best, Good, Invalid (+6 more)

### Community 1 - "Shared.csproj"
Cohesion: 0.07
Nodes (20): net10.0, Microsoft.NET.Sdk, net10.0, Microsoft.NET.Sdk, net10.0, Microsoft.NET.Sdk, net10.0, Microsoft.NET.Sdk (+12 more)

### Community 2 - ".Tokenize"
Cohesion: 0.07
Nodes (29): DotnetExpertAdvisor.Shared, DotnetExpertAdvisor.Architecture.Tests, LanguageCapabilities, CSharpLexer, LexerDiagnostic, LexResult, SyntaxToken, TextChangeRange (+21 more)

### Community 3 - "Contracts.cs"
Cohesion: 0.06
Nodes (49): DotnetExpertAdvisor.RuleEngine, DotnetExpertAdvisor.RoslynHost, DotnetExpertAdvisor.SemanticGraph, DotnetExpertAdvisor.RankingEngine, 1. VS Code Client, 2. Custom C# Language Server, 3. C# Compiler Intelligence Host, 4. Project Semantic Graph (+41 more)

### Community 4 - "adr/README.md"
Cohesion: 0.06
Nodes (26): ADR-001: semantic engine اختصاصی, Consequences, Decision, ADR-002: language server جدا از VS Code client, Consequences, Decision, ADR-003: knowledge pack امضاشده, Consequences (+18 more)

### Community 5 - "LanguageFeature"
Cohesion: 0.07
Nodes (29): LanguageFeature, CollectionExpressions, ExtensionMembers, FieldKeyword, FileScopedNamespaces, GenericMath, GlobalUsings, InitOnlySetters (+21 more)

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

### Community 10 - "TypeProvider"
Cohesion: 0.06
Nodes (9): AssemblyMetadataIndex, AssemblyMetadataReader, MetadataAssemblyReference, MetadataGenericParameter, MetadataMember, MetadataSignatureDecoder, MetadataType, MetadataTypeForwarder (+1 more)

### Community 11 - "backlog اصلی"
Cohesion: 0.11
Nodes (17): backlog اصلی, milestone دقت‌محور Binder, milestone شبیه‌ساز EF و translatability, تعریف انجام‌شدن, دفتر کار پروژه, فاز 1: قرارداد و هسته زبان, فاز 2: semantic engine اختصاصی, فاز 3: metadata، assembly و NuGet (+9 more)

### Community 12 - "ConversionKind"
Cohesion: 0.12
Nodes (17): ConversionKind, AnonymousFunction, Boxing, CollectionExpression, ExplicitNumeric, Identity, ImplicitNullable, ImplicitNumeric (+9 more)

### Community 13 - "قرارداد LSP و مدل پاسخ پیشنهادها"
Cohesion: 0.13
Nodes (14): DotnetExpertAdvisor.Protocol, `runtime/explainSuggestion`, زمان‌بندی و سطح تحلیل, قابلیت اختصاصی, قابلیت‌های پایه, قرارداد Completion, قرارداد LSP و مدل پاسخ پیشنهادها, هدف (+6 more)

### Community 14 - "SymbolKind"
Cohesion: 0.12
Nodes (16): SymbolKind, ArrayType, DynamicType, ErrorType, Event, Field, FunctionPointerType, Local (+8 more)

### Community 15 - "package.json"
Cohesion: 0.12
Nodes (15): activationEvents, categories, contributes, commands, description, displayName, engines, vscode (+7 more)

### Community 16 - "Symbols.cs"
Cohesion: 0.27
Nodes (12): Symbols, CollectionExpressionTypeSymbol, EventSymbol, FieldSymbol, InterpolatedStringTypeSymbol, LambdaTypeSymbol, LocalSymbol, MethodGroupTypeSymbol (+4 more)

### Community 17 - "طراحی شبیه‌ساز EF Core و Translatability"
Cohesion: 0.14
Nodes (13): cross-checkهای اختیاری, gate پیشنهادی, provenance و confidence, Query provenance, Translation verdict, اصل, ترتیب بازسازی مدل, سطح اول: تحلیل ایستا (+5 more)

### Community 18 - "extension.js"
Cohesion: 0.18
Nodes (4): cp, fs, path, vscode

### Community 19 - "MemberLookup"
Cohesion: 0.27
Nodes (4): MemberLookup, MemberLookupOptions, MemberLookupResult, IsSuccess

### Community 20 - "Product Backlog"
Cohesion: 0.15
Nodes (12): Backlog تفصیلی, Compiler core, Definition of Done, Definition of Ready, Editor و کیفیت, EF و rule knowledge, Metadata و project index, Product Backlog (+4 more)

### Community 21 - "Symbol"
Cohesion: 0.27
Nodes (8): Binder, Kind, Parent, LookupOptions, LookupResult, HasCandidates, IsSuccess, Symbol

### Community 22 - "TokenKind"
Cohesion: 0.15
Nodes (13): TokenKind, Bad, Character, Comment, Directive, EndOfFile, Identifier, Keyword (+5 more)

### Community 23 - "TypeKind"
Cohesion: 0.15
Nodes (12): ErrorTypeSymbol, IsError, TypeKind, SymbolFactory, TypeKind, Class, Delegate, Enum (+4 more)

### Community 24 - "BinderScopeKind"
Cohesion: 0.17
Nodes (10): BinderScopeKind, Block, Global, Imports, Lambda, Member, Namespace, Pattern (+2 more)

### Community 25 - "طراحی Rule Engine"
Cohesion: 0.18
Nodes (10): Correctness, Design, Performance, Schema پیشنهادی Rule, اصل اصلی, انواع Rule, سطوح اطمینان, طراحی Rule Engine (+2 more)

### Community 26 - "LanguageVersion"
Cohesion: 0.18
Nodes (11): LanguageVersion, CSharp10, CSharp11, CSharp12, CSharp13, CSharp14, CSharp15, CSharp6 (+3 more)

### Community 27 - "ماتریس پوشش C# و CLR"
Cohesion: 0.20
Nodes (9): 1. اجرای CLR، assembly و deployment, 2. مدل انواع و dispatch, 3. انواع ضروری و امکانات زبان, 4. exception، memory و resource lifetime, 5. hosting، loading، reflection و serialization, 6. threading و asynchronous execution, استفاده در پیشنهادها, قرارداد پوشش (+1 more)

### Community 28 - "مشارکت و Git Flow"
Cohesion: 0.22
Nodes (8): جریان feature, جریان hotfix, جریان release, شاخه‌ها, قواعد تغییر, مشارکت و Git Flow, نام‌گذاری commit, نسخه‌گذاری

### Community 29 - "Schemaهای داده"
Cohesion: 0.22
Nodes (8): Compatibility, GraphEdge, GraphNode, KnowledgeReference, RuleDescriptor, Schemaهای داده, Settings, شناسه‌ها

### Community 30 - "NamedTypeSymbol"
Cohesion: 0.39
Nodes (4): NamedTypeSymbol, NullableTypeSymbol, PropertySymbol, ContractSmokeTests

### Community 31 - "Ponytail Help"
Cohesion: 0.25
Nodes (7): Configure Default Mode, Deactivate, Levels, More, Ponytail Help, Skills, Update

### Community 32 - "طراحی Knowledge Pipeline"
Cohesion: 0.25
Nodes (7): Blog crawler, Manifest, تولید Rule, شناسه و نسخه, طراحی Knowledge Pipeline, مراحل ingestion, منابع

### Community 33 - "Accessibility"
Cohesion: 0.25
Nodes (8): Accessibility, Internal, NotApplicable, Private, PrivateProtected, Protected, ProtectedInternal, Public

### Community 34 - "RuntimeModel.cs"
Cohesion: 0.48
Nodes (4): DotnetExpertAdvisor.RuntimeModel, IRuntimeModel, RuntimeEstimate, RuntimeModel

### Community 35 - "قرارداد دقت پیشنهاد"
Cohesion: 0.29
Nodes (6): Quality gates, SuggestionContract, SuggestionEvidence, قرارداد دقت پیشنهاد, قواعد اجباری, منابع دانشی

### Community 36 - "طراحی ارزیابی و کیفیت پیشنهادها"
Cohesion: 0.29
Nodes (6): Feedback کاربر, آستانه اولیه نسخه آزمایشی, تست‌های لازم, طراحی ارزیابی و کیفیت پیشنهادها, مجموعه آزمون, معیارهای اصلی

### Community 37 - "LookupStatus"
Cohesion: 0.29
Nodes (7): LookupStatus, Ambiguous, Inaccessible, NotAValue, NotFound, Viable, WrongArity

### Community 38 - "ConstructedTypeSymbol"
Cohesion: 0.33
Nodes (6): ConstructedTypeSymbol, Variances, VarianceKind, Contravariant, Covariant, Invariant

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

### Community 45 - "ponytail-debt/SKILL.md"
Cohesion: 0.50
Nodes (3): Boundaries, Output, Scan

## Knowledge Gaps
- **168 isolated node(s):** `net10.0`, `Microsoft.NET.Sdk`, `net10.0`, `Microsoft.NET.Sdk`, `net10.0` (+163 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 473 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **9 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `DotnetExpertAdvisor.Shared` connect `.Tokenize` to `TypeSymbol`, `RuntimeModel.cs`, `Contracts.cs`, `قراردادهای داخلی سامانه`, `TypeProvider`, `قرارداد LSP و مدل پاسخ پیشنهادها`, `Symbols.cs`, `MemberLookup`, `Symbol`?**
  _High betweenness centrality (0.191) - this node is a cross-community bridge._
- **Are the 3 inferred relationships involving `ConversionClassifier` (e.g. with `MemberLookup` and `.ValidateConversionClassifier()`) actually correct?**
  _`ConversionClassifier` has 3 INFERRED edges - model-reasoned connections that need verification._
- **What connects `net10.0`, `Microsoft.NET.Sdk`, `net10.0` to the rest of the system?**
  _168 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `TypeSymbol` be split into smaller, more focused modules?**
  _Cohesion score 0.1184939091915836 - nodes in this community are weakly interconnected._
- **Should `Shared.csproj` be split into smaller, more focused modules?**
  _Cohesion score 0.06984126984126984 - nodes in this community are weakly interconnected._
- **Should `.Tokenize` be split into smaller, more focused modules?**
  _Cohesion score 0.06804214223002635 - nodes in this community are weakly interconnected._
- **Should `Contracts.cs` be split into smaller, more focused modules?**
  _Cohesion score 0.06126331811263318 - nodes in this community are weakly interconnected._