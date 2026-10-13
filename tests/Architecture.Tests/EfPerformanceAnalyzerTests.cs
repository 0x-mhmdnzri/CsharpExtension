using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class EfPerformanceAnalyzerTests
{
    public static void Run()
    {
        var findings = EfPerformanceAnalyzer.AnalyzeQuery("db.Customers.ToList().Where(c => c.Id > 1).AsTracking()");
        if (!findings.Any(finding => finding.RuleId == "EF001") || !findings.Any(finding => finding.RuleId == "EF002"))
            throw new InvalidOperationException("EF query performance findings were not produced.");
        if (!EfPerformanceAnalyzer.IsMigrationClass("class Init : Migration { protected override void Up(MigrationBuilder b){} protected override void Down(MigrationBuilder b){} }"))
            throw new InvalidOperationException("EF migration was not detected.");
    }
}
