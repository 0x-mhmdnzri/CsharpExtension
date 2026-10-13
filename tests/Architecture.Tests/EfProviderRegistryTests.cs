using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class EfProviderRegistryTests
{
    public static void Run()
    {
        var project = new NuGetProjectIndex("sample.csproj", [
            new NuGetPackageReference("Npgsql.EntityFrameworkCore.PostgreSQL", "9.0.0", true, "none", "all"),
            new NuGetPackageReference("Other.Package", "1.0.0", true, "none", "all")], []);
        var providers = EfProviderRegistry.Detect(project);
        if (providers.Length != 1 || providers[0].DisplayName != "PostgreSQL" || providers[0].Version != "9.0.0")
            throw new InvalidOperationException("EF provider metadata was not detected.");
    }
}
