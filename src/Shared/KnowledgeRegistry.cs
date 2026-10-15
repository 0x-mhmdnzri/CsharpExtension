using System.Collections.Immutable;

namespace DotnetExpertAdvisor.Shared;

public sealed record KnowledgeSource(string Id, string CanonicalUrl, string Title);
public sealed record KnowledgeConcept(string Id, string Title, ImmutableArray<string> Tags);
public sealed record KnowledgeClaim(string Id, string ConceptId, string SourceId, string Text);
public sealed record KnowledgeRule(string Id, string ClaimId, string RuleText);

public sealed class KnowledgeRegistry
{
    private readonly ImmutableDictionary<string, KnowledgeConcept> concepts;
    private readonly ImmutableDictionary<string, KnowledgeSource> sources;
    private readonly ImmutableDictionary<string, KnowledgeClaim> claims;
    private readonly ImmutableDictionary<string, KnowledgeRule> rules;

    public KnowledgeRegistry(IEnumerable<KnowledgeConcept> concepts, IEnumerable<KnowledgeSource> sources,
        IEnumerable<KnowledgeClaim> claims, IEnumerable<KnowledgeRule> rules)
    {
        this.concepts = ToMap(concepts, "concept");
        this.sources = ToMap(sources, "source");
        this.claims = ToMap(claims, "claim");
        this.rules = ToMap(rules, "rule");
        foreach (var claim in this.claims.Values)
        {
            if (!this.concepts.ContainsKey(claim.ConceptId) || !this.sources.ContainsKey(claim.SourceId))
                throw new InvalidDataException($"Claim references missing concept or source: {claim.Id}");
        }
        foreach (var rule in this.rules.Values)
            if (!this.claims.ContainsKey(rule.ClaimId)) throw new InvalidDataException($"Rule references missing claim: {rule.Id}");
    }

    public IReadOnlyList<KnowledgeConcept> Concepts => concepts.Values.ToArray();
    public IReadOnlyList<KnowledgeSource> Sources => sources.Values.ToArray();
    public IReadOnlyList<KnowledgeClaim> Claims => claims.Values.ToArray();
    public IReadOnlyList<KnowledgeRule> Rules => rules.Values.ToArray();

    public KnowledgeConcept? ResolveConcept(string id)
        => id is not null && concepts.TryGetValue(id, out var concept) ? concept : null;

    public KnowledgeSource? ResolveSource(string id)
        => id is not null && sources.TryGetValue(id, out var source) ? source : null;

    public IReadOnlyList<KnowledgeConcept> FindConceptsByTag(string tag)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tag);
        return concepts.Values.Where(concept => concept.Tags.Contains(tag, StringComparer.OrdinalIgnoreCase)).ToArray();
    }

    private static ImmutableDictionary<string, T> ToMap<T>(IEnumerable<T> values, string kind) where T : class
    {
        ArgumentNullException.ThrowIfNull(values);
        var map = ImmutableDictionary.CreateBuilder<string, T>(StringComparer.Ordinal);
        foreach (var value in values)
        {
            var id = value switch { KnowledgeConcept concept => concept.Id, KnowledgeSource source => source.Id, KnowledgeClaim claim => claim.Id, KnowledgeRule rule => rule.Id, _ => "" };
            if (string.IsNullOrWhiteSpace(id) || !map.TryAdd(id, value)) throw new InvalidDataException($"Duplicate or empty {kind} ID.");
        }
        return map.ToImmutable();
    }
}
