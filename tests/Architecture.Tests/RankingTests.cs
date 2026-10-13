using DotnetExpertAdvisor.RankingEngine;
using DotnetExpertAdvisor.Shared;
namespace DotnetExpertAdvisor.Architecture.Tests;
public static class RankingTests
{
    public static void Run()
    {
        var candidates = new[] { new SuggestionCandidate("1", "r", SuggestionCategory.Completion, SuggestionSeverity.Info, "Name", "", .5, [], []), new SuggestionCandidate("2", "r", SuggestionCategory.Design, SuggestionSeverity.Info, "Count", "", .8, [], []) };
        var context = CompletionContextAnalyzer.Analyze("Name", 4);
        var graph = new GraphSnapshot(new WorkspaceId("w"), 1, [new GraphNode("1", "member", "Name", new ProjectId("p"))], []);
        var ranked = new DotnetExpertAdvisor.RankingEngine.RankingEngine().Rank(candidates, context, graph);
        if (ranked[0].Candidate.Title != "Name" || ranked[0].Score <= ranked[1].Score) throw new InvalidOperationException("Context ranking failed.");
    }
}




