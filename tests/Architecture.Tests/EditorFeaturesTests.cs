using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class EditorFeaturesTests
{
    public static void Run()
    {
        var path = typeof(CSharpLexer).Assembly.Location;
        var member = new AssemblyMetadataReader().Read(path).Types.Single(type => type.Name == "CSharpLexer")
            .Members.Single(member => member.Name == "Tokenize");
        var signature = MetadataEditorFeatures.SignatureHelp(member);
        if (signature.Signatures.Length != 1 || !signature.Signatures[0].Label.StartsWith("Tokenize(", StringComparison.Ordinal))
            throw new InvalidOperationException("Signature help was not generated.");
        if (!MetadataEditorFeatures.Hover(member).Markdown.Contains("Tokenize", StringComparison.Ordinal) ||
            MetadataEditorFeatures.Definition(member, path).Token != member.Token)
            throw new InvalidOperationException("Hover or definition navigation was not generated.");
        var overloads = MetadataEditorFeatures.SignatureHelp([member, member with { DecodedSignature = "(string) -> void" },
            member with { Kind = "field" }], 1);
        if (overloads.Signatures.Length != 2 || overloads.ActiveParameter != 1 || overloads.ActiveSignature != 0)
            throw new InvalidOperationException("Signature candidates or active argument were lost.");
        var empty = MetadataEditorFeatures.SignatureHelp(Array.Empty<MetadataMember>());
        if (!empty.Signatures.IsEmpty || empty.ActiveSignature != -1)
            throw new InvalidOperationException("An empty candidate set selected a nonexistent signature.");
        try
        {
            MetadataEditorFeatures.SignatureHelp([member], -1);
            throw new InvalidOperationException("Negative parameter accepted.");
        }
        catch (ArgumentOutOfRangeException) { }
        var definition = MetadataEditorFeatures.Definition(member, path);
        if (new Uri(definition.Uri).LocalPath != Path.GetFullPath(path))
            throw new InvalidOperationException("Definition assembly path was lost.");
    }
}
