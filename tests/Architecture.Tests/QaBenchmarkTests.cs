using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class QaBenchmarkTests
{
    public static void Run()
    {
        var result = QaBenchmarkRunner.Run(() => ["Name", "Other"], new HashSet<string> { "Name" }, 3);
        if (result.Iterations != 3 || result.Metrics.Precision != .5 || result.Metrics.Recall != 1 || result.Metrics.P95Milliseconds < 0)
            throw new InvalidOperationException("QA benchmark did not collect metrics.");
    }
}
