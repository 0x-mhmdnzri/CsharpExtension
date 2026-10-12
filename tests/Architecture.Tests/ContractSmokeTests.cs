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
}

