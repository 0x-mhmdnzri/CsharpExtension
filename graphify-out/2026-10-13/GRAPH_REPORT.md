# Graph Report - CsharpVscode  (2026-10-13)

## Corpus Check
- 99 files · ~25,508 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 5 file(s) not represented in the graph (top: (none) 3, .props 1, .Config 1)

## Summary
- 961 nodes · 1628 edges · 54 communities (40 shown, 14 thin omitted)
- Extraction: 94% EXTRACTED · 6% INFERRED · 0% AMBIGUOUS · INFERRED: 90 edges (avg confidence: 0.87)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `33ca6e18`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- قرارداد LSP و مدل پاسخ پیشنهادها
- Shared.csproj
- Contracts.cs
- ImmutableArray
- Binder
- LanguageFeature
- DotnetExpertAdvisor.Shared
- adr/README.md
- معماری سامانه هوشمندی C# و .NET برای VS Code
- NuGetProjectIndex.cs
- تصمیم‌های معماری
- قراردادهای داخلی سامانه
- EditorFeatures.cs
- AssemblyMetadataReader.cs
- .Tokenize
- TypeProvider
- backlog ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â§ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚ÂµÃƒÆ’Ã¢â€žÂ¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¾ÃƒÆ’Ã¢â‚¬ÂºÃƒâ€¦Ã¢â‚¬â„¢
- ConversionKind
- extension.js
- SymbolKind
- package.json
- طراحی شبیه‌ساز EF Core و Translatability
- Product Backlog
- KnowledgeRegistry
- KnowledgeCrawler
- طراحی Rule Engine
- .Read
- .Analyze
- ماتریس پوشش C# و CLR
- مشارکت و Git Flow
- Schemaهای داده
- MetadataReader
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
2. `DotnetExpertAdvisor.Shared` - 39 edges
3. `ConversionClassifier` - 33 edges
4. `LanguageFeature` - 33 edges
5. `Symbol` - 30 edges
6. `TypeProvider` - 19 edges
7. `ConversionKind` - 19 edges
8. `CSharpLexer` - 18 edges
9. `SymbolKind` - 18 edges
10. `NamedTypeSymbol` - 16 edges

## Surprising Connections (you probably didn't know these)
- `3. C# Compiler Intelligence Host` --references--> `RoslynHost`  [INFERRED]
  docs/architecture.md → src/RoslynHost/RoslynHost.cs
- `مرزهای ماژول‌ها` --references--> `RoslynHost`  [INFERRED]
  docs/internal-contracts.md → src/RoslynHost/RoslynHost.cs
- `قرارداد RuleEngine` --references--> `KnowledgeSnapshot`  [INFERRED]
  docs/internal-contracts.md → src/KnowledgeEngine/KnowledgeEngine.cs
- `Binder chain` --references--> `LookupResult`  [INFERRED]
  docs/design/binder-overload-resolution.md → src/Shared/Binder.cs
- `قرارداد Completion` --references--> `CompletionItem`  [INFERRED]
  docs/lsp-contract.md → src/Protocol/ProtocolModels.cs

## Import Cycles
- None detected.

## Communities (54 total, 14 thin omitted)

### Community 0 - "قرارداد LSP و مدل پاسخ پیشنهادها"
Cohesion: 0.08
Nodes (18): DotnetExpertAdvisor.Protocol, DotnetExpertAdvisor.LanguageServer, `runtime/explainSuggestion`, زمان‌بندی و سطح تحلیل, قابلیت اختصاصی, قابلیت‌های پایه, قرارداد Completion, قرارداد LSP و مدل پاسخ پیشنهادها (+10 more)

### Community 1 - "Shared.csproj"
Cohesion: 0.07
Nodes (20): net10.0, Microsoft.NET.Sdk, net10.0, Microsoft.NET.Sdk, net10.0, Microsoft.NET.Sdk, net10.0, Microsoft.NET.Sdk (+12 more)

