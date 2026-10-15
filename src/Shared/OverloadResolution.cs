using System.Collections.Immutable;

namespace DotnetExpertAdvisor.Shared;

public sealed record RejectedCandidate(MethodSymbol Method, string Reason);
public sealed record OverloadResolutionResult(ImmutableArray<MethodSymbol> Applicable, ImmutableArray<RejectedCandidate> Rejected, MethodSymbol? Best, bool IsAmbiguous);

public static class OverloadResolver
{
    public static OverloadResolutionResult Resolve(IEnumerable<MethodSymbol> methods, IReadOnlyList<TypeSymbol> arguments, ConversionContext context)
    {
        ArgumentNullException.ThrowIfNull(methods);
        ArgumentNullException.ThrowIfNull(arguments);
        var classifier = new ConversionClassifier();
        var applicable = ImmutableArray.CreateBuilder<MethodSymbol>();
        var rejected = ImmutableArray.CreateBuilder<RejectedCandidate>();
        foreach (var method in methods)
        {
            if (method.Parameters.Any(parameter => parameter.IsParams))
            {
                rejected.Add(new(method, "params expansion is not yet supported"));
                continue;
            }
            var candidate = method;
            if (!method.TypeParameters.IsEmpty)
            {
                var inference = GenericInference.Infer(method, arguments);
                if (!inference.Success || inference.Method is null)
                {
                    rejected.Add(new(method, inference.Reason ?? "generic inference failed"));
                    continue;
                }
                candidate = inference.Method;
            }
            var required = candidate.Parameters.Count(parameter => !parameter.IsOptional);
            if (arguments.Count < required || arguments.Count > candidate.Parameters.Length)
            {
                rejected.Add(new(method, $"argument count {arguments.Count} does not match {candidate.Parameters.Length} parameters"));
                continue;
            }
            var failed = false;
            for (var i = 0; i < arguments.Count; i++)
            {
                var conversion = classifier.Classify(arguments[i], candidate.Parameters[i].Type, context);
                if (!conversion.Exists || !conversion.IsImplicit)
                {
                    rejected.Add(new(method, $"argument {i + 1}: {conversion.Reason ?? "no implicit conversion"}"));
                    failed = true;
                    break;
                }
            }
            if (!failed) applicable.Add(candidate);
        }
        if (applicable.Count == 0) return new([], rejected.ToImmutable(), null, false);
        var winners = applicable.Where(candidate => applicable.All(other =>
            ReferenceEquals(candidate, other) || Better(candidate, other, arguments, classifier, context))).ToArray();
        return new(applicable.ToImmutable(), rejected.ToImmutable(), winners.Length == 1 ? winners[0] : null, winners.Length != 1);
    }

    private static bool Better(MethodSymbol left, MethodSymbol right, IReadOnlyList<TypeSymbol> arguments,
        ConversionClassifier classifier, ConversionContext context)
    {
        var better = false;
        for (var i = 0; i < arguments.Count; i++)
        {
            var a = left.Parameters[i].Type;
            var b = right.Parameters[i].Type;
            if (a.Id == b.Id) continue;
            if (arguments[i].Id == a.Id) { better = true; continue; }
            if (arguments[i].Id == b.Id) return false;
            var ab = classifier.Classify(a, b, context);
            var ba = classifier.Classify(b, a, context);
            if (ab.Exists && ab.IsImplicit && !(ba.Exists && ba.IsImplicit)) better = true;
            else return false;
        }
        return better || (left.Parameters.Length == arguments.Count && right.Parameters.Length > arguments.Count);
    }
}
