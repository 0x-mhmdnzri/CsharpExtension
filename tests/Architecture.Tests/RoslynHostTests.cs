using DotnetExpertAdvisor.RoslynHost;
using DotnetExpertAdvisor.Shared;
using RoslynCompletionHost = DotnetExpertAdvisor.RoslynHost.RoslynHost;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class RoslynHostTests
{
    public static async Task RunAsync()
    {
        var text = "class Customer { public string Name { get; set; } } class C { void M() { Cu } }";
        var request = new DocumentRequest(new WorkspaceId("test"), new DocumentId("test.cs"),
            new ProjectId("test"), new TargetFrameworkId("net10.0"), 1, text);
        var result = await new RoslynCompletionHost().GetCompletionAsync(request, 0, text.IndexOf("Cu", StringComparison.Ordinal) + 2, CancellationToken.None);

        if (!result.Candidates.Any(candidate => candidate.Label == "Customer"))
            throw new InvalidOperationException("Roslyn completion did not return the declared type.");
        if (result.Candidates.Any(candidate => candidate.Label == "NotARealSymbol"))
            throw new InvalidOperationException("Roslyn completion returned a fabricated symbol.");

        var overloadText = "class C { void Do(int value) {} void Do(string value) {} void M() { Do } }";
        var overloadRequest = request with { Text = overloadText };
        var overloads = await new RoslynCompletionHost().GetCompletionAsync(overloadRequest, 0,
            overloadText.LastIndexOf("Do", StringComparison.Ordinal) + 2, CancellationToken.None);
        var doCandidates = overloads.Candidates.Where(candidate => candidate.Label == "Do").ToArray();
        if (doCandidates.Length != 2 || doCandidates.Any(candidate => candidate.EditLength != 2 || candidate.EditStart < 0
            || candidate.SymbolId.Length == 0 || candidate.LspKind != 2))
            throw new InvalidOperationException($"Roslyn candidate normalization lost overload identity or edit range: {string.Join(";", doCandidates.Select(candidate => $"{candidate.Label}/{candidate.Detail}/{candidate.EditStart}/{candidate.EditLength}/{candidate.LspKind}"))}");
    }
}
