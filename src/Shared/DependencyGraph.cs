using System.Collections.Immutable;
using System.Xml.Linq;

namespace DotnetExpertAdvisor.Shared;

public sealed record DependencyNode(string Id, string Kind, string Name);
public sealed record DependencyEdge(string Source, string Target, string Kind);
public sealed record DependencyGraphSnapshot(ImmutableArray<DependencyNode> Nodes, ImmutableArray<DependencyEdge> Edges);

public static class DependencyGraphBuilder
{
    public static DependencyGraphSnapshot Build(IEnumerable<string> projectPaths, CancellationToken cancellationToken = default)
    {
        var nodes = ImmutableDictionary.CreateBuilder<string, DependencyNode>(StringComparer.OrdinalIgnoreCase);
        var edges = ImmutableArray.CreateBuilder<DependencyEdge>();
        foreach (var path in projectPaths.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!File.Exists(path)) continue;
            var projectId = Path.GetFullPath(path);
            nodes[projectId] = new(projectId, "project", Path.GetFileNameWithoutExtension(path));
            var document = XDocument.Load(path, LoadOptions.PreserveWhitespace);
            foreach (var reference in document.Descendants().Where(element => element.Name.LocalName == "ProjectReference"))
            {
                var include = (string?)reference.Attribute("Include");
                if (string.IsNullOrWhiteSpace(include)) continue;
                var target = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(path)!, include));
                nodes[target] = new(target, "project", Path.GetFileNameWithoutExtension(target));
                edges.Add(new(projectId, target, "project-reference"));
            }
            foreach (var package in document.Descendants().Where(element => element.Name.LocalName == "PackageReference"))
            {
                var id = (string?)package.Attribute("Include");
                if (string.IsNullOrWhiteSpace(id)) continue;
                var packageId = $"nuget:{id}";
                nodes[packageId] = new(packageId, "package", id);
                edges.Add(new(projectId, packageId, "package-reference"));
            }
        }
        return new(nodes.Values.ToImmutableArray(), edges.ToImmutable());
    }
}
