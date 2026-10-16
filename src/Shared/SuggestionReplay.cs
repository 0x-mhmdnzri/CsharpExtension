using System.Collections.Immutable;
using System.Diagnostics;

namespace DotnetExpertAdvisor.Shared;

public sealed record SuggestionReplayCase(string Name, string Source, int SymbolStart, string Symbol);

public sealed record SuggestionReplayObservation(string CaseName, int PrefixLength, int Rank,
    bool Top1, bool Top5, int KeystrokesSaved, bool CompileValid, bool FalsePositive, double LatencyMilliseconds);

public sealed record SuggestionReplayReport(ImmutableArray<SuggestionReplayObservation> Observations,
    double Top1Accuracy, double Top5Accuracy, double KeystrokesSaved, double CompileValidRate,
    double FalsePositiveRate, double P95Milliseconds);

public static class SuggestionReplayHarness
{
    public static SuggestionReplayReport Run(
        IEnumerable<SuggestionReplayCase> cases,
        Func<string, int, IReadOnlyList<string>> complete,
        Func<string, string, bool> compileValid)
    {
        ArgumentNullException.ThrowIfNull(cases);
        ArgumentNullException.ThrowIfNull(complete);
        ArgumentNullException.ThrowIfNull(compileValid);
        var observations = ImmutableArray.CreateBuilder<SuggestionReplayObservation>();

        foreach (var replayCase in cases)
        {
            ArgumentException.ThrowIfNullOrEmpty(replayCase.Name);
            ArgumentException.ThrowIfNullOrEmpty(replayCase.Symbol);
            if (replayCase.SymbolStart < 0 || replayCase.SymbolStart + replayCase.Symbol.Length > replayCase.Source.Length
                || !string.Equals(replayCase.Source.Substring(replayCase.SymbolStart, replayCase.Symbol.Length), replayCase.Symbol, StringComparison.Ordinal))
                throw new ArgumentException($"Replay symbol does not match source for '{replayCase.Name}'.", nameof(cases));

            for (var prefixLength = 0; prefixLength <= Math.Min(3, replayCase.Symbol.Length); prefixLength++)
            {
                var prefix = replayCase.Symbol[..prefixLength];
                var source = replayCase.Source.Remove(replayCase.SymbolStart, replayCase.Symbol.Length)
                    .Insert(replayCase.SymbolStart, prefix);
                var cursor = replayCase.SymbolStart + prefixLength;
                var stopwatch = Stopwatch.StartNew();
                var suggestions = complete(source, cursor) ?? Array.Empty<string>();
                stopwatch.Stop();
                var rank = suggestions.Select((label, index) => (label, index))
                    .Where(item => string.Equals(item.label, replayCase.Symbol, StringComparison.Ordinal))
                    .Select(item => item.index + 1).FirstOrDefault();
                var top1 = rank == 1;
                var top5 = rank is > 0 and <= 5;
                var selected = suggestions.FirstOrDefault();
                observations.Add(new(replayCase.Name, prefixLength, rank, top1, top5,
                    top1 ? replayCase.Symbol.Length - prefixLength : 0,
                    selected is not null && compileValid(source, selected),
                    selected is not null && !string.Equals(selected, replayCase.Symbol, StringComparison.Ordinal),
                    stopwatch.Elapsed.TotalMilliseconds));
            }
        }

        var result = observations.ToImmutable();
        var count = result.Length;
        var latencies = result.Select(item => item.LatencyMilliseconds).OrderBy(value => value).ToArray();
        return new(result,
            count == 0 ? 0 : result.Count(item => item.Top1) / (double)count,
            count == 0 ? 0 : result.Count(item => item.Top5) / (double)count,
            count == 0 ? 0 : result.Sum(item => item.KeystrokesSaved) / (double)count,
            count == 0 ? 0 : result.Count(item => item.CompileValid) / (double)count,
            count == 0 ? 0 : result.Count(item => item.FalsePositive) / (double)count,
            Percentile(latencies, .95));
    }

    private static double Percentile(double[] values, double percentile)
    {
        if (values.Length == 0) return 0;
        return values[Math.Min(values.Length - 1, (int)Math.Ceiling(values.Length * percentile) - 1)];
    }
}
