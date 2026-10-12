namespace DotnetExpertAdvisor.Shared;

public readonly record struct WorkspaceId(string Value);
public readonly record struct ProjectId(string Value);
public readonly record struct DocumentId(string Value);
public readonly record struct TargetFrameworkId(string Value);
public sealed record DocumentRequest(WorkspaceId WorkspaceId, DocumentId DocumentId, ProjectId ProjectId, TargetFrameworkId TargetFramework, int Version, string Text);
public sealed record WorkspaceRequest(WorkspaceId WorkspaceId, string RootPath, IReadOnlyList<string> ProjectPaths);
public sealed record WorkspaceSnapshot(WorkspaceId WorkspaceId, IReadOnlyList<ProjectSnapshot> Projects, DateTimeOffset CreatedAt);
public sealed record ProjectSnapshot(ProjectId Id, string Name, TargetFrameworkId TargetFramework, IReadOnlyList<string> Documents);
public sealed record DocumentAnalysis(DocumentRequest Request, IReadOnlyList<SymbolFact> Symbols, IReadOnlyList<OperationFact> Operations);
public sealed record SymbolFact(string Id, string Name, string Kind, string? ContainingSymbol);
public sealed record OperationFact(string Kind, string? SymbolId, int Start, int Length);
public sealed record GraphSnapshot(WorkspaceId WorkspaceId, long Generation, IReadOnlyList<GraphNode> Nodes, IReadOnlyList<GraphEdge> Edges);
public sealed record GraphNode(string Id, string Kind, string Name, ProjectId ProjectId);
public sealed record GraphEdge(string Id, string Source, string Target, string Kind);
public enum SuggestionCategory { Correctness, Design, Performance, Completion }
public enum SuggestionSeverity { Info, Hint, Warning, Error }
public sealed record Evidence(string Kind, string Value, string? Description = null);
public sealed record KnowledgeReference(string Id, string Title);
public sealed record SuggestionCandidate(string Id, string RuleId, SuggestionCategory Category, SuggestionSeverity Severity, string Title, string Rationale, double Confidence, IReadOnlyList<Evidence> Evidence, IReadOnlyList<KnowledgeReference> References);
public sealed record RankedSuggestion(SuggestionCandidate Candidate, double Score, int Rank);
