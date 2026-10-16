using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.RoslynHost;

public interface IRoslynHost
{
    Task<WorkspaceSnapshot> LoadWorkspaceAsync(WorkspaceRequest request, CancellationToken cancellationToken);
    Task<DocumentAnalysis> AnalyzeDocumentAsync(DocumentRequest request, CancellationToken cancellationToken);
    Task<RoslynCompletionResult> GetCompletionAsync(DocumentRequest request, int line, int character, CancellationToken cancellationToken);
}

public sealed record RoslynCompletionCandidate(string Label, string Kind, int LspKind, string Detail,
    string Documentation, string SymbolId, int EditStart, int EditLength, double IntentScore = 0,
    ImmutableArray<string> Signals = default, string? AssemblyName = null, double Confidence = 0.35,
    string? RequiredImport = null);
public sealed record RoslynCompletionResult(ImmutableArray<RoslynCompletionCandidate> Candidates,
    ImmutableArray<string> Diagnostics, string? ExpectedType = null);
public sealed record RoslynSymbolInfo(string Name, string Detail, string Documentation, string AssemblyName,
    string? PackageId, string? TargetPath, int StartLine, int StartCharacter, int EndLine, int EndCharacter);

public sealed class RoslynHost : IRoslynHost
{
    public ProjectContextSnapshot? Context { get; init; }
    public Task<WorkspaceSnapshot> LoadWorkspaceAsync(WorkspaceRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var inventory = WorkspaceInventoryBuilder.Build(request.RootPath, cancellationToken);
        var projects = request.ProjectPaths.Select(path =>
        {
            var projectRoot = Path.GetDirectoryName(Path.GetFullPath(path)) ?? request.RootPath;
            var documents = inventory.Files.Where(file => file.Kind == "document"
                && Path.GetFullPath(Path.Combine(inventory.RootPath, file.Path)).StartsWith(projectRoot, StringComparison.OrdinalIgnoreCase))
                .Select(file => Path.Combine(inventory.RootPath, file.Path)).ToArray();
            return new ProjectSnapshot(new ProjectId(path), Path.GetFileNameWithoutExtension(path),
                new TargetFrameworkId("unknown"), documents);
        }).ToArray();
        return Task.FromResult(new WorkspaceSnapshot(request.WorkspaceId, projects, DateTimeOffset.UtcNow));
    }

    public Task<DocumentAnalysis> AnalyzeDocumentAsync(DocumentRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(new DocumentAnalysis(request, Array.Empty<SymbolFact>(), Array.Empty<OperationFact>()));
    }

