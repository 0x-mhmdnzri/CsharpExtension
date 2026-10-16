using DotnetExpertAdvisor.RoslynHost;
using DotnetExpertAdvisor.Shared;
using RoslynCompletionHost = DotnetExpertAdvisor.RoslynHost.RoslynHost;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class RoslynHostTests
{
    public static async Task RunAsync()
    {
        var text = "class Customer { public string Name { get; set; } } class C { void M() { Cu } }";
        var request = new DocumentRequest(new WorkspaceId("test"), new DocumentId("test.cs"),
            new ProjectId("test"), new TargetFrameworkId("net10.0"), 1, text);
        var result = await new RoslynCompletionHost().GetCompletionAsync(request, 0, text.IndexOf("Cu", StringComparison.Ordinal) + 2, CancellationToken.None);

        if (!result.Candidates.Any(candidate => candidate.Label == "Customer"))
            throw new InvalidOperationException("Roslyn completion did not return the declared type.");
        if (result.Candidates.Any(candidate => candidate.Label == "NotARealSymbol"))
            throw new InvalidOperationException("Roslyn completion returned a fabricated symbol.");

        var overloadText = "class C { void Do(int value) {} void Do(string value) {} void M() { Do } }";
        var overloadRequest = request with { Text = overloadText };
        var overloads = await new RoslynCompletionHost().GetCompletionAsync(overloadRequest, 0,
            overloadText.LastIndexOf("Do", StringComparison.Ordinal) + 2, CancellationToken.None);
        var doCandidates = overloads.Candidates.Where(candidate => candidate.Label == "Do").ToArray();
        if (doCandidates.Length != 2 || doCandidates.Any(candidate => candidate.EditLength != 2 || candidate.EditStart < 0
            || candidate.SymbolId.Length == 0 || candidate.LspKind != 2))
            throw new InvalidOperationException($"Roslyn candidate normalization lost overload identity or edit range: {string.Join(";", doCandidates.Select(candidate => $"{candidate.Label}/{candidate.Detail}/{candidate.EditStart}/{candidate.EditLength}/{candidate.LspKind}"))}");

        var typeText = "class C { static string Text => \"\"; static int Count => 1; void M() { string value = Te; } }";
        var typeResult = await new RoslynCompletionHost().GetCompletionAsync(request with { Text = typeText }, 0,
            typeText.LastIndexOf("Te", StringComparison.Ordinal) + 2, CancellationToken.None);
        if (typeResult.ExpectedType != "string" || !typeResult.Candidates.Any(candidate => candidate.Label == "Text")
            || typeResult.Candidates.Any(candidate => candidate.Label == "Count"))
            throw new InvalidOperationException($"Expected-type filtering failed: expected={typeResult.ExpectedType}; {string.Join(";", typeResult.Candidates.Select(candidate => $"{candidate.Label}/{candidate.Detail}"))}");

        var argumentText = "class C { static void Use(string value) {} static string Text => \"\"; static int Count => 1; void M() { Use(Te); } }";
        var argumentResult = await new RoslynCompletionHost().GetCompletionAsync(request with { Text = argumentText }, 0,
            argumentText.LastIndexOf("Te", StringComparison.Ordinal) + 2, CancellationToken.None);
        if (argumentResult.ExpectedType != "string" || !argumentResult.Candidates.Any(candidate => candidate.Label == "Text")
            || argumentResult.Candidates.Any(candidate => candidate.Label == "Count"))
            throw new InvalidOperationException($"Argument expected-type filtering failed: expected={argumentResult.ExpectedType}; {string.Join(";", argumentResult.Candidates.Select(candidate => $"{candidate.Label}/{candidate.Detail}"))}");

        var intentText = "class C { void M() { int unusedCount = 1; string used = \"\"; used.ToString();  } }";
        var intentResult = await new RoslynCompletionHost().GetCompletionAsync(request with { Text = intentText }, 0,
            intentText.IndexOf("  }", StringComparison.Ordinal) + 1, CancellationToken.None);
        var unused = intentResult.Candidates.FirstOrDefault(candidate => candidate.Label == "unusedCount");
        if (unused is null || !unused.Signals.Contains("unused-local-or-parameter") || unused.IntentScore <= 0)
            throw new InvalidOperationException("Cheap intent signals did not identify an unused local.");

        var packageAssembly = typeof(CSharpLexer).Assembly.Location;
        var packageMetadata = new AssemblyMetadataReader().Read(packageAssembly);
        var packageContext = new ProjectContextSnapshot("sample.csproj", "net10.0", "14", "Debug", 1,
            ["DotnetExpertAdvisor.Shared"], [], [], [],
            new NuGetApiIndex("net10.0", [new NuGetPackageApi("Example.Core", "1.0.0", packageAssembly, packageMetadata)]));
        var packageText = "using DotnetExpertAdvisor.Shared; class C { void M() { CSharpLe } }";
        var packageResult = await new RoslynCompletionHost { Context = packageContext }.GetCompletionAsync(
            request with { Text = packageText }, 0, packageText.LastIndexOf("CSharpLe", StringComparison.Ordinal) + 8, CancellationToken.None);
        var packageCandidate = packageResult.Candidates.FirstOrDefault(candidate => candidate.Label == "CSharpLexer");
        if (packageCandidate is null || packageCandidate.AssemblyName != packageMetadata.Name)
            throw new InvalidOperationException("Package API assembly was not available to Roslyn completion.");

        var patterns = ProjectPatternIndexer.Build(["class C { void Save() {} void Load() { Load(); Save(); Save(); } }"]);
        if (!patterns.SymbolFrequency.ContainsKey("Save") || !ProjectPatternIndexer.HasCallOrder(patterns, "Load", "Save"))
            throw new InvalidOperationException("Project pattern index did not capture frequency and call order.");
        var patternText = "class C { void Save() {} void Load() { Load(); Sa } }";
        var patternContext = packageContext with { Patterns = patterns };
        var patternResult = await new RoslynCompletionHost { Context = patternContext }.GetCompletionAsync(
            request with { Text = patternText }, 0, patternText.LastIndexOf("Sa", StringComparison.Ordinal) + 2, CancellationToken.None);
        var save = patternResult.Candidates.FirstOrDefault(candidate => candidate.Label == "Save");
        if (save is null || !save.Signals.Contains("project-frequency") || !save.Signals.Contains("project-call-order"))
            throw new InvalidOperationException("Project pattern signals were not applied to completion ranking.");

        var varText = "using System; class C { void M() { var date = DateTi; } }";
        var varResult = await new RoslynCompletionHost().GetCompletionAsync(request with { Text = varText }, 0,
            varText.IndexOf("DateTi", StringComparison.Ordinal) + 6, CancellationToken.None);
        if (varResult.ExpectedType is not null || !varResult.Candidates.Any(candidate => candidate.Label == "DateTime"))
            throw new InvalidOperationException("var initializer completion incorrectly applied a hard expected-type filter.");

        var sourceFile = Path.Combine(Path.GetTempPath(), $"cross-file-{Guid.NewGuid():N}.cs");
        try
        {
            File.WriteAllText(sourceFile, "public class CrossFileType { }");
            var crossContext = packageContext with { SourceFiles = [sourceFile] };
            var crossText = "class C { void M() { CrossFi } }";
            var crossResult = await new RoslynCompletionHost { Context = crossContext }.GetCompletionAsync(
                request with { Text = crossText }, 0, crossText.LastIndexOf("CrossFi", StringComparison.Ordinal) + 7, CancellationToken.None);
            if (!crossResult.Candidates.Any(candidate => candidate.Label == "CrossFileType"))
                throw new InvalidOperationException("Cross-file Roslyn compilation did not expose a referenced source symbol.");
            var beforeCache = RoslynSyntaxTreeCache.GetStats();
            await new RoslynCompletionHost { Context = crossContext }.GetCompletionAsync(
                request with { Text = crossText }, 0, crossText.LastIndexOf("CrossFi", StringComparison.Ordinal) + 7, CancellationToken.None);
            if (RoslynSyntaxTreeCache.GetStats().Hits <= beforeCache.Hits)
                throw new InvalidOperationException("Unchanged cross-file syntax was reparsed instead of using the cache.");
        }
        finally { File.Delete(sourceFile); }
    }
}
