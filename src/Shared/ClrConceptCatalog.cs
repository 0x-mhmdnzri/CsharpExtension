using System.Collections.Immutable;

namespace DotnetExpertAdvisor.Shared;

public static class ClrConceptCatalog
{
    private static readonly (string Id, string Title, string[] Tags, string SourceId, string Url)[] Entries =
    [
        ("csharp.types", "C# types", ["C#", "language"], "ms.csharp", "https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/"),
        ("csharp.classes", "C# classes", ["C#", "object-oriented"], "ms.csharp", "https://learn.microsoft.com/dotnet/csharp/fundamentals/types/classes"),
        ("csharp.records", "C# records", ["C#", "language"], "ms.csharp", "https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/record"),
        ("csharp.async", "C# asynchronous programming", ["C#", "async"], "ms.csharp", "https://learn.microsoft.com/dotnet/csharp/asynchronous-programming/"),
        ("csharp.linq", "Language Integrated Query", ["C#", "LINQ"], "ms.linq", "https://learn.microsoft.com/dotnet/csharp/linq/"),
        ("csharp.nullable", "Nullable reference types", ["C#", "nullability"], "ms.csharp", "https://learn.microsoft.com/dotnet/csharp/nullable-references"),
        ("clr.type-system", "CLR type system", ["CLR", "types"], "ms.clr", "https://learn.microsoft.com/dotnet/standard/base-types/common-type-system"),
        ("clr.gc", "Garbage collection", ["CLR", "memory"], "ms.gc", "https://learn.microsoft.com/dotnet/standard/garbage-collection/"),
        ("clr.exceptions", "CLR exception handling", ["CLR", "reliability"], "ms.clr", "https://learn.microsoft.com/dotnet/standard/exceptions/"),
        ("clr.threading", "CLR threading", ["CLR", "concurrency"], "ms.threading", "https://learn.microsoft.com/dotnet/standard/threading/threads-and-threading")
    ];

    public static KnowledgeRegistry Create()
    {
        var sources = Entries.GroupBy(entry => entry.SourceId, StringComparer.Ordinal)
            .Select(group => new KnowledgeSource(group.Key, group.First().Url, group.Key switch
            {
                "ms.csharp" => "Microsoft C# documentation",
                "ms.linq" => "Microsoft LINQ documentation",
                "ms.gc" => "Microsoft .NET garbage collection documentation",
                "ms.threading" => "Microsoft .NET threading documentation",
                _ => "Microsoft .NET documentation"
            })).ToImmutableArray();
        var concepts = Entries.Select(entry => new KnowledgeConcept(entry.Id, entry.Title, entry.Tags.ToImmutableArray())).ToImmutableArray();
        var claims = Entries.Select(entry => new KnowledgeClaim(
            $"{entry.Id}:definition", entry.Id, entry.SourceId, entry.Title)).ToImmutableArray();
        var rules = Entries.Select(entry => new KnowledgeRule(
            $"{entry.Id}:source", $"{entry.Id}:definition", $"Use the official {entry.Title} reference.")).ToImmutableArray();
        return new KnowledgeRegistry(concepts, sources, claims, rules);
    }
}
