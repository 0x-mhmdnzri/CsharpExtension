using System.Collections.Immutable;

namespace DotnetExpertAdvisor.Shared;

public sealed record RegressionCaseResult(string Name, bool Passed, string? Failure);
public sealed record RegressionReport(ImmutableArray<RegressionCaseResult> Cases, bool Passed)
{
    public int PassedCount => Cases.Count(result => result.Passed);
}

public static class QaRegressionRunner
{
    public static RegressionReport Run(IEnumerable<(string Name, Func<bool> Check)> cases)
    {
        ArgumentNullException.ThrowIfNull(cases);
        var results = ImmutableArray.CreateBuilder<RegressionCaseResult>();
        foreach (var (name, check) in cases)
        {
            try { results.Add(new(name, check(), null)); }
            catch (Exception exception) when (exception is InvalidOperationException or ArgumentException)
            { results.Add(new(name, false, exception.Message)); }
        }
        return new(results.ToImmutable(), results.All(result => result.Passed));
    }
}
