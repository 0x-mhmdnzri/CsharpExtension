using System.Collections.Immutable;

namespace DotnetExpertAdvisor.Shared;

public sealed record VerificationResult(
    bool Compiles,
    ImmutableArray<LexerDiagnostic> NewErrors,
    ImmutableArray<LexerDiagnostic> NewNullableWarnings);

public static class SpeculativeVerifier
{
    public static VerificationResult Verify(string baseline, string candidate, LanguageCapabilities? capabilities = null)
    {
        ArgumentNullException.ThrowIfNull(baseline);
        ArgumentNullException.ThrowIfNull(candidate);
        capabilities ??= LanguageCapabilities.For(LanguageVersion.CSharp10);

        var lexer = new CSharpLexer();
        var parser = new CSharpParser();
        var baselineResult = parser.Parse(lexer.Tokenize(baseline, capabilities), capabilities);
        var candidateResult = parser.Parse(lexer.Tokenize(candidate, capabilities), capabilities);
        var baselineErrors = baselineResult.Diagnostics
            .Select(diagnostic => (diagnostic.Span.Start, diagnostic.Message))
            .ToHashSet();
        var newErrors = candidateResult.Diagnostics
            .Where(diagnostic => !baselineErrors.Contains((diagnostic.Span.Start, diagnostic.Message)))
            .ToImmutableArray();
        return new(candidateResult.Diagnostics.IsEmpty, newErrors, ImmutableArray<LexerDiagnostic>.Empty);
    }

    public static VerificationResult VerifyInsert(string document, int position, string insertText,
        LanguageCapabilities? capabilities = null)
    {
        ArgumentNullException.ThrowIfNull(document);
        ArgumentNullException.ThrowIfNull(insertText);
        if ((uint)position > (uint)document.Length) throw new ArgumentOutOfRangeException(nameof(position));
        return Verify(document, document[..position] + insertText + document[position..], capabilities);
    }
}
