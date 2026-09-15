namespace NuvyntraLabs.UIKit;

/// <summary>Lumina type roles. Controls should call these instead of hard-coded sizes.</summary>
public static class NVTypography
{
    public static Label Display(string text) => Make(text, NVTokens.DisplaySize, NVTokens.FontSemiBold);
    public static Label Title(string text) => Make(text, NVTokens.TitleSize, NVTokens.FontSemiBold);
    public static Label Body(string text) => Make(text, NVTokens.BodySize, NVTokens.FontRegular);
    public static Label Label(string text) => Make(text, NVTokens.LabelSize, NVTokens.FontSemiBold);
    public static Label Caption(string text) => Make(text, NVTokens.CaptionSize, NVTokens.FontRegular);
    public static Label Mono(string text) => Make(text, NVTokens.BodySize, NVTokens.FontRegular);

    static Label Make(string text, double size, string font) =>
        new()
        {
            Text = text,
            FontSize = size,
            FontFamily = font,
            TextColor = NVTheme.Current.Ink
        };
}
