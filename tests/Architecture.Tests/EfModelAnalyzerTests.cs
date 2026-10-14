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

        var context = model.Contexts.Single();
        if (context.Name != "AppDbContext"
            || !context.DbSets.Any(set => set.Value == "Customers" && set.Source == EfProvenance.Convention)
            || !context.IsComplete
            || !model.IsComplete)
            throw new InvalidOperationException("EF context provenance or IsComplete was wrong.");

        var entity = model.Entities.Single();
        if (entity.Name != "Customer"
            || entity.Properties.Length != 2
            || entity.Properties.Any(property => property.Confidence != EfConfidence.High))
            throw new InvalidOperationException("EF entity facts were not detected with confidence.");

        var incomplete = EfModelAnalyzer.Analyze("""
            public class AppDbContext : DbContext
            {
                public DbSet<Customer> Customers { get; set; }
                protected override void OnModelCreating(ModelBuilder modelBuilder)
                {
                    dynamic cfg = Activator.CreateInstance(Type.GetType("External.Config"));
                }
            }
            public class Customer { public int Id { get; set; } }
            """);

        if (incomplete.IsComplete || incomplete.Contexts.Single().IsComplete)
            throw new InvalidOperationException("Dynamic/Activator model must set IsComplete=false.");
    }
}
