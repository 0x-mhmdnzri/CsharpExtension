using DotnetExpertAdvisor.Shared;
namespace DotnetExpertAdvisor.Protocol;
public sealed record InitializeResponse(string ServerName, string ServerVersion, string KnowledgePackVersion);
public sealed record CompletionRequest(DocumentRequest Document, int Line, int Character);
public sealed record CompletionResponse(IReadOnlyList<CompletionItem> Items, bool IsIncomplete);
public sealed record CompletionItem(string Label, string Kind, string Detail, string SuggestionId, string SortText);
public sealed record ExplainSuggestionRequest(string SuggestionId, DocumentId DocumentId);
