using DotnetExpertAdvisor.RoslynHost;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class ProjectContextTests
{
    public static async Task RunAsync()
    {
        var directory = Path.Combine(Path.GetTempPath(), $"project-context-{Guid.NewGuid():N}");
        Directory.CreateDirectory(directory);
        try
        {
            var project = Path.Combine(directory, "First.csproj");
            await File.WriteAllTextAsync(project, """
                <Project Sdk="Microsoft.NET.Sdk">
                  <PropertyGroup>
                    <TargetFrameworks>net8.0;net10.0</TargetFrameworks>
                    <LangVersion Condition="'$(TargetFramework)' == 'net8.0'">12</LangVersion>
                    <LangVersion Condition="'$(TargetFramework)' == 'net10.0'">14</LangVersion>
                  </PropertyGroup>
                  <ItemGroup><Using Include="System.Threading.Tasks" /><Reference Include="Example" /></ItemGroup>
                </Project>
                """);
            var uri = new Uri(Path.Combine(directory, "Test.cs")).AbsoluteUri;
            var older = await ProjectContextReader.ReadAsync(uri, 7, project, "net8.0", "Release", CancellationToken.None,
                "using System.Linq; class C {}");
            var newer = await ProjectContextReader.ReadAsync(uri, 8, project, "net10.0", "Debug", CancellationToken.None);
            if (older.LanguageVersion != "12" || newer.LanguageVersion != "14" || older.Configuration != "Release"
                || older.DocumentVersion != 7 || !older.Imports.Contains("System.Linq")
                || !older.References.Contains("Example") || older.IsComplete)
                throw new InvalidOperationException("Evaluated project/TFM snapshot is incorrect or overconfident.");
            var ambiguousTfm = await ProjectContextReader.ReadAsync(uri, 9, project, null, "Debug", CancellationToken.None);
            if (!ambiguousTfm.Issues.Any(issue => issue.Contains("ambiguous")))
                throw new InvalidOperationException("Multi-TFM context must require an explicit selection.");
            await File.WriteAllTextAsync(Path.Combine(directory, "Second.csproj"), "<Project />");
            var ambiguousProject = await ProjectContextReader.ReadAsync(uri, 10, null, null, "Debug", CancellationToken.None);
            if (ambiguousProject.ProjectPath is not null || ambiguousProject.IsComplete)
                throw new InvalidOperationException("Ambiguous active project must not be selected arbitrarily.");
        }
        finally { Directory.Delete(directory, recursive: true); }
    }
}
