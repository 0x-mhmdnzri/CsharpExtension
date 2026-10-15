using System.Collections.Immutable;

namespace DotnetExpertAdvisor.Shared;

public sealed record EfTranslationVerdict(
    EfTranslationStatus Status,
    string? Sql,
    string? Reason,
    string Provider,
    Version? ProviderVersion);

/// <summary>Applies provider/version gates to the small LINQ translation surface.</summary>
public static class EfTranslationMatrix
{
    private static readonly ImmutableDictionary<string, string> Providers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["Microsoft.EntityFrameworkCore.SqlServer"] = "SQL Server",
        ["Npgsql.EntityFrameworkCore.PostgreSQL"] = "PostgreSQL",
        ["Pomelo.EntityFrameworkCore.MySql"] = "MySQL",
        ["Microsoft.EntityFrameworkCore.Sqlite"] = "SQLite",
        ["Microsoft.EntityFrameworkCore.Cosmos"] = "Cosmos DB"
    }.ToImmutableDictionary(StringComparer.OrdinalIgnoreCase);

    public static EfTranslationVerdict Analyze(string query, string table, string provider, string providerVersion)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(provider);
        ArgumentException.ThrowIfNullOrWhiteSpace(providerVersion);

        if (!Providers.TryGetValue(provider, out var displayName))
            return new(EfTranslationStatus.Unknown, null, $"Unknown EF provider: {provider}.", provider, null);
        if (!Version.TryParse(providerVersion, out var version))
            return new(EfTranslationStatus.Unknown, null, $"Invalid provider version: {providerVersion}.", displayName, null);
        if (version.Major < 6)
            return new(EfTranslationStatus.Unknown, null, "EF provider versions before 6 are outside the translation matrix.", displayName, version);

        var result = EfLinqAnalyzer.Analyze(query, table);
        return new(result.Status, result.Sql, result.Reason, displayName, version);
    }
}
