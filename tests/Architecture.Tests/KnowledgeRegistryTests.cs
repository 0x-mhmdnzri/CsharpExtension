using System.Collections.Immutable;
using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class KnowledgeRegistryTests
{
    public static void Run()
    {
        var registry = new KnowledgeRegistry(
            [new KnowledgeConcept("clr.gc", "GC", ImmutableArray.Create("CLR", "memory"))],
            [new KnowledgeSource("ms.gc", "https://learn.microsoft.com/dotnet/standard/garbage-collection/", "GC")],
            [new KnowledgeClaim("claim.gc", "clr.gc", "ms.gc", "GC reclaims unreachable objects.")],
            [new KnowledgeRule("rule.gc", "claim.gc", "Prefer bounded allocations.")]);
        if (registry.Concepts.Count != 1 || registry.Claims.Count != 1 || registry.Rules.Count != 1)
            throw new InvalidOperationException("Knowledge registry did not retain linked records.");
        try
        {
            _ = new KnowledgeRegistry([], [], [new KnowledgeClaim("bad", "missing", "missing", "")], []);
            throw new InvalidOperationException("Invalid knowledge links were accepted.");
        }
        catch (InvalidDataException) { }
    }
}
