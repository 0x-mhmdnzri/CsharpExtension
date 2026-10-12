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
    ImplicitTuple,
    ImplicitVariance,
    AnonymousFunction,
    MethodGroup,
    InterpolatedStringHandler,
    CollectionExpression,
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

        if (source is TupleTypeSymbol sourceTuple && target is TupleTypeSymbol targetTuple)
        {
            if (sourceTuple.ElementTypes.Length != targetTuple.ElementTypes.Length)
                return NoConversion("tuple arity differs");
            var elementConversions = sourceTuple.ElementTypes.Zip(targetTuple.ElementTypes, (from, to) => ClassifyCore(from, to, context));
            return elementConversions.All(conversion => conversion.Exists && conversion.IsImplicit)
                ? new(ConversionKind.ImplicitTuple, ConversionQuality.Good, true, true, "all tuple element conversions are implicit")
                : NoConversion("at least one tuple element has no implicit conversion");
        }

        if (source is ConstructedTypeSymbol sourceConstructed && target is ConstructedTypeSymbol targetConstructed &&
            sourceConstructed.Definition.Id == targetConstructed.Definition.Id)
        {
            if (sourceConstructed.TypeArguments.Length != targetConstructed.TypeArguments.Length)
                return NoConversion("constructed type arity differs");
            var variances = sourceConstructed.Variances.Length == sourceConstructed.TypeArguments.Length
                ? sourceConstructed.Variances
                : targetConstructed.Variances;
            if (variances.Length == sourceConstructed.TypeArguments.Length)
            {
                var valid = true;
                for (var i = 0; i < variances.Length; i++)
                {
                    var from = sourceConstructed.TypeArguments[i];
                    var to = targetConstructed.TypeArguments[i];
                    var element = variances[i] switch
                    {
                        VarianceKind.Covariant => ClassifyCore(from, to, context),
                        VarianceKind.Contravariant => ClassifyCore(to, from, context),
                        _ => from.Id == to.Id ? ConversionResult.Identity : NoConversion("invariant type argument differs")
                    };
                    valid &= element.Exists && element.IsImplicit;
                }
                if (valid)
                    return new(ConversionKind.ImplicitVariance, ConversionQuality.Good, true, true, "all variance positions are compatible");
            }
        }

        if (target is NamedTypeSymbol delegateTarget && delegateTarget.TypeKind == TypeKind.Delegate && source is LambdaTypeSymbol lambda)
            return ClassifyLambda(lambda, delegateTarget, context);

        if (target is NamedTypeSymbol delegateTargetForMethodGroup && delegateTargetForMethodGroup.TypeKind == TypeKind.Delegate && source is MethodGroupTypeSymbol methodGroup)
            return ClassifyMethodGroup(methodGroup, delegateTargetForMethodGroup, context);

        if (source is InterpolatedStringTypeSymbol && target is NamedTypeSymbol { TypeKind: TypeKind.Struct } handler && handler.Name.Contains("handler", StringComparison.OrdinalIgnoreCase))
            return new(ConversionKind.InterpolatedStringHandler, ConversionQuality.Good, true, true, "interpolated string handler target");

        if (source is CollectionExpressionTypeSymbol collection && TryGetCollectionElementType(target, out var elementType))
        {
            var elementsValid = collection.ElementTypes.All(element => ClassifyCore(element, elementType, context).IsImplicit);
            return elementsValid
                ? new(ConversionKind.CollectionExpression, ConversionQuality.Good, true, true, "all collection elements convert to the target element type")
                : NoConversion("a collection element cannot convert to the target element type");
        }

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

        if (TryFindUserDefinedOperator(source, target, "op_Implicit", out _))
            return new(ConversionKind.UserDefined, ConversionQuality.Uncertain, true, true, "implicit conversion operator selected");
        if (TryFindUserDefinedOperator(source, target, "op_Explicit", out _))
            return new(ConversionKind.UserDefined, ConversionQuality.RequiresRuntimeCheck, true, false, "explicit conversion operator selected");
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
            TupleTypeSymbol => true,
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

    private static ConversionResult ClassifyLambda(LambdaTypeSymbol lambda, NamedTypeSymbol target, ConversionContext context)
    {
        var invoke = GetDelegateInvoke(target);
        if (invoke is null || invoke.Parameters.Length != lambda.ParameterTypes.Length)
            return NoConversion("delegate Invoke signature is unavailable or has a different arity");
        for (var i = 0; i < invoke.Parameters.Length; i++)
        {
            var parameter = ClassifyCore(lambda.ParameterTypes[i], invoke.Parameters[i].Type, context);
            if (!parameter.Exists || !parameter.IsImplicit)
                return NoConversion("lambda parameter is incompatible with the delegate");
        }
        var result = ClassifyCore(lambda.ReturnType, invoke.ReturnType, context);
        return result.Exists && result.IsImplicit
            ? new(ConversionKind.AnonymousFunction, ConversionQuality.Good, true, true, "lambda matches delegate Invoke")
            : NoConversion("lambda return type is incompatible with the delegate");
    }

    private static ConversionResult ClassifyMethodGroup(MethodGroupTypeSymbol methodGroup, NamedTypeSymbol target, ConversionContext context)
    {
        var invoke = GetDelegateInvoke(target);
        if (invoke is null)
            return NoConversion("delegate Invoke signature is unavailable");
        foreach (var method in methodGroup.Methods)
        {
            if (method.Parameters.Length != invoke.Parameters.Length)
                continue;
            var parametersMatch = method.Parameters.Zip(invoke.Parameters, (methodParameter, delegateParameter) =>
                ClassifyCore(delegateParameter.Type, methodParameter.Type, context).IsImplicit).All(match => match);
            var returnMatch = ClassifyCore(method.ReturnType, invoke.ReturnType, context);
            if (parametersMatch && returnMatch.Exists && returnMatch.IsImplicit)
                return new(ConversionKind.MethodGroup, ConversionQuality.Good, true, true, "method group has a compatible overload");
        }
        return NoConversion("no method-group overload matches the delegate");
    }

    private static MethodSymbol? GetDelegateInvoke(NamedTypeSymbol target)
        => target.Members.OfType<MethodSymbol>().FirstOrDefault(method => method.Name == "Invoke");

    private static bool TryGetCollectionElementType(TypeSymbol target, out TypeSymbol elementType)
    {
        if (target is ConstructedTypeSymbol constructed && constructed.TypeArguments.Length == 1 &&
            (constructed.Name.StartsWith("IEnumerable<", StringComparison.Ordinal) || constructed.Name.StartsWith("List<", StringComparison.Ordinal) || constructed.Name.StartsWith("Span<", StringComparison.Ordinal)))
        {
            elementType = constructed.TypeArguments[0];
            return true;
        }
        elementType = null!;
        return false;
    }

    private static bool TryFindUserDefinedOperator(TypeSymbol source, TypeSymbol target, string name, out MethodSymbol? result)
    {
        var symbols = new List<Symbol>();
        if (source is NamedTypeSymbol sourceNamed) symbols.AddRange(sourceNamed.Members);
        if (target is NamedTypeSymbol targetNamed) symbols.AddRange(targetNamed.Members);
        result = symbols.OfType<MethodSymbol>().FirstOrDefault(method =>
            method.Name == name && method.Parameters.Length == 1 &&
            method.Parameters[0].Type.Id == source.Id && method.ReturnType.Id == target.Id);
        return result is not null;
    }

    private readonly record struct ConversionCacheKey(TypeSymbol Source, TypeSymbol Target, ConversionContext Context);
}
