using System.Collections.Immutable;

namespace DotnetExpertAdvisor.Shared;

public sealed record EfProviderInfo(string Id, string DisplayName, string PackageId, string? Version);

public static class EfProviderRegistry
{
    private static readonly ImmutableDictionary<string, string> Known = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["Microsoft.EntityFrameworkCore.SqlServer"] = "SQL Server",
        ["Npgsql.EntityFrameworkCore.PostgreSQL"] = "PostgreSQL",
        ["Pomelo.EntityFrameworkCore.MySql"] = "MySQL",
        ["Microsoft.EntityFrameworkCore.Sqlite"] = "SQLite",
        ["Microsoft.EntityFrameworkCore.Cosmos"] = "Cosmos DB"
    }.ToImmutableDictionary(StringComparer.OrdinalIgnoreCase);

    public static ImmutableArray<EfProviderInfo> Detect(NuGetProjectIndex project)
    {
        ArgumentNullException.ThrowIfNull(project);
        return project.Packages.Where(package => Known.ContainsKey(package.Id))
            .Select(package => new EfProviderInfo(package.Id, Known[package.Id], package.Id, package.Version))
            .ToImmutableArray();
    }
}
