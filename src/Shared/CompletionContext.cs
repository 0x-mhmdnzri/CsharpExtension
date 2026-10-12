namespace DotnetExpertAdvisor.Shared;

public sealed record CompletionContext(string Prefix, int Start, int End, char? TriggerCharacter,
    bool IsMemberAccess);

public static class CompletionContextAnalyzer
{
    public static CompletionContext Analyze(string text, int cursorOffset)
    {
        ArgumentNullException.ThrowIfNull(text);
        if ((uint)cursorOffset > (uint)text.Length) throw new ArgumentOutOfRangeException(nameof(cursorOffset));
        var start = cursorOffset;
        while (start > 0 && (char.IsLetterOrDigit(text[start - 1]) || text[start - 1] == '_')) start--;
        var prefix = text[start..cursorOffset];
        char? trigger = start > 0 ? text[start - 1] : null;
        return new CompletionContext(prefix, start, cursorOffset, trigger, trigger == '.');
    }
}
