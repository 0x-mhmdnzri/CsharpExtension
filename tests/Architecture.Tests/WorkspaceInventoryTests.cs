using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class WorkspaceInventoryTests
{
    public static void Run()
    {
        var root = Path.Combine(Path.GetTempPath(), $"inventory-{Guid.NewGuid():N}");
        Directory.CreateDirectory(Path.Combine(root, "src"));
        Directory.CreateDirectory(Path.Combine(root, "bin"));
        try
        {
            File.WriteAllText(Path.Combine(root, "src", "One.cs"), "class One {}");
            File.WriteAllText(Path.Combine(root, "src", "One.csproj"), "<Project />");
            File.WriteAllText(Path.Combine(root, "bin", "Ignored.cs"), "class Ignored {}");
            var inventory = WorkspaceInventoryBuilder.Build(root);
            var file = inventory.Files.Single(item => item.Path.EndsWith("One.cs", StringComparison.Ordinal));
            if (!file.Accessible || file.ContentHash.Length != 64 || inventory.Files.Any(item => item.Path.Contains("bin")))
                throw new InvalidOperationException("Workspace inventory did not hash source files or exclude build output.");
        }
        finally { Directory.Delete(root, recursive: true); }
    }
}
