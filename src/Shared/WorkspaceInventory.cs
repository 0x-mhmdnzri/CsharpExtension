using System.Collections.Immutable;
using System.Security.Cryptography;

namespace DotnetExpertAdvisor.Shared;

public sealed record IndexedFile(string Path, string Kind, string ContentHash, long Length, bool Accessible);

public sealed record WorkspaceInventory(string RootPath, ImmutableArray<IndexedFile> Files, DateTimeOffset CreatedAt);

public static class WorkspaceInventoryBuilder
{
    public static WorkspaceInventory Build(string rootPath, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rootPath);
        var root = Path.GetFullPath(rootPath);
        var files = ImmutableArray.CreateBuilder<IndexedFile>();
        if (!Directory.Exists(root)) return new(root, [], DateTimeOffset.UtcNow);
        foreach (var path in Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories)
                     .Where(path => !IsExcluded(path)))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var kind = Path.GetExtension(path).ToLowerInvariant() switch
            {
                ".cs" => "document",
                ".csproj" => "project",
                ".props" or ".targets" => "build",
                ".json" when path.EndsWith("project.assets.json", StringComparison.OrdinalIgnoreCase) => "assets",
                ".dll" or ".exe" => "assembly",
                _ => "other"
            };
            try
            {
                var info = new FileInfo(path);
                using var stream = File.OpenRead(path);
                files.Add(new(Path.GetRelativePath(root, path), kind,
                    Convert.ToHexString(SHA256.HashData(stream)).ToLowerInvariant(), info.Length, true));
            }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
            {
                files.Add(new(Path.GetRelativePath(root, path), kind, string.Empty, 0, false));
            }
        }
        return new(root, files.ToImmutable(), DateTimeOffset.UtcNow);
    }

    private static bool IsExcluded(string path)
        => path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            .Any(part => part is ".git" or "bin" or "obj" or ".vs" or ".idea");
}
