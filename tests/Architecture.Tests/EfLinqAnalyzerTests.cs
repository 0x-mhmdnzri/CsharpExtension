using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class EfLinqAnalyzerTests
{
    public static void Run()
    {
        var query = EfLinqAnalyzer.Analyze("db.Customers.Where(c => c.Id == 1).Select(c => c.Name)", "Customers");
        if (query.Status != EfTranslationStatus.Translatable || query.Sql != "SELECT c.Name FROM Customers WHERE c.Id = 1")
            throw new InvalidOperationException("LINQ query was not translated.");
        if (EfLinqAnalyzer.Analyze("db.Customers.Where(c => DateTime.Now > c.Created).Select(c => c.Id)", "Customers").Status != EfTranslationStatus.ClientEvaluationRisk)
            throw new InvalidOperationException("Client evaluation risk was not detected.");
    }
}
