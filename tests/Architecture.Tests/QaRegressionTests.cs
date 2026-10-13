using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class QaRegressionTests
{
    public static void Run()
    {
        var report = QaRegressionRunner.Run([("pass", () => true), ("fail", () => false)]);
        if (report.Passed || report.PassedCount != 1 || report.Cases.Length != 2 || report.Cases[1].Passed)
            throw new InvalidOperationException("Regression report did not preserve case results.");
    }
}
