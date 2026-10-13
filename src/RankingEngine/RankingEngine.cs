using DotnetExpertAdvisor.Shared;
namespace DotnetExpertAdvisor.RankingEngine;
public interface IRankingEngine
{
    IReadOnlyList<RankedSuggestion> Rank(IReadOnlyList<SuggestionCandidate> candidates);
    IReadOnlyList<RankedSuggestion> Rank(IReadOnlyList<SuggestionCandidate> candidates, CompletionContext context, GraphSnapshot? graph = null);
}
public sealed class RankingEngine : IRankingEngine
{
    public IReadOnlyList<RankedSuggestion> Rank(IReadOnlyList<SuggestionCandidate> candidates) => candidates.OrderByDescending(c => c.Confidence).Select((c, i) => new RankedSuggestion(c, c.Confidence, i + 1)).ToArray();
    public IReadOnlyList<RankedSuggestion> Rank(IReadOnlyList<SuggestionCandidate> candidates, CompletionContext context, GraphSnapshot? graph = null)
    {
        ArgumentNullException.ThrowIfNull(candidates); ArgumentNullException.ThrowIfNull(context);
        return candidates.Select(candidate => {
            var score = candidate.Confidence;
            if (candidate.Title.StartsWith(context.Prefix, StringComparison.OrdinalIgnoreCase)) score += .5;
            else if (context.Prefix.Length > 0 && candidate.Title.Contains(context.Prefix, StringComparison.OrdinalIgnoreCase)) score += .1;
            if (context.IsMemberAccess && candidate.Category == SuggestionCategory.Completion) score += .1;
            if (graph?.Nodes.Any(n => n.Name.Equals(candidate.Title, StringComparison.OrdinalIgnoreCase)) == true) score += .2;
            return (candidate, score);
        }).OrderByDescending(x => x.Item2).Select((x, i) => new RankedSuggestion(x.candidate, x.Item2, i + 1)).ToArray();
    }
}
