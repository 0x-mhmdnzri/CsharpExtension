using System.Collections.Concurrent;

namespace DotnetExpertAdvisor.Shared;

public enum ConversionKind
{
    Identity,
    ImplicitNumeric,
    ExplicitNumeric,
    ImplicitReference,
    Boxing,
    ImplicitNullable,
    NullLiteral,
    UserDefined,
    Unknown,
    NoConversion
}

public enum ConversionQuality { Best, Good, RequiresRuntimeCheck, Uncertain, Invalid }

public readonly record struct ConversionContext(int LanguageVersion, bool CheckedContext = false);

public sealed record ConversionResult(
    ConversionKind Kind,
    ConversionQuality Quality,
    bool Exists,
    bool IsImplicit,
    string? Reason)
{
    public static ConversionResult Identity { get; } = new(ConversionKind.Identity, ConversionQuality.Best, true, true, null);
}

public interface IConversionClassifier
{
    ConversionResult Classify(TypeSymbol source, TypeSymbol target, ConversionContext context);
}

public sealed class ConversionClassifier : IConversionClassifier
{
    private readonly ConcurrentDictionary<ConversionCacheKey, ConversionResult> _cache = new();

    public ConversionResult Classify(TypeSymbol source, TypeSymbol target, ConversionContext context)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(target);

