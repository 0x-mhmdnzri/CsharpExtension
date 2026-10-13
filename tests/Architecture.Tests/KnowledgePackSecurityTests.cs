using System.Text;
using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class KnowledgePackSecurityTests
{
    public static void Run()
    {
        var key = Encoding.UTF8.GetBytes("local-test-key");
        var pack = KnowledgePackSecurity.Create("1.0.0", "{\"claims\":[]}", key);
        if (!KnowledgePackSecurity.Verify(pack, key) || KnowledgePackSecurity.Verify(pack with { Payload = "tampered" }, key))
            throw new InvalidOperationException("Knowledge pack hash/signature validation failed.");
    }
}
