namespace NuvyntraLabs.UIKit;

/// <summary>MAUI host registration for NuvyntraLabs.UIKit.</summary>
public static class MauiAppBuilderExtensions
{
    /// <summary>
    /// Registers Lumina fonts and applies the default theme.
    /// Call from <c>MauiProgram</c> after <c>UseMauiApp</c>.
    /// </summary>
    public static MauiAppBuilder UseNuvyntraUIKit(this MauiAppBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ConfigureFonts(fonts =>
        {
            fonts.AddFont("Outfit-Regular.ttf", NVTokens.FontRegular);
            fonts.AddFont("Outfit-SemiBold.ttf", NVTokens.FontSemiBold);
        });

        NVTheme.Current.UseLumina();
        return builder;
    }
}