        if (context.LanguageVersion is < 6 or > 15)
            throw new ArgumentOutOfRangeException(nameof(context));
        var key = new ConversionCacheKey(source, target, context);
        return _cache.GetOrAdd(key, _ => ClassifyCore(source, target, context));
    }

    public int CachedConversionCount => _cache.Count;

    private static ConversionResult ClassifyCore(TypeSymbol source, TypeSymbol target, ConversionContext context)
    {
        if (source is ErrorTypeSymbol || target is ErrorTypeSymbol)
            return new(ConversionKind.Unknown, ConversionQuality.Uncertain, false, false, "an error type prevents a sound conversion decision");

        if (source.Id == target.Id)
            return ConversionResult.Identity;

        if ((Normalize(source.Name) is "nint" or "nuint" || Normalize(target.Name) is "nint" or "nuint") && context.LanguageVersion < 9)
            return NoConversion("native integers require C# 9");

        if (IsNullLiteral(source))
            return IsReferenceType(target) || IsNullableType(target)
                ? new(ConversionKind.NullLiteral, ConversionQuality.Best, true, true, null)
                : NoConversion("null literal cannot convert to a non-nullable value type");

        if (TryClassifyNumeric(source, target, out var numeric))
            return numeric;

        if (target is NullableTypeSymbol nullableTarget)
        {
            var underlying = source is NullableTypeSymbol nullableSource ? nullableSource.UnderlyingType : source;
            if (IsValueType(underlying))
            {
                var lifted = ClassifyCore(underlying, nullableTarget.UnderlyingType, context);
                if (lifted.Exists && lifted.IsImplicit)
                    return new(ConversionKind.ImplicitNullable, ConversionQuality.Good, true, true, "underlying implicit conversion is lifted");
            }
        }

        if (IsReferenceType(source) && IsReferenceType(target) && IsBaseType(target, source))
            return new(ConversionKind.ImplicitReference, ConversionQuality.Good, true, true, "source type derives from target type");

        if ((IsValueType(source) || source is NullableTypeSymbol) &&
            (IsObjectType(target) || IsImplementedInterface(source is NullableTypeSymbol n ? n.UnderlyingType : source, target)))
            return new(ConversionKind.Boxing, ConversionQuality.RequiresRuntimeCheck, true, true, "value type is boxed");

        if (IsTypeParameter(source) || IsTypeParameter(target) || IsDynamic(source) || IsDynamic(target))
            return new(ConversionKind.Unknown, ConversionQuality.Uncertain, false, false, "type parameter or dynamic dispatch requires binding context");

        // shortcut: custom conversion operators are not bound; complete during overload-resolution work.
        if (source is NamedTypeSymbol { Members.Length: > 0 } || target is NamedTypeSymbol { Members.Length: > 0 })
            return new(ConversionKind.Unknown, ConversionQuality.Uncertain, false, false, "custom operators require member binding");
        return NoConversion($"no implicit conversion from {source.Name} to {target.Name} was found");
    }

    private static bool TryClassifyNumeric(TypeSymbol source, TypeSymbol target, out ConversionResult result)
    {
        var sourceName = Normalize(source.Name);
        var targetName = Normalize(target.Name);
        if (!NumericRanks.TryGetValue(sourceName, out var sourceRank) || !NumericRanks.TryGetValue(targetName, out var targetRank))
        {
            result = default!;
            return false;
        }

        if (ImplicitNumericTargets.TryGetValue(sourceName, out var targets) && targets.Contains(targetName))
        {
            result = new(ConversionKind.ImplicitNumeric, ConversionQuality.Good, true, true, null);
            return true;
        }

        if (sourceName != targetName)
        {
            result = new(ConversionKind.ExplicitNumeric, ConversionQuality.RequiresRuntimeCheck, true, false, "numeric conversion may lose range or precision");
            return true;
        }

        result = ConversionResult.Identity;
        return true;
    }

    private static bool IsBaseType(TypeSymbol target, TypeSymbol source)
    {
        var current = source switch
        {
            NamedTypeSymbol named => named.BaseType,
            ConstructedTypeSymbol constructed => constructed.Definition is NamedTypeSymbol named ? named.BaseType : null,
            _ => null
        };

        while (current is not null)
        {
            if (current.Id == target.Id)
                return true;
            current = current switch
            {
                NamedTypeSymbol named => named.BaseType,
                ConstructedTypeSymbol constructed => constructed.Definition is NamedTypeSymbol named ? named.BaseType : null,
                _ => null
            };
        }

        return IsObjectType(target) && IsReferenceType(source);
    }

    private static bool IsReferenceType(TypeSymbol type)
        => type switch
        {
            NamedTypeSymbol named => named.TypeKind is TypeKind.Class or TypeKind.Interface or TypeKind.Delegate or TypeKind.Record,
            ConstructedTypeSymbol constructed => IsReferenceType(constructed.Definition),
            _ => false
        };

    private static bool IsValueType(TypeSymbol type)
        => type switch
        {
            NamedTypeSymbol named => named.TypeKind is TypeKind.Struct or TypeKind.Enum,
            ConstructedTypeSymbol constructed => IsValueType(constructed.Definition),
            _ => false
        };

    private static bool IsInterfaceType(TypeSymbol type) => type is NamedTypeSymbol { TypeKind: TypeKind.Interface };
    private static bool IsObjectType(TypeSymbol type) => Normalize(type.Name) is "object" or "system.object";
    private static bool IsNullableType(TypeSymbol type) => type is NullableTypeSymbol;
    private static bool IsTypeParameter(TypeSymbol type) => type is TypeParameterSymbol;
    private static bool IsDynamic(TypeSymbol type) => Normalize(type.Name) == "dynamic";
    private static bool IsNullLiteral(TypeSymbol type) => Normalize(type.Name) is "null" or "<null>";
    private static bool IsImplementedInterface(TypeSymbol source, TypeSymbol target)
        => source is NamedTypeSymbol named && IsInterfaceType(target) && named.Interfaces.Any(i => i.Id == target.Id);
    private static string Normalize(string value) => value.Replace(" ", string.Empty, StringComparison.Ordinal).ToLowerInvariant();
    private static ConversionResult NoConversion(string reason) => new(ConversionKind.NoConversion, ConversionQuality.Invalid, false, false, reason);

    private static readonly IReadOnlyDictionary<string, int> NumericRanks = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["sbyte"] = 1, ["byte"] = 2, ["short"] = 3, ["ushort"] = 4, ["char"] = 4,
        ["int"] = 5, ["uint"] = 6, ["long"] = 7, ["ulong"] = 8,
        ["nint"] = 7, ["nuint"] = 8, ["float"] = 9, ["double"] = 10, ["decimal"] = 11
    };

    private static readonly IReadOnlyDictionary<string, IReadOnlySet<string>> ImplicitNumericTargets = new Dictionary<string, IReadOnlySet<string>>(StringComparer.Ordinal)
    {
        ["sbyte"] = Set("short", "int", "nint", "long", "float", "double", "decimal"),
        ["byte"] = Set("short", "ushort", "int", "uint", "nint", "nuint", "long", "ulong", "float", "double", "decimal"),
        ["short"] = Set("int", "nint", "long", "float", "double", "decimal"),
        ["ushort"] = Set("int", "uint", "nint", "nuint", "long", "ulong", "float", "double", "decimal"),
        ["int"] = Set("nint", "long", "float", "double", "decimal"),
        ["uint"] = Set("nuint", "long", "ulong", "float", "double", "decimal"),
        ["long"] = Set("float", "double", "decimal"),
        ["ulong"] = Set("float", "double", "decimal"),
        ["char"] = Set("ushort", "int", "uint", "nint", "nuint", "long", "ulong", "float", "double", "decimal"),
        ["nint"] = Set("long", "float", "double", "decimal"),
        ["nuint"] = Set("ulong", "float", "double", "decimal"),
        ["float"] = Set("double")
    };

    private static IReadOnlySet<string> Set(params string[] values) => values.ToHashSet(StringComparer.Ordinal);

    private readonly record struct ConversionCacheKey(TypeSymbol Source, TypeSymbol Target, ConversionContext Context);
}
