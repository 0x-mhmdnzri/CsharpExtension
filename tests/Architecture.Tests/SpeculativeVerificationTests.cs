using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class SpeculativeVerificationTests
{
    public static void Run()
    {
        var baseline = "class Demo { void Run() { Missing( }";
        var candidate = "class Demo { void Run() { Missing(); } }";
        var result = SpeculativeVerifier.Verify(baseline, candidate);
        if (!result.Compiles || result.NewErrors.Length != 0)
            throw new InvalidOperationException("Speculative verification did not remove baseline errors after a valid edit.");

        var invalid = SpeculativeVerifier.VerifyInsert("class Demo { }", 13, "(");
        if (invalid.Compiles || invalid.NewErrors.Length == 0)
            throw new InvalidOperationException("Speculative verification did not report a new diagnostic.");
    }
}
