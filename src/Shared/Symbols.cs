using System.Collections.Immutable;

namespace DotnetExpertAdvisor.Shared;

public enum SymbolKind
{
    Namespace,
    NamedType,
    TypeParameter,
    ArrayType,
    PointerType,
    FunctionPointerType,
    TupleType,
    DynamicType,
    Method,
    Property,
    Field,
    Event,
    Parameter,
    Local,
    ErrorType
}

public enum TypeKind { Class, Struct, Interface, Enum, Delegate, Record, Error }
public enum Accessibility { NotApplicable, Private, Protected, Internal, ProtectedInternal, PrivateProtected, Public }

public abstract record Symbol(
    string Id,
    string Name,
    SymbolKind Kind,
    Symbol? ContainingSymbol,
    Accessibility Accessibility);

public abstract record TypeSymbol(
    string Id,
    string Name,
    SymbolKind Kind,
    Symbol? ContainingSymbol,
    Accessibility Accessibility)
    : Symbol(Id, Name, Kind, ContainingSymbol, Accessibility);

public sealed record NamedTypeSymbol(
    string Id,
    string Name,
    TypeKind TypeKind,
    int Arity,
    Symbol? ContainingSymbol,
    Accessibility Accessibility,
    TypeSymbol? BaseType,
    ImmutableArray<TypeSymbol> Interfaces,
    ImmutableArray<Symbol> Members)
    : TypeSymbol(Id, Name, SymbolKind.NamedType, ContainingSymbol, Accessibility);

public sealed record ConstructedTypeSymbol(
    string Id,
    string Name,
    TypeSymbol Definition,
    ImmutableArray<TypeSymbol> TypeArguments,
    Symbol? ContainingSymbol)
    : TypeSymbol(Id, Name, SymbolKind.NamedType, ContainingSymbol, Accessibility.Public);

public sealed record NullableTypeSymbol(TypeSymbol UnderlyingType)
    : TypeSymbol($"nullable:{UnderlyingType.Id}", $"Nullable<{UnderlyingType.Name}>", SymbolKind.NamedType, null, Accessibility.Public);

public sealed record TupleTypeSymbol(ImmutableArray<TypeSymbol> ElementTypes)
    : TypeSymbol(
        $"tuple:{string.Join(',', ElementTypes.Select(element => element.Id))}",
        $"({string.Join(", ", ElementTypes.Select(element => element.Name))})",
        SymbolKind.TupleType,
        null,
        Accessibility.Public);

public sealed record LambdaTypeSymbol(ImmutableArray<TypeSymbol> ParameterTypes, TypeSymbol ReturnType)
    : TypeSymbol(
        $"lambda:{string.Join(',', ParameterTypes.Select(parameter => parameter.Id))}->{ReturnType.Id}",
        "lambda",
        SymbolKind.DynamicType,
        null,
        Accessibility.NotApplicable);

public sealed record MethodGroupTypeSymbol(ImmutableArray<MethodSymbol> Methods)
    : TypeSymbol(
        $"method-group:{string.Join(',', Methods.Select(method => method.Id))}",
        "method-group",
        SymbolKind.DynamicType,
        null,
        Accessibility.NotApplicable);

public sealed record ErrorTypeSymbol(
    string Id,
    string Name,
    Symbol? ContainingSymbol,
    ImmutableArray<Symbol> CandidateSymbols,
    string FailureReason)
    : TypeSymbol(Id, Name, SymbolKind.ErrorType, ContainingSymbol, Accessibility.NotApplicable)
{
    public TypeKind TypeKind => TypeKind.Error;
    public bool IsError => true;
};

public sealed record MethodSymbol(
    string Id,
    string Name,
    Symbol? ContainingSymbol,
    Accessibility Accessibility,
    TypeSymbol ReturnType,
    ImmutableArray<ParameterSymbol> Parameters,
    ImmutableArray<TypeParameterSymbol> TypeParameters,
    bool IsStatic,
    bool IsVirtual,
    bool IsExtension)
    : Symbol(Id, Name, SymbolKind.Method, ContainingSymbol, Accessibility);

public sealed record PropertySymbol(
    string Id,
    string Name,
    Symbol? ContainingSymbol,
    Accessibility Accessibility,
    TypeSymbol Type,
    bool HasGet,
    bool HasSet,
    bool IsStatic)
    : Symbol(Id, Name, SymbolKind.Property, ContainingSymbol, Accessibility);

public sealed record FieldSymbol(
    string Id,
    string Name,
    Symbol? ContainingSymbol,
    Accessibility Accessibility,
    TypeSymbol Type,
    bool IsStatic,
    bool IsReadOnly)
    : Symbol(Id, Name, SymbolKind.Field, ContainingSymbol, Accessibility);

public sealed record EventSymbol(
    string Id,
    string Name,
    Symbol? ContainingSymbol,
    Accessibility Accessibility,
    TypeSymbol EventType)
    : Symbol(Id, Name, SymbolKind.Event, ContainingSymbol, Accessibility);

public sealed record ParameterSymbol(
    string Id,
    string Name,
    Symbol? ContainingSymbol,
    TypeSymbol Type,
    bool IsOptional,
    bool IsParams)
    : Symbol(Id, Name, SymbolKind.Parameter, ContainingSymbol, Accessibility.NotApplicable);

public sealed record LocalSymbol(
    string Id,
    string Name,
    Symbol? ContainingSymbol,
    TypeSymbol Type,
    bool IsReadOnly)
    : Symbol(Id, Name, SymbolKind.Local, ContainingSymbol, Accessibility.NotApplicable);

public sealed record TypeParameterSymbol(
    string Id,
    string Name,
    Symbol? ContainingSymbol,
    int Ordinal,
    ImmutableArray<TypeSymbol> Constraints,
    bool HasReferenceTypeConstraint,
    bool HasValueTypeConstraint,
    bool HasNotNullConstraint)
    : TypeSymbol(Id, Name, SymbolKind.TypeParameter, ContainingSymbol, Accessibility.NotApplicable);

public static class SymbolFactory
{
    public static ErrorTypeSymbol Error(
        string id,
        string name,
        IEnumerable<Symbol> candidates,
        string failureReason,
        Symbol? containingSymbol = null)
        => new(id, name, containingSymbol, candidates.ToImmutableArray(), failureReason);
}
