using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class KnowledgeCrawlerTests
{
    public static void Run()
    {
        var links = KnowledgeCrawler.ParseLinks("<urlset><url><loc>/dotnet/csharp</loc></url><url><loc>/dotnet/csharp#old</loc></url></urlset>", new Uri("https://devblogs.microsoft.com/"));
        if (links.Length != 1 || links[0].CanonicalUrl != "https://devblogs.microsoft.com/dotnet/csharp")
            throw new InvalidOperationException("Crawler did not normalize canonical links.");
    }
}
