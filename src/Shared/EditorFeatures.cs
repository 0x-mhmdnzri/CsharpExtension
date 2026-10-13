using System.Collections.Immutable;

namespace DotnetExpertAdvisor.Shared;

public sealed record SignatureInformation(string Label, string Documentation);
public sealed record SignatureHelpResult(ImmutableArray<SignatureInformation> Signatures, int ActiveSignature,
    int ActiveParameter);
public sealed record HoverResult(string Markdown);
public sealed record DefinitionLocation(string Uri, int Token);

public static class MetadataEditorFeatures
{
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
