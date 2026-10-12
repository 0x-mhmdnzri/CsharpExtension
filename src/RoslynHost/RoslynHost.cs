using DotnetExpertAdvisor.Shared;
namespace DotnetExpertAdvisor.RoslynHost;
public interface IRoslynHost { Task<WorkspaceSnapshot> LoadWorkspaceAsync(WorkspaceRequest request, CancellationToken cancellationToken); Task<DocumentAnalysis> AnalyzeDocumentAsync(DocumentRequest request, CancellationToken cancellationToken); }
public sealed class RoslynHost : IRoslynHost
{
    public Task<WorkspaceSnapshot> LoadWorkspaceAsync(WorkspaceRequest request, CancellationToken cancellationToken) { cancellationToken.ThrowIfCancellationRequested(); var projects = request.ProjectPaths.Select(path => new ProjectSnapshot(new ProjectId(path), Path.GetFileNameWithoutExtension(path), new TargetFrameworkId("unknown"), Array.Empty<string>())).ToArray(); return Task.FromResult(new WorkspaceSnapshot(request.WorkspaceId, projects, DateTimeOffset.UtcNow)); }
    public Task<DocumentAnalysis> AnalyzeDocumentAsync(DocumentRequest request, CancellationToken cancellationToken) { cancellationToken.ThrowIfCancellationRequested(); return Task.FromResult(new DocumentAnalysis(request, Array.Empty<SymbolFact>(), Array.Empty<OperationFact>())); }
}
