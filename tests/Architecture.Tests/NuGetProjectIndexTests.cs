using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class NuGetProjectIndexTests
{
    public static void Run()
    {
        var root = Path.Combine(Path.GetTempPath(), $"nuget-index-{Guid.NewGuid():N}");
        Directory.CreateDirectory(Path.Combine(root, "obj"));
        var project = Path.Combine(root, "Sample.csproj");
        var assets = Path.Combine(root, "obj", "project.assets.json");
        try
        {
            File.WriteAllText(project, """
                <Project><ItemGroup>
                  <PackageReference Include="Example.Core" Version="1.2.3" />
                  <PackageReference Include="Example.Tools"><Version>4.5.6</Version><PrivateAssets>all</PrivateAssets></PackageReference>
                </ItemGroup></Project>
                """);
            File.WriteAllText(assets, """
                {"targets":{".NETCoreApp,Version=v10.0":{
                  "Example.Core/1.2.3":{"compile":{"ref/net10.0/Example.Core.dll":{}},"runtime":{"lib/net10.0/Example.Core.dll":{}}},
                  "Example.Tools/4.5.6":{"compile":{"ref/net10.0/Example.Tools.dll":{}}}
                }}}
                """);
            var index = NuGetProjectIndexReader.Read(project);
            if (index.Packages.Length != 2 || index.Packages[1].Version != "4.5.6" || index.Packages[1].PrivateAssets != "all")
                throw new InvalidOperationException("PackageReference entries were not indexed.");
            var framework = index.TargetFrameworks.Single(framework => framework.Name == ".NETCoreApp,Version=v10.0");
            if (framework.Assets.Length != 3 || !framework.Assets.Any(asset => asset.Kind == "runtime" && asset.PackageId == "Example.Core"))
                throw new InvalidOperationException("project.assets.json compile/runtime assets were not indexed.");
            File.WriteAllText(assets, "{\"targets\":{\"net8.0\":{}}}");
            if (NuGetProjectIndexReader.Read(project).TargetFrameworks.Single().Name != "net8.0")
                throw new InvalidOperationException("Target framework changes were not observed.");
        }
        finally { if (Directory.Exists(root)) Directory.Delete(root, true); }
    }
}
