using System.Collections.Immutable;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Xml;
using System.Xml.Linq;

namespace DotnetExpertAdvisor.Shared;

public sealed record MetadataMember(int Token, string Name, string Kind, int Attributes,
    ImmutableArray<byte> Signature, ImmutableArray<byte> Il, string DecodedSignature,
    ImmutableArray<MetadataGenericParameter> GenericParameters, string DocumentationId,
    string Documentation);
public sealed record MetadataGenericParameter(int Index, string Name,
    GenericParameterAttributes Attributes, ImmutableArray<int> ConstraintTokens);
public sealed record MetadataAssemblyReference(string Name, Version Version, string Culture,
    AssemblyFlags Flags, ImmutableArray<byte> PublicKeyOrToken);
public sealed record MetadataTypeForwarder(string Namespace, string Name, int TargetAssemblyReferenceToken);
public sealed record MetadataType(int Token, string Namespace, string Name,
    TypeAttributes Attributes, int BaseTypeToken, int GenericArity,
    ImmutableArray<MetadataMember> Members, ImmutableArray<MetadataGenericParameter> GenericParameters,
    string DocumentationId, string Documentation);
public sealed record AssemblyMetadataIndex(string Name, Version Version, Guid ModuleId,
    ImmutableArray<MetadataType> Types, ImmutableArray<string> References,
    ImmutableDictionary<string, string> Documentation,
    ImmutableArray<MetadataAssemblyReference> AssemblyReferences,
    ImmutableArray<MetadataTypeForwarder> TypeForwarders, TargetFrameworkId TargetFramework);

public sealed class AssemblyMetadataReader
{
    public AssemblyMetadataIndex Read(string assemblyPath, string? documentationPath = null,
        CancellationToken cancellationToken = default)
    {
        using var stream = File.OpenRead(assemblyPath);
        return Read(stream, documentationPath, cancellationToken);
    }

