using System.Collections.Immutable;

namespace DotnetExpertAdvisor.Shared;

public enum LanguageVersion { CSharp6 = 6, CSharp7, CSharp8, CSharp9, CSharp10, CSharp11, CSharp12, CSharp13, CSharp14, CSharp15 }
public enum LanguageFeature { NullConditional, StringInterpolation, OutVariable, Tuples, PatternMatching, LocalFunctions, NullableReferenceTypes, SwitchExpressions, Ranges, Records, InitOnlySetters, TargetTypedNew, TopLevelStatements, FileScopedNamespaces, GlobalUsings, RecordStructs, RawStringLiterals, RequiredMembers, ListPatterns, GenericMath, PrimaryConstructors, CollectionExpressions, ParamsCollections, OverloadResolutionPriority, ExtensionMembers, FieldKeyword, NullConditionalAssignment, PreviewCSharp15 }

public sealed record LanguageCapabilities(LanguageVersion Version, bool PreviewEnabled, ImmutableHashSet<LanguageFeature> Features)
{
    public bool Supports(LanguageFeature feature) => Features.Contains(feature);

    public static LanguageCapabilities For(LanguageVersion version, bool previewEnabled = false)
    {
        var features = ImmutableHashSet.CreateBuilder<LanguageFeature>();
        Add(features, LanguageFeature.NullConditional, version >= LanguageVersion.CSharp6);
        Add(features, LanguageFeature.StringInterpolation, version >= LanguageVersion.CSharp6);
        Add(features, LanguageFeature.OutVariable, version >= LanguageVersion.CSharp7);
        Add(features, LanguageFeature.Tuples, version >= LanguageVersion.CSharp7);
        Add(features, LanguageFeature.PatternMatching, version >= LanguageVersion.CSharp7);
        Add(features, LanguageFeature.LocalFunctions, version >= LanguageVersion.CSharp7);
        Add(features, LanguageFeature.NullableReferenceTypes, version >= LanguageVersion.CSharp8);
        Add(features, LanguageFeature.SwitchExpressions, version >= LanguageVersion.CSharp8);
        Add(features, LanguageFeature.Ranges, version >= LanguageVersion.CSharp8);
        Add(features, LanguageFeature.Records, version >= LanguageVersion.CSharp9);
        Add(features, LanguageFeature.InitOnlySetters, version >= LanguageVersion.CSharp9);
        Add(features, LanguageFeature.TargetTypedNew, version >= LanguageVersion.CSharp9);
        Add(features, LanguageFeature.TopLevelStatements, version >= LanguageVersion.CSharp9);
        Add(features, LanguageFeature.FileScopedNamespaces, version >= LanguageVersion.CSharp10);
        Add(features, LanguageFeature.GlobalUsings, version >= LanguageVersion.CSharp10);
        Add(features, LanguageFeature.RecordStructs, version >= LanguageVersion.CSharp10);
        Add(features, LanguageFeature.RawStringLiterals, version >= LanguageVersion.CSharp11);
        Add(features, LanguageFeature.RequiredMembers, version >= LanguageVersion.CSharp11);
        Add(features, LanguageFeature.ListPatterns, version >= LanguageVersion.CSharp11);
        Add(features, LanguageFeature.GenericMath, version >= LanguageVersion.CSharp11);
        Add(features, LanguageFeature.PrimaryConstructors, version >= LanguageVersion.CSharp12);
        Add(features, LanguageFeature.CollectionExpressions, version >= LanguageVersion.CSharp12);
        Add(features, LanguageFeature.ParamsCollections, version >= LanguageVersion.CSharp13);
        Add(features, LanguageFeature.OverloadResolutionPriority, version >= LanguageVersion.CSharp13);
        Add(features, LanguageFeature.ExtensionMembers, version >= LanguageVersion.CSharp14);
        Add(features, LanguageFeature.FieldKeyword, version >= LanguageVersion.CSharp14);
        Add(features, LanguageFeature.NullConditionalAssignment, version >= LanguageVersion.CSharp14);
        Add(features, LanguageFeature.PreviewCSharp15, version == LanguageVersion.CSharp15 && previewEnabled);
        return new LanguageCapabilities(version, previewEnabled, features.ToImmutable());
    }

    private static void Add(ImmutableHashSet<LanguageFeature>.Builder features, LanguageFeature feature, bool enabled)
    { if (enabled) features.Add(feature); }
}
