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
    }
}
