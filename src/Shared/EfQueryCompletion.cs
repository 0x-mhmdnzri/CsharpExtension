using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace DotnetExpertAdvisor.Shared;

public sealed record EfQueryCompletionResult(bool IsSupported, ImmutableArray<string> Candidates, string? Reason);

public static partial class EfQueryCompletion
{
    public static EfQueryCompletionResult Complete(string query, EfModelSnapshot model)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(model);

        var root = RootPattern().Match(query);
        if (!root.Success)
            return Unknown("The query root is not a known EF DbSet.");

        var entity = FindDbSetEntity(model, root.Groups[1].Value);
        if (entity is null)
            return Unknown("The query root is not a known EF DbSet.");

        var include = IncludePattern().Match(query);
        var thenInclude = ThenIncludePattern().Match(query);
        if (!include.Success && !thenInclude.Success)
            return Unknown("Completion is available inside Include or ThenInclude.");

        if (thenInclude.Success)
        {
            var navigationName = thenInclude.Groups[1].Value;
            var navigation = entity.Navigations.FirstOrDefault(item => item.Name == navigationName);
            if (navigation is null)
                return Unknown("The Include navigation is not part of the model.");
            entity = model.Entities.FirstOrDefault(item => item.Name == navigation.TargetType);
            if (entity is null)
                return Unknown("The Include target type is not part of the model.");
        }

        return new(true, entity.Navigations.Select(navigation => navigation.Name)
            .Distinct(StringComparer.Ordinal).OrderBy(name => name, StringComparer.Ordinal).ToImmutableArray(), null);
    }

    private static EfQueryCompletionResult Unknown(string reason)
        => new(false, ImmutableArray<string>.Empty, reason);

    private static EfEntityModel? FindDbSetEntity(EfModelSnapshot model, string dbSetName)
    {
        foreach (var context in model.Contexts)
        {
            var set = context.DbSets.FirstOrDefault(item => item.Value == dbSetName);
            if (set is null || set.Origin is null) continue;
            var match = DbSetTypePattern().Match(set.Origin);
            if (match.Success)
                return model.Entities.FirstOrDefault(entity => entity.Name == match.Groups[1].Value);
        }
        return null;
    }

    [GeneratedRegex(@"\b(?:db|context)\.(\w+)")]
    private static partial Regex RootPattern();

    [GeneratedRegex(@"\.Include\s*\(\s*\w+\s*=>\s*\w+\.(\w+)\s*$")]
    private static partial Regex IncludePattern();

    [GeneratedRegex(@"\.ThenInclude\s*\(\s*\w+\s*=>\s*\w+\.(\w+)\s*$")]
    private static partial Regex ThenIncludePattern();

    [GeneratedRegex(@"DbSet<\s*(\w+)\s*>")]
    private static partial Regex DbSetTypePattern();
}
