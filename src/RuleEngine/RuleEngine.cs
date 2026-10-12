using DotnetExpertAdvisor.Shared;
namespace DotnetExpertAdvisor.RuleEngine;
public interface IRuleEngine { IReadOnlyList<SuggestionCandidate> Evaluate(DocumentAnalysis analysis, GraphSnapshot graph); }
public sealed class RuleEngine : IRuleEngine { public IReadOnlyList<SuggestionCandidate> Evaluate(DocumentAnalysis analysis, GraphSnapshot graph) => Array.Empty<SuggestionCandidate>(); }
