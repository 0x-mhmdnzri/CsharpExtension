using System.Collections.Immutable;
using System.Diagnostics;
using System.Text.Json;
using System.Xml;
using Microsoft.CodeAnalysis.CSharp;
using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.RoslynHost;

public sealed record LoadedAssemblyEvidence(string Name, string Path, string TargetFramework, bool HasTypeForwarders);

public sealed record ProjectContextSnapshot(string? ProjectPath, string? TargetFramework,
    string? LanguageVersion, string Configuration, int DocumentVersion,
    ImmutableArray<string> Imports, ImmutableArray<string> References,
    ImmutableArray<string> Defines, ImmutableArray<string> Issues, NuGetApiIndex? NuGetApi = null,
    ImmutableArray<LoadedAssemblyEvidence> Assemblies = default)
{
    public bool IsComplete => Issues.IsEmpty;
}

public static class ProjectContextReader
{
    public static async Task<ProjectContextSnapshot> ReadAsync(string documentUri, int version,
        string? projectPath, string? targetFramework, string configuration, CancellationToken cancellationToken,
        string documentText = "")
    {
        var issues = ImmutableArray.CreateBuilder<string>();
        if (string.IsNullOrWhiteSpace(projectPath)) projectPath = null;
        if (string.IsNullOrWhiteSpace(targetFramework)) targetFramework = null;
        if (projectPath is null && Uri.TryCreate(documentUri, UriKind.Absolute, out var uri) && uri.IsFile)
        {
            for (var directory = Path.GetDirectoryName(uri.LocalPath); directory is not null; directory = Path.GetDirectoryName(directory))
            {
                var projects = Directory.GetFiles(directory, "*.csproj");
                if (projects.Length == 0) continue;
                if (projects.Length == 1) projectPath = projects[0];
                else issues.Add("Multiple projects; select the active project explicitly.");
                break;
            }
        }
        if (version < 0) issues.Add("Document version is unknown.");
        if (projectPath is null || !File.Exists(projectPath))
            return new(projectPath, targetFramework, null, configuration, version, [], [], [],
                issues.ToImmutable().Add("Project is unknown or missing."));

        var start = new ProcessStartInfo("dotnet") { RedirectStandardOutput = true, RedirectStandardError = true, UseShellExecute = false };
        foreach (var argument in new[] { "msbuild", Path.GetFullPath(projectPath), "-nologo",
                     "-getProperty:LangVersion,TargetFramework,TargetFrameworks,Configuration,DefineConstants,TargetPath",
                     "-getItem:Using,Reference,ReferencePath,ProjectReference", $"-property:Configuration={configuration}" })
            start.ArgumentList.Add(argument);
        if (!string.IsNullOrEmpty(targetFramework)) start.ArgumentList.Add($"-property:TargetFramework={targetFramework}");
        using var process = new Process { StartInfo = start };
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeout.CancelAfter(TimeSpan.FromSeconds(10));
        try
        {
            process.Start();
            var stdout = process.StandardOutput.ReadToEndAsync(timeout.Token);
            var stderr = process.StandardError.ReadToEndAsync(timeout.Token);
            await process.WaitForExitAsync(timeout.Token);
            var output = await stdout;
            await stderr;
            if (process.ExitCode != 0) throw new InvalidDataException("Project evaluation failed.");
            using var json = JsonDocument.Parse(output);
            var properties = json.RootElement.GetProperty("Properties");
            string Property(string name) => properties.GetProperty(name).GetString() ?? "";
            var frameworks = Property("TargetFrameworks").Split(';', StringSplitOptions.RemoveEmptyEntries);
            targetFramework = string.IsNullOrEmpty(targetFramework) ? Property("TargetFramework") : targetFramework;
            if (string.IsNullOrEmpty(targetFramework)) issues.Add("Target framework is unknown or ambiguous.");
            else if (frameworks.Length > 0 && !frameworks.Contains(targetFramework)) issues.Add("Selected target framework is not declared by the project.");
            var language = Property("LangVersion");
            if (!LanguageVersionFacts.TryParse(language, out _)) issues.Add("Language version is unknown.");
            var items = json.RootElement.GetProperty("Items");
            ImmutableArray<string> Items(string name) => items.GetProperty(name).EnumerateArray()
                .Select(item => item.GetProperty("Identity").GetString()!).ToImmutableArray();
            NuGetApiIndex? nuGetApi = null;
            try
            {
                var index = NuGetProjectIndexReader.Read(projectPath);
                var target = index.TargetFrameworks.FirstOrDefault(framework => framework.Name.Equals(targetFramework, StringComparison.OrdinalIgnoreCase)
                    || framework.Name.EndsWith($"Version=v{targetFramework.TrimStart('n', 'e', 't')}", StringComparison.OrdinalIgnoreCase));
                var packageRoot = Environment.GetEnvironmentVariable("NUGET_PACKAGES")
                    ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nuget", "packages");
                if (target is not null) nuGetApi = NuGetProjectIndexReader.ReadApiIndex(index, packageRoot, target.Name,
                    cancellationToken: cancellationToken);
                else issues.Add("NuGet assets are missing for the selected target framework.");
            }
            catch (Exception exception) when (exception is IOException or JsonException or InvalidOperationException or XmlException)
            {
                issues.Add("NuGet API index is unavailable.");
            }
            var assemblyPaths = Items("ReferencePath").Concat([Property("TargetPath")])
                .Where(path => !string.IsNullOrWhiteSpace(path) && File.Exists(path)).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
            var assemblies = assemblyPaths.Select(path => ReadAssemblyEvidence(path, targetFramework!))
                .Where(evidence => evidence is not null).Cast<LoadedAssemblyEvidence>().ToImmutableArray();
            if (assemblies.IsEmpty) issues.Add("Assembly references are unavailable for the selected target framework.");
            return new(Path.GetFullPath(projectPath), targetFramework, language, Property("Configuration"), version,
                Items("Using").AddRange(CSharpSyntaxTree.ParseText(documentText, cancellationToken: cancellationToken)
                    .GetCompilationUnitRoot(cancellationToken).Usings
                    .Where(item => item.Alias is null && item.StaticKeyword.RawKind == 0 && item.Name is not null)
                    .Select(item => item.Name!.ToString())).Distinct(StringComparer.Ordinal).ToImmutableArray(),
                Items("Reference").AddRange(Items("ReferencePath")).AddRange(Items("ProjectReference")),
                Property("DefineConstants").Split(';', StringSplitOptions.RemoveEmptyEntries).ToImmutableArray(), issues.ToImmutable(), nuGetApi, assemblies);
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            issues.Add("Project evaluation timed out.");
        }
        catch (Exception exception) when (exception is IOException or System.ComponentModel.Win32Exception or JsonException or InvalidOperationException)
        {
            issues.Add("Project evaluation is unavailable.");
        }
        finally
        {
            try { if (process.Id > 0 && !process.HasExited) process.Kill(entireProcessTree: true); }
            catch (InvalidOperationException) { }
        }
        return new(projectPath, targetFramework, null, configuration, version, [], [], [], issues.ToImmutable());
    }

    private static LoadedAssemblyEvidence? ReadAssemblyEvidence(string path, string targetFramework)
    {
        try
        {
            var metadata = new AssemblyMetadataReader().Read(path);
            return new(metadata.Name, Path.GetFullPath(path), targetFramework, !metadata.TypeForwarders.IsEmpty);
        }
        catch (Exception exception) when (exception is IOException or BadImageFormatException or UnauthorizedAccessException)
        {
            return null;
        }
    }
}
