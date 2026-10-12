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
}