### Community 2 - "Contracts.cs"
Cohesion: 0.08
Nodes (38): DotnetExpertAdvisor.RuleEngine, DotnetExpertAdvisor.RoslynHost, DotnetExpertAdvisor.RuntimeModel, IRankingEngine, RankingEngine, IRoslynHost, RoslynHost, IRuleEngine (+30 more)

### Community 3 - "ImmutableArray"
Cohesion: 0.06
Nodes (56): Symbols, LookupOptions, ConversionCacheKey, ConversionClassifier, CachedConversionCount, ConversionContext, ConversionResult, Identity (+48 more)

### Community 4 - "Binder"
Cohesion: 0.05
Nodes (40): Best-effort و speculative binding, Better function member, Binder chain, Candidate set, Conversion Classifier, Generic Inference, Member Lookup, Nullable flow (+32 more)

### Community 5 - "LanguageFeature"
Cohesion: 0.05
Nodes (41): LanguageCapabilities, LanguageFeature, CollectionExpressions, ExtensionMembers, FieldKeyword, FileScopedNamespaces, GenericMath, GlobalUsings (+33 more)

### Community 6 - "DotnetExpertAdvisor.Shared"
Cohesion: 0.06
Nodes (21): DotnetExpertAdvisor.Shared, DotnetExpertAdvisor.Architecture.Tests, DotnetExpertAdvisor.SemanticGraph, DotnetExpertAdvisor.RankingEngine, EfPerformanceAnalyzer, EfPerformanceFinding, EfRiskSeverity, Info (+13 more)

### Community 7 - "adr/README.md"
Cohesion: 0.06
Nodes (26): ADR-001: semantic engine اختصاصی, Consequences, Decision, ADR-002: language server جدا از VS Code client, Consequences, Decision, ADR-003: knowledge pack امضاشده, Consequences (+18 more)

### Community 8 - "معماری سامانه هوشمندی C# و .NET برای VS Code"
Cohesion: 0.06
Nodes (33): 1. VS Code Client, 2. Custom C# Language Server, 3. C# Compiler Intelligence Host, 4. Project Semantic Graph, 5. Runtime and Memory Model, 6. Rule Engine, 7. Knowledge Engine, 8. Ranking Engine (+25 more)

### Community 9 - "NuGetProjectIndex.cs"
Cohesion: 0.12
Nodes (12): EfProviderInfo, EfProviderRegistry, NuGetApiCache, NuGetApiIndex, NuGetAsset, NuGetCachedAssembly, NuGetPackageApi, NuGetPackageReference (+4 more)

### Community 10 - "تصمیم‌های معماری"
Cohesion: 0.07
Nodes (26): ADR-001: موتور semantic اختصاصی, ADR-002: language server جدا از VS Code client, ADR-003: knowledge pack به‌جای self-modifying code, ADR-004: snapshot و generation برای consistency, ADR-005: precision مقدم بر recall در ruleهای فعال, ADR-006: عدم وابستگی خارجی در runtime افزونه, ADR-007: پشتیبانی C# 6 تا 15 با capability matrix, ADR-008: NuGet و EF به‌عنوان بخشی از semantic context (+18 more)

### Community 11 - "قراردادهای داخلی سامانه"
Cohesion: 0.10
Nodes (20): DotnetExpertAdvisor.KnowledgeEngine, DocumentContext, Evidence, observability, SemanticContext, SuggestionCandidate, جریان درخواست completion, خطا و cancellation (+12 more)

### Community 12 - "EditorFeatures.cs"
Cohesion: 0.16
Nodes (15): MetadataMember, SuggestionSeverity, Error, Hint, Info, Warning, CodeActionInfo, DefinitionLocation (+7 more)

### Community 13 - "AssemblyMetadataReader.cs"
Cohesion: 0.18
Nodes (5): AssemblyMetadataIndex, MetadataAssemblyReference, MetadataGenericParameter, MetadataType, MetadataTypeForwarder

