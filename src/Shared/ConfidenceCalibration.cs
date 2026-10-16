namespace DotnetExpertAdvisor.Shared;

public static class ConfidenceCalibration
{
    public static double Calibrate(double intentScore, bool contextComplete,
        bool expectedTypeKnown, bool hasDiagnostics)
    {
        var confidence = 0.35 + Math.Min(0.35, Math.Max(0, intentScore) * 0.25);
        if (expectedTypeKnown) confidence += 0.15;
        if (contextComplete) confidence += 0.15;
        if (hasDiagnostics) confidence = Math.Min(confidence, 0.60);
        if (!contextComplete) confidence = Math.Min(confidence, 0.70);
        if (!expectedTypeKnown) confidence = Math.Min(confidence, 0.78);
        return Math.Clamp(confidence, 0, 1);
    }
}
