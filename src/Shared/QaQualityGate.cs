using System.Collections.Immutable;

namespace DotnetExpertAdvisor.Shared;

public sealed record QualityGateThresholds(
    double MinCorrectnessPrecision = 0.95,
    double MinPerformancePrecision = 0.85,
    double MaxActiveFalsePositiveRate = 0.10,
    double MinCompileValidRate = 0.99,
    double MaxP95Milliseconds = 150);

public sealed record QualityGateInput(
    double CorrectnessPrecision,
    double PerformancePrecision,
    double FalsePositiveRate,
    double CompileValidRate,
    double P95Milliseconds);

public sealed record QualityGateViolation(string Name, string Detail);

public sealed record QualityGateReport(
    bool Passed,
    ImmutableArray<QualityGateViolation> Violations)
{
    public int ViolationCount => Violations.Length;
}

public static class QaQualityGate
{
    public static QualityGateReport Evaluate(QualityGateInput input, QualityGateThresholds? thresholds = null)
    {
        ArgumentNullException.ThrowIfNull(input);
        thresholds ??= new QualityGateThresholds();
        var violations = ImmutableArray.CreateBuilder<QualityGateViolation>();

        if (input.CorrectnessPrecision < thresholds.MinCorrectnessPrecision)
            violations.Add(new("correctness-precision",
                $"precision {input.CorrectnessPrecision:F3} < {thresholds.MinCorrectnessPrecision:F3}"));

        if (input.PerformancePrecision < thresholds.MinPerformancePrecision)
            violations.Add(new("performance-precision",
                $"precision {input.PerformancePrecision:F3} < {thresholds.MinPerformancePrecision:F3}"));

        if (input.FalsePositiveRate > thresholds.MaxActiveFalsePositiveRate)
            violations.Add(new("false-positive-rate",
                $"rate {input.FalsePositiveRate:F3} > {thresholds.MaxActiveFalsePositiveRate:F3}"));

        if (input.CompileValidRate < thresholds.MinCompileValidRate)
            violations.Add(new("compile-valid-rate",
                $"rate {input.CompileValidRate:F3} < {thresholds.MinCompileValidRate:F3}"));

        if (input.P95Milliseconds > thresholds.MaxP95Milliseconds)
            violations.Add(new("p95-latency",
                $"p95 {input.P95Milliseconds:F1}ms > {thresholds.MaxP95Milliseconds:F1}ms"));

        var result = violations.ToImmutable();
        return new(result.Length == 0, result);
    }
}
