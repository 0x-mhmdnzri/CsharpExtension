using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace DotnetExpertAdvisor.Shared;

public enum EfProvenance
{
    Convention,
    DataAnnotation,
    FluentApi,
    ModelSnapshot,
    CompiledModel,
    RuntimeProbe
}

public enum EfConfidence
{
    High,
    Medium,
    Low
}

public sealed record EfFact<T>(
    T Value,
    EfProvenance Source,
    EfConfidence Confidence,
    string? Origin = null);

public sealed record EfEntityModel(
    string Name,
    ImmutableArray<EfFact<string>> Properties);

public sealed record EfDbContextModel(
    string Name,
    ImmutableArray<EfFact<string>> DbSets,
    bool IsComplete);

public sealed record EfModelSnapshot(
    ImmutableArray<EfDbContextModel> Contexts,
    ImmutableArray<EfEntityModel> Entities,
    bool IsComplete);

public static partial class EfModelAnalyzer
{
    public static EfModelSnapshot Analyze(string source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var incomplete = IncompletePattern().IsMatch(source);
        var contextMatches = ContextPattern().Matches(source);
        var dbSetMatches = DbSetPattern().Matches(source);

        var contexts = contextMatches.Select(match =>
        {
            var sets = dbSetMatches
                .Select(set => new EfFact<string>(
                    set.Groups[2].Value,
                    EfProvenance.Convention,
                    EfConfidence.High,
                    $"DbSet<{set.Groups[1].Value}>"))
                .ToImmutableArray();
            return new EfDbContextModel(match.Groups[1].Value, sets, IsComplete: !incomplete && sets.Length > 0);
        }).ToImmutableArray();

        var dbSetNames = contexts
            .SelectMany(context => context.DbSets.Select(set => set.Value))
            .ToHashSet(StringComparer.Ordinal);

        var entities = ClassPattern().Matches(source)
            .Select(match =>
            {
                var properties = PropertyPattern().Matches(source)
                    .Select(property => property.Groups[1].Value)
                    .Where(property => !dbSetNames.Contains(property))
                    .Select(property => new EfFact<string>(
                        property,
                        DetectPropertyProvenance(source, property),
                        EfConfidence.High,
                        property))
                    .ToImmutableArray();
                return new EfEntityModel(match.Groups[1].Value, properties);
            })
            .Where(entity => contexts.All(context => context.Name != entity.Name))
            .ToImmutableArray();

        var complete = !incomplete
            && contexts.Length > 0
            && contexts.All(context => context.IsComplete);

        return new EfModelSnapshot(contexts, entities, complete);
    }

    private static EfProvenance DetectPropertyProvenance(string source, string propertyName)
    {
        if (source.Contains($"[Key]", StringComparison.Ordinal)
            && source.Contains(propertyName, StringComparison.Ordinal))
            return EfProvenance.DataAnnotation;
        if (source.Contains($"Property(x => x.{propertyName})", StringComparison.Ordinal)
            || source.Contains($"Property(e => e.{propertyName})", StringComparison.Ordinal))
            return EfProvenance.FluentApi;
        return EfProvenance.Convention;
    }

    [GeneratedRegex(@"\bclass\s+(\w+)\s*:\s*DbContext\b")]
    private static partial Regex ContextPattern();

    [GeneratedRegex(@"\bDbSet\s*<\s*(\w+)\s*>\s+(\w+)")]
    private static partial Regex DbSetPattern();

    [GeneratedRegex(@"\bclass\s+(\w+)\b")]
    private static partial Regex ClassPattern();

    [GeneratedRegex(@"\b(\w+)\s*\{\s*get\s*;")]
    private static partial Regex PropertyPattern();

    // Markers that static analysis cannot fully recover the EF model.
    [GeneratedRegex(@"\b(Assembly\.Load|Activator\.CreateInstance|dynamic\b|ModelBuilder\s+\w+\s*=)")]
    private static partial Regex IncompletePattern();
}
