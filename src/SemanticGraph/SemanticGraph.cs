using DotnetExpertAdvisor.Shared;
namespace DotnetExpertAdvisor.SemanticGraph;
public interface ISemanticGraph { GraphSnapshot GetSnapshot(WorkspaceId workspaceId); Task<GraphSnapshot> UpdateAsync(DocumentAnalysis analysis, CancellationToken cancellationToken); }
public sealed class SemanticGraph : ISemanticGraph
{
    private readonly object _gate = new(); private readonly Dictionary<WorkspaceId, GraphSnapshot> _snapshots = new();
    public GraphSnapshot GetSnapshot(WorkspaceId workspaceId) { lock (_gate) { return _snapshots.TryGetValue(workspaceId, out var snapshot) ? snapshot : new GraphSnapshot(workspaceId, 0, Array.Empty<GraphNode>(), Array.Empty<GraphEdge>()); } }
    public Task<GraphSnapshot> UpdateAsync(DocumentAnalysis analysis, CancellationToken cancellationToken) { cancellationToken.ThrowIfCancellationRequested(); lock (_gate) { var current = GetSnapshot(analysis.Request.WorkspaceId); var nodes = analysis.Symbols.Select(symbol => new GraphNode(symbol.Id, symbol.Kind, symbol.Name, analysis.Request.ProjectId)).ToArray(); var updated = current with { Generation = current.Generation + 1, Nodes = nodes }; _snapshots[analysis.Request.WorkspaceId] = updated; return Task.FromResult(updated); } }
}
