using DotnetExpertAdvisor.Shared;
namespace DotnetExpertAdvisor.RuntimeModel;
public interface IRuntimeModel { RuntimeEstimate Estimate(DocumentAnalysis analysis); }
public sealed record RuntimeEstimate(IReadOnlyList<string> Signals, double Confidence);
public sealed class RuntimeModel : IRuntimeModel { public RuntimeEstimate Estimate(DocumentAnalysis analysis) => new(Array.Empty<string>(), 0d); }
