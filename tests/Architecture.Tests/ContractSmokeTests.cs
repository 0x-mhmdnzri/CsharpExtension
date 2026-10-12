using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class ContractSmokeTests
{
    public static void ValidateCoreRecords()
    {
        var request = new DocumentRequest(
            new WorkspaceId("smoke"),
            new DocumentId("document:test"),
            new ProjectId("project:test"),
            new TargetFrameworkId("net10.0"),
            1,
            "class C {}");

        if (request.Version != 1 || request.TargetFramework.Value != "net10.0")
            throw new InvalidOperationException("Shared contract smoke test failed.");
    }
}
