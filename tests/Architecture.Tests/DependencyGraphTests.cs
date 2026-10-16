using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class DependencyGraphTests
{
    public static void Run()
    {
        var root = Path.Combine(Path.GetTempPath(), $"dependency-graph-{Guid.NewGuid():N}");
        Directory.CreateDirectory(root);
        var app = Path.Combine(root, "App.csproj");
        var core = Path.Combine(root, "Core.csproj");
        try
        {
            File.WriteAllText(core, "<Project />");
            File.WriteAllText(app, $"<Project><ItemGroup><ProjectReference Include=\"{Path.GetFileName(core)}\" /><PackageReference Include=\"Example.Core\" Version=\"1.0.0\" /></ItemGroup></Project>");
            var graph = DependencyGraphBuilder.Build([app, core]);
            if (!graph.Edges.Any(edge => edge.Kind == "project-reference" && edge.Target == Path.GetFullPath(core))
                || !graph.Edges.Any(edge => edge.Kind == "package-reference" && edge.Target == "nuget:Example.Core"))
                throw new InvalidOperationException("Dependency graph lost project or package references.");
        }
        finally { Directory.Delete(root, recursive: true); }
    }
}