    public Task<RoslynCompletionResult> GetCompletionAsync(DocumentRequest request, int line, int character, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var offset = OffsetAt(request.Text, line, character);
        if (offset < 0)
            return Task.FromResult(new RoslynCompletionResult(ImmutableArray<RoslynCompletionCandidate>.Empty,
                ImmutableArray.Create("Invalid document position.")));

        var (compilation, tree, model) = CreateSemanticModel(request, cancellationToken);
        var prefix = CurrentPrefix(request.Text, offset);
        var editStart = offset - prefix.Length;
        var expectedType = FindExpectedType(model, tree.GetRoot(cancellationToken), offset);
        var ranked = model.LookupSymbols(offset)
            .Where(symbol => symbol.CanBeReferencedByName && symbol.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            .GroupBy(symbol => $"{symbol.ContainingAssembly?.Identity}:{symbol.GetDocumentationCommentId() ?? symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}:{editStart}:{prefix.Length}", StringComparer.Ordinal)
            .Select(group => group.OrderBy(symbol => symbol.Kind).First())
            .Where(symbol => expectedType is null || IsCompatible(symbol, expectedType, compilation))
            .Select(symbol => (Symbol: symbol, Intent: IntentSignals.Score(symbol, model, tree.GetRoot(cancellationToken), request.Text, offset, prefix, Context?.Patterns)))
            .OrderByDescending(item => item.Intent.Score)
            .ThenBy(item => item.Symbol.Name, StringComparer.OrdinalIgnoreCase)
            .Take(100)
            .ToArray();
        var diagnostics = compilation.GetDiagnostics(cancellationToken)
            .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
            .Select(diagnostic => diagnostic.Id).Distinct(StringComparer.Ordinal).ToImmutableArray();
        var baseCandidates = ranked.Select(item => new RoslynCompletionCandidate(item.Symbol.Name, item.Symbol.Kind.ToString(), ToLspKind(item.Symbol.Kind),
                item.Symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
                item.Symbol.GetDocumentationCommentXml() ?? string.Empty,
                item.Symbol.GetDocumentationCommentId() ?? item.Symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat), editStart, prefix.Length,
                item.Intent.Score, item.Intent.Signals, item.Symbol.ContainingAssembly?.Identity.Name,
                ConfidenceCalibration.Calibrate(item.Intent.Score, Context?.IsComplete == true,
                    expectedType is not null, !diagnostics.IsEmpty)))
            .ToImmutableArray();
        var importCandidates = BuildImportAwareCandidates(compilation, tree.GetCompilationUnitRoot(cancellationToken), request.Text,
            offset, prefix, editStart, cancellationToken);
        var candidates = baseCandidates.Concat(importCandidates)
            .GroupBy(candidate => $"{candidate.SymbolId}:{candidate.EditStart}:{candidate.EditLength}", StringComparer.Ordinal)
            .Select(group => group.OrderByDescending(candidate => candidate.IntentScore).First())
            .OrderByDescending(candidate => candidate.IntentScore)
            .ThenBy(candidate => candidate.Label, StringComparer.OrdinalIgnoreCase)
            .Take(100)
            .ToImmutableArray();
        return Task.FromResult(new RoslynCompletionResult(candidates, diagnostics, expectedType?.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
    }

    public Task<RoslynSymbolInfo?> GetSymbolInfoAsync(DocumentRequest request, int line, int character, CancellationToken cancellationToken)
    {
        var offset = OffsetAt(request.Text, line, character);
        if (offset < 0) return Task.FromResult<RoslynSymbolInfo?>(null);
        var (compilation, tree, model) = CreateSemanticModel(request, cancellationToken);
        var token = tree.GetRoot(cancellationToken).FindToken(Math.Max(0, offset - 1));
        if (token.Parent is null) return Task.FromResult<RoslynSymbolInfo?>(null);
        var symbol = model.GetSymbolInfo(token.Parent, cancellationToken).Symbol ?? model.GetDeclaredSymbol(token.Parent, cancellationToken);
        if (symbol is null) return Task.FromResult<RoslynSymbolInfo?>(null);
        var location = symbol.Locations.FirstOrDefault(candidate => candidate.IsInSource);
        var targetPath = location?.SourceTree?.FilePath;
        var startLine = 0;
        var startCharacter = 0;
        var endLine = 0;
        var endCharacter = 0;
        if (location is not null)
        {
            var span = location.SourceTree!.GetText(cancellationToken).Lines.GetLinePositionSpan(location.SourceSpan);
            startLine = span.Start.Line; startCharacter = span.Start.Character;
            endLine = span.End.Line; endCharacter = span.End.Character;
        }
        var assemblyName = symbol.ContainingAssembly?.Identity.Name ?? "source";
        var package = Context?.NuGetApi?.Packages.FirstOrDefault(item => item.Metadata.Name == assemblyName);
        var target = targetPath ?? package?.AssemblyPath;
        return Task.FromResult<RoslynSymbolInfo?>(new(symbol.Name,
            symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
            symbol.GetDocumentationCommentXml() ?? string.Empty, assemblyName, package?.PackageId, target,
            startLine, startCharacter, endLine, endCharacter));
    }

    private (CSharpCompilation Compilation, SyntaxTree Tree, SemanticModel Model) CreateSemanticModel(
        DocumentRequest request, CancellationToken cancellationToken)
    {
        var language = LanguageVersionFacts.TryParse(Context?.LanguageVersion ?? "default", out var parsed)
            ? parsed : Microsoft.CodeAnalysis.CSharp.LanguageVersion.Default;
        var options = new CSharpParseOptions(language, preprocessorSymbols: Context?.Defines ?? []);
        var tree = CSharpSyntaxTree.ParseText(request.Text, options, cancellationToken: cancellationToken);
        var trees = new List<SyntaxTree> { tree };
        if (Context?.SourceFiles.IsDefault == false)
            foreach (var sourceFile in Context.SourceFiles.Where(File.Exists))
                trees.Add(RoslynSyntaxTreeCache.Get(sourceFile, File.ReadAllText(sourceFile), options, cancellationToken));
        var compilation = CSharpCompilation.Create(request.ProjectId.Value, trees, References,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, usings: Context?.Imports ?? []));
        return (compilation, tree, compilation.GetSemanticModel(tree));
    }

    private static ImmutableArray<RoslynCompletionCandidate> BuildImportAwareCandidates(
        CSharpCompilation compilation, CompilationUnitSyntax root, string source, int offset, string prefix,
        int editStart, CancellationToken cancellationToken)
    {
        var builder = ImmutableArray.CreateBuilder<RoslynCompletionCandidate>();
        var before = source[..offset];
        var memberMatch = Regex.Match(before, @"(?<receiver>[A-Za-z_]\w*)\.(?<member>[A-Za-z_]\w*)?$");
        if (memberMatch.Success)
        {
            var receiver = memberMatch.Groups["receiver"].Value;
            var type = compilation.GetTypeByMetadataName($"System.{receiver}");
            if (type is not null)
            {
                var import = MissingImport(root, type.ContainingNamespace?.ToDisplayString());
                foreach (var member in type.GetMembers().Where(member => member.CanBeReferencedByName
                             && member.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    builder.Add(new RoslynCompletionCandidate(member.Name, member.Kind.ToString(), ToLspKind(member.Kind),
                        member.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat), member.GetDocumentationCommentXml() ?? string.Empty,
                        member.GetDocumentationCommentId() ?? member.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat), editStart, prefix.Length, 0.65,
                        ImmutableArray.Create("unimported-type", "required-import"), type.ContainingAssembly?.Identity.Name,
                        0.55, import));
                }
            }
            return builder.ToImmutable();
        }

        var system = compilation.GlobalNamespace.GetNamespaceMembers().FirstOrDefault(namespaceSymbol => namespaceSymbol.Name == "System");
        if (system is not null && prefix.Length >= 2)
        {
            var import = MissingImport(root, "System");
            foreach (var type in system.GetTypeMembers().Where(type => type.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
                builder.Add(new RoslynCompletionCandidate(type.Name, type.Kind.ToString(), ToLspKind(type.Kind),
                    type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat), type.GetDocumentationCommentXml() ?? string.Empty,
                    type.GetDocumentationCommentId() ?? type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat), editStart, prefix.Length, 0.55,
                    ImmutableArray.Create("unimported-type", "required-import"), type.ContainingAssembly?.Identity.Name,
                    0.5, import));
        }
        return builder.ToImmutable();
    }

