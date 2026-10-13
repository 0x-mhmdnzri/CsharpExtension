using System.Collections.Immutable;

namespace DotnetExpertAdvisor.Shared;

public sealed record KnowledgeExtractionResult(ImmutableArray<KnowledgeClaim> Claims,
    ImmutableArray<KnowledgeRule> Rules);

public static class KnowledgeExtractor
{
    public static KnowledgeExtractionResult Extract(string conceptId, string sourceId, string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(conceptId);
        ArgumentException.ThrowIfNullOrWhiteSpace(sourceId);
        ArgumentNullException.ThrowIfNull(text);
        var claims = ImmutableArray.CreateBuilder<KnowledgeClaim>();
        var rules = ImmutableArray.CreateBuilder<KnowledgeRule>();
        foreach (var line in text.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (line.StartsWith("Claim:", StringComparison.OrdinalIgnoreCase))
            {
                var value = line[6..].Trim();
                if (value.Length > 0) claims.Add(new KnowledgeClaim($"{sourceId}:claim:{claims.Count}", conceptId, sourceId, value));
            }
            else if (line.StartsWith("Rule:", StringComparison.OrdinalIgnoreCase))
            {
                var value = line[5..].Trim();
                if (value.Length > 0)
                {
                    var claimId = $"{sourceId}:claim:{claims.Count}";
                    claims.Add(new KnowledgeClaim(claimId, conceptId, sourceId, value));
                    rules.Add(new KnowledgeRule($"{sourceId}:rule:{rules.Count}", claimId, value));
                }
            }
        }
        return new(claims.ToImmutable(), rules.ToImmutable());
    }
}
