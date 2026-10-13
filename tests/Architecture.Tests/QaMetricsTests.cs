using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class QaMetricsTests
{
    public static void Run()
    {
        var metrics = QaMetricsCalculator.Calculate(["Name", "Other", "Count"], new HashSet<string> { "Name", "Count", "Missing" }, [2, 5, 20, 100]);
        if (metrics.Precision != 2d / 3 || metrics.Recall != 2d / 3 || metrics.MeanReciprocalRank != 1 || metrics.P50Milliseconds != 5 || metrics.P95Milliseconds != 100)
            throw new InvalidOperationException("QA precision, recall, ranking or latency metrics were incorrect.");
    }
}

