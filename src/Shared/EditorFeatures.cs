using System.Collections.Immutable;

namespace DotnetExpertAdvisor.Shared;

public sealed record SignatureInformation(string Label, string Documentation);
public sealed record SignatureHelpResult(ImmutableArray<SignatureInformation> Signatures, int ActiveSignature,
    int ActiveParameter);
public sealed record HoverResult(string Markdown);
public sealed record DefinitionLocation(string Uri, int Token);
public sealed record DiagnosticInfo(int Start, int Length, SuggestionSeverity Severity, string Message, string Code);
public sealed record TextEdit(int Start, int Length, string NewText);
public sealed record CodeActionInfo(string Title, string Kind, ImmutableArray<TextEdit> Edits);
public sealed record RenameResult(string NewName, ImmutableArray<TextEdit> Edits);

public static class MetadataEditorFeatures
{
    public static DiagnosticInfo Diagnostic(int start, int length, SuggestionSeverity severity, string message, string code)
        => new(start, length, severity, message, code);

    public static CodeActionInfo CodeAction(string title, string kind, IEnumerable<TextEdit> edits)
    {
        ArgumentNullException.ThrowIfNull(edits);
        return new(title, kind, edits.ToImmutableArray());
    }

    public static RenameResult Rename(string text, string oldName, string newName)
    {
        ArgumentNullException.ThrowIfNull(text);
        ArgumentException.ThrowIfNullOrWhiteSpace(oldName);
        ArgumentException.ThrowIfNullOrWhiteSpace(newName);
        if (newName.Any(character => !char.IsLetterOrDigit(character) && character != '_'))
            throw new ArgumentException("Rename must be an identifier.", nameof(newName));
        var edits = ImmutableArray.CreateBuilder<TextEdit>();
        for (var offset = 0; offset <= text.Length - oldName.Length; offset++)
            if (text.AsSpan(offset, oldName.Length).SequenceEqual(oldName) &&
                (offset == 0 || !char.IsLetterOrDigit(text[offset - 1]) && text[offset - 1] != '_') &&
                (offset + oldName.Length == text.Length || !char.IsLetterOrDigit(text[offset + oldName.Length]) && text[offset + oldName.Length] != '_'))
                edits.Add(new TextEdit(offset, oldName.Length, newName));
        return new(newName, edits.ToImmutable());
    }

    public static SignatureHelpResult SignatureHelp(MetadataMember member)
        => SignatureHelp([member]);

    public static SignatureHelpResult SignatureHelp(IEnumerable<MetadataMember> members, int activeParameter = 0)
    {
        ArgumentNullException.ThrowIfNull(members);
        ArgumentOutOfRangeException.ThrowIfNegative(activeParameter);
        var signatures = members.Where(member => member.Kind == "method")
            .Select(member => new SignatureInformation($"{member.Name}{member.DecodedSignature}", member.Documentation))
            .ToImmutableArray();
        return new(signatures, signatures.IsEmpty ? -1 : 0, activeParameter);
    }

    public static HoverResult Hover(MetadataMember member)
        => new($"```csharp\n{member.Name}{member.DecodedSignature}\n```\n\n{member.Documentation}");

    public static DefinitionLocation Definition(MetadataMember member, string assemblyPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(assemblyPath);
        return new(new Uri(Path.GetFullPath(assemblyPath)).AbsoluteUri, member.Token);
    }
}
