using System.Collections.Immutable;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;

namespace DotnetExpertAdvisor.Shared;

public sealed record NuGetPackageReference(string Id, string Version, bool IsDirect,
    string PrivateAssets, string IncludeAssets);
public sealed record NuGetAsset(string PackageId, string PackageVersion, string Path, string Kind);
public sealed record NuGetTargetFramework(string Name, ImmutableArray<NuGetAsset> Assets);
public sealed record NuGetProjectIndex(string ProjectPath, ImmutableArray<NuGetPackageReference> Packages,
    ImmutableArray<NuGetTargetFramework> TargetFrameworks);
public sealed record NuGetPackageApi(string PackageId, string PackageVersion, string AssemblyPath,
    AssemblyMetadataIndex Metadata);
public sealed record NuGetApiIndex(string TargetFramework,
    ImmutableArray<NuGetPackageApi> Packages);

public static class NuGetProjectIndexReader
{
    public static NuGetProjectIndex Read(string projectPath, string? assetsPath = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(projectPath);
        cancellationToken.ThrowIfCancellationRequested();
        var packages = ReadPackageReferences(projectPath, cancellationToken);
        assetsPath ??= Path.Combine(Path.GetDirectoryName(projectPath) ?? "", "obj", "project.assets.json");
        var frameworks = File.Exists(assetsPath)
            ? ReadAssets(assetsPath, cancellationToken)
            : ImmutableArray<NuGetTargetFramework>.Empty;
        return new NuGetProjectIndex(Path.GetFullPath(projectPath), packages, frameworks);
    }

    public static NuGetApiIndex ReadApiIndex(NuGetProjectIndex project, string packageRoot,
        string targetFramework, AssemblyMetadataReader? metadataReader = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(project);
        ArgumentException.ThrowIfNullOrWhiteSpace(packageRoot);
        var target = project.TargetFrameworks.SingleOrDefault(framework => framework.Name == targetFramework)
            ?? throw new InvalidOperationException($"Target framework is not present: {targetFramework}");
        metadataReader ??= new AssemblyMetadataReader();
        var packages = ImmutableArray.CreateBuilder<NuGetPackageApi>();
        foreach (var asset in target.Assets.Where(asset => asset.Kind == "compile"))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var assemblyPath = Path.Combine(packageRoot, asset.PackageId.ToLowerInvariant(),
                asset.PackageVersion.ToLowerInvariant(), asset.Path.Replace('/', Path.DirectorySeparatorChar));
            if (!File.Exists(assemblyPath)) continue;
            packages.Add(new NuGetPackageApi(asset.PackageId, asset.PackageVersion, assemblyPath,
                metadataReader.Read(assemblyPath, cancellationToken: cancellationToken)));
        }
        return new NuGetApiIndex(target.Name, packages.ToImmutable());
    }

    private static ImmutableArray<NuGetPackageReference> ReadPackageReferences(string path,
        CancellationToken cancellationToken)
    {
        using var xml = XmlReader.Create(path, new XmlReaderSettings
        {
            DtdProcessing = DtdProcessing.Prohibit,
            XmlResolver = null,
            MaxCharactersInDocument = 8 * 1024 * 1024
        });
        var document = XDocument.Load(xml);
        var packages = ImmutableArray.CreateBuilder<NuGetPackageReference>();
        foreach (var element in document.Descendants().Where(element => element.Name.LocalName == "PackageReference"))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var id = (string?)element.Attribute("Include");
            if (string.IsNullOrWhiteSpace(id)) throw new XmlException("PackageReference has no Include.");
            var version = (string?)element.Attribute("Version") ??
                (string?)element.Elements().FirstOrDefault(child => child.Name.LocalName == "Version");
            packages.Add(new NuGetPackageReference(id, version ?? "",
                true, Metadata(element, "PrivateAssets", "none"), Metadata(element, "IncludeAssets", "all")));
        }
        return packages.ToImmutable();
    }

    private static string Metadata(XElement element, string name, string fallback)
        => (string?)element.Attribute(name) ??
            (string?)element.Elements().FirstOrDefault(child => child.Name.LocalName == name) ?? fallback;

    private static ImmutableArray<NuGetTargetFramework> ReadAssets(string path,
        CancellationToken cancellationToken)
    {
        using var stream = File.OpenRead(path);
        using var document = JsonDocument.Parse(stream, new JsonDocumentOptions
        {
            MaxDepth = 128,
            CommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        });
        if (!document.RootElement.TryGetProperty("targets", out var targets) || targets.ValueKind != JsonValueKind.Object)
            return ImmutableArray<NuGetTargetFramework>.Empty;
        var frameworks = ImmutableArray.CreateBuilder<NuGetTargetFramework>();
        foreach (var target in targets.EnumerateObject())
        {
            cancellationToken.ThrowIfCancellationRequested();
            var assets = ImmutableArray.CreateBuilder<NuGetAsset>();
            if (target.Value.ValueKind != JsonValueKind.Object) continue;
            foreach (var package in target.Value.EnumerateObject())
            {
                var separator = package.Name.IndexOf('/');
                if (separator <= 0 || separator == package.Name.Length - 1) continue;
                var id = package.Name[..separator];
                var version = package.Name[(separator + 1)..];
                AddAssets(package.Value, "compile", id, version, assets);
                AddAssets(package.Value, "runtime", id, version, assets);
            }
            frameworks.Add(new NuGetTargetFramework(target.Name, assets.ToImmutable()));
        }
        return frameworks.ToImmutable();
    }

    private static void AddAssets(JsonElement package, string kind, string id, string version,
        ImmutableArray<NuGetAsset>.Builder assets)
    {
        if (!package.TryGetProperty(kind, out var files) || files.ValueKind != JsonValueKind.Object) return;
        foreach (var file in files.EnumerateObject())
            assets.Add(new NuGetAsset(id, version, file.Name, kind));
    }
}
