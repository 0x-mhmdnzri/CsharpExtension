using System.Collections.Immutable;
using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class GenericInferenceTests
{
    public static void Run()
    {
        var typeParameter = new TypeParameterSymbol("T", "T", null, 0, [], false, false, false);
        var owner = Type("Enumerable", TypeKind.Class);
        var method = new MethodSymbol("M:Select", "Select", owner, Accessibility.Public, typeParameter,
            ImmutableArray.Create(new ParameterSymbol("M:Select.source", "source", null, typeParameter, false, false)),
            ImmutableArray.Create(typeParameter), false, false, true);
        var value = Type("string", TypeKind.Class);
        var result = GenericInference.Infer(method, [value]);
        if (!result.Success || result.Method?.Parameters[0].Type.Id != value.Id || !result.Method.TypeParameters.IsEmpty)
            throw new InvalidOperationException("Generic inference did not substitute inferred type arguments.");
    }

    private static NamedTypeSymbol Type(string name, TypeKind kind)
        => new($"T:{name}", name, kind, 0, null, Accessibility.Public, null, [], []);
}
