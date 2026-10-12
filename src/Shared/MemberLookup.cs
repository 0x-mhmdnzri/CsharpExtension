using System.Collections.Immutable;

namespace DotnetExpertAdvisor.Shared;

public sealed record MemberLookupOptions(
    Func<Symbol, bool>? IsAccessible = null,
    int? GenericArity = null,
    bool IncludeInherited = true);

public sealed record MemberLookupResult(
    string Name,
    LookupStatus Status,
    ImmutableArray<Symbol> Matches,
    ImmutableArray<Symbol> Candidates,
    ImmutableArray<Symbol> HiddenMembers,
    string? Reason)
{
    public bool IsSuccess => Status == LookupStatus.Viable;
}

public sealed class MemberLookup
{
    private readonly IConversionClassifier _conversions;

    public MemberLookup(IConversionClassifier? conversions = null)
        => _conversions = conversions ?? new ConversionClassifier();

    public MemberLookupResult Lookup(TypeSymbol receiver, string name, MemberLookupOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(receiver);
        options ??= new MemberLookupOptions();
        if (string.IsNullOrWhiteSpace(name))
            return Empty(name, LookupStatus.NotFound, "member name is empty");

        var levels = GetTypeLevels(receiver, options.IncludeInherited);
        var matches = ImmutableArray.CreateBuilder<Symbol>();
        var hidden = ImmutableArray.CreateBuilder<Symbol>();
        var signatures = new HashSet<string>(StringComparer.Ordinal);

        foreach (var level in levels)
        {
            var levelMembers = level.Members.Where(member => member.Name == name).ToArray();
            foreach (var member in levelMembers)
            {
                var signature = Signature(member);
                if (signatures.Add(signature))
                    matches.Add(member);
                else
                    hidden.Add(member);
            }
        }

        if (matches.Count == 0)
            return Empty(name, LookupStatus.NotFound, "member was not found on the type or its base types");

        var arityMatches = options.GenericArity is null
            ? matches.ToImmutable()
            : matches.Where(member => GenericArity(member) == options.GenericArity.Value).ToImmutableArray();
        if (arityMatches.Length == 0)
            return new(name, LookupStatus.WrongArity, matches.ToImmutable(), ImmutableArray<Symbol>.Empty, hidden.ToImmutable(), "member generic arity does not match");

        var accessible = options.IsAccessible is null
            ? arityMatches
            : arityMatches.Where(options.IsAccessible).ToImmutableArray();
        if (accessible.Length == 0)
            return new(name, LookupStatus.Inaccessible, arityMatches, ImmutableArray<Symbol>.Empty, hidden.ToImmutable(), "all matching members are inaccessible");

        var status = accessible.Length == 1 && accessible[0] is not MethodSymbol
            ? LookupStatus.Viable
            : accessible.Length > 0 ? LookupStatus.Ambiguous : LookupStatus.NotFound;
        return new(name, status, matches.ToImmutable(), accessible, hidden.ToImmutable(), status == LookupStatus.Ambiguous ? "multiple members form a method group or conflict" : null);
    }

    public ImmutableArray<MethodSymbol> LookupExtensions(
        TypeSymbol receiver,
        string name,
        IEnumerable<MethodSymbol> methods,
        ConversionContext context,
        Func<Symbol, bool>? isAccessible = null)
    {
        ArgumentNullException.ThrowIfNull(receiver);
        ArgumentNullException.ThrowIfNull(methods);
        return methods
            .Where(method => method.IsExtension && method.Name == name && method.Parameters.Length > 0)
            .Where(method => isAccessible is null || isAccessible(method))
            .Where(method => IsValidExtensionReceiver(receiver, method.Parameters[0].Type, context))
            .GroupBy(Signature, StringComparer.Ordinal)
            .Select(group => group.First())
            .ToImmutableArray();
    }

    private bool IsValidExtensionReceiver(TypeSymbol receiver, TypeSymbol parameter, ConversionContext context)
    {
        var conversion = _conversions.Classify(receiver, parameter, context);
        return conversion.Kind is ConversionKind.Identity or ConversionKind.ImplicitReference or ConversionKind.Boxing
            && conversion.Exists
            && conversion.IsImplicit;
    }

    private static IEnumerable<NamedTypeSymbol> GetTypeLevels(TypeSymbol receiver, bool includeInherited)
    {
        var current = receiver switch
        {
            NamedTypeSymbol named => named,
            ConstructedTypeSymbol constructed when constructed.Definition is NamedTypeSymbol named => named,
            _ => null
        };

        while (current is not null)
        {
            yield return current;
            if (!includeInherited)
                yield break;
            current = current.BaseType as NamedTypeSymbol;
        }
    }

    private static string Signature(Symbol member)
        => member switch
        {
            MethodSymbol method => $"method:{method.Name}/{method.TypeParameters.Length}:{string.Join(',', method.Parameters.Select(parameter => parameter.Type.Id))}",
            PropertySymbol property => $"property:{property.Name}:{property.Type.Id}",
            FieldSymbol field => $"field:{field.Name}:{field.Type.Id}",
            EventSymbol @event => $"event:{@event.Name}:{@event.EventType.Id}",
            _ => $"{member.Kind}:{member.Name}"
        };

    private static int GenericArity(Symbol member)
        => member switch
        {
            NamedTypeSymbol type => type.Arity,
            MethodSymbol method => method.TypeParameters.Length,
            _ => 0
        };

    private static MemberLookupResult Empty(string name, LookupStatus status, string reason)
        => new(name, status, ImmutableArray<Symbol>.Empty, ImmutableArray<Symbol>.Empty, ImmutableArray<Symbol>.Empty, reason);
}
