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

        var translated = EfTranslationMatrix.Analyze(
            "db.Customers.Where(c => c.Id == 1).Select(c => c.Name)", "Customers",
            "Npgsql.EntityFrameworkCore.PostgreSQL", "9.0.0");
        if (translated.Status != EfTranslationStatus.Translatable || translated.Provider != "PostgreSQL")
            throw new InvalidOperationException("Provider translation matrix did not accept a supported provider/version.");

        if (EfTranslationMatrix.Analyze("db.Customers.Where(c => c.Id == 1).Select(c => c.Name)", "Customers",
                "Unknown.Provider", "9.0.0").Status != EfTranslationStatus.Unknown)
            throw new InvalidOperationException("Unknown provider must not receive a translation verdict.");

        if (EfTranslationMatrix.Analyze("db.Customers.Where(c => c.Id == 1).Select(c => c.Name)", "Customers",
                "Npgsql.EntityFrameworkCore.PostgreSQL", "5.0.0").Status != EfTranslationStatus.Unknown)
            throw new InvalidOperationException("Unsupported provider version must not receive a translation verdict.");
    }
}
