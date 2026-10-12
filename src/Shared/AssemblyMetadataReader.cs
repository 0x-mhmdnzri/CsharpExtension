using System.Collections.Immutable;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Xml;
using System.Xml.Linq;

namespace DotnetExpertAdvisor.Shared;

public sealed record MetadataMember(int Token, string Name, string Kind, int Attributes,
    ImmutableArray<byte> Signature, ImmutableArray<byte> Il);
public sealed record MetadataType(int Token, string Namespace, string Name,
    TypeAttributes Attributes, int BaseTypeToken, int GenericArity,
    ImmutableArray<MetadataMember> Members);
public sealed record AssemblyMetadataIndex(string Name, Version Version, Guid ModuleId,
    ImmutableArray<MetadataType> Types, ImmutableArray<string> References,
    ImmutableDictionary<string, string> Documentation);

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
        var types = ImmutableArray.CreateBuilder<MetadataType>();
        foreach (var handle in reader.TypeDefinitions)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var type = reader.GetTypeDefinition(handle);
            var members = ImmutableArray.CreateBuilder<MetadataMember>();
            foreach (var methodHandle in type.GetMethods())
            {
                var method = reader.GetMethodDefinition(methodHandle);
                var il = ImmutableArray<byte>.Empty;
                if (method.RelativeVirtualAddress != 0 &&
                    (method.ImplAttributes & MethodImplAttributes.CodeTypeMask) == MethodImplAttributes.IL)
                    il = ImmutableArray.CreateRange(pe.GetMethodBody(method.RelativeVirtualAddress).GetILBytes() ?? []);
                members.Add(new MetadataMember(MetadataTokens.GetToken(methodHandle),
                    reader.GetString(method.Name), "method", (int)method.Attributes,
                    ImmutableArray.CreateRange(reader.GetBlobBytes(method.Signature)), il));
            }
            foreach (var fieldHandle in type.GetFields())
            {
                var field = reader.GetFieldDefinition(fieldHandle);
                members.Add(new MetadataMember(MetadataTokens.GetToken(fieldHandle),
                    reader.GetString(field.Name), "field", (int)field.Attributes,
                    ImmutableArray.CreateRange(reader.GetBlobBytes(field.Signature)), []));
            }
            foreach (var propertyHandle in type.GetProperties())
            {
                var property = reader.GetPropertyDefinition(propertyHandle);
                members.Add(new MetadataMember(MetadataTokens.GetToken(propertyHandle),
                    reader.GetString(property.Name), "property", (int)property.Attributes,
                    ImmutableArray.CreateRange(reader.GetBlobBytes(property.Signature)), []));
            }
            foreach (var eventHandle in type.GetEvents())
            {
                var definition = reader.GetEventDefinition(eventHandle);
                members.Add(new MetadataMember(MetadataTokens.GetToken(eventHandle),
                    reader.GetString(definition.Name), "event", (int)definition.Attributes, [], []));
            }
            types.Add(new MetadataType(MetadataTokens.GetToken(handle), reader.GetString(type.Namespace),
                reader.GetString(type.Name), type.Attributes,
                type.BaseType.IsNil ? 0 : MetadataTokens.GetToken(type.BaseType),
                type.GetGenericParameters().Count, members.ToImmutable()));
        }
        var references = reader.AssemblyReferences.Select(handle =>
            reader.GetString(reader.GetAssemblyReference(handle).Name)).ToImmutableArray();
        var module = reader.GetModuleDefinition();
        return new AssemblyMetadataIndex(reader.GetString(assembly.Name), assembly.Version,
            reader.GetGuid(module.Mvid), types.ToImmutable(), references,
            ReadDocumentation(documentationPath, cancellationToken));
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
