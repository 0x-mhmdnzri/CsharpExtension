using System.Collections.Immutable;

namespace DotnetExpertAdvisor.Shared;

public sealed record RejectedCandidate(MethodSymbol Method, string Reason);
public sealed record OverloadResolutionResult(
    ImmutableArray<MethodSymbol> Applicable,
    ImmutableArray<RejectedCandidate> Rejected,
    MethodSymbol? Best,
    bool IsAmbiguous);

public static class OverloadResolver
{
    public static OverloadResolutionResult Resolve(
        IEnumerable<MethodSymbol> methods,
        IReadOnlyList<TypeSymbol> arguments,
        ConversionContext context)
    {
        ArgumentNullException.ThrowIfNull(methods);
        ArgumentNullException.ThrowIfNull(arguments);
        var classifier = new ConversionClassifier();
        var applicable = ImmutableArray.CreateBuilder<MethodSymbol>();
        var rejected = ImmutableArray.CreateBuilder<RejectedCandidate>();

        foreach (var method in methods)
        {
            // shortcut: only positional, non-generic normal-form calls; extend after inference and array symbols exist.
            if (!method.TypeParameters.IsEmpty || method.Parameters.Any(parameter => parameter.IsParams))
            {
                rejected.Add(new(method, "unsupported generic or params candidate; no definite winner"));
                continue;
            }
            var required = method.Parameters.Count(parameter => !parameter.IsOptional && !parameter.IsParams);
            if (arguments.Count < required || (!method.Parameters.Any(parameter => parameter.IsParams)
                                                && arguments.Count > method.Parameters.Length))
            {
                rejected.Add(new(method, $"argument count {arguments.Count} does not match {method.Parameters.Length} parameters"));
                continue;
            }

            var failed = false;
            for (var index = 0; index < arguments.Count; index++)
            {
                var parameter = index < method.Parameters.Length
                    ? method.Parameters[index]
                    : method.Parameters[^1];
                var conversion = classifier.Classify(arguments[index], parameter.Type, context);
                if (!conversion.Exists || !conversion.IsImplicit)
                {
                    rejected.Add(new(method, $"argument {index + 1}: {conversion.Reason ?? "no implicit conversion"}"));
                    failed = true;
                    break;
                }
            }
            if (!failed)
                applicable.Add(method);
        }

        if (applicable.Count == 0)
            return new([], rejected.ToImmutable(), null, false);

        var winners = applicable.Where(candidate => applicable.All(other =>
            ReferenceEquals(candidate, other) || Better(candidate, other, arguments, classifier, context))).ToArray();
        return new(applicable.ToImmutable(), rejected.ToImmutable(),
            winners.Length == 1 && !rejected.Any(item => item.Reason.StartsWith("unsupported", StringComparison.Ordinal)) ? winners[0] : null, applicable.Count > 1 && winners.Length != 1);
    }

    private static bool Better(MethodSymbol left, MethodSymbol right, IReadOnlyList<TypeSymbol> arguments,
        ConversionClassifier classifier, ConversionContext context)
    {
        var better = false;
        for (var index = 0; index < arguments.Count; index++)
        {
            var a = left.Parameters[index].Type;
            var b = right.Parameters[index].Type;
            if (a.Id == b.Id) continue;
            if (arguments[index].Id == a.Id) { better = true; continue; }
            if (arguments[index].Id == b.Id) return false;
            var ab = classifier.Classify(a, b, context);
            var ba = classifier.Classify(b, a, context);
            if (ab.Exists && ab.IsImplicit && !(ba.Exists && ba.IsImplicit)) better = true;
            else return false;
        }
        if (better) return true;
        // With identical supplied parameter types, prefer a call needing no omitted defaults.
        return left.Parameters.Length == arguments.Count && right.Parameters.Length > arguments.Count;
    }
}
