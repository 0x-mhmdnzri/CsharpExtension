using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class EfModelAnalyzerTests
{
    public static void Run()
    {
        var model = EfModelAnalyzer.Analyze("""
            public class AppDbContext : DbContext { public DbSet<Customer> Customers { get; set; } }
            public class Customer { public int Id { get; set; } public string Name { get; set; } }
            """);
if (model.Contexts.Single().Name != "AppDbContext" || !model.Contexts.Single().DbSets.Contains("Customers") ||
            model.Entities.Single().Name != "Customer" || model.Entities.Single().Properties.Length != 2)
            throw new InvalidOperationException("EF DbContext, DbSet or entity model was not detected.");
    }
}

