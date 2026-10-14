using System.Diagnostics;

namespace DotnetExpertAdvisor.Shared;

public sealed record QaBenchmarkResult(QaMetrics Metrics, int Iterations);

public static class QaBenchmarkRunner
{
    public static QaBenchmarkResult Run(Func<IReadOnlyList<string>> operation,
        IReadOnlySet<string> relevant, int iterations = 20)
    {
        ArgumentNullException.ThrowIfNull(operation);
        ArgumentNullException.ThrowIfNull(relevant);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(iterations, 0);

        IReadOnlyList<string>? predictions = null;
        var latencies = new double[iterations];
        for (var i = 0; i < iterations; i++)
        {
            var stopwatch = Stopwatch.StartNew();
            predictions = operation() ?? throw new InvalidOperationException("Benchmark operation returned null.");
            stopwatch.Stop();
            latencies[i] = stopwatch.Elapsed.TotalMilliseconds;
        }

        return new(QaMetricsCalculator.Calculate(predictions!, relevant, latencies), iterations);
    }
}
