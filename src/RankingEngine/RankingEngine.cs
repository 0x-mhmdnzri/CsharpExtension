using DotnetExpertAdvisor.Shared;
namespace DotnetExpertAdvisor.RankingEngine;
public interface IRankingEngine { IReadOnlyList<RankedSuggestion> Rank(IReadOnlyList<SuggestionCandidate> candidates); }
public sealed class RankingEngine : IRankingEngine { public IReadOnlyList<RankedSuggestion> Rank(IReadOnlyList<SuggestionCandidate> candidates) => candidates.OrderByDescending(candidate => candidate.Confidence).Select((candidate, index) => new RankedSuggestion(candidate, candidate.Confidence, index + 1)).ToArray(); }