    private static string? MissingImport(CompilationUnitSyntax root, string? namespaceName)
    {
        if (string.IsNullOrWhiteSpace(namespaceName) || root.Usings.Any(usingDirective => usingDirective.Name?.ToString() == namespaceName))
            return null;
        return namespaceName;
    }

    private static readonly ImmutableArray<MetadataReference> FrameworkReferences =
    [
        MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
        MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
        MetadataReference.CreateFromFile(typeof(Task).Assembly.Location)
    ];

    private ImmutableArray<MetadataReference> References => FrameworkReferences.AddRange(
        (Context?.Assemblies.IsDefault == false ? Context.Assemblies.Select(assembly => assembly.Path) : [])
            .Concat(Context?.NuGetApi?.Packages.Select(package => package.AssemblyPath) ?? [])
            .Where(File.Exists).Distinct(StringComparer.OrdinalIgnoreCase)
            .Select(path => MetadataReference.CreateFromFile(path)).Cast<MetadataReference>());

    private static string CurrentPrefix(string text, int offset)
    {
        var start = offset;
        while (start > 0 && (char.IsLetterOrDigit(text[start - 1]) || text[start - 1] == '_')) start--;
        return text[start..offset];
    }

    private static ITypeSymbol? FindExpectedType(SemanticModel model, Microsoft.CodeAnalysis.SyntaxNode root, int offset)
    {
        var probe = Math.Max(0, Math.Min(offset, root.FullSpan.End) - 1);
        var node = root.FindToken(probe).Parent;
        for (var current = node; current is not null; current = current.Parent)
        {
            if (current is ReturnStatementSyntax)
                return (model.GetEnclosingSymbol(offset) as IMethodSymbol)?.ReturnType;
            if (current is AssignmentExpressionSyntax assignment)
                return SymbolType(model.GetSymbolInfo(assignment.Left).Symbol) ?? model.GetTypeInfo(assignment.Left).Type;
            if (current is EqualsValueClauseSyntax equalsValue)
            {
                if (equalsValue.Parent is VariableDeclaratorSyntax { Parent: VariableDeclarationSyntax declaration })
                    return declaration.Type.IsVar ? null : model.GetTypeInfo(declaration.Type).Type;
                if (equalsValue.Parent is PropertyDeclarationSyntax property)
                    return model.GetTypeInfo(property.Type).Type;
            }
            if (current is ArgumentSyntax argument)
            {
                var invocation = argument.Ancestors().OfType<InvocationExpressionSyntax>().FirstOrDefault();
                if (invocation is not null)
                {
                    var info = model.GetSymbolInfo(invocation.Expression);
                    var method = info.Symbol as IMethodSymbol ?? info.CandidateSymbols.OfType<IMethodSymbol>().FirstOrDefault();
                    if (method is null) continue;
                    var index = invocation.ArgumentList.Arguments.IndexOf(argument);
                    if (index >= 0 && index < method.Parameters.Length) return method.Parameters[index].Type;
                }
            }
        }
        return null;
    }

