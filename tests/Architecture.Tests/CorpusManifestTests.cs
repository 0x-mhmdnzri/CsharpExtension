using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class CorpusManifestTests
{
    public static void Run()
    {
        var root = Path.Combine(Path.GetTempPath(), "corpus-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            File.WriteAllText(Path.Combine(root, "Sample.csproj"), "<Project><PropertyGroup><TargetFrameworks>net8.0;net10.0</TargetFrameworks><LangVersion>preview</LangVersion></PropertyGroup></Project>");
            File.WriteAllText(Path.Combine(root, "Sample.cs"), "class Sample {}");
            var manifest = CorpusManifestBuilder.Build(root);
            if (manifest.Entries.Length != 1 || manifest.Entries[0].LanguageVersion != "preview" || manifest.Entries[0].TargetFrameworks.Length != 2)
                throw new InvalidOperationException("Corpus manifest lost language or target framework metadata.");
        }
        finally { Directory.Delete(root, true); }
    }
}
