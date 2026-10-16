using DotnetExpertAdvisor.Shared;

namespace DotnetExpertAdvisor.Architecture.Tests;

public static class ConfidenceCalibrationTests
{
    public static void Run()
    {
        var high = ConfidenceCalibration.Calibrate(1.4, contextComplete: true, expectedTypeKnown: true, hasDiagnostics: false);
        var incomplete = ConfidenceCalibration.Calibrate(1.4, contextComplete: false, expectedTypeKnown: true, hasDiagnostics: false);
        var diagnostic = ConfidenceCalibration.Calibrate(1.4, contextComplete: true, expectedTypeKnown: true, hasDiagnostics: true);
        var unknown = ConfidenceCalibration.Calibrate(1.4, contextComplete: true, expectedTypeKnown: false, hasDiagnostics: false);
        if (high < .85 || incomplete >= .85 || diagnostic >= .85 || unknown >= .85)
            throw new InvalidOperationException("Confidence calibration did not enforce conservative preselection thresholds.");
    }
}
