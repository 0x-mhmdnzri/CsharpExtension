using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace DotnetExpertAdvisor.Shared;

public sealed record CompletionSuggestion(string Label, string Detail, string SortText,
    string SuggestionId, string RuleId, double Confidence, string Source);

public static partial class CompletionEngine
{
    private static readonly ImmutableHashSet<string> Keywords =
        ["class", "public", "private", "protected", "internal", "static", "void", "int", "string", "bool",
         "var", "new", "return", "if", "else", "for", "foreach", "while", "using", "namespace", "null",
         "true", "false", "this", "async", "await", "record", "struct", "interface", "enum"];

    public static ImmutableArray<CompletionSuggestion> Complete(string text, int line, int character)
    {
        ArgumentNullException.ThrowIfNull(text);
        if (line < 0 || character < 0) return ImmutableArray<CompletionSuggestion>.Empty;
        var offset = OffsetAt(text, line, character);
        if (offset < 0) return ImmutableArray<CompletionSuggestion>.Empty;

        var context = CompletionContextAnalyzer.Analyze(text, offset);
        var candidates = new Dictionary<string, (string Detail, double Confidence, string RuleId, string Source)>(StringComparer.Ordinal);
        foreach (Match match in IdentifierPattern().Matches(text))
        {
            var value = match.Value;
            if (!Keywords.Contains(value) && value.StartsWith(context.Prefix, StringComparison.OrdinalIgnoreCase))
                candidates.TryAdd(value, ("C# symbol", 0.65, "completion.symbol", "internal://csharp"));
        }

        if (context.IsMemberAccess)
        {
            var ef = EfQueryCompletion.Complete(text[..offset], EfModelAnalyzer.Analyze(text));
            foreach (var candidate in ef.Candidates.Where(candidate =>
                         candidate.StartsWith(context.Prefix, StringComparison.OrdinalIgnoreCase)))
                candidates[candidate] = ("EF navigation", 0.95, "completion.ef-navigation", "internal://ef-model");
        }

        return candidates.OrderByDescending(pair => pair.Key.Equals(context.Prefix, StringComparison.OrdinalIgnoreCase))
            .ThenBy(pair => pair.Key, StringComparer.OrdinalIgnoreCase).Take(100)
            .Select((pair, index) => new CompletionSuggestion(pair.Key, pair.Value.Detail, index.ToString("D5"),
                $"{pair.Value.RuleId}:{pair.Key}", pair.Value.RuleId, pair.Value.Confidence, pair.Value.Source))
            .ToImmutableArray();
    }

    private static int OffsetAt(string text, int line, int character)
    {
        var currentLine = 0;
        var offset = 0;
        while (currentLine < line)
        {
            var newline = text.IndexOf('\n', offset);
            if (newline < 0) return -1;
            offset = newline + 1;
            currentLine++;
        }
        var lineEnd = text.IndexOf('\n', offset);
        if (lineEnd < 0) lineEnd = text.Length;
        return offset + Math.Min(character, lineEnd - offset);
    }

    [GeneratedRegex(@"\b[A-Za-z_]\w*\b")]
    private static partial Regex IdentifierPattern();
}
