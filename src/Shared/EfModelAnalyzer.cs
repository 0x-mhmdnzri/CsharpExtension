using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace DotnetExpertAdvisor.Shared;

public sealed record EfEntityModel(string Name, ImmutableArray<string> Properties);
public sealed record EfDbContextModel(string Name, ImmutableArray<string> DbSets);
public sealed record EfModelSnapshot(ImmutableArray<EfDbContextModel> Contexts,
    ImmutableArray<EfEntityModel> Entities);

public static partial class EfModelAnalyzer
{
    public static EfModelSnapshot Analyze(string source)
    {
        ArgumentNullException.ThrowIfNull(source);
        var contexts = ContextPattern().Matches(source).Select(match =>
            new EfDbContextModel(match.Groups[1].Value,
                DbSetPattern().Matches(source).Select(set => set.Groups[2].Value).ToImmutableArray()))
            .ToImmutableArray();
        var dbSetNames = contexts.SelectMany(context => context.DbSets).ToHashSet(StringComparer.Ordinal);
        var entities = ClassPattern().Matches(source).Select(match =>
            new EfEntityModel(match.Groups[1].Value,
                PropertyPattern().Matches(source).Select(property => property.Groups[1].Value)
                    .Where(property => !dbSetNames.Contains(property)).ToImmutableArray()))
            .Where(entity => contexts.All(context => context.Name != entity.Name)).ToImmutableArray();
        return new EfModelSnapshot(contexts, entities);
    }

    [GeneratedRegex(@"\bclass\s+(\w+)\s*:\s*DbContext\b")]
    private static partial Regex ContextPattern();
    [GeneratedRegex(@"\bDbSet\s*<\s*(\w+)\s*>\s+(\w+)")]
    private static partial Regex DbSetPattern();
    [GeneratedRegex(@"\bclass\s+(\w+)\b")]
    private static partial Regex ClassPattern();
    [GeneratedRegex(@"\b(\w+)\s*\{\s*get\s*;")]
    private static partial Regex PropertyPattern();
}
