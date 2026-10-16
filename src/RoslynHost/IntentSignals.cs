using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotnetExpertAdvisor.RoslynHost;

public sealed record IntentSignalResult(double Score, ImmutableArray<string> Signals);

public static partial class IntentSignals
{
    public static IntentSignalResult Score(ISymbol symbol, SemanticModel model,
        Microsoft.CodeAnalysis.SyntaxNode root, string source, int offset, string prefix)
    {
        var score = symbol.Name.Equals(prefix, StringComparison.OrdinalIgnoreCase) ? 0.5 : 0d;
        var signals = ImmutableArray.CreateBuilder<string>();
        if (score > 0) signals.Add("exact-prefix");

        var enclosing = model.GetEnclosingSymbol(offset);
        if (enclosing is not null && SymbolEqualityComparer.Default.Equals(symbol.ContainingSymbol, enclosing))
        {
            score += 0.2;
            signals.Add("locality");
        }
        if (symbol is ILocalSymbol or IParameterSymbol)
        {
            score += 0.15;
            signals.Add("local-or-parameter");
            if (IsUnused(symbol, model, root))
            {
                score += 0.35;
                signals.Add("unused-local-or-parameter");
            }
        }

        var candidateWords = Words(symbol.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var contextWords = IdentifierPattern().Matches(source[..Math.Min(offset, source.Length)])
            .Select(match => match.Value).TakeLast(8);
        if (contextWords.Any(word => candidateWords.Contains(word)))
        {
            score += 0.2;
            signals.Add("name-context-similarity");
        }
        return new(score, signals.ToImmutable());
    }

    private static bool IsUnused(ISymbol symbol, SemanticModel model, Microsoft.CodeAnalysis.SyntaxNode root)
    {
        var declarationStart = symbol.Locations.FirstOrDefault(location => location.IsInSource)?.SourceSpan.Start ?? -1;
        return !root.DescendantNodes().OfType<IdentifierNameSyntax>()
            .Where(identifier => identifier.Identifier.ValueText == symbol.Name && identifier.SpanStart != declarationStart)
            .Any(identifier => SymbolEqualityComparer.Default.Equals(model.GetSymbolInfo(identifier).Symbol, symbol));
    }

    private static IEnumerable<string> Words(string value)
    {
        foreach (Match match in WordPattern().Matches(value)) yield return match.Value;
    }

    [GeneratedRegex(@"[A-Z]?[a-z]+|[A-Z]+(?=[A-Z]|$)|[0-9]+")]
    private static partial Regex WordPattern();

    [GeneratedRegex(@"\b[A-Za-z_][A-Za-z0-9_]*\b")]
    private static partial Regex IdentifierPattern();
}