### Community 14 - ".Tokenize"
Cohesion: 0.06
Nodes (37): CSharpLexer, LexerDiagnostic, LexResult, SyntaxToken, TextChangeRange, TextSpan, End, TokenKind (+29 more)

### Community 16 - "backlog ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â§ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚ÂµÃƒÆ’Ã¢â€žÂ¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¾ÃƒÆ’Ã¢â‚¬ÂºÃƒâ€¦Ã¢â‚¬â„¢"
Cohesion: 0.11
Nodes (17): backlog ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â§ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚ÂµÃƒÆ’Ã¢â€žÂ¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¾ÃƒÆ’Ã¢â‚¬ÂºÃƒâ€¦Ã¢â‚¬â„¢, milestone ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â¯ÃƒÆ’Ã¢â€žÂ¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚ÂªÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã¢â‚¬â„¢ÃƒÆ’Ã¢â€žÂ¢ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â¦ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â­ÃƒÆ’Ã¢â€žÂ¢Ãƒâ€¹Ã¢â‚¬Â ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â± Binder, milestone ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â´ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â¨ÃƒÆ’Ã¢â‚¬ÂºÃƒâ€¦Ã¢â‚¬â„¢ÃƒÆ’Ã¢â€žÂ¢ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â¡ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã¢â‚¬â„¢ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â³ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â§ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â² EF ÃƒÆ’Ã¢â€žÂ¢Ãƒâ€¹Ã¢â‚¬Â  translatability, ÃƒÆ’Ã…Â¡Ãƒâ€šÃ‚Â©ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â§ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â±ÃƒÆ’Ã¢â€žÂ¢ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â¡ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â§ÃƒÆ’Ã¢â‚¬ÂºÃƒâ€¦Ã¢â‚¬â„¢ ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚ÂªÃƒÆ’Ã…Â¡Ãƒâ€šÃ‚Â©ÃƒÆ’Ã¢â€žÂ¢ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â¦ÃƒÆ’Ã¢â‚¬ÂºÃƒâ€¦Ã¢â‚¬â„¢ÃƒÆ’Ã¢â€žÂ¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¾ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã¢â‚¬â„¢ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â´ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â¯ÃƒÆ’Ã¢â€žÂ¢ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â¡, ÃƒÆ’Ã¢â€žÂ¢Ãƒâ€¹Ã¢â‚¬Â ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â¶ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â¹ÃƒÆ’Ã¢â‚¬ÂºÃƒâ€¦Ã¢â‚¬â„¢ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚ÂªÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã¢â‚¬â„¢ÃƒÆ’Ã¢â€žÂ¢ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â¡ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â§ÃƒÆ’Ã¢â‚¬ÂºÃƒâ€¦Ã¢â‚¬â„¢ ÃƒÆ’Ã¢â€žÂ¢ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â¦ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â§ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â², ÃƒÆ’Ã¢â€žÂ¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â€žÂ¢Ãƒâ€¹Ã¢â‚¬Â ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â§ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â¹ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â¯ ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â§ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â³ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚ÂªÃƒÆ’Ã¢â€žÂ¢Ãƒâ€šÃ‚ÂÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â§ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â¯ÃƒÆ’Ã¢â€žÂ¢ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â¡, ÃƒÆ’Ã¢â€žÂ¢ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â¦ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â³ÃƒÆ’Ã¢â‚¬ÂºÃƒâ€¦Ã¢â‚¬â„¢ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â± ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â§ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â±ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â§ÃƒÆ’Ã¢â‚¬ÂºÃƒâ€¦Ã¢â‚¬â„¢ ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â¨ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â¹ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â¯ÃƒÆ’Ã¢â‚¬ÂºÃƒâ€¦Ã¢â‚¬â„¢, ÃƒÆ’Ã¢â€žÂ¢ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â¦ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â§ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚ÂªÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â±ÃƒÆ’Ã¢â‚¬ÂºÃƒâ€¦Ã¢â‚¬â„¢ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â³ ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â¯ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â§ÃƒÆ’Ã¢â€žÂ¢ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Â´ ÃƒÆ’Ã¢â€žÂ¢Ãƒâ€¹Ã¢â‚¬Â  rule coverage (+9 more)

