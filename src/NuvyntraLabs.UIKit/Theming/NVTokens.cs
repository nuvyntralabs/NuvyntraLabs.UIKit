namespace NuvyntraLabs.UIKit;

/// <summary>Lumina spacing, radius, type, and motion. Colors live on <see cref="NVTheme"/>.</summary>
public static class NVTokens
{
    public const double Space1 = 4;
    public const double Space2 = 8;
    public const double Space3 = 12;
    public const double Space4 = 16;
    public const double Space5 = 24;
    public const double Space6 = 32;

    public const double RadiusSmall = 10;
    public const double RadiusMedium = 14;
    public const double RadiusLarge = 20;

    public const double DisplaySize = 32;
    public const double TitleSize = 22;
    public const double BodySize = 16;
    public const double LabelSize = 13;
    public const double CaptionSize = 12;

    public const uint MotionFast = 180;
    public const uint MotionNormal = 280;
    public const uint MotionSlow = 420;

    public const string FontRegular = "OutfitRegular";
    public const string FontSemiBold = "OutfitSemiBold";

    public static double Space(double baseSpace, NVDensity density) =>
        density switch
        {
            NVDensity.Compact => baseSpace * 0.75,
            NVDensity.Spacious => baseSpace * 1.25,
            _ => baseSpace
        };

    /// <summary>Type role size after <see cref="NVTheme.TypeScale"/> (clamped 80–200%).</summary>
    public static double Type(double baseSize) =>
        baseSize * NVTheme.Current.TypeScale;

    /// <summary>Minimum tap target. Never shrinks below 44 when type scale is 80%.</summary>
    public static double MinTap => Math.Max(44, Type(44));

    public static bool IsRtl => NVTheme.Current.IsRtl;

    /// <summary>Start-edge padding that flips under RTL.</summary>
    public static Thickness StartPad(double start, double top = 0, double end = 0, double bottom = 0) =>
        IsRtl ? new Thickness(end, top, start, bottom) : new Thickness(start, top, end, bottom);

    /// <summary>Swipe that advances a carousel (toward start in the reading direction).</summary>
    public static SwipeDirection AdvanceSwipe =>
        IsRtl ? SwipeDirection.Right : SwipeDirection.Left;

    public static uint Motion(uint milliseconds, bool reduceMotion) =>
        reduceMotion ? 0 : milliseconds;
}
