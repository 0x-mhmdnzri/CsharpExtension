using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class CompletionEngineTests
{
    public static void Run()
    {
        var source = """
            public class AppDbContext : DbContext { public DbSet<Order> Orders { get; set; } }
            public class Order { public Customer Customer { get; set; } }
            public class Customer { public string Name { get; set; } }
            class Query { void Run(AppDbContext db) { db.Orders.Include(o => o.); } }
            """;
        var lines = source.Split('\n');
        var line = lines.Length - 1;
        var character = lines[^1].IndexOf("o.)", StringComparison.Ordinal) + 2;
        var suggestions = CompletionEngine.Complete(source, line, character);
        if (!suggestions.Any(suggestion => suggestion.Label == "Customer"))
            throw new InvalidOperationException("Completion engine did not return EF navigation suggestions.");
        var metadata = suggestions.Single(suggestion => suggestion.Label == "Customer");
        if (metadata.RuleId != "completion.ef-navigation" || metadata.Confidence <= 0 || metadata.Source.Length == 0)
            throw new InvalidOperationException("Completion metadata was incomplete.");

        if (!CompletionEngine.Complete("class Customer { }", 0, 6)
                .Any(suggestion => suggestion.Label == "Customer"))
            throw new InvalidOperationException("Completion engine did not return symbols for Ctrl+Space.");
    }
}