### Community 17 - "ConversionKind"
Cohesion: 0.12
Nodes (17): ConversionKind, AnonymousFunction, Boxing, CollectionExpression, ExplicitNumeric, Identity, ImplicitNullable, ImplicitNumeric (+9 more)

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
Nodes (12): Backlog ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚ÂªÃƒÆ’Ã¢â€žÂ¢Ãƒâ€šÃ‚ÂÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚ÂµÃƒÆ’Ã¢â‚¬ÂºÃƒâ€¦Ã¢â‚¬â„¢ÃƒÆ’Ã¢â€žÂ¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¾ÃƒÆ’Ã¢â‚¬ÂºÃƒâ€¦Ã¢â‚¬â„¢, Compiler core, Definition of Done, Definition of Ready, Editor ÃƒÆ’Ã¢â€žÂ¢Ãƒâ€¹Ã¢â‚¬Â  ÃƒÆ’Ã…Â¡Ãƒâ€šÃ‚Â©ÃƒÆ’Ã¢â‚¬ÂºÃƒâ€¦Ã¢â‚¬â„¢ÃƒÆ’Ã¢â€žÂ¢Ãƒâ€šÃ‚ÂÃƒÆ’Ã¢â‚¬ÂºÃƒâ€¦Ã¢â‚¬â„¢ÃƒÆ’Ã‹Å“Ãƒâ€šÃ‚Âª, EF ÃƒÆ’Ã¢â€žÂ¢Ãƒâ€¹Ã¢â‚¬Â  rule knowledge, Metadata ÃƒÆ’Ã¢â€žÂ¢Ãƒâ€¹Ã¢â‚¬Â  project index, Product Backlog (+4 more)

### Community 23 - "KnowledgeRegistry"
Cohesion: 0.30
Nodes (8): KnowledgeClaim, KnowledgeConcept, KnowledgeRegistry, Claims, Concepts, Rules, KnowledgeRule, KnowledgeSource

### Community 25 - "طراحی Rule Engine"
Cohesion: 0.18
Nodes (10): Correctness, Design, Performance, Schema پیشنهادی Rule, اصل اصلی, انواع Rule, سطوح اطمینان, طراحی Rule Engine (+2 more)

### Community 27 - ".Analyze"
Cohesion: 0.18
Nodes (10): EfLinqAnalyzer, EfQueryAnalysis, EfTranslationStatus, ClientEvaluationRisk, Translatable, Unknown, EfDbContextModel, EfEntityModel (+2 more)

### Community 28 - "ماتریس پوشش C# و CLR"
Cohesion: 0.20
Nodes (9): 1. اجرای CLR، assembly و deployment, 2. مدل انواع و dispatch, 3. انواع ضروری و امکانات زبان, 4. exception، memory و resource lifetime, 5. hosting، loading، reflection و serialization, 6. threading و asynchronous execution, استفاده در پیشنهادها, قرارداد پوشش (+1 more)

### Community 29 - "مشارکت و Git Flow"
Cohesion: 0.22
Nodes (8): جریان feature, جریان hotfix, جریان release, شاخه‌ها, قواعد تغییر, مشارکت و Git Flow, نام‌گذاری commit, نسخه‌گذاری

### Community 30 - "Schemaهای داده"
Cohesion: 0.22
Nodes (8): Compatibility, GraphEdge, GraphNode, KnowledgeReference, RuleDescriptor, Schemaهای داده, Settings, شناسه‌ها

### Community 32 - "Ponytail Help"
Cohesion: 0.25
Nodes (7): Configure Default Mode, Deactivate, Levels, More, Ponytail Help, Skills, Update

### Community 33 - "طراحی Knowledge Pipeline"
Cohesion: 0.25
Nodes (7): Blog crawler, Manifest, تولید Rule, شناسه و نسخه, طراحی Knowledge Pipeline, مراحل ingestion, منابع

