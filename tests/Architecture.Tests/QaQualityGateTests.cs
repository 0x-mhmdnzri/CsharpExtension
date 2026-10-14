using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class QaQualityGateTests
{
    public static void Run()
    {
        var pass = QaQualityGate.Evaluate(new(
            CorrectnessPrecision: 0.97,
            PerformancePrecision: 0.90,
            FalsePositiveRate: 0.05,
            CompileValidRate: 0.995,
            P95Milliseconds: 120));

        if (!pass.Passed || pass.ViolationCount != 0)
            throw new InvalidOperationException("Quality gate rejected a passing input.");

        var fail = QaQualityGate.Evaluate(new(
            CorrectnessPrecision: 0.80,
            PerformancePrecision: 0.70,
            FalsePositiveRate: 0.20,
            CompileValidRate: 0.90,
            P95Milliseconds: 300));

        if (fail.Passed || fail.ViolationCount != 5)
            throw new InvalidOperationException($"Quality gate expected 5 violations, got {fail.ViolationCount}.");

        var custom = QaQualityGate.Evaluate(
            new(0.50, 0.50, 0.50, 0.50, 500),
            new QualityGateThresholds(MinCorrectnessPrecision: 0.40, MinPerformancePrecision: 0.40,
                MaxActiveFalsePositiveRate: 0.60, MinCompileValidRate: 0.40, MaxP95Milliseconds: 1000));

        if (!custom.Passed)
            throw new InvalidOperationException("Custom thresholds were not respected.");
    }
}
