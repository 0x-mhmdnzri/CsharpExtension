using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class EfQueryCompletionTests
{
    public static void Run()
    {
        var model = EfModelAnalyzer.Analyze("""
            public class AppDbContext : DbContext
            {
                public DbSet<Order> Orders { get; set; }
            }
            public class Order { public Customer Customer { get; set; } public ICollection<Line> Lines { get; set; } }
            public class Customer { public ICollection<Order> Orders { get; set; } }
            public class Line { public Order Order { get; set; } }
            """);

        var include = EfQueryCompletion.Complete("db.Orders.Include(o => o.", model);
        if (!include.IsSupported || !include.Candidates.SequenceEqual(["Customer", "Lines"]))
            throw new InvalidOperationException("Include completion did not use model navigations.");

        var thenInclude = EfQueryCompletion.Complete("db.Orders.Include(o => o.Customer).ThenInclude(c => c.", model);
        if (!thenInclude.IsSupported || !thenInclude.Candidates.SequenceEqual(["Orders"]))
            throw new InvalidOperationException("ThenInclude completion did not follow the navigation target.");

        if (EfQueryCompletion.Complete("repository.Orders.Include(o => o.", model).IsSupported)
            throw new InvalidOperationException("Unknown IQueryable roots must not receive certain completion candidates.");
    }
}
