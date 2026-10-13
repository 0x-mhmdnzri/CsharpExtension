namespace DotnetExpertAdvisor.Shared;

public sealed record QaMetrics(double Precision, double Recall, double MeanReciprocalRank,
    double P50Milliseconds, double P95Milliseconds);

public static class QaMetricsCalculator
{
    public static QaMetrics Calculate(IReadOnlyList<string> predictions, IReadOnlySet<string> relevant,
        IReadOnlyList<double> latencies)
    {
        ArgumentNullException.ThrowIfNull(predictions);
        ArgumentNullException.ThrowIfNull(relevant);
        ArgumentNullException.ThrowIfNull(latencies);
        var hits = predictions.Count(relevant.Contains);
        var first = predictions.Select((id, index) => (id, index)).FirstOrDefault(item => relevant.Contains(item.id));
        var reciprocal = first.id is null ? 0 : 1d / (first.index + 1);
        var sorted = latencies.OrderBy(value => value).ToArray();
        return new(predictions.Count == 0 ? 0 : (double)hits / predictions.Count,
            relevant.Count == 0 ? 0 : (double)hits / relevant.Count, reciprocal,
            Percentile(sorted, .50), Percentile(sorted, .95));
    }

    private static double Percentile(double[] values, double percentile)
    {
        if (values.Length == 0) return 0;
        var index = Math.Min(values.Length - 1, (int)Math.Ceiling(values.Length * percentile) - 1);
        return values[index];
    }
}
