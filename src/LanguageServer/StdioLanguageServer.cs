using System.Text.Json;
using DotnetExpertAdvisor.Protocol;
using DotnetExpertAdvisor.Shared;
using DotnetExpertAdvisor.RoslynHost;
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
                try
                {
                    await StdioTransport.WriteAsync(output, new
                    {
                        jsonrpc = "2.0",
                        id = completionId,
                        result = await CompleteAsync(root, output, cancellationToken)
                    }, cancellationToken);
                }
                catch (Exception exception) when (exception is not OperationCanceledException)
                {
                    await StdioTransport.WriteAsync(output, new { jsonrpc = "2.0", id = completionId,
                        error = new { code = -32603, message = exception.ToString() } }, cancellationToken);
                }
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
                await StdioTransport.WriteAsync(output, new { jsonrpc = "2.0", id = hoverId,
                    result = await HoverAsync(root, cancellationToken) }, cancellationToken);
            }
            else if (method == "textDocument/definition" && root.TryGetProperty("id", out var definitionId))
            {
                await StdioTransport.WriteAsync(output, new { jsonrpc = "2.0", id = definitionId,
                    result = await DefinitionAsync(root, cancellationToken) }, cancellationToken);
            }
            else if (root.TryGetProperty("id", out var id))
            {
                await StdioTransport.WriteAsync(output, new { jsonrpc = "2.0", id, error = new { code = -32601, message = "Method not implemented" } }, cancellationToken);
            }
        }
    }

    private static async Task<object> CompleteAsync(JsonElement root, Stream output, CancellationToken cancellationToken)
    {
        if (!root.TryGetProperty("params", out var parameters)
            || !parameters.TryGetProperty("textDocument", out var document)
            || !document.TryGetProperty("text", out var textProperty)
            || !parameters.TryGetProperty("position", out var position)
            || !position.TryGetProperty("line", out var lineProperty)
            || !position.TryGetProperty("character", out var characterProperty))
            return new { items = Array.Empty<object>(), isIncomplete = false };

        var text = textProperty.GetString() ?? string.Empty;
        var progressToken = parameters.TryGetProperty("progressToken", out var token) ? token.GetString() : null;
        async Task ReportAsync(int percentage, string message)
        {
            if (progressToken is null) return;
            await StdioTransport.WriteAsync(output, new { jsonrpc = "2.0", method = "$/progress",
                @params = new { token = progressToken, value = new { kind = "report", percentage, message } } }, cancellationToken);
        }
        await ReportAsync(10, "Loading project context");
        var documentUri = document.TryGetProperty("uri", out var uri) ? uri.GetString() ?? "document" : "document";
        var version = document.TryGetProperty("version", out var documentVersion) ? documentVersion.GetInt32() : -1;
        var settings = parameters.TryGetProperty("projectContext", out var projectContext) ? projectContext : default;
        string? Setting(string name) => settings.ValueKind == JsonValueKind.Object && settings.TryGetProperty(name, out var value)
            && value.ValueKind == JsonValueKind.String ? value.GetString() : null;
        var context = await ProjectContextReader.ReadAsync(documentUri, version, Setting("projectPath"),
            Setting("targetFramework"), Setting("configuration") ?? "Debug", cancellationToken, text);
        await ReportAsync(45, context.IndexCacheHit ? "Loaded cached project index" : "Built and cached project index");
        var request = new DocumentRequest(new WorkspaceId("lsp"),
            new DocumentId(documentUri), new ProjectId(context.ProjectPath ?? "lsp"),
            new TargetFrameworkId(context.TargetFramework ?? "unknown"), version, text);
        var result = await new RoslynCompletionHost { Context = context }.GetCompletionAsync(request, lineProperty.GetInt32(),
            characterProperty.GetInt32(), cancellationToken);
        await ReportAsync(95, "Resolving Roslyn candidates");
        await ReportAsync(100, $"Complete ({result.Candidates.Length} suggestions)");
        return new
        {
            items = result.Candidates.Select((candidate, index) => new
            {
                label = candidate.Label,
                kind = candidate.LspKind,
                detail = candidate.Detail,
                sortText = index.ToString("D5"),
                preselect = candidate.Confidence >= 0.85,
                documentation = string.IsNullOrEmpty(candidate.Documentation) ? null : candidate.Documentation,
                textEdit = new
                {
                    range = new
                    {
                        start = new { line = lineProperty.GetInt32(), character = characterProperty.GetInt32() - candidate.EditLength },
                        end = new { line = lineProperty.GetInt32(), character = characterProperty.GetInt32() }
                    },
                    newText = candidate.Label
                },
                additionalTextEdits = candidate.RequiredImport is null ? null : new[]
                {
                    new
                    {
                        range = new { start = new { line = 0, character = 0 }, end = new { line = 0, character = 0 } },
                        newText = $"using {candidate.RequiredImport};{Environment.NewLine}"
                    }
                },
                data = new { suggestionId = $"roslyn:{candidate.SymbolId}", ruleId = "roslyn.semantic",
                    confidence = candidate.Confidence,
                    source = "roslyn://semantic-model", expectedType = result.ExpectedType,
                    intentScore = candidate.IntentScore, signals = candidate.Signals, assembly = candidate.AssemblyName,
                    projectContext = new
                    {
                        context.ProjectPath, context.TargetFramework, context.LanguageVersion,
                        context.Configuration, context.DocumentVersion,
                        packages = context.NuGetApi?.Packages.Select(package => new { package.PackageId, package.PackageVersion })
                    } }
            }).ToArray(),
            isIncomplete = false
        };
    }

    private static async Task<object?> HoverAsync(JsonElement root, CancellationToken cancellationToken)
    {
        var request = await ReadDocumentRequestAsync(root, cancellationToken);
        if (request is null) return null;
        var info = await request.Value.Host.GetSymbolInfoAsync(request.Value.Document, request.Value.Line, request.Value.Character, cancellationToken);
        if (info is null) return null;
        var package = info.PackageId is null ? "project/source" : $"package `{info.PackageId}`";
        var link = info.TargetPath is null ? string.Empty : $"\n\n[Open code behind]({FileUri(info.TargetPath)})";
        return new { contents = new object[] { new { language = "csharp", value = info.Detail },
            new { kind = "markdown", value = $"{package}\n\n{info.Documentation}{link}" } } };
    }

    private static async Task<object> DefinitionAsync(JsonElement root, CancellationToken cancellationToken)
    {
        var request = await ReadDocumentRequestAsync(root, cancellationToken);
        if (request is null) return Array.Empty<object>();
        var info = await request.Value.Host.GetSymbolInfoAsync(request.Value.Document, request.Value.Line, request.Value.Character, cancellationToken);
        if (info?.TargetPath is null) return Array.Empty<object>();
        return new[] { new { uri = FileUri(info.TargetPath),
            range = new { start = new { line = info.StartLine, character = info.StartCharacter },
                end = new { line = info.EndLine, character = info.EndCharacter } } } };
    }

    private static async Task<(RoslynCompletionHost Host, DocumentRequest Document, int Line, int Character)?> ReadDocumentRequestAsync(JsonElement root, CancellationToken cancellationToken)
    {
        var parameters = root.GetProperty("params");
        var document = parameters.GetProperty("textDocument");
        var text = document.GetProperty("text").GetString() ?? string.Empty;
        var uri = document.TryGetProperty("uri", out var uriProperty) ? uriProperty.GetString() ?? "document" : "document";
        var version = document.TryGetProperty("version", out var versionProperty) ? versionProperty.GetInt32() : -1;
        var position = parameters.GetProperty("position");
        var settings = parameters.TryGetProperty("projectContext", out var context) ? context : default;
        string? Setting(string name) => settings.ValueKind == JsonValueKind.Object && settings.TryGetProperty(name, out var value) && value.ValueKind == JsonValueKind.String ? value.GetString() : null;
        var project = await ProjectContextReader.ReadAsync(uri, version, Setting("projectPath"), Setting("targetFramework"), Setting("configuration") ?? "Debug", cancellationToken, text);
        return (new RoslynCompletionHost { Context = project }, new DocumentRequest(new WorkspaceId("lsp"), new DocumentId(uri),
            new ProjectId(project.ProjectPath ?? "lsp"), new TargetFrameworkId(project.TargetFramework ?? "unknown"), version, text),
            position.GetProperty("line").GetInt32(), position.GetProperty("character").GetInt32());
    }

    private static string FileUri(string path) => new UriBuilder("file", string.Empty) { Path = Path.GetFullPath(path) }.Uri.AbsoluteUri;
}
