using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class EfModelAnalyzerTests
{
    public static void Run()
    {
        var model = EfModelAnalyzer.Analyze("""
            public class AppDbContext : DbContext
            {
                public DbSet<Customer> Customers { get; set; }
                public DbSet<Order> Orders { get; set; }
            }
            public class Customer
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public ICollection<Order> Orders { get; set; }
            }
            public class Order
            {
                [Key]
                public int OrderKey { get; set; }
                public int CustomerId { get; set; }
                [ForeignKey("CustomerId")]
                public Customer Customer { get; set; }
            }
            """);

        var context = model.Contexts.Single();
        if (context.Name != "AppDbContext" || !context.IsComplete || !model.IsComplete)
            throw new InvalidOperationException("EF context completeness failed.");

        var customer = model.Entities.Single(entity => entity.Name == "Customer");
        if (customer.Keys.Single().Value != "Id" || customer.Keys.Single().Source != EfProvenance.Convention)
            throw new InvalidOperationException("Convention key Id was not detected.");
        if (customer.Navigations.Single().Name != "Orders" || !customer.Navigations.Single().IsCollection)
            throw new InvalidOperationException("Collection navigation was not detected.");

        var order = model.Entities.Single(entity => entity.Name == "Order");
        if (order.Keys.Single().Value != "OrderKey" || order.Keys.Single().Source != EfProvenance.DataAnnotation)
            throw new InvalidOperationException("[Key] annotation was not detected.");
        if (order.ForeignKeys.All(fk => fk.Value != "CustomerId"))
            throw new InvalidOperationException("Foreign key CustomerId was not detected.");
        if (order.Navigations.Single().TargetType != "Customer" || order.Navigations.Single().IsCollection)
            throw new InvalidOperationException("Reference navigation was not detected.");

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
            throw new InvalidOperationException("Dynamic model must set IsComplete=false.");
    }
}
