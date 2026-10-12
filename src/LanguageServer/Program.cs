using KnowledgeEngineService = DotnetExpertAdvisor.KnowledgeEngine.KnowledgeEngine;
using RankingEngineService = DotnetExpertAdvisor.RankingEngine.RankingEngine;
using RoslynHostService = DotnetExpertAdvisor.RoslynHost.RoslynHost;
using RuleEngineService = DotnetExpertAdvisor.RuleEngine.RuleEngine;
using SemanticGraphService = DotnetExpertAdvisor.SemanticGraph.SemanticGraph;

Console.WriteLine("Dotnet Expert Advisor language server bootstrap");
Console.WriteLine($"Knowledge pack: {new KnowledgeEngineService().GetSnapshot().Version}");
Console.WriteLine($"Components: {typeof(RoslynHostService).Assembly.GetName().Name}, {typeof(SemanticGraphService).Assembly.GetName().Name}, {typeof(RuleEngineService).Assembly.GetName().Name}, {typeof(RankingEngineService).Assembly.GetName().Name}");
