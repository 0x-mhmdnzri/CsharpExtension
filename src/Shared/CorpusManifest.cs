using System.Collections.Immutable;
using System.Xml.Linq;

namespace DotnetExpertAdvisor.Shared;

public sealed record CorpusEntry(string Path, string LanguageVersion, ImmutableArray<string> TargetFrameworks);
public sealed record CorpusManifest(ImmutableArray<CorpusEntry> Entries);

public static class CorpusManifestBuilder
{
    public static CorpusManifest Build(string root)
    {
        var basePath = Path.GetFullPath(root);
        var projects = Directory.EnumerateFiles(basePath, "*.csproj", SearchOption.AllDirectories).ToArray();
        var entries = ImmutableArray.CreateBuilder<CorpusEntry>();
        foreach (var file in Directory.EnumerateFiles(basePath, "*.cs", SearchOption.AllDirectories))
        {
            var project = projects.FirstOrDefault(candidate => file.StartsWith(Path.GetDirectoryName(candidate)!, StringComparison.OrdinalIgnoreCase));
            var properties = project is null ? null : XDocument.Load(project).Descendants().Where(e => e.Parent?.Name.LocalName == "PropertyGroup").ToDictionary(e => e.Name.LocalName, e => e.Value);
            var frameworks = properties is null ? [] : (properties.TryGetValue("TargetFrameworks", out var many) ? many.Split(';') : properties.TryGetValue("TargetFramework", out var one) ? [one] : []);
            entries.Add(new CorpusEntry(Path.GetRelativePath(basePath, file), properties?.GetValueOrDefault("LangVersion", "default") ?? "default", frameworks.ToImmutableArray()));
        }
        return new CorpusManifest(entries.ToImmutable());
    }
}
