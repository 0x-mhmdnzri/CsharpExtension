using DotnetExpertAdvisor.Shared;
namespace DotnetExpertAdvisor.KnowledgeEngine;
public sealed record KnowledgeSnapshot(string PackId, string Version);
public interface IKnowledgeEngine { KnowledgeSnapshot GetSnapshot(); KnowledgeReference? Resolve(string referenceId); }
public sealed class KnowledgeEngine : IKnowledgeEngine { private readonly KnowledgeSnapshot _snapshot = new("empty", "0.0.0"); public KnowledgeSnapshot GetSnapshot() => _snapshot; public KnowledgeReference? Resolve(string referenceId) => null; }
