using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class MetadataReaderTests
{
    public static void Run(string? referenceAssemblyPath = null)
    {
        var reader = new AssemblyMetadataReader();
        if (referenceAssemblyPath is not null)
        {
            var reference = reader.Read(referenceAssemblyPath, Path.ChangeExtension(referenceAssemblyPath, ".xml"));
            if (!reference.Types.Any(type => type.Namespace == "System" && type.Name == "String") ||
                !reference.Documentation.ContainsKey("T:System.String"))
                throw new InvalidOperationException("Reference assembly API or XML docs missing.");
        }
        var path = typeof(CSharpLexer).Assembly.Location;
        var xmlPath = Path.Combine(Path.GetTempPath(), $"metadata-{Guid.NewGuid():N}.xml");
        try
        {
            File.WriteAllText(xmlPath, "<doc><members><member name='T:Example'><summary>Example docs</summary></member></members></doc>");
            using var stream = File.OpenRead(path);
            var index = reader.Read(stream, xmlPath);
            if (!stream.CanRead || index.Name != "Shared" || index.ModuleId == Guid.Empty)
                throw new InvalidOperationException("Assembly identity or stream ownership is incorrect.");
            var type = index.Types.Single(t => t.Name == "CSharpLexer");
            var method = type.Members.Single(m => m.Name == "Tokenize");
            if (method.Signature.IsEmpty || method.Il.IsEmpty || !index.Documentation.ContainsKey("T:Example"))
                throw new InvalidOperationException("Method signature, IL or documentation was lost.");
            using var invalid = new MemoryStream([1, 2, 3]);
            try { reader.Read(invalid); throw new InvalidOperationException("Invalid PE accepted."); }
            catch (BadImageFormatException) { }
            using var cancelled = File.OpenRead(path);
            try { reader.Read(cancelled, cancellationToken: new CancellationToken(true)); throw new InvalidOperationException("Cancellation ignored."); }
            catch (OperationCanceledException) { }
            File.WriteAllText(xmlPath, "<!DOCTYPE doc [<!ENTITY x SYSTEM 'file:///missing'>]><doc>&x;</doc>");
            try { reader.Read(path, xmlPath); throw new InvalidOperationException("DTD accepted."); }
            catch (System.Xml.XmlException) { }
        }
        finally { File.Delete(xmlPath); }
    }
}
