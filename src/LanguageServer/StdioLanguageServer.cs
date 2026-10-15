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
                    result = new { capabilities = new { textDocumentSync = 1, completionProvider = new { triggerCharacters = new[] { "." }, resolveProvider = true } } }
                }, cancellationToken);
            }
            else if (method == "textDocument/completion" && root.TryGetProperty("id", out var completionId))
            {
                await StdioTransport.WriteAsync(output, new
                {
                    jsonrpc = "2.0",
                    id = completionId,
                    result = Complete(root)
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

    private static object Complete(JsonElement root)
    {
        if (!root.TryGetProperty("params", out var parameters)
            || !parameters.TryGetProperty("textDocument", out var document)
            || !document.TryGetProperty("text", out var textProperty)
            || !parameters.TryGetProperty("position", out var position)
            || !position.TryGetProperty("line", out var lineProperty)
            || !position.TryGetProperty("character", out var characterProperty))
            return new { items = Array.Empty<object>(), isIncomplete = false };

        var text = textProperty.GetString() ?? string.Empty;
        var suggestions = CompletionEngine.Complete(text, lineProperty.GetInt32(), characterProperty.GetInt32());
        return new
        {
            items = suggestions.Select(suggestion => new
            {
                label = suggestion.Label,
                kind = 3,
                detail = suggestion.Detail,
                sortText = suggestion.SortText,
                data = new { suggestionId = suggestion.SuggestionId, ruleId = suggestion.RuleId, confidence = suggestion.Confidence, source = suggestion.Source }
            }).ToArray(),
            isIncomplete = false
        };
    }
}
