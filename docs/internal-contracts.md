# قراردادهای داخلی سامانه

## مرزهای ماژول‌ها

```text
LanguageServer
  ├── Protocol
  ├── AnalysisCoordinator
  │     ├── RoslynHost
  │     ├── SemanticGraph
  │     ├── RuntimeModel
  │     └── RuleEngine
  ├── RankingEngine
  └── KnowledgeEngine
```

هر ماژول فقط از قرارداد عمومی ماژول پایین‌تر استفاده می‌کند. `LanguageServer` نباید مستقیماً Syntax API را فراخوانی کند؛ این کار از طریق `RoslynHost` و contextهای immutable انجام می‌شود.

## مدل‌های اصلی

### DocumentContext

```csharp
public sealed record DocumentContext(
    DocumentId DocumentId,
    ProjectId ProjectId,
    Uri Uri,
    VersionStamp Version,
    SourceText Text,
    TextSpan RequestedSpan,
    TargetFrameworkId TargetFramework);
```

### SemanticContext

```csharp
public sealed record SemanticContext(
    DocumentContext Document,
    SyntaxNode Root,
    SemanticModel SemanticModel,
    ImmutableArray<ISymbol> EnclosingSymbols,
    ImmutableArray<IOperation> NearbyOperations,
    ProjectSnapshot Project,
    GraphSnapshot Graph);
```

`SemanticContext` فقط برای یک درخواست معتبر است و پس از تغییر document نباید دوباره استفاده شود.

### SuggestionCandidate

```csharp
public sealed record SuggestionCandidate(
    string Id,
    string RuleId,
    SuggestionCategory Category,
    DiagnosticSeverity Severity,
    TextEdit? Edit,
    string Title,
    string Rationale,
    double Confidence,
    ImmutableArray<Evidence> Evidence,
    ImmutableArray<KnowledgeReference> References,
    ImmutableArray<string> ApplicableFrameworks);
```

### Evidence

```csharp
public sealed record Evidence(
    EvidenceKind Kind,
    string Value,
    DocumentSpan? Location,
    string? Description);
```

Evidence باید قابل بازتولید باشد. رشته‌ای که به symbol، operation یا project snapshot اشاره نمی‌کند، برای rule فعال معتبر نیست.

## قرارداد RoslynHost

```csharp
public interface IRoslynHost
{
    Task<WorkspaceSnapshot> LoadWorkspaceAsync(
        WorkspaceRequest request,
        CancellationToken cancellationToken);

    Task<DocumentAnalysis> AnalyzeDocumentAsync(
        DocumentRequest request,
        CancellationToken cancellationToken);

    Task<CompilationResult> GetCompilationAsync(
        ProjectId projectId,
        TargetFrameworkId targetFramework,
        CancellationToken cancellationToken);

    Task<CodeFixResult> ApplyCodeFixAsync(
        CodeFixRequest request,
        CancellationToken cancellationToken);
}
```

RoslynHost مالک `Workspace` و `Compilation` است. هیچ ماژول دیگری حق dispose یا mutation مستقیم آن‌ها را ندارد.

## قرارداد SemanticGraph

```csharp
public interface ISemanticGraph
{
    GraphSnapshot GetSnapshot(ProjectId projectId);

    Task<GraphDelta> UpdateAsync(
        SemanticDelta delta,
        CancellationToken cancellationToken);

    ImmutableArray<GraphNode> FindDependents(
        GraphNodeId nodeId,
        GraphQueryOptions options);

    ImmutableArray<GraphPath> FindPaths(
        GraphNodeId source,
        GraphNodeId target,
        int maxDepth);
}
```

Graph update باید idempotent باشد. دریافت دوباره یک `DocumentVersion` نباید edge یا node تکراری ایجاد کند.

## قرارداد RuleEngine

```csharp
public interface IRuleEngine
{
    ValueTask<ImmutableArray<SuggestionCandidate>> EvaluateAsync(
        SemanticContext context,
        RuleEvaluationOptions options,
        CancellationToken cancellationToken);

    RuleDescriptor GetRule(string ruleId, SemVersion version);

    ImmutableArray<RuleDescriptor> GetApplicableRules(
        TargetFrameworkId targetFramework,
        RuleCategory category);
}
```

RuleEngine نباید فایل، network یا editor state را مستقیماً بخواند. همه ورودی‌ها باید در `SemanticContext` یا `KnowledgeSnapshot` موجود باشند.

## قرارداد RankingEngine

```csharp
public interface IRankingEngine
{
    ImmutableArray<RankedSuggestion> Rank(
        ImmutableArray<SuggestionCandidate> candidates,
        RankingContext context);
}
```

RankingEngine باید deterministic باشد. یک input یکسان با یک نسخه engine و pack یکسان، باید ترتیب یکسان تولید کند.

## قرارداد KnowledgeEngine

```csharp
public interface IKnowledgeEngine
{
    KnowledgeSnapshot GetSnapshot();

    KnowledgeReference? Resolve(string reference);

    ImmutableArray<RuleDescriptor> GetRulesForConcept(
        string conceptId,
        TargetFrameworkId targetFramework);
}
```

KnowledgeEngine در مسیر completion فقط read-only است. تعویض pack در یک generation جدید انجام می‌شود و درخواست‌های در حال اجرا از snapshot قبلی استفاده می‌کنند.

## جریان درخواست completion

```text
LSP request
  → RequestCoordinator assigns request/version
  → RoslynHost creates SemanticContext
  → SemanticGraph snapshot is attached
  → RuleEngine emits candidates
  → Built-in Roslyn candidates are normalized
  → RankingEngine scores and deduplicates
  → LSP adapter serializes result
```

اگر document version در پایان درخواست با version جاری برابر نباشد، پاسخ به‌عنوان stale حذف می‌شود.

## خطا و cancellation

- cancellation عادی، خطا محسوب نمی‌شود و log سطح Debug دارد.
- خطای Roslyn برای همان document به diagnostic داخلی تبدیل می‌شود و server را متوقف نمی‌کند.
- خطای corruption در knowledge pack باعث rollback به آخرین pack سالم می‌شود.
- خطای protocol باید error code پایدار و trace id داشته باشد.

## observability

هر درخواست باید این شناسه‌ها را داشته باشد:

- `traceId`
- `workspaceId`
- `documentVersion`
- `knowledgePackVersion`
- `ruleEngineVersion`

metrics حداقلی:

- completion latency به تفکیک سطح تحلیل
- تعداد candidate، حذف duplicate و false-positive feedback
- graph update duration
- Roslyn compilation cache hit rate
- memory usage و تعداد cancellation

