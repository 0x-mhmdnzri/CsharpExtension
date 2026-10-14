namespace DotnetExpertAdvisor.Shared;

public sealed record QaQualityGateResult(bool Passed, string[] Failures);

public static class QaQualityGate
{
    public static QaQualityGateResult Evaluate(QaMetrics metrics, double minimumPrecision = .8,
        double maximumP95Milliseconds = 150)
    {
        ArgumentNullException.ThrowIfNull(metrics);
        var failures = new List<string>();
        if (metrics.Precision < minimumPrecision) failures.Add("precision");
        if (metrics.Recall <= 0) failures.Add("recall");
        if (metrics.P95Milliseconds > maximumP95Milliseconds) failures.Add("latency");
        return new(failures.Count == 0, failures.ToArray());
    }
}
