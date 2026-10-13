using System.Collections.Immutable;

namespace DotnetExpertAdvisor.Shared;

public enum EfRiskSeverity { Info, Warning }
public sealed record EfPerformanceFinding(EfRiskSeverity Severity, string RuleId, string Message);

public static class EfPerformanceAnalyzer
{
    public static ImmutableArray<EfPerformanceFinding> AnalyzeQuery(string query)
    {
        ArgumentNullException.ThrowIfNull(query);
        var findings = ImmutableArray.CreateBuilder<EfPerformanceFinding>();
        if (query.Contains("ToList().Where", StringComparison.Ordinal) || query.Contains("ToArray().Where", StringComparison.Ordinal))
            findings.Add(new(EfRiskSeverity.Warning, "EF001", "Materialization occurs before filtering; move Where before ToList/ToArray."));
        if (query.Contains("AsTracking", StringComparison.Ordinal) && !query.Contains("Update", StringComparison.Ordinal))
            findings.Add(new(EfRiskSeverity.Info, "EF002", "Read-only query appears tracked; consider AsNoTracking."));
        if (query.Contains("Include(", StringComparison.Ordinal) && query.Count(character => character == '.') > 8)
            findings.Add(new(EfRiskSeverity.Warning, "EF003", "Deep include/query chain may create an expensive SQL shape."));
        return findings.ToImmutable();
    }

    public static bool IsMigrationClass(string source)
        => source.Contains(": Migration", StringComparison.Ordinal) &&
           source.Contains("Up(MigrationBuilder", StringComparison.Ordinal) &&
           source.Contains("Down(MigrationBuilder", StringComparison.Ordinal);
}
