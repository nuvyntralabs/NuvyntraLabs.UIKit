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

    public static uint Motion(uint milliseconds, bool reduceMotion) =>
        reduceMotion ? 0 : milliseconds;
}