    // The caller owns the stream; no assembly is loaded into the execution context.
    public AssemblyMetadataIndex Read(Stream stream, string? documentationPath = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream);
        cancellationToken.ThrowIfCancellationRequested();
        using var pe = new PEReader(stream, PEStreamOptions.LeaveOpen);
        if (!pe.HasMetadata)
            throw new BadImageFormatException("PE image has no managed metadata.");
        var reader = pe.GetMetadataReader();
        if (!reader.IsAssembly)
            throw new BadImageFormatException("A managed assembly manifest is required.");
        var assembly = reader.GetAssemblyDefinition();
        var documentation = ReadDocumentation(documentationPath, cancellationToken);
        var targetFramework = ReadTargetFramework(reader, assembly, cancellationToken);
        var types = ImmutableArray.CreateBuilder<MetadataType>();
        foreach (var handle in reader.TypeDefinitions)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var type = reader.GetTypeDefinition(handle);
            var namespaceName = reader.GetString(type.Namespace);
            var typeName = reader.GetString(type.Name);
            var typeDocumentation = FindDocumentation(documentation, $"T:{QualifiedName(namespaceName, typeName)}");
            var members = ImmutableArray.CreateBuilder<MetadataMember>();
            foreach (var methodHandle in type.GetMethods())
            {
                var method = reader.GetMethodDefinition(methodHandle);
                var il = ImmutableArray<byte>.Empty;
                if (method.RelativeVirtualAddress != 0 &&
                    (method.ImplAttributes & MethodImplAttributes.CodeTypeMask) == MethodImplAttributes.IL)
                    il = ImmutableArray.CreateRange(pe.GetMethodBody(method.RelativeVirtualAddress).GetILBytes() ?? []);
                var genericParameters = ReadGenericParameters(reader, method.GetGenericParameters());
                var memberDocumentation = FindDocumentation(documentation,
                    $"M:{QualifiedName(namespaceName, typeName)}.{reader.GetString(method.Name)}");
                members.Add(new MetadataMember(MetadataTokens.GetToken(methodHandle),
                    reader.GetString(method.Name), "method", (int)method.Attributes,
                    ImmutableArray.CreateRange(reader.GetBlobBytes(method.Signature)), il,
                    MetadataSignatureDecoder.DecodeMethod(reader, method.Signature),
                    genericParameters, memberDocumentation.Id, memberDocumentation.Text));
            }
            foreach (var fieldHandle in type.GetFields())
            {
                var field = reader.GetFieldDefinition(fieldHandle);
                var memberDocumentation = FindDocumentation(documentation,
                    $"F:{QualifiedName(namespaceName, typeName)}.{reader.GetString(field.Name)}");
                members.Add(new MetadataMember(MetadataTokens.GetToken(fieldHandle),
                    reader.GetString(field.Name), "field", (int)field.Attributes,
                    ImmutableArray.CreateRange(reader.GetBlobBytes(field.Signature)), [],
                    MetadataSignatureDecoder.DecodeField(reader, field.Signature), [], memberDocumentation.Id, memberDocumentation.Text));
            }
            foreach (var propertyHandle in type.GetProperties())
            {
                var property = reader.GetPropertyDefinition(propertyHandle);
                var memberDocumentation = FindDocumentation(documentation,
                    $"P:{QualifiedName(namespaceName, typeName)}.{reader.GetString(property.Name)}");
                members.Add(new MetadataMember(MetadataTokens.GetToken(propertyHandle),
                    reader.GetString(property.Name), "property", (int)property.Attributes,
                    ImmutableArray.CreateRange(reader.GetBlobBytes(property.Signature)), [],
                    MetadataSignatureDecoder.DecodeProperty(reader, property.Signature), [], memberDocumentation.Id, memberDocumentation.Text));
            }
            foreach (var eventHandle in type.GetEvents())
            {
                var definition = reader.GetEventDefinition(eventHandle);
                var memberDocumentation = FindDocumentation(documentation,
                    $"E:{QualifiedName(namespaceName, typeName)}.{reader.GetString(definition.Name)}");
                members.Add(new MetadataMember(MetadataTokens.GetToken(eventHandle),
                    reader.GetString(definition.Name), "event", (int)definition.Attributes, [], [], "", [], memberDocumentation.Id, memberDocumentation.Text));
            }
            var typeParameters = ReadGenericParameters(reader, type.GetGenericParameters());
            types.Add(new MetadataType(MetadataTokens.GetToken(handle), namespaceName,
                typeName, type.Attributes,
                type.BaseType.IsNil ? 0 : MetadataTokens.GetToken(type.BaseType),
                typeParameters.Length, members.ToImmutable(), typeParameters,
                typeDocumentation.Id, typeDocumentation.Text));
        }
        var assemblyReferences = reader.AssemblyReferences.Select(handle =>
        {
            var reference = reader.GetAssemblyReference(handle);
            return new MetadataAssemblyReference(reader.GetString(reference.Name), reference.Version,
                reference.Culture.IsNil ? "" : reader.GetString(reference.Culture), reference.Flags,
                reference.PublicKeyOrToken.IsNil ? [] : ImmutableArray.CreateRange(reader.GetBlobBytes(reference.PublicKeyOrToken)));
        }).ToImmutableArray();
        var references = assemblyReferences.Select(reference => reference.Name).ToImmutableArray();
        var forwarders = reader.ExportedTypes.Where(handle => reader.GetExportedType(handle).IsForwarder)
            .Select(handle =>
            {
                var exported = reader.GetExportedType(handle);
                var target = exported.Implementation.Kind == HandleKind.AssemblyReference
                    ? MetadataTokens.GetToken((AssemblyReferenceHandle)exported.Implementation) : 0;
                return new MetadataTypeForwarder(exported.Namespace.IsNil ? "" : reader.GetString(exported.Namespace),
                    exported.Name.IsNil ? "" : reader.GetString(exported.Name), target);
            }).ToImmutableArray();
        var module = reader.GetModuleDefinition();
        return new AssemblyMetadataIndex(reader.GetString(assembly.Name), assembly.Version,
            reader.GetGuid(module.Mvid), types.ToImmutable(), references,
            documentation, assemblyReferences, forwarders, targetFramework);
    }

    private static TargetFrameworkId ReadTargetFramework(MetadataReader reader,
        AssemblyDefinition assembly, CancellationToken cancellationToken)
    {
        foreach (var handle in assembly.GetCustomAttributes())
        {
            cancellationToken.ThrowIfCancellationRequested();
            var attribute = reader.GetCustomAttribute(handle);
            if (attribute.Constructor.Kind != HandleKind.MemberReference) continue;
            var member = reader.GetMemberReference((MemberReferenceHandle)attribute.Constructor);
            if (member.Parent.Kind != HandleKind.TypeReference) continue;
            var type = reader.GetTypeReference((TypeReferenceHandle)member.Parent);
            if (reader.GetString(type.Name) != "TargetFrameworkAttribute") continue;
            var value = reader.GetBlobReader(attribute.Value);
            if (value.ReadUInt16() != 1) throw new BadImageFormatException("Invalid TargetFrameworkAttribute blob.");
            var framework = value.ReadSerializedString();
            if (!string.IsNullOrWhiteSpace(framework)) return new TargetFrameworkId(framework);
        }
        return new TargetFrameworkId("");
    }

    private static string QualifiedName(string namespaceName, string typeName)
        => string.IsNullOrEmpty(namespaceName) ? typeName : $"{namespaceName}.{typeName}";

    private static (string Id, string Text) FindDocumentation(
        ImmutableDictionary<string, string> documentation, string prefix)
    {
        if (documentation.TryGetValue(prefix, out var exact)) return (prefix, exact);
        var overload = documentation.FirstOrDefault(pair => pair.Key.StartsWith(prefix + "(", StringComparison.Ordinal));
        return overload.Equals(default(KeyValuePair<string, string>)) ? ("", "") : (overload.Key, overload.Value);
    }

    private static ImmutableArray<MetadataGenericParameter> ReadGenericParameters(
        MetadataReader reader, GenericParameterHandleCollection handles)
    {
        var result = ImmutableArray.CreateBuilder<MetadataGenericParameter>();
        foreach (var handle in handles)
        {
            var parameter = reader.GetGenericParameter(handle);
            var constraints = parameter.GetConstraints()
                .Select(c => MetadataTokens.GetToken(reader.GetGenericParameterConstraint(c).Type))
                .ToImmutableArray();
            result.Add(new MetadataGenericParameter(parameter.Index,
                reader.GetString(parameter.Name), parameter.Attributes, constraints));
        }
        return result.ToImmutable();
    }

    private static ImmutableDictionary<string, string> ReadDocumentation(string? path, CancellationToken cancellationToken)
    {
        var result = ImmutableDictionary.CreateBuilder<string, string>(StringComparer.Ordinal);
        if (path is null) return result.ToImmutable();
        using var xml = XmlReader.Create(path, new XmlReaderSettings
        {
            DtdProcessing = DtdProcessing.Prohibit,
            XmlResolver = null,
            MaxCharactersInDocument = 32 * 1024 * 1024
        });
        var document = XDocument.Load(xml);
        foreach (var member in document.Root?.Element("members")?.Elements("member") ?? [])
        {
            cancellationToken.ThrowIfCancellationRequested();
            var id = (string?)member.Attribute("name");
            if (string.IsNullOrWhiteSpace(id)) throw new XmlException("XML documentation member has no ID.");
            if (!result.TryAdd(id, member.ToString(SaveOptions.DisableFormatting)))
                throw new XmlException($"Duplicate XML documentation ID: {id}");
        }
        return result.ToImmutable();
    }
}

