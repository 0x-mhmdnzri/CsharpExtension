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
        var tupleSource = new TupleTypeSymbol(ImmutableArray.Create<TypeSymbol>(intType, stringType));
        var tupleTarget = new TupleTypeSymbol(ImmutableArray.Create<TypeSymbol>(longType, objectType));
        AssertConversion(classifier.Classify(tupleSource, tupleTarget, context), ConversionKind.ImplicitTuple, true);
        var invoke = new MethodSymbol(
            "M:Func.Invoke",
            "Invoke",
            null,
            Accessibility.Public,
            stringType,
            ImmutableArray.Create(new ParameterSymbol("P:Func.Invoke.value", "value", null, intType, false, false)),
            ImmutableArray<TypeParameterSymbol>.Empty,
            false,
            false,
            false);
        var delegateType = new NamedTypeSymbol("T:Func", "Func", TypeKind.Delegate, 0, null, Accessibility.Public, null, ImmutableArray<TypeSymbol>.Empty, ImmutableArray.Create<Symbol>(invoke));
        var lambda = new LambdaTypeSymbol(ImmutableArray.Create<TypeSymbol>(intType), stringType);
        var methodGroup = new MethodGroupTypeSymbol(ImmutableArray.Create(new MethodSymbol("M:Factory.Create", "Create", null, Accessibility.Public, stringType, ImmutableArray.Create(new ParameterSymbol("P:Factory.Create.value", "value", null, intType, false, false)), ImmutableArray<TypeParameterSymbol>.Empty, true, false, false)));
        AssertConversion(classifier.Classify(lambda, delegateType, context), ConversionKind.AnonymousFunction, true);
        AssertConversion(classifier.Classify(methodGroup, delegateType, context), ConversionKind.MethodGroup, true);
        if (classifier.CachedConversionCount < 7)
            throw new InvalidOperationException("Conversion results were not cached.");
    }

    public static void ValidateMemberLookup()
    {
        var objectType = Primitive("object", TypeKind.Class);
        var intType = Primitive("int", TypeKind.Struct);
        var baseType = new NamedTypeSymbol("T:Base", "Base", TypeKind.Class, 0, null, Accessibility.Public, objectType, ImmutableArray<TypeSymbol>.Empty, ImmutableArray<Symbol>.Empty);
        var baseRun = Method("M:Base.Run", "Run", baseType, intType);
        var baseOther = Method("M:Base.Run.string", "Run", baseType, Primitive("string", TypeKind.Class));
        baseType = baseType with { Members = ImmutableArray.Create<Symbol>(baseRun, baseOther) };
        var derivedType = new NamedTypeSymbol("T:Derived", "Derived", TypeKind.Class, 0, null, Accessibility.Public, baseType, ImmutableArray<TypeSymbol>.Empty, ImmutableArray<Symbol>.Empty);
        var derivedRun = Method("M:Derived.Run", "Run", derivedType, intType);
        derivedType = derivedType with { Members = ImmutableArray.Create<Symbol>(derivedRun) };

        var lookup = new MemberLookup(new ConversionClassifier());
        var run = lookup.Lookup(derivedType, "Run");
        if (run.Status != LookupStatus.Ambiguous || run.Candidates.Length != 2 || run.HiddenMembers.Length != 1)
            throw new InvalidOperationException("Member lookup did not preserve overloads and hidden members.");

        var inaccessible = lookup.Lookup(derivedType, "Run", new MemberLookupOptions(IsAccessible: symbol => symbol.Accessibility == Accessibility.Private));
        if (inaccessible.Status != LookupStatus.Inaccessible)
            throw new InvalidOperationException("Member lookup did not report inaccessible members.");

        var extension = Method("M:Extensions.Run", "Run", null, intType) with { IsExtension = true };
        var extensionResult = lookup.LookupExtensions(intType, "Run", new[] { extension }, new ConversionContext(13));
        if (extensionResult.Length != 1)
            throw new InvalidOperationException("Extension lookup rejected an identity receiver unexpectedly.");
    }

    public static void ValidateLanguageCapabilities()
    {
        var csharp8 = LanguageCapabilities.For(LanguageVersion.CSharp8);
        if (!csharp8.Supports(LanguageFeature.NullableReferenceTypes) || csharp8.Supports(LanguageFeature.Records)) throw new InvalidOperationException("C# 8 capability gates are incorrect.");
        if (!LanguageCapabilities.For(LanguageVersion.CSharp14).Supports(LanguageFeature.ExtensionMembers)) throw new InvalidOperationException("C# 14 capability gates are incorrect.");
        if (LanguageCapabilities.For(LanguageVersion.CSharp15).Supports(LanguageFeature.PreviewCSharp15)) throw new InvalidOperationException("C# 15 preview was enabled without the flag.");
        if (!LanguageCapabilities.For(LanguageVersion.CSharp15, true).Supports(LanguageFeature.PreviewCSharp15)) throw new InvalidOperationException("C# 15 preview was not enabled with the flag.");
    }

    public static void ValidateLexer()
    {
        var source = "#nullable enable\nclass C { string Name = \"A\\\"B\"; // note\n }";
        var lexer = new CSharpLexer();
        var result = lexer.Tokenize(source, LanguageCapabilities.For(LanguageVersion.CSharp10));
        if (result.Reconstruct() != source || result.Diagnostics.Length != 0 || !result.Tokens.Any(token => token.Kind == TokenKind.Keyword && token.Text == "class"))
            throw new InvalidOperationException("Lexer did not preserve source or classify tokens correctly.");
        var changed = lexer.TokenizeIncremental(source, result, source.Replace("Name", "Title", StringComparison.Ordinal), new TextChangeRange(29, 4, 5));
        if (changed.ReusedPrefixCount == 0 || changed.ReusedSuffixCount == 0)
            throw new InvalidOperationException("Incremental lexer did not reuse unchanged token ranges.");
        var broken = lexer.Tokenize("class C { string Name = \"unterminated;", LanguageCapabilities.For(LanguageVersion.CSharp10));
        if (broken.Diagnostics.Length == 0 || !broken.Tokens.Any(token => token.Kind == TokenKind.String))
            throw new InvalidOperationException("Lexer did not recover from an unterminated string.");
    }

    public static void ValidateParser()
    {
        var source = "namespace Demo { public class C { } }";
        var lexer = new CSharpLexer();
        var parsed = new CSharpParser().Parse(lexer.Tokenize(source), LanguageCapabilities.For(LanguageVersion.CSharp10));
        if (parsed.Reconstruct() != source || parsed.Diagnostics.Length != 0 || !parsed.Root.Children.Any(child => child.Kind == SyntaxKind.NamespaceDeclaration))
            throw new InvalidOperationException("Parser did not preserve the source or create a namespace node.");
        var broken = new CSharpParser().Parse(lexer.Tokenize("class C {"), LanguageCapabilities.For(LanguageVersion.CSharp10));
        if (broken.Diagnostics.Length == 0)
            throw new InvalidOperationException("Parser did not recover from an unclosed declaration.");
        var old = new CSharpParser().Parse(lexer.Tokenize("record C {}"), LanguageCapabilities.For(LanguageVersion.CSharp8));
        if (!old.Diagnostics.Any(diagnostic => diagnostic.Message.Contains("Records", StringComparison.Ordinal)))
            throw new InvalidOperationException("Parser did not enforce the language version gate.");
    }

    private static MethodSymbol Method(string id, string name, Symbol? containing, TypeSymbol parameterType)
        => new(id, name, containing, Accessibility.Public, parameterType, ImmutableArray.Create(new ParameterSymbol($"{id}.parameter", "value", null, parameterType, false, false)), ImmutableArray<TypeParameterSymbol>.Empty, false, false, false);

    private static NamedTypeSymbol Primitive(string name, TypeKind kind)
        => new($"T:System.{name}", name, kind, 0, null, Accessibility.Public, null, ImmutableArray<TypeSymbol>.Empty, ImmutableArray<Symbol>.Empty);

    private static void AssertConversion(ConversionResult result, ConversionKind expectedKind, bool expectedImplicit, bool expectedExists = true)
    {
        if (result.Kind != expectedKind || result.IsImplicit != expectedImplicit || result.Exists != expectedExists)
            throw new InvalidOperationException($"Expected {expectedKind}, got {result.Kind}.");
    }
}

