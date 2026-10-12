using System.Collections.Immutable;
using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class ContractSmokeTests
{
    public static void ValidateCoreRecords()
    {
        var request = new DocumentRequest(
            new WorkspaceId("smoke"),
            new DocumentId("document:test"),
            new ProjectId("project:test"),
            new TargetFrameworkId("net10.0"),
            1,
            "class C {}");

        if (request.Version != 1 || request.TargetFramework.Value != "net10.0")
            throw new InvalidOperationException("Shared contract smoke test failed.");
    }

    public static void ValidateErrorTypePreservesCandidates()
    {
        var objectType = new NamedTypeSymbol(
            "T:System.Object",
            "Object",
            TypeKind.Class,
            0,
            null,
            Accessibility.Public,
            null,
            ImmutableArray<TypeSymbol>.Empty,
            ImmutableArray<Symbol>.Empty);
        var candidate = new PropertySymbol(
            "P:Order.Total",
            "Total",
            objectType,
            Accessibility.Public,
            objectType,
            true,
            false,
            false);

        var error = SymbolFactory.Error(
            "error:order.missing",
            "Missing",
            new[] { candidate },
            "member was not found");

        if (!error.IsError || error.CandidateSymbols.Length != 1 || error.CandidateSymbols[0].Id != candidate.Id)
            throw new InvalidOperationException("ErrorTypeSymbol did not preserve candidate symbols.");
    }

    public static void ValidateBinderStatuses()
    {
        var type = new NamedTypeSymbol(
            "T:Sample.Order",
            "Order",
            TypeKind.Class,
            0,
            null,
            Accessibility.Public,
            null,
            ImmutableArray<TypeSymbol>.Empty,
            ImmutableArray<Symbol>.Empty);
        var publicValue = new PropertySymbol("P:Order.Total", "Total", type, Accessibility.Public, type, true, false, false);
        var privateValue = new PropertySymbol("P:Order.Secret", "Secret", type, Accessibility.Private, type, true, false, false);
        var generic = new NamedTypeSymbol("T:Sample.Box`1", "Box", TypeKind.Class, 1, null, Accessibility.Public, null, ImmutableArray<TypeSymbol>.Empty, ImmutableArray<Symbol>.Empty);

        var root = Binder.CreateRoot().Declare(publicValue).Declare(privateValue).Declare(generic);
        var child = root.CreateChild(BinderScopeKind.Block).Declare(new PropertySymbol("P:Local.Total", "Total", type, Accessibility.Public, type, true, false, false));

        if (child.Lookup("Total").Status != LookupStatus.Ambiguous)
            throw new InvalidOperationException("Binder did not preserve ambiguous candidates across scopes.");
        if (root.Lookup("Secret", new LookupOptions(IsAccessible: symbol => symbol.Accessibility == Accessibility.Public)).Status != LookupStatus.Inaccessible)
            throw new InvalidOperationException("Binder did not report inaccessible symbols.");
        if (root.Lookup("Box", new LookupOptions(GenericArity: 0)).Status != LookupStatus.WrongArity)
            throw new InvalidOperationException("Binder did not report wrong generic arity.");
        if (root.Lookup("Missing").Status != LookupStatus.NotFound)
            throw new InvalidOperationException("Binder did not report missing symbols.");
    }

    public static void ValidateConversionClassifier()
    {
        var classifier = new ConversionClassifier();
        var intType = Primitive("int", TypeKind.Struct);
        var longType = Primitive("long", TypeKind.Struct);
        var stringType = Primitive("string", TypeKind.Class);
        var objectType = Primitive("object", TypeKind.Class);
        var nullableInt = new NullableTypeSymbol(intType);
        var context = new ConversionContext(13);

        AssertConversion(classifier.Classify(intType, intType, context), ConversionKind.Identity, true);
        AssertConversion(classifier.Classify(intType, longType, context), ConversionKind.ImplicitNumeric, true);
        AssertConversion(classifier.Classify(longType, intType, context), ConversionKind.ExplicitNumeric, false);
        AssertConversion(classifier.Classify(intType, nullableInt, context), ConversionKind.ImplicitNullable, true);
        AssertConversion(classifier.Classify(intType, objectType, context), ConversionKind.Boxing, true);
        AssertConversion(classifier.Classify(stringType, objectType, context), ConversionKind.ImplicitReference, true);
        AssertConversion(classifier.Classify(stringType, intType, context), ConversionKind.NoConversion, false, false);
        AssertConversion(classifier.Classify(Primitive("int", TypeKind.Struct), Primitive("uint", TypeKind.Struct), context), ConversionKind.ExplicitNumeric, false);
        AssertConversion(classifier.Classify(Primitive("char", TypeKind.Struct), Primitive("ushort", TypeKind.Struct), context), ConversionKind.ImplicitNumeric, true);
        AssertConversion(classifier.Classify(Primitive("ushort", TypeKind.Struct), Primitive("char", TypeKind.Struct), context), ConversionKind.ExplicitNumeric, false);
        AssertConversion(classifier.Classify(Primitive("decimal", TypeKind.Struct), Primitive("double", TypeKind.Struct), context), ConversionKind.ExplicitNumeric, false);
        AssertConversion(classifier.Classify(longType, nullableInt, context), ConversionKind.NoConversion, false, false);
        var unrelatedInterface = Primitive("Unrelated", TypeKind.Interface);
        AssertConversion(classifier.Classify(intType, unrelatedInterface, context), ConversionKind.NoConversion, false, false);
        if (classifier.CachedConversionCount < 7)
            throw new InvalidOperationException("Conversion results were not cached.");
    }

    private static NamedTypeSymbol Primitive(string name, TypeKind kind)
        => new($"T:System.{name}", name, kind, 0, null, Accessibility.Public, null, ImmutableArray<TypeSymbol>.Empty, ImmutableArray<Symbol>.Empty);

    private static void AssertConversion(ConversionResult result, ConversionKind expectedKind, bool expectedImplicit, bool expectedExists = true)
    {
        if (result.Kind != expectedKind || result.IsImplicit != expectedImplicit || result.Exists != expectedExists)
            throw new InvalidOperationException($"Expected {expectedKind}, got {result.Kind}.");
    }
}

