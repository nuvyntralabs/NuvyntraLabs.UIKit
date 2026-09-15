namespace NuvyntraLabs.UIKit;

/// <summary>Duration helpers that honor reduced motion.</summary>
public static class NVMotion
{
    public static bool ReduceMotion { get; set; }

    public static uint Fast => NVTokens.Motion(NVTokens.MotionFast, ReduceMotion);
    public static uint Normal => NVTokens.Motion(NVTokens.MotionNormal, ReduceMotion);
    public static uint Slow => NVTokens.Motion(NVTokens.MotionSlow, ReduceMotion);
}
