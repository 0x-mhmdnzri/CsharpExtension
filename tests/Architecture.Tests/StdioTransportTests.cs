using System.Text.Json;
using DotnetExpertAdvisor.Protocol;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class StdioTransportTests
{
    public static async Task RunAsync()
    {
        await using var stream = new MemoryStream();
        await StdioTransport.WriteAsync(stream, new { jsonrpc = "2.0", id = 7, method = "initialize" });
        stream.Position = 0;
        using var message = await StdioTransport.ReadAsync(stream);
        for (var index = 0; index < 64; index++) _ = new byte[4096];
        if (message is null || message.RootElement.GetProperty("id").GetInt32() != 7 ||
            message.RootElement.GetProperty("method").GetString() != "initialize")
            throw new InvalidOperationException("stdio JSON-RPC framing round-trip failed.");
    }
}
