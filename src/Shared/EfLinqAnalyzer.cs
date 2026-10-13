using System.Text.RegularExpressions;

namespace DotnetExpertAdvisor.Shared;

public enum EfTranslationStatus { Translatable, Unknown, ClientEvaluationRisk }
public sealed record EfQueryAnalysis(EfTranslationStatus Status, string? Sql, string? Reason);

public static partial class EfLinqAnalyzer
{
    public static EfQueryAnalysis Analyze(string query, string table)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(query);
        ArgumentException.ThrowIfNullOrWhiteSpace(table);
        var match = WhereSelectPattern().Match(query);
        if (!match.Success) return new(EfTranslationStatus.Unknown, null, "Only Where(...).Select(...) is recognized.");
        var predicate = match.Groups["predicate"].Value;
        var projection = match.Groups["projection"].Value;
        if (predicate.Contains("DateTime.Now", StringComparison.Ordinal) || predicate.Contains("ToString(", StringComparison.Ordinal))
            return new(EfTranslationStatus.ClientEvaluationRisk, null, "The expression may require client evaluation.");
        var sql = $"SELECT {projection} FROM {table} WHERE {predicate.Replace("==", "=").Replace("&&", " AND ").Replace("||", " OR ")}";
        return new(EfTranslationStatus.Translatable, sql, null);
    }

    [GeneratedRegex(@"\.Where\s*\(\s*\w+\s*=>\s*(?<predicate>[^)]*)\)\s*\.Select\s*\(\s*\w+\s*=>\s*(?<projection>[^)]*)\)", RegexOptions.Singleline)]
    private static partial Regex WhereSelectPattern();
}
