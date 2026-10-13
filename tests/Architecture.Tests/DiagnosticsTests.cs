using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class DiagnosticsTests
{
    public static void Run()
    {
        var diagnostic = MetadataEditorFeatures.Diagnostic(2, 3, SuggestionSeverity.Warning, "Unknown member", "CS0117");
        var action = MetadataEditorFeatures.CodeAction("Rename member", "quickfix", [new TextEdit(2, 3, "Name")]);
        var rename = MetadataEditorFeatures.Rename("var value = value; // value", "value", "result");
        if (diagnostic.Code != "CS0117" || action.Edits.Length != 1 || rename.Edits.Length != 3 ||
            rename.Edits[0].NewText != "result")
            throw new InvalidOperationException("Diagnostics, code action or rename edits were incorrect.");
        try { MetadataEditorFeatures.Rename("value", "value", "bad-name"); throw new InvalidOperationException("Invalid rename accepted."); }
        catch (ArgumentException) { }
    }
}
