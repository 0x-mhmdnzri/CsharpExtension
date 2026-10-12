using System.Collections.Immutable;

namespace DotnetExpertAdvisor.Shared;

public enum BinderScopeKind
{
    Global,
    Imports,
    Namespace,
    Type,
    Member,
    Block,
    Lambda,
    Query,
    Pattern
}

public enum LookupStatus
{
    Viable,
    Ambiguous,
    Inaccessible,
    WrongArity,
    NotAValue,
    NotFound
}

public sealed record LookupOptions(
    int? GenericArity = null,
    Func<Symbol, bool>? IsAccessible = null,
    Func<Symbol, bool>? IsValue = null);

public sealed record LookupResult(
    string Name,
    LookupStatus Status,
    ImmutableArray<Symbol> Matches,
    ImmutableArray<Symbol> Candidates,
    string? Reason)
{
    public bool IsSuccess => Status == LookupStatus.Viable;
    public bool HasCandidates => Candidates.Length > 0;
}

public sealed class Binder
{
    private readonly ImmutableDictionary<string, ImmutableArray<Symbol>> _declarations;

    private Binder(
        BinderScopeKind kind,
        Binder? parent,
        ImmutableDictionary<string, ImmutableArray<Symbol>> declarations)
    {
        Kind = kind;
        Parent = parent;
        _declarations = declarations;
    }

    public BinderScopeKind Kind { get; }
    public Binder? Parent { get; }

    public static Binder CreateRoot(BinderScopeKind kind = BinderScopeKind.Global)
        => new(kind, null, ImmutableDictionary<string, ImmutableArray<Symbol>>.Empty);

    public Binder CreateChild(BinderScopeKind kind)
        => new(kind, this, ImmutableDictionary<string, ImmutableArray<Symbol>>.Empty);

    public Binder Declare(Symbol symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol.Name))
            throw new ArgumentException("A symbol must have a name.", nameof(symbol));

        var existing = _declarations.TryGetValue(symbol.Name, out var values)
            ? values
            : ImmutableArray<Symbol>.Empty;

        return new Binder(
            Kind,
            Parent,
            _declarations.SetItem(symbol.Name, existing.Add(symbol)));
    }

    public LookupResult Lookup(string name, LookupOptions? options = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return new LookupResult(name, LookupStatus.NotFound, ImmutableArray<Symbol>.Empty, ImmutableArray<Symbol>.Empty, "name is empty");

        options ??= new LookupOptions();
        var matches = CollectMatches(name);
        if (matches.Length == 0)
            return new LookupResult(name, LookupStatus.NotFound, matches, ImmutableArray<Symbol>.Empty, "name was not found in the binder chain");

        var arityMatches = options.GenericArity is null
            ? matches
            : matches.Where(symbol => GetGenericArity(symbol) == options.GenericArity.Value).ToImmutableArray();
        if (arityMatches.Length == 0)
            return new LookupResult(name, LookupStatus.WrongArity, matches, ImmutableArray<Symbol>.Empty, "matching symbols have a different generic arity");

        var accessible = options.IsAccessible is null
            ? arityMatches
            : arityMatches.Where(options.IsAccessible).ToImmutableArray();
        if (accessible.Length == 0)
            return new LookupResult(name, LookupStatus.Inaccessible, arityMatches, ImmutableArray<Symbol>.Empty, "all matching symbols are inaccessible");

        var values = options.IsValue is null
            ? accessible
            : accessible.Where(options.IsValue).ToImmutableArray();
        if (values.Length == 0)
            return new LookupResult(name, LookupStatus.NotAValue, accessible, ImmutableArray<Symbol>.Empty, "matching symbols are not values in this context");

        return values.Length == 1
            ? new LookupResult(name, LookupStatus.Viable, accessible, values, null)
            : new LookupResult(name, LookupStatus.Ambiguous, accessible, values, "multiple accessible symbols match");
    }

    private ImmutableArray<Symbol> CollectMatches(string name)
    {
        var result = ImmutableArray.CreateBuilder<Symbol>();
        var binder = this;
        while (binder is not null)
        {
            if (binder._declarations.TryGetValue(name, out var declarations))
            {
                foreach (var declaration in declarations)
                {
                    if (result.All(existing => existing.Id != declaration.Id))
                        result.Add(declaration);
                }
            }

            binder = binder.Parent;
        }

        return result.ToImmutable();
    }

    private static int GetGenericArity(Symbol symbol)
        => symbol switch
        {
            NamedTypeSymbol namedType => namedType.Arity,
            MethodSymbol method => method.TypeParameters.Length,
            _ => 0
        };
}
