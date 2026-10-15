using System.Collections.Immutable;
using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class OverloadResolutionTests
{
    public static void Run()
    {
        var intType = Type("int", TypeKind.Struct);
        var stringType = Type("string", TypeKind.Class);
        var owner = Type("Calculator", TypeKind.Class);
        var intOverload = Method("M:int", owner, intType, intType);
        var stringOverload = Method("M:string", owner, intType, stringType);
        var result = OverloadResolver.Resolve([intOverload, stringOverload], [intType], new ConversionContext(13));
        if (result.Best?.Id != intOverload.Id || result.IsAmbiguous || result.Rejected.Length != 1)
            throw new InvalidOperationException("Overload resolver did not select the best member and preserve rejection reasons.");
    }

    private static MethodSymbol Method(string id, NamedTypeSymbol owner, TypeSymbol returnType, TypeSymbol parameterType)
        => new(id, "Calculate", owner, Accessibility.Public, returnType,
            ImmutableArray.Create(new ParameterSymbol($"{id}.p", "value", null, parameterType, false, false)),
            ImmutableArray<TypeParameterSymbol>.Empty, false, false, false);

    private static NamedTypeSymbol Type(string name, TypeKind kind)
        => new($"T:{name}", name, kind, 0, null, Accessibility.Public, null, [], []);
}
