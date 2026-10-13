using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class KnowledgeExtractorTests
{
    public static void Run()
    {
        var result = KnowledgeExtractor.Extract("clr.gc", "ms.gc", "Claim: unreachable objects can be reclaimed.\nRule: prefer bounded allocations.");
        if (result.Claims.Length != 2 || result.Rules.Length != 1 || result.Rules[0].ClaimId != result.Claims[1].Id)
            throw new InvalidOperationException("Knowledge claims and rules were not extracted or linked.");
    }
}
