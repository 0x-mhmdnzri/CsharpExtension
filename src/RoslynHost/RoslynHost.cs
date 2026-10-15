using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.RoslynHost;

public interface IRoslynHost
{
    Task<WorkspaceSnapshot> LoadWorkspaceAsync(WorkspaceRequest request, CancellationToken cancellationToken);
    Task<DocumentAnalysis> AnalyzeDocumentAsync(DocumentRequest request, CancellationToken cancellationToken);
    Task<RoslynCompletionResult> GetCompletionAsync(DocumentRequest request, int line, int character, CancellationToken cancellationToken);
}

public sealed record RoslynCompletionCandidate(string Label, string Kind, string Detail, string SymbolId);
public sealed record RoslynCompletionResult(ImmutableArray<RoslynCompletionCandidate> Candidates, ImmutableArray<string> Diagnostics);

public sealed class RoslynHost : IRoslynHost
{
    public Task<WorkspaceSnapshot> LoadWorkspaceAsync(WorkspaceRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var projects = request.ProjectPaths.Select(path => new ProjectSnapshot(new ProjectId(path),
            Path.GetFileNameWithoutExtension(path), new TargetFrameworkId("unknown"), Array.Empty<string>())).ToArray();
        return Task.FromResult(new WorkspaceSnapshot(request.WorkspaceId, projects, DateTimeOffset.UtcNow));
    }

    public Task<DocumentAnalysis> AnalyzeDocumentAsync(DocumentRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(new DocumentAnalysis(request, Array.Empty<SymbolFact>(), Array.Empty<OperationFact>()));
    }

    public Task<RoslynCompletionResult> GetCompletionAsync(DocumentRequest request, int line, int character, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var offset = OffsetAt(request.Text, line, character);
        if (offset < 0)
            return Task.FromResult(new RoslynCompletionResult(ImmutableArray<RoslynCompletionCandidate>.Empty,
                ImmutableArray.Create("Invalid document position.")));

        var tree = CSharpSyntaxTree.ParseText(request.Text, cancellationToken: cancellationToken);
        var compilation = CSharpCompilation.Create(request.ProjectId.Value, [tree], FrameworkReferences,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        var model = compilation.GetSemanticModel(tree, ignoreAccessibility: true);
        var prefix = CurrentPrefix(request.Text, offset);
        var candidates = model.LookupSymbols(offset)
            .Where(symbol => symbol.CanBeReferencedByName && symbol.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            .GroupBy(symbol => symbol.Name, StringComparer.Ordinal)
            .Select(group => group.OrderBy(symbol => symbol.Kind).First())
            .OrderBy(symbol => !symbol.Name.Equals(prefix, StringComparison.OrdinalIgnoreCase))
            .ThenBy(symbol => symbol.Name, StringComparer.OrdinalIgnoreCase)
            .Take(100)
            .Select(symbol => new RoslynCompletionCandidate(symbol.Name, symbol.Kind.ToString(),
                symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
                symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)))
            .ToImmutableArray();
        var diagnostics = compilation.GetDiagnostics(cancellationToken)
            .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
            .Select(diagnostic => diagnostic.Id).Distinct(StringComparer.Ordinal).ToImmutableArray();
        return Task.FromResult(new RoslynCompletionResult(candidates, diagnostics));
    }

    private static readonly ImmutableArray<MetadataReference> FrameworkReferences =
    [
        MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
        MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
        MetadataReference.CreateFromFile(typeof(Task).Assembly.Location)
    ];

    private static string CurrentPrefix(string text, int offset)
    {
        var start = offset;
        while (start > 0 && (char.IsLetterOrDigit(text[start - 1]) || text[start - 1] == '_')) start--;
        return text[start..offset];
    }

    private static int OffsetAt(string text, int line, int character)
    {
        if (line < 0 || character < 0) return -1;
        var offset = 0;
        for (var currentLine = 0; currentLine < line; currentLine++)
        {
            var newline = text.IndexOf('\n', offset);
            if (newline < 0) return -1;
            offset = newline + 1;
        }
        var lineEnd = text.IndexOf('\n', offset);
        if (lineEnd < 0) lineEnd = text.Length;
        return offset + Math.Min(character, lineEnd - offset);
    }
}
