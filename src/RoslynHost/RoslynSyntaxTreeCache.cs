using System.Collections.Concurrent;
using System.Security.Cryptography;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace DotnetExpertAdvisor.RoslynHost;

public static class RoslynSyntaxTreeCache
{
    private static readonly ConcurrentDictionary<string, SyntaxTree> Trees = new(StringComparer.Ordinal);
    private static long _hits;
    private static long _misses;

    public static SyntaxTree Get(string path, string text, CSharpParseOptions options, CancellationToken cancellationToken)
    {
        var hash = Convert.ToHexString(SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(text)));
        var key = $"{Path.GetFullPath(path)}|{hash}|{options.LanguageVersion}|{string.Join(';', options.PreprocessorSymbolNames)}";
        if (Trees.TryGetValue(key, out var tree))
        {
            Interlocked.Increment(ref _hits);
            return tree;
        }
        Interlocked.Increment(ref _misses);
        tree = CSharpSyntaxTree.ParseText(text, options, path, cancellationToken: cancellationToken);
        Trees[key] = tree;
        return tree;
    }

    public static (long Hits, long Misses) GetStats() => (Volatile.Read(ref _hits), Volatile.Read(ref _misses));
}
