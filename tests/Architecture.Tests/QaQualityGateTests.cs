using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class QaQualityGateTests
{
    public static void Run()
    {
        var pass = QaQualityGate.Evaluate(new QaMetrics(.9, .8, .7, 20, 100));
        var fail = QaQualityGate.Evaluate(new QaMetrics(.4, .8, .7, 20, 100));
        var slow = QaQualityGate.Evaluate(new QaMetrics(.9, .8, .7, 20, 151));
        if (!pass.Passed || fail.Passed || !fail.Failures.Contains("precision") || slow.Passed || !slow.Failures.Contains("latency"))
            throw new InvalidOperationException("QA quality gate did not enforce thresholds.");
    }
}
