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
            if (method.Signature.IsEmpty || method.Il.IsEmpty || string.IsNullOrWhiteSpace(method.DecodedSignature) ||
                !index.Documentation.ContainsKey("T:Example"))
                throw new InvalidOperationException("Method signature, IL or documentation was lost.");
            var generic = reader.Read(typeof(Dictionary<,>).Assembly.Location);
            var dictionary = generic.Types.Single(t => t.Namespace == "System.Collections.Generic" && t.Name == "Dictionary`2");
            if (dictionary.GenericParameters.Length != 2 || dictionary.Members.All(m => string.IsNullOrWhiteSpace(m.DecodedSignature)))
                throw new InvalidOperationException("Generic parameters or decoded signatures were lost.");
            var fixture = reader.Read(typeof(MetadataFixture<>).Assembly.Location).Types.Single(t => t.Name == "MetadataFixture`1");
            var parameter = fixture.GenericParameters.Single();
            if (parameter.Name != "T" || parameter.Index != 0 || parameter.ConstraintTokens.Length != 1 ||
                (parameter.Attributes & System.Reflection.GenericParameterAttributes.DefaultConstructorConstraint) == 0 ||
                (parameter.Attributes & System.Reflection.GenericParameterAttributes.ReferenceTypeConstraint) == 0)
                throw new InvalidOperationException("Type constraints were lost.");
            var echo = fixture.Members.Single(m => m.Name == "Echo");
            if (echo.DecodedSignature != "(ref !0, !!0[,], int[]) -> !!0" ||
                echo.GenericParameters.Single().Name != "U" ||
                (echo.GenericParameters.Single().Attributes & System.Reflection.GenericParameterAttributes.NotNullableValueTypeConstraint) == 0)
                throw new InvalidOperationException($"Generic method signature or constraints are incorrect: {echo.DecodedSignature}");
            if (fixture.Members.Single(m => m.Name == "Item").DecodedSignature != "(int) -> !0")
                throw new InvalidOperationException("Indexer signature was lost.");
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

internal sealed class MetadataFixture<T> where T : class, IDisposable, new()
{
    public T this[int index] => new();
    public U Echo<U>(ref T value, U[,] matrix, int[] numbers) where U : struct => default;
}