    private static bool IsCompatible(ISymbol symbol, ITypeSymbol expected, Compilation compilation)
    {
        var actual = SymbolType(symbol);
        return actual is not null && compilation.ClassifyConversion(actual, expected).IsImplicit;
    }

    private static ITypeSymbol? SymbolType(ISymbol? symbol) => symbol switch
    {
        ITypeSymbol type => type,
        IMethodSymbol method => method.ReturnsVoid ? null : method.ReturnType,
        IPropertySymbol property => property.Type,
        IFieldSymbol field => field.Type,
        IEventSymbol @event => @event.Type,
        ILocalSymbol local => local.Type,
        IParameterSymbol parameter => parameter.Type,
        _ => null
    };

    private static int ToLspKind(Microsoft.CodeAnalysis.SymbolKind kind) => kind switch
    {
        Microsoft.CodeAnalysis.SymbolKind.Namespace => 9,
        Microsoft.CodeAnalysis.SymbolKind.NamedType => 7,
        Microsoft.CodeAnalysis.SymbolKind.Method => 2,
        Microsoft.CodeAnalysis.SymbolKind.Property => 10,
        Microsoft.CodeAnalysis.SymbolKind.Field => 5,
        Microsoft.CodeAnalysis.SymbolKind.Event => EventKind,
        Microsoft.CodeAnalysis.SymbolKind.Parameter => 6,
        Microsoft.CodeAnalysis.SymbolKind.Local => 6,
        Microsoft.CodeAnalysis.SymbolKind.TypeParameter => 25,
        _ => 1
    };

    private const int EventKind = 24;

    private static int OffsetAt(string text, int line, int character)
    {
        if (line < 0 || character < 0) return -1;
        var offset = 0;
        for (var currentLine = 0; currentLine < line; currentLine++)
        {
            var newline = text.IndexOf('\n', offset);
            if (newline < 0) return -1;
            offset = newline + 1;
        }
        var lineEnd = text.IndexOf('\n', offset);
        if (lineEnd < 0) lineEnd = text.Length;
        return offset + Math.Min(character, lineEnd - offset);
    }
}
