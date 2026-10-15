using System.Collections.Immutable;

namespace DotnetExpertAdvisor.Shared;

public sealed record GenericInferenceResult(bool Success, MethodSymbol? Method, string? Reason);

public static class GenericInference
{
    public static GenericInferenceResult Infer(MethodSymbol method, IReadOnlyList<TypeSymbol> arguments)
    {
        ArgumentNullException.ThrowIfNull(method);
        ArgumentNullException.ThrowIfNull(arguments);
        if (method.TypeParameters.IsEmpty) return new(true, method, null);
        if (arguments.Count > method.Parameters.Length) return new(false, null, "argument count exceeds parameters");
        var bindings = new Dictionary<string, TypeSymbol>(StringComparer.Ordinal);
        for (var i = 0; i < arguments.Count; i++)
            if (!InferType(method.Parameters[i].Type, arguments[i], bindings))
                return new(false, null, $"argument {i + 1} does not provide compatible type bounds");
        var missing = method.TypeParameters.Where(parameter => !bindings.ContainsKey(parameter.Id)).ToArray();
        if (missing.Length > 0) return new(false, null, $"could not infer {string.Join(", ", missing.Select(parameter => parameter.Name))}");
        foreach (var parameter in method.TypeParameters)
            if (!SatisfiesConstraints(parameter, bindings[parameter.Id]))
                return new(false, null, $"inferred type for {parameter.Name} violates its constraints");
        var parameters = method.Parameters.Select(parameter => parameter with { Type = Substitute(parameter.Type, bindings) }).ToImmutableArray();
        return new(true, method with { Parameters = parameters, ReturnType = Substitute(method.ReturnType, bindings), TypeParameters = [] }, null);
    }

    private static bool InferType(TypeSymbol parameter, TypeSymbol argument, IDictionary<string, TypeSymbol> bindings)
    {
        if (parameter is TypeParameterSymbol typeParameter)
        {
            if (bindings.TryGetValue(typeParameter.Id, out var existing)) return existing.Id == argument.Id;
            bindings[typeParameter.Id] = argument;
            return true;
        }
        if (parameter is ConstructedTypeSymbol parameterType && argument is ConstructedTypeSymbol argumentType)
            return parameterType.Definition.Id == argumentType.Definition.Id
                && parameterType.TypeArguments.Length == argumentType.TypeArguments.Length
                && parameterType.TypeArguments.Zip(argumentType.TypeArguments).All(pair => InferType(pair.First, pair.Second, bindings));
        return parameter.Id == argument.Id;
    }

    private static TypeSymbol Substitute(TypeSymbol type, IReadOnlyDictionary<string, TypeSymbol> bindings)
        => type switch
        {
            TypeParameterSymbol parameter when bindings.TryGetValue(parameter.Id, out var value) => value,
            ConstructedTypeSymbol constructed => constructed with { TypeArguments = constructed.TypeArguments.Select(argument => Substitute(argument, bindings)).ToImmutableArray() },
            _ => type
        };

    private static bool SatisfiesConstraints(TypeParameterSymbol parameter, TypeSymbol value)
        => (!parameter.HasReferenceTypeConstraint || value is NamedTypeSymbol { TypeKind: TypeKind.Class or TypeKind.Interface })
           && (!parameter.HasValueTypeConstraint || value is NamedTypeSymbol { TypeKind: TypeKind.Struct })
           && parameter.Constraints.All(constraint => constraint.Id == value.Id || value is NamedTypeSymbol named && named.BaseType?.Id == constraint.Id);
}
