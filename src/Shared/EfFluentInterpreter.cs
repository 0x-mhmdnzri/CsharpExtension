using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace DotnetExpertAdvisor.Shared;

/// <summary>Static Fluent API hints from OnModelCreating (symbol-name based, not full binder).</summary>
public static partial class EfFluentInterpreter
{
    public sealed record FluentKey(string EntityType, string PropertyName);
    public sealed record FluentRelationship(string EntityType, string Navigation, string? ForeignKey, bool IsCollection);

    public sealed record FluentModel(
        ImmutableArray<FluentKey> Keys,
        ImmutableArray<FluentRelationship> Relationships);

    public static FluentModel Interpret(string source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var keys = HasKeyPattern().Matches(source)
            .Select(match => new FluentKey(match.Groups[1].Value, match.Groups[2].Value))
            .ToImmutableArray();

        var foreignKeys = ForeignKeyMatches(source)
            .ToImmutableArray();

        var relationships = RelationshipMatches(source)
            .Select(match => new FluentRelationship(
                match.Groups[1].Value,
                match.Groups[2].Value,
                match.Groups[3].Success
                    ? match.Groups[3].Value
                    : foreignKeys.FirstOrDefault(key => key.EntityType == match.Groups[1].Value).PropertyName,
                IsCollection: match.Value.Contains("WithMany", StringComparison.Ordinal)))
            .ToImmutableArray();

        return new FluentModel(keys, relationships);
    }

    public static EfModelSnapshot Apply(EfModelSnapshot snapshot, string source)
    {
        var fluent = Interpret(source);
        if (fluent.Keys.Length == 0 && fluent.Relationships.Length == 0)
            return snapshot;

        var entities = snapshot.Entities.Select(entity =>
        {
            var fluentKeys = fluent.Keys
                .Where(key => key.EntityType == entity.Name)
                .Select(key => new EfFact<string>(key.PropertyName, EfProvenance.FluentApi, EfConfidence.High, "HasKey"))
                .ToImmutableArray();

            var keys = fluentKeys.Length > 0 ? fluentKeys : entity.Keys;

            var fluentNavs = fluent.Relationships
                .Where(rel => rel.EntityType == entity.Name)
                .Select(rel => new EfNavigation(
                    rel.Navigation,
                    InferTarget(snapshot, rel.Navigation),
                    rel.IsCollection,
                    EfProvenance.FluentApi))
                .Where(nav => !string.IsNullOrEmpty(nav.TargetType))
                .ToImmutableArray();

            var navigations = fluentNavs.Length > 0
                ? entity.Navigations.AddRange(fluentNavs)
                    .GroupBy(nav => nav.Name, StringComparer.Ordinal)
                    .Select(group => group.First())
                    .ToImmutableArray()
                : entity.Navigations;

            var fluentFks = fluent.Relationships
                .Where(rel => rel.EntityType == entity.Name && rel.ForeignKey is not null)
                .Select(rel => new EfFact<string>(rel.ForeignKey!, EfProvenance.FluentApi, EfConfidence.High, "HasForeignKey"))
                .ToImmutableArray();

            var foreignKeys = fluentFks.Length > 0
                ? fluentFks.AddRange(entity.ForeignKeys)
                    .GroupBy(fk => fk.Value, StringComparer.Ordinal)
                    .Select(group => group.First())
                    .ToImmutableArray()
                : entity.ForeignKeys;

            return entity with { Keys = keys, Navigations = navigations, ForeignKeys = foreignKeys };
        }).ToImmutableArray();

        return snapshot with { Entities = entities };
    }

    private static string InferTarget(EfModelSnapshot snapshot, string navigationName)
    {
        // Prefer existing navigation target; otherwise strip plural s as weak heuristic.
        foreach (var entity in snapshot.Entities)
        {
            var existing = entity.Navigations.FirstOrDefault(nav => nav.Name == navigationName);
            if (existing is not null)
                return existing.TargetType;
        }

        var candidate = navigationName.EndsWith('s') ? navigationName[..^1] : navigationName;
        return snapshot.Entities.Any(entity => entity.Name == candidate) ? candidate : string.Empty;
    }

    private static ImmutableArray<(string EntityType, string PropertyName)> ForeignKeyMatches(string source)
    {
        var result = ImmutableArray.CreateBuilder<(string, string)>();
        foreach (Match match in Regex.Matches(source, @"HasForeignKey\s*\(\s*\w+\s*=>\s*\w+\.(\w+)\s*\)"))
        {
            var before = source[..match.Index];
            var entityMatches = Regex.Matches(before, @"Entity\s*<\s*(\w+)\s*>");
            if (entityMatches.Count > 0)
                result.Add((entityMatches[^1].Groups[1].Value, match.Groups[1].Value));
        }
        return result.ToImmutable();
    }

    private static ImmutableArray<Match> RelationshipMatches(string source)
        => Regex.Matches(source,
            @"Entity\s*<\s*(\w+)\s*>\s*\(\s*\)\s*\.[^;]*?HasOne\s*\(\s*\w+\s*=>\s*\w+\.(\w+)\s*\)[^;]*?(?:HasForeignKey\s*\(\s*\w+\s*=>\s*\w+\.(\w+)\s*\)|(?=;))",
            RegexOptions.Singleline).Cast<Match>().ToImmutableArray();

    // modelBuilder.Entity<Order>().HasKey(x => x.OrderKey)
    [GeneratedRegex(@"Entity\s*<\s*(\w+)\s*>\s*\(\s*\)\s*\.[\s\S]*?HasKey\s*\(\s*\w+\s*=>\s*\w+\.(\w+)\s*\)")]
    private static partial Regex HasKeyPattern();

    // Entity<Order>().HasOne(o => o.Customer).WithMany(...).HasForeignKey(o => o.CustomerId)
    [GeneratedRegex(@"Entity\s*<\s*(\w+)\s*>\s*\(\s*\)\s*\.[\s\S]*?HasOne\s*\(\s*\w+\s*=>\s*\w+\.(\w+)\s*\)[\s\S]*?(?:HasForeignKey\s*\(\s*\w+\s*=>\s*\w+\.(\w+)\s*\)|(?=;|$))")]
    private static partial Regex HasOnePattern();

}
