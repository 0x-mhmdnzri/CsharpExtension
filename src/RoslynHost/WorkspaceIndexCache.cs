using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.RoslynHost;

public sealed record WorkspaceIndexCacheEntry(string Fingerprint, ProjectContextSnapshot Context);
public sealed record WorkspaceIndexCacheFile(Dictionary<string, WorkspaceIndexCacheEntry> Entries);

public static class WorkspaceIndexCache
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.General) { WriteIndented = false };

    public static string CachePath(string projectPath)
    {
        var directory = Path.GetDirectoryName(Path.GetFullPath(projectPath))!;
        for (var current = directory; current is not null; current = Path.GetDirectoryName(current))
        {
            if (Directory.Exists(Path.Combine(current, ".vscode")))
                return Path.Combine(current, ".vscode", "dotnet-expert-advisor.index.json");
        }
        var path = Path.Combine(directory, ".vscode");
        Directory.CreateDirectory(path);
        return Path.Combine(path, "dotnet-expert-advisor.index.json");
    }

    public static string Fingerprint(string projectPath, string? targetFramework, string configuration)
    {
        var root = Path.GetDirectoryName(Path.GetFullPath(projectPath))!;
        IEnumerable<string> files = Directory.Exists(root)
            ? Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories)
                .Where(path => !path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                    .Any(part => part is ".git" or "bin" or "obj" or ".vscode"))
                .Where(path => Path.GetExtension(path).ToLowerInvariant() is ".cs" or ".csproj" or ".props" or ".targets" ||
                    Path.GetFileName(path).Equals("project.assets.json", StringComparison.OrdinalIgnoreCase))
                .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
            : Array.Empty<string>();
        var input = new StringBuilder().Append(Path.GetFullPath(projectPath)).Append('|')
            .Append(targetFramework).Append('|').Append(configuration);
        foreach (var file in files)
        {
            var info = new FileInfo(file);
            input.Append('|').Append(file).Append('|').Append(info.Length).Append('|').Append(info.LastWriteTimeUtc.Ticks);
        }
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(input.ToString()))).ToLowerInvariant();
    }

    public static bool TryLoad(string path, string key, string fingerprint, out ProjectContextSnapshot context)
    {
        context = default!;
        try
        {
            if (!File.Exists(path)) return false;
            var cache = JsonSerializer.Deserialize<WorkspaceIndexCacheFile>(File.ReadAllText(path), Options);
            if (cache?.Entries is null || !cache.Entries.TryGetValue(key, out var entry) || entry is null || entry.Fingerprint != fingerprint) return false;
            context = entry.Context;
            return true;
        }
        catch (Exception exception) when (exception is IOException or JsonException or NotSupportedException)
        {
            return false;
        }
    }

    public static void Save(string path, string key, string fingerprint, ProjectContextSnapshot context)
    {
        try
        {
            var cache = File.Exists(path)
                ? JsonSerializer.Deserialize<WorkspaceIndexCacheFile>(File.ReadAllText(path), Options) ?? new(new())
                : new(new Dictionary<string, WorkspaceIndexCacheEntry>(StringComparer.Ordinal));
            cache.Entries[key] = new(fingerprint, context with { IndexCacheHit = false, DocumentVersion = 0 });
            var temporary = path + ".tmp";
            File.WriteAllText(temporary, JsonSerializer.Serialize(cache, Options));
            File.Move(temporary, path, true);
        }
        catch (Exception exception) when (exception is IOException or JsonException or NotSupportedException)
        {
            // Cache failure must never block completion.
        }
    }
}
