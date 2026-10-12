using System.Text.Json;
using DotnetExpertAdvisor.Protocol;

namespace DotnetExpertAdvisor.LanguageServer;

public static class StdioLanguageServer
{
    public static async Task RunAsync(Stream input, Stream output, CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using var message = await StdioTransport.ReadAsync(input, cancellationToken);
            if (message is null) return;
            var root = message.RootElement;
            var method = root.TryGetProperty("method", out var methodProperty) ? methodProperty.GetString() : null;
            if (method == "exit") return;
            if (method == "shutdown")
            {
                if (root.TryGetProperty("id", out var shutdownId))
                    await StdioTransport.WriteAsync(output, new { jsonrpc = "2.0", id = shutdownId, result = (object?)null }, cancellationToken);
                continue;
            }
            if (method == "initialize" && root.TryGetProperty("id", out var initializeId))
            {
                await StdioTransport.WriteAsync(output, new
                {
                    jsonrpc = "2.0",
                    id = initializeId,
                    result = new { capabilities = new { textDocumentSync = 1, completionProvider = new { triggerCharacters = new[] { "." } } } }
                }, cancellationToken);
            }
            else if (root.TryGetProperty("id", out var id))
            {
                await StdioTransport.WriteAsync(output, new { jsonrpc = "2.0", id, error = new { code = -32601, message = "Method not implemented" } }, cancellationToken);
            }
        }
    }
}
