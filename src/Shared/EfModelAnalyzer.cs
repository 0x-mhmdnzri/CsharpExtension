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

public sealed record EfNavigation(
    string Name,
    string TargetType,
    bool IsCollection,
    EfProvenance Source);

public sealed record EfEntityModel(
    string Name,
    ImmutableArray<EfFact<string>> Properties,
    ImmutableArray<EfFact<string>> Keys,
    ImmutableArray<EfNavigation> Navigations,
    ImmutableArray<EfFact<string>> ForeignKeys,
    bool IsKeyless);

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

        var entityTypeNames = ClassPattern().Matches(source)
            .Select(match => match.Groups[1].Value)
            .Where(name => contexts.All(context => context.Name != name))
            .ToHashSet(StringComparer.Ordinal);

        var entities = ClassPattern().Matches(source)
            .Select(match => BuildEntity(source, match.Groups[1].Value, dbSetNames, entityTypeNames))
            .Where(entity => contexts.All(context => context.Name != entity.Name))
            .ToImmutableArray();

        var complete = !incomplete
            && contexts.Length > 0
            && contexts.All(context => context.IsComplete);

        var snapshot = new EfModelSnapshot(contexts, entities, complete);
        return EfFluentInterpreter.Apply(snapshot, source);
    }

    private static EfEntityModel BuildEntity(
        string source,
        string entityName,
        HashSet<string> dbSetNames,
        HashSet<string> entityTypeNames)
    {
        var isKeyless = KeylessPattern().IsMatch(source)
            && source.Contains($"class {entityName}", StringComparison.Ordinal);

        var properties = PropertyPattern().Matches(source)
            .Select(property => property.Groups[1].Value)
            .Where(property => !dbSetNames.Contains(property))
            .Where(property => !entityTypeNames.Contains(property))
            .Distinct(StringComparer.Ordinal)
            .Select(property => new EfFact<string>(
                property,
                DetectPropertyProvenance(source, property),
                EfConfidence.High,
                property))
            .ToImmutableArray();

        var annotationKeys = KeyAnnotationPattern().Matches(source)
            .Select(match => match.Groups[1].Value)
            .Select(name => new EfFact<string>(name, EfProvenance.DataAnnotation, EfConfidence.High, "[Key]"))
            .ToImmutableArray();

        var conventionKeys = isKeyless || annotationKeys.Length > 0
            ? ImmutableArray<EfFact<string>>.Empty
            : properties
                .Where(property =>
                    property.Value == "Id"
                    || property.Value.Equals(entityName + "Id", StringComparison.Ordinal))
                .Select(property => new EfFact<string>(property.Value, EfProvenance.Convention, EfConfidence.High, "by convention"))
                .ToImmutableArray();

        var entityAnnotationKeys = annotationKeys
            .Where(key => properties.Any(property => property.Value == key.Value))
            .ToImmutableArray();
        var keys = entityAnnotationKeys.Length > 0 ? entityAnnotationKeys : conventionKeys;

        var navigations = NavigationPattern().Matches(source)
            .Select(match =>
            {
                var isCollection = match.Groups["collType"].Success;
                var target = isCollection ? match.Groups["collType"].Value : match.Groups["refType"].Value;
                var name = isCollection ? match.Groups["collName"].Value : match.Groups["refName"].Value;
                if (string.IsNullOrEmpty(target) || !entityTypeNames.Contains(target))
                    return null;
                return new EfNavigation(name, target, isCollection, EfProvenance.Convention);
            })
            .Where(navigation => navigation is not null)
            .Select(navigation => navigation!)
            .ToImmutableArray();

        var foreignKeys = ForeignKeyAnnotationPattern().Matches(source)
            .Select(match => new EfFact<string>(match.Groups[1].Value, EfProvenance.DataAnnotation, EfConfidence.High, "[ForeignKey]"))
            .Concat(properties
                .Where(property => property.Value.EndsWith("Id", StringComparison.Ordinal) && property.Value.Length > 2)
                .Where(property => keys.All(key => key.Value != property.Value))
                .Select(property => new EfFact<string>(property.Value, EfProvenance.Convention, EfConfidence.Medium, "shadow/FK convention")))
            .GroupBy(fact => fact.Value, StringComparer.Ordinal)
            .Select(group => group.First())
            .ToImmutableArray();

        return new EfEntityModel(entityName, properties, keys, navigations, foreignKeys, isKeyless);
    }

    private static EfProvenance DetectPropertyProvenance(string source, string propertyName)
    {
        if (source.Contains("[Key]", StringComparison.Ordinal)
            && Regex.IsMatch(source, $"\\[Key\\]\\s*(?:public\\s+)?(?:\\w+\\s+)?{Regex.Escape(propertyName)}\\s*\\{{"))
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

    [GeneratedRegex(@"\[Key\]\s*(?:public\s+)?(?:\w+\s+)?(\w+)\s*\{\s*get")]
    private static partial Regex KeyAnnotationPattern();

    [GeneratedRegex(@"\[ForeignKey\s*\(\s*\"([^\"]+)\"\s*\)\]")]
    private static partial Regex ForeignKeyAnnotationPattern();

    [GeneratedRegex(@"\[Keyless\]")]
    private static partial Regex KeylessPattern();

    [GeneratedRegex(@"(?:ICollection|List|IEnumerable)\s*<\s*(?<collType>\w+)\s*>\s+(?<collName>\w+)\s*\{|(?<refType>\w+)\s+(?<refName>\w+)\s*\{\s*get")]
    private static partial Regex NavigationPattern();

    [GeneratedRegex(@"\b(Assembly\.Load|Activator\.CreateInstance|dynamic\b|ModelBuilder\s+\w+\s*=)")]
    private static partial Regex IncompletePattern();
}
