using System.Security.Cryptography;
using System.Text.Json;
using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class KnowledgeUpdateTests
{
    public static async Task RunAsync()
    {
        var root = Path.Combine(Path.GetTempPath(), "knowledge-update-" + Guid.NewGuid().ToString("N"));
        var path = Path.Combine(root, "state.json");
        var key = RandomNumberGenerator.GetBytes(32);
        var client = new KnowledgeUpdateClient(path, new Version(1, 0, 0), key);
        var payload = JsonSerializer.Serialize(new KnowledgePackContent(1, "1.0.0", [], [], [], []));
        var first = KnowledgePackSecurity.Create("1.0.0", payload, key);
        var second = KnowledgePackSecurity.Create("1.1.0", payload, key);
        try
        {
            client.Install(first);
            var original = File.ReadAllText(path);
            Reject(() => client.Install(first with { Version = "9.0.0" }));
            Reject(() => client.Install(second with { Payload = "tampered" }));
            Reject(() => client.Install(KnowledgePackSecurity.Create("2.0.0",
                JsonSerializer.Serialize(new KnowledgePackContent(2, "1.0.0", [], [], [], [])), key)));
            Reject(() => client.Install(KnowledgePackSecurity.Create("2.0.0",
                JsonSerializer.Serialize(new KnowledgePackContent(1, "2.0.0", [], [], [], [])), key)));
            if (File.ReadAllText(path) != original) throw new InvalidOperationException("Rejected update changed current state.");
            using var http = new HttpClient(new PackHandler(second));
            await client.UpdateAsync(http, new Uri("https://example.test/pack.json"));
            var restarted = new KnowledgeUpdateClient(path, new Version(1, 0, 0), key);
            if (restarted.ReadState().Current != second || restarted.ReadState().Previous != first)
                throw new InvalidOperationException("Update or restart lost persisted pack state.");
            Reject(() => restarted.Install(first));
            restarted.Rollback();
            if (restarted.ReadState().Current != first || restarted.ReadState().Previous is not null)
                throw new InvalidOperationException("Rollback did not restore the previous pack.");
        }
        finally
        {
            if (Directory.Exists(root)) Directory.Delete(root, true);
        }
    }

    private static void Reject(Action action)
    {
        try { action(); }
        catch (InvalidDataException) { return; }
        throw new InvalidOperationException("Invalid update was accepted.");
    }

    private sealed class PackHandler(KnowledgePackEnvelope pack) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                { Content = new StringContent(JsonSerializer.Serialize(pack)) });
    }
}
