using System.Text.Json;
using DotnetExpertAdvisor.Protocol;
using DotnetExpertAdvisor.Shared;
using RoslynCompletionHost = DotnetExpertAdvisor.RoslynHost.RoslynHost;

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
                    result = new { capabilities = new { textDocumentSync = 1, completionProvider = new { triggerCharacters = new[] { "." }, resolveProvider = true } } }
                }, cancellationToken);
            }
            else if (method == "textDocument/completion" && root.TryGetProperty("id", out var completionId))
            {
                await StdioTransport.WriteAsync(output, new
                {
                    jsonrpc = "2.0",
                    id = completionId,
                    result = await CompleteAsync(root, cancellationToken)
                }, cancellationToken);
            }
            else if (method == "completionItem/resolve" && root.TryGetProperty("id", out var resolveId))
            {
                await StdioTransport.WriteAsync(output, new { jsonrpc = "2.0", id = resolveId, result = root.GetProperty("params") }, cancellationToken);
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

    private static async Task<object> CompleteAsync(JsonElement root, CancellationToken cancellationToken)
    {
        if (!root.TryGetProperty("params", out var parameters)
            || !parameters.TryGetProperty("textDocument", out var document)
            || !document.TryGetProperty("text", out var textProperty)
            || !parameters.TryGetProperty("position", out var position)
            || !position.TryGetProperty("line", out var lineProperty)
            || !position.TryGetProperty("character", out var characterProperty))
            return new { items = Array.Empty<object>(), isIncomplete = false };

        var text = textProperty.GetString() ?? string.Empty;
        var request = new DocumentRequest(new WorkspaceId("lsp"),
            new DocumentId(document.TryGetProperty("uri", out var uri) ? uri.GetString() ?? "document" : "document"),
            new ProjectId("lsp"), new TargetFrameworkId("unknown"), 0, text);
        var result = await new RoslynCompletionHost().GetCompletionAsync(request, lineProperty.GetInt32(),
            characterProperty.GetInt32(), cancellationToken);
        return new
        {
            items = result.Candidates.Select((candidate, index) => new
            {
                label = candidate.Label,
                kind = 3,
                detail = candidate.Detail,
                sortText = index.ToString("D5"),
                data = new { suggestionId = $"roslyn:{candidate.SymbolId}", ruleId = "roslyn.semantic", confidence = 1.0, source = "roslyn://semantic-model" }
            }).ToArray(),
            isIncomplete = false
        };
    }
}
