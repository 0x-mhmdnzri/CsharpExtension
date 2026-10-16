using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class SuggestionReplayTests
{
    public static void Run()
    {
        const string source = "class Customer {} class Other {}";
        var replay = SuggestionReplayHarness.Run(
            [new SuggestionReplayCase("customer-declaration", source, 6, "Customer")],
            (text, cursor) =>
            {
                var start = cursor;
                while (start > 0 && char.IsLetter(text[start - 1])) start--;
                var prefix = text[start..cursor];
                return prefix.Length == 0 ? ["Other", "Customer"] : ["Customer"];
            },
            (_, selected) => selected == "Customer");

        if (replay.Observations.Length != 4 || replay.Top1Accuracy != .75 || replay.Top5Accuracy != 1
            || replay.KeystrokesSaved != 4.5 || replay.CompileValidRate != .75
            || replay.FalsePositiveRate != 1d / 4)
            throw new InvalidOperationException($"Replay metrics were incorrect: {replay}.");
        if (replay.Observations.Any(item => item.Rank < 0 || item.LatencyMilliseconds < 0))
            throw new InvalidOperationException("Replay observations contained invalid rank or latency.");
    }
}