### Community 34 - "قرارداد دقت پیشنهاد"
Cohesion: 0.29
Nodes (6): Quality gates, SuggestionContract, SuggestionEvidence, قرارداد دقت پیشنهاد, قواعد اجباری, منابع دانشی

### Community 35 - "طراحی ارزیابی و کیفیت پیشنهادها"
Cohesion: 0.29
Nodes (6): Feedback کاربر, آستانه اولیه نسخه آزمایشی, تست‌های لازم, طراحی ارزیابی و کیفیت پیشنهادها, مجموعه آزمون, معیارهای اصلی

### Community 36 - "ConversionQuality"
Cohesion: 0.33
Nodes (6): ConversionQuality, Best, Good, Invalid, RequiresRuntimeCheck, Uncertain

### Community 37 - "Ponytail, lazy senior dev mode"
Cohesion: 0.40
Nodes (4): Before you write, graphify, Ponytail, lazy senior dev mode, The smallest complete change

### Community 38 - "ponytail-audit/SKILL.md"
Cohesion: 0.40
Nodes (4): 1. Map first, 2. Look for, 3. Check before you report, 4. Output

### Community 39 - "Ponytail Gain"
Cohesion: 0.40
Nodes (4): Boundaries, Honesty boundary, Ponytail Gain, Scoreboard

### Community 40 - "ponytail-review/SKILL.md"
Cohesion: 0.40
Nodes (4): 1. Understand first, 2. Look for, 3. Check before you report, 4. Output

### Community 41 - "Ponytail"
Cohesion: 0.40
Nodes (4): Before you write, Levels, Ponytail, The smallest complete change

### Community 42 - "سیاست نسخه‌گذاری"
Cohesion: 0.40
Nodes (4): سیاست نسخه‌گذاری, منبع نسخه, نسخه‌های بعدی پیشنهادی, نگاشت تغییر به نسخه

### Community 43 - "ponytail-debt/SKILL.md"
Cohesion: 0.50
Nodes (3): Boundaries, Output, Scan

## Knowledge Gaps
- **176 isolated node(s):** `net10.0`, `Microsoft.NET.Sdk`, `net10.0`, `Microsoft.NET.Sdk`, `net10.0` (+171 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 483 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **14 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `DotnetExpertAdvisor.Shared` connect `DotnetExpertAdvisor.Shared` to `قرارداد LSP و مدل پاسخ پیشنهادها`, `Contracts.cs`, `ImmutableArray`, `Binder`, `LanguageFeature`, `NuGetProjectIndex.cs`, `قراردادهای داخلی سامانه`, `EditorFeatures.cs`, `AssemblyMetadataReader.cs`, `.Tokenize`, `KnowledgeRegistry`, `KnowledgeCrawler`, `.Analyze`?**
  _High betweenness centrality (0.228) - this node is a cross-community bridge._
- **Are the 3 inferred relationships involving `ConversionClassifier` (e.g. with `MemberLookup` and `.ValidateConversionClassifier()`) actually correct?**
  _`ConversionClassifier` has 3 INFERRED edges - model-reasoned connections that need verification._
- **What connects `net10.0`, `Microsoft.NET.Sdk`, `net10.0` to the rest of the system?**
  _176 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `قرارداد LSP و مدل پاسخ پیشنهادها` be split into smaller, more focused modules?**
  _Cohesion score 0.07954545454545454 - nodes in this community are weakly interconnected._
- **Why does `RoslynHost` connect `Contracts.cs` to `معماری سامانه هوشمندی C# و .NET برای VS Code`, `قراردادهای داخلی سامانه`?**
  _High betweenness centrality (0.060) - this node is a cross-community bridge._
- **Should `Shared.csproj` be split into smaller, more focused modules?**
  _Cohesion score 0.07301587301587302 - nodes in this community are weakly interconnected._
- **Why does `3. C# Compiler Intelligence Host` connect `معماری سامانه هوشمندی C# و .NET برای VS Code` to `Contracts.cs`?**
  _High betweenness centrality (0.045) - this node is a cross-community bridge._