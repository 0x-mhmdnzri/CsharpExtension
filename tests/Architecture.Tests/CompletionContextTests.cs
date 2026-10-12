using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class CompletionContextTests
{
    public static void Run()
    {
        var context = CompletionContextAnalyzer.Analyze("customer.na", 11);
        if (context.Prefix != "na" || context.Start != 9 || context.End != 11 ||
            context.TriggerCharacter != '.' || !context.IsMemberAccess)
            throw new InvalidOperationException("Member completion context was parsed incorrectly.");
        var root = CompletionContextAnalyzer.Analyze("Con", 3);
        if (root.Prefix != "Con" || root.IsMemberAccess || root.Start != 0)
            throw new InvalidOperationException("Root completion context was parsed incorrectly.");
    }
}
