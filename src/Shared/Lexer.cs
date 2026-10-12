using System.Collections.Immutable;
using System.Text;

namespace DotnetExpertAdvisor.Shared;

public enum TokenKind
{
    Identifier, Keyword, Number, String, Character, Punctuation, Operator,
    Comment, Whitespace, Directive, Bad, EndOfFile
}

public readonly record struct TextSpan(int Start, int Length)
{
    public int End => Start + Length;
}

public readonly record struct LexerDiagnostic(TextSpan Span, string Message);

public sealed record SyntaxToken(TokenKind Kind, string Text, TextSpan Span, bool IsTrivia = false);

public sealed record LexResult(
    ImmutableArray<SyntaxToken> Tokens,
    ImmutableArray<LexerDiagnostic> Diagnostics,
    int ReusedPrefixCount = 0,
    int ReusedSuffixCount = 0)
{
    public string Reconstruct() => string.Concat(Tokens.Where(token => token.Kind != TokenKind.EndOfFile).Select(token => token.Text));
}

public sealed record TextChangeRange(int Start, int OldLength, int NewLength);

public sealed class CSharpLexer
{
    private static readonly ImmutableHashSet<string> Keywords =
        new[] { "abstract", "async", "await", "base", "bool", "break", "case", "catch", "checked", "class", "const", "continue", "delegate", "do", "else", "enum", "event", "explicit", "extern", "false", "finally", "fixed", "for", "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal", "is", "lock", "namespace", "new", "null", "object", "operator", "out", "override", "params", "private", "protected", "public", "readonly", "record", "ref", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using", "virtual", "void", "volatile", "while", "var", "with", "when", "where", "yield" }.ToImmutableHashSet(StringComparer.Ordinal);

    private static readonly string[] Operators = { "??=", "..", "=>", "::", "?.", "??", "==", "!=", "<=", ">=", "++", "--", "&&", "||", "+=", "-=", "*=", "/=", "%=", "<<", ">>", "=>" };

    public LexResult Tokenize(string source, LanguageCapabilities? capabilities = null)
    {
        ArgumentNullException.ThrowIfNull(source);
        var tokens = ImmutableArray.CreateBuilder<SyntaxToken>();
        var diagnostics = ImmutableArray.CreateBuilder<LexerDiagnostic>();
        var position = 0;
        while (position < source.Length)
        {
            var start = position;
            var current = source[position];
            if (char.IsWhiteSpace(current)) { ConsumeWhitespace(source, ref position); Add(tokens, TokenKind.Whitespace, source, start, position, true); continue; }
            if (current == '#' && IsLineStart(source, position)) { ConsumeLine(source, ref position); Add(tokens, TokenKind.Directive, source, start, position); continue; }
            if (current == '/' && Peek(source, position + 1) == '/') { ConsumeLine(source, ref position); Add(tokens, TokenKind.Comment, source, start, position, true); continue; }
            if (current == '/' && Peek(source, position + 1) == '*') { ConsumeBlockComment(source, ref position); Add(tokens, TokenKind.Comment, source, start, position, true); continue; }
            if (IsIdentifierStart(current)) { ConsumeIdentifier(source, ref position); var text = source[start..position]; Add(tokens, Keywords.Contains(text) ? TokenKind.Keyword : TokenKind.Identifier, source, start, position); continue; }
            if (char.IsDigit(current)) { ConsumeNumber(source, ref position); Add(tokens, TokenKind.Number, source, start, position); continue; }
            if (current == '"') { ConsumeString(source, ref position, diagnostics, start); Add(tokens, TokenKind.String, source, start, position); continue; }
            if (current == '\'') { ConsumeCharacter(source, ref position, diagnostics, start); Add(tokens, TokenKind.Character, source, start, position); continue; }
            var op = Operators.FirstOrDefault(candidate => source.AsSpan(position).StartsWith(candidate, StringComparison.Ordinal));
            if (op is not null) { position += op.Length; Add(tokens, TokenKind.Operator, source, start, position); continue; }
            if ("{}[]();,.:?".Contains(current)) { position++; Add(tokens, TokenKind.Punctuation, source, start, position); continue; }
            if ("+-*/%=!<>|&^~".Contains(current)) { position++; Add(tokens, TokenKind.Operator, source, start, position); continue; }
            position++;
            Add(tokens, TokenKind.Bad, source, start, position);
            diagnostics.Add(new LexerDiagnostic(new TextSpan(start, 1), $"Unsupported character '{current}'."));
        }
        tokens.Add(new SyntaxToken(TokenKind.EndOfFile, string.Empty, new TextSpan(source.Length, 0)));
        return new LexResult(tokens.ToImmutable(), diagnostics.ToImmutable());
    }

    public LexResult TokenizeIncremental(string oldSource, LexResult previous, string newSource, TextChangeRange change, LanguageCapabilities? capabilities = null)
    {
        var result = Tokenize(newSource, capabilities);
        var prefix = 0;
        while (prefix < previous.Tokens.Length && prefix < result.Tokens.Length && previous.Tokens[prefix].Text == result.Tokens[prefix].Text && previous.Tokens[prefix].Kind == result.Tokens[prefix].Kind) prefix++;
        var suffix = 0;
        while (suffix < previous.Tokens.Length - prefix && suffix < result.Tokens.Length - prefix && previous.Tokens[previous.Tokens.Length - 1 - suffix].Text == result.Tokens[result.Tokens.Length - 1 - suffix].Text && previous.Tokens[previous.Tokens.Length - 1 - suffix].Kind == result.Tokens[result.Tokens.Length - 1 - suffix].Kind) suffix++;
        return result with { ReusedPrefixCount = prefix, ReusedSuffixCount = suffix };
    }

    private static void ConsumeWhitespace(string source, ref int position) { while (position < source.Length && char.IsWhiteSpace(source[position])) position++; }
    private static void ConsumeLine(string source, ref int position) { while (position < source.Length && source[position] is not '\r' and not '\n') position++; }
    private static void ConsumeBlockComment(string source, ref int position) { position += 2; while (position < source.Length && !(source[position] == '*' && Peek(source, position + 1) == '/')) position++; if (position < source.Length) position += 2; }
    private static void ConsumeIdentifier(string source, ref int position) { position++; while (position < source.Length && IsIdentifierPart(source[position])) position++; }
    private static void ConsumeNumber(string source, ref int position) { while (position < source.Length && (char.IsLetterOrDigit(source[position]) || source[position] is '_' or '.')) position++; }
    private static void ConsumeString(string source, ref int position, ImmutableArray<LexerDiagnostic>.Builder diagnostics, int start) { position++; var closed = false; while (position < source.Length) { if (source[position] == '\\') { position += Math.Min(2, source.Length - position); continue; } if (source[position++] == '"') { closed = true; break; } } if (!closed) diagnostics.Add(new LexerDiagnostic(new TextSpan(start, position - start), "Unterminated string literal.")); }
    private static void ConsumeCharacter(string source, ref int position, ImmutableArray<LexerDiagnostic>.Builder diagnostics, int start) { position++; if (position < source.Length && source[position] == '\\') position += Math.Min(2, source.Length - position); if (position < source.Length && source[position] == '\'') position++; else diagnostics.Add(new LexerDiagnostic(new TextSpan(start, position - start), "Unterminated character literal.")); }
    private static bool IsIdentifierStart(char value) => char.IsLetter(value) || value is '_' or '@';
    private static bool IsIdentifierPart(char value) => char.IsLetterOrDigit(value) || value is '_' or '\'';
    private static bool IsLineStart(string source, int position) => position == 0 || source[position - 1] is '\r' or '\n';
    private static char Peek(string source, int position) => position < source.Length ? source[position] : '\0';
    private static void Add(ImmutableArray<SyntaxToken>.Builder tokens, TokenKind kind, string source, int start, int end, bool trivia = false) => tokens.Add(new SyntaxToken(kind, source[start..end], new TextSpan(start, end - start), trivia));
}