// shortcut: signatures are display strings with !n/!!n placeholders; use structured types when binding metadata symbols.
internal static class MetadataSignatureDecoder
{
    public static string DecodeMethod(MetadataReader reader, BlobHandle signature)
    {
        var decoder = new SignatureDecoder<string, object?>(new TypeProvider(), reader, null);
        var blob = reader.GetBlobReader(signature);
        var decoded = decoder.DecodeMethodSignature(ref blob);
        return $"({string.Join(", ", decoded.ParameterTypes)}) -> {decoded.ReturnType}";
    }

    public static string DecodeField(MetadataReader reader, BlobHandle signature)
    {
            var decoder = new SignatureDecoder<string, object?>(new TypeProvider(), reader, null);
            var blob = reader.GetBlobReader(signature);
            return decoder.DecodeFieldSignature(ref blob);
    }

    public static string DecodeProperty(MetadataReader reader, BlobHandle signature)
        => DecodeMethod(reader, signature);

    private sealed class TypeProvider : ISignatureTypeProvider<string, object?>
    {
        public string GetArrayType(string elementType, ArrayShape shape)
            => $"{elementType}[{new string(',', shape.Rank - 1)}]";
        public string GetByReferenceType(string elementType) => $"ref {elementType}";
        public string GetFunctionPointerType(MethodSignature<string> signature)
            => $"delegate* ({string.Join(", ", signature.ParameterTypes)}) -> {signature.ReturnType}";
        public string GetGenericInstantiation(string genericType, ImmutableArray<string> typeArguments)
            => $"{genericType}<{string.Join(", ", typeArguments)}>";
        public string GetGenericMethodParameter(object? genericContext, int index) => $"!!{index}";
        public string GetGenericTypeParameter(object? genericContext, int index) => $"!{index}";
        public string GetModifiedType(string modifier, string unmodifiedType, bool isRequired)
            => $"{unmodifiedType} {(isRequired ? "modreq" : "modopt")}({modifier})";
        public string GetPinnedType(string elementType) => $"pinned {elementType}";
        public string GetPointerType(string elementType) => $"{elementType}*";
        public string GetPrimitiveType(PrimitiveTypeCode typeCode) => typeCode switch
        {
            PrimitiveTypeCode.Void => "void",
            PrimitiveTypeCode.Boolean => "bool",
            PrimitiveTypeCode.Char => "char",
            PrimitiveTypeCode.SByte => "sbyte",
            PrimitiveTypeCode.Byte => "byte",
            PrimitiveTypeCode.Int16 => "short",
            PrimitiveTypeCode.UInt16 => "ushort",
            PrimitiveTypeCode.Int32 => "int",
            PrimitiveTypeCode.UInt32 => "uint",
            PrimitiveTypeCode.Int64 => "long",
            PrimitiveTypeCode.UInt64 => "ulong",
            PrimitiveTypeCode.Single => "float",
            PrimitiveTypeCode.Double => "double",
            PrimitiveTypeCode.String => "string",
            PrimitiveTypeCode.IntPtr => "nint",
            PrimitiveTypeCode.UIntPtr => "nuint",
            PrimitiveTypeCode.Object => "object",
            _ => typeCode.ToString()
        };
        public string GetSZArrayType(string elementType) => $"{elementType}[]";
        public string GetTypeFromDefinition(MetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind)
        {
            var type = reader.GetTypeDefinition(handle);
            var ns = reader.GetString(type.Namespace);
            var name = reader.GetString(type.Name);
            if (!type.GetDeclaringType().IsNil)
                return $"{GetTypeFromDefinition(reader, type.GetDeclaringType(), rawTypeKind)}+{name}";
            return string.IsNullOrEmpty(ns) ? name : $"{ns}.{name}";
        }
        public string GetTypeFromReference(MetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind)
        {
            var type = reader.GetTypeReference(handle);
            var ns = reader.GetString(type.Namespace);
            var name = reader.GetString(type.Name);
            if (type.ResolutionScope.Kind == HandleKind.TypeReference)
                return $"{GetTypeFromReference(reader, (TypeReferenceHandle)type.ResolutionScope, rawTypeKind)}+{name}";
            return string.IsNullOrEmpty(ns) ? name : $"{ns}.{name}";
        }
        public string GetTypeFromSpecification(MetadataReader reader, object? genericContext,
            TypeSpecificationHandle handle, byte rawTypeKind)
        {
            var decoder = new SignatureDecoder<string, object?>(this, reader, genericContext);
            var blob = reader.GetBlobReader(reader.GetTypeSpecification(handle).Signature);
            return decoder.DecodeType(ref blob, true);
        }
        public string GetUnsupportedSignatureTypeKind(byte rawTypeKind) => $"<type 0x{rawTypeKind:X2}>";
    }
}
