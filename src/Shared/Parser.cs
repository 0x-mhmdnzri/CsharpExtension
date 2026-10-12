using System.Collections.Immutable;

namespace DotnetExpertAdvisor.Shared;

public enum SyntaxKind
{
    CompilationUnit,
    NamespaceDeclaration,
    TypeDeclaration,
    Block,
    Token,
    Error
}

public sealed record SyntaxNode(
    SyntaxKind Kind,
    TextSpan Span,
    ImmutableArray<SyntaxToken> Tokens,
    ImmutableArray<SyntaxNode> Children,
    string? Error = null)
{
    public string ToFullText() => string.Concat(Tokens.Where(token => token.Kind != TokenKind.EndOfFile).Select(token => token.Text));
}

public sealed record ParseResult(SyntaxNode Root, ImmutableArray<LexerDiagnostic> Diagnostics)
{
    public string Reconstruct() => Root.ToFullText();
}

public sealed class CSharpParser
{
    public ParseResult Parse(LexResult lexed, LanguageCapabilities capabilities)
    {
        ArgumentNullException.ThrowIfNull(lexed);
        var diagnostics = ImmutableArray.CreateBuilder<LexerDiagnostic>();
        diagnostics.AddRange(lexed.Diagnostics);
        var tokens = lexed.Tokens.Where(token => token.Kind != TokenKind.EndOfFile).ToImmutableArray();
        var children = ImmutableArray.CreateBuilder<SyntaxNode>();
        var significant = tokens.Where(token => !token.IsTrivia).ToArray();

        ValidateLanguageGates(significant, capabilities, diagnostics);
        var bracePairs = FindBracePairs(significant, diagnostics);
        for (var index = 0; index < significant.Length; index++)
        {
            var token = significant[index];
            if (token.Text is "namespace")
            {
                var declaration = ParseDeclaration(SyntaxKind.NamespaceDeclaration, token, significant, index, bracePairs, diagnostics);
                if (declaration is not null) children.Add(declaration);
            }
            else if (token.Text is "class" or "struct" or "interface" or "record")
            {
                var declaration = ParseDeclaration(SyntaxKind.TypeDeclaration, token, significant, index, bracePairs, diagnostics);
                if (declaration is not null) children.Add(declaration);
            }
        }

        var end = tokens.Length == 0 ? 0 : tokens[^1].Span.End;
        var root = new SyntaxNode(SyntaxKind.CompilationUnit, new TextSpan(0, end), tokens, children.ToImmutable());
        return new ParseResult(root, diagnostics.ToImmutable());
    }

    private static SyntaxNode? ParseDeclaration(SyntaxKind kind, SyntaxToken keyword, IReadOnlyList<SyntaxToken> significant, int index, IReadOnlyDictionary<int, int> bracePairs, ImmutableArray<LexerDiagnostic>.Builder diagnostics)
    {
        var nameIndex = index + 1;
        if (kind == SyntaxKind.TypeDeclaration && significant[index].Text == "record" && nameIndex < significant.Count && significant[nameIndex].Text is "class" or "struct") nameIndex++;
        if (nameIndex >= significant.Count || significant[nameIndex].Kind != TokenKind.Identifier)
        {
            diagnostics.Add(new LexerDiagnostic(keyword.Span, $"Expected a name after '{keyword.Text}'."));
            return new SyntaxNode(SyntaxKind.Error, keyword.Span, ImmutableArray.Create(keyword), ImmutableArray<SyntaxNode>.Empty, "missing declaration name");
        }

        var openIndex = Array.FindIndex(significant.ToArray(), nameIndex, token => token.Text == "{");
        if (openIndex < 0)
        {
            var semicolon = Array.FindIndex(significant.ToArray(), nameIndex, token => token.Text == ";");
            var endToken = semicolon >= 0 ? significant[semicolon] : significant[^1];
            return new SyntaxNode(kind, new TextSpan(keyword.Span.Start, endToken.Span.End - keyword.Span.Start), ImmutableArray<SyntaxToken>.Empty, ImmutableArray<SyntaxNode>.Empty);
        }

        if (!bracePairs.TryGetValue(openIndex, out var closeIndex))
        {
            diagnostics.Add(new LexerDiagnostic(significant[openIndex].Span, "Unclosed declaration body."));
            closeIndex = significant.Count - 1;
        }
        var end = significant[closeIndex].Span.End;
        return new SyntaxNode(kind, new TextSpan(keyword.Span.Start, end - keyword.Span.Start), ImmutableArray<SyntaxToken>.Empty, ImmutableArray<SyntaxNode>.Empty);
    }

    private static Dictionary<int, int> FindBracePairs(IReadOnlyList<SyntaxToken> significant, ImmutableArray<LexerDiagnostic>.Builder diagnostics)
    {
        var pairs = new Dictionary<int, int>();
        var stack = new Stack<int>();
        for (var i = 0; i < significant.Count; i++)
        {
            if (significant[i].Text == "{") stack.Push(i);
            if (significant[i].Text == "}" && stack.Count > 0) { var open = stack.Pop(); pairs[open] = i; }
            else if (significant[i].Text == "}") diagnostics.Add(new LexerDiagnostic(significant[i].Span, "Unexpected closing brace."));
        }
        while (stack.Count > 0) { var open = significant[stack.Pop()]; diagnostics.Add(new LexerDiagnostic(open.Span, "Unclosed brace.")); }
        return pairs;
    }

    private static void ValidateLanguageGates(IReadOnlyList<SyntaxToken> tokens, LanguageCapabilities capabilities, ImmutableArray<LexerDiagnostic>.Builder diagnostics)
    {
        foreach (var token in tokens)
        {
            if (token.Text == "record" && !capabilities.Supports(LanguageFeature.Records)) diagnostics.Add(new LexerDiagnostic(token.Span, "Records require C# 9 or later."));
            if (token.Text == "namespace" && tokens.Any(candidate => candidate.Text == ";" && candidate.Span.Start > token.Span.Start) && !capabilities.Supports(LanguageFeature.FileScopedNamespaces)) diagnostics.Add(new LexerDiagnostic(token.Span, "File-scoped namespaces require C# 10 or later."));
        }
    }
}
