using System.Text.Json;
using DotnetExpertAdvisor.Protocol;
using DotnetExpertAdvisor.Shared;

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
            else if (method == "textDocument/signatureHelp" && root.TryGetProperty("id", out var signatureId))
            {
                var result = new { signatures = Array.Empty<object>(), activeSignature = -1, activeParameter = 0 };
                await StdioTransport.WriteAsync(output, new { jsonrpc = "2.0", id = signatureId, result }, cancellationToken);
            }
            else if (method == "textDocument/hover" && root.TryGetProperty("id", out var hoverId))
            {
                await StdioTransport.WriteAsync(output, new { jsonrpc = "2.0", id = hoverId, result = (object?)null }, cancellationToken);
            }
            else if (method == "textDocument/definition" && root.TryGetProperty("id", out var definitionId))
            {
                await StdioTransport.WriteAsync(output, new { jsonrpc = "2.0", id = definitionId, result = Array.Empty<object>() }, cancellationToken);
            }
            else if (root.TryGetProperty("id", out var id))
            {
                await StdioTransport.WriteAsync(output, new { jsonrpc = "2.0", id, error = new { code = -32601, message = "Method not implemented" } }, cancellationToken);
            }
        }
    }
}
