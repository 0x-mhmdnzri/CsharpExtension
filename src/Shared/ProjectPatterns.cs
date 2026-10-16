using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace DotnetExpertAdvisor.Shared;

public sealed record ProjectPatternSnapshot(
    ImmutableDictionary<string, int> SymbolFrequency,
    ImmutableDictionary<string, int> CallOrder,
    string NamingStyle)
{
    public static ProjectPatternSnapshot Empty { get; } = new([], [], "unknown");
}

public static partial class ProjectPatternIndexer
{
    private static readonly ImmutableHashSet<string> NonSymbols =
        ["if", "for", "foreach", "while", "switch", "catch", "using", "typeof", "nameof", "return", "new"];

    public static ProjectPatternSnapshot Build(IEnumerable<string> sources)
    {
        ArgumentNullException.ThrowIfNull(sources);
        var frequency = new Dictionary<string, int>(StringComparer.Ordinal);
        var order = new Dictionary<string, int>(StringComparer.Ordinal);
        var pascal = 0;
        var camel = 0;
        foreach (var source in sources)
        {
            if (string.IsNullOrEmpty(source)) continue;
            foreach (Match match in IdentifierPattern().Matches(source))
            {
                if (!NonSymbols.Contains(match.Value)) frequency[match.Value] = frequency.GetValueOrDefault(match.Value) + 1;
                if (char.IsUpper(match.Value[0])) pascal++; else camel++;
            }
            var calls = InvocationPattern().Matches(source).Select(match => match.Groups[1].Value)
                .Where(name => !NonSymbols.Contains(name)).ToArray();
            for (var index = 1; index < calls.Length; index++)
            {
                var key = $"{calls[index - 1]}->{calls[index]}";
                order[key] = order.GetValueOrDefault(key) + 1;
            }
        }
        var style = pascal == 0 && camel == 0 ? "unknown" : pascal >= camel ? "PascalCase" : "camelCase";
        return new(frequency.ToImmutableDictionary(StringComparer.Ordinal), order.ToImmutableDictionary(StringComparer.Ordinal), style);
    }

    public static bool HasCallOrder(ProjectPatternSnapshot patterns, string previous, string next)
        => patterns.CallOrder.TryGetValue($"{previous}->{next}", out var count) && count > 0;

    [GeneratedRegex(@"\b[A-Za-z_][A-Za-z0-9_]*\b")]
    private static partial Regex IdentifierPattern();

    [GeneratedRegex(@"\b([A-Za-z_][A-Za-z0-9_]*)\s*\(")]
    private static partial Regex InvocationPattern();
}
