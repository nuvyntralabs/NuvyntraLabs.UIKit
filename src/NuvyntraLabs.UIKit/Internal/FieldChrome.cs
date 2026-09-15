namespace NuvyntraLabs.UIKit;

static class FieldChrome
{
    public static Border Box(View inner)
    {
        var border = new Border
        {
            Padding = new Thickness(NVTokens.Space3, NVTokens.Space2),
            StrokeThickness = 1,
            Content = inner
        };
        Paint(border, error: false);
        return border;
    }

    public static void Paint(Border border, bool error)
    {
        var theme = NVTheme.Current;
        border.BackgroundColor = theme.Surface;
        border.Stroke = error ? theme.Danger : theme.Fog;
        border.StrokeShape = new RoundRectangle { CornerRadius = NVTokens.RadiusSmall };
    }

    public static VerticalStackLayout Stack(Label label, View field, Label hint) =>
        new()
        {
            Spacing = NVTokens.Space1,
            Children = { label, field, hint }
        };

    public static Label Caption() =>
        new()
        {
            FontFamily = NVTokens.FontRegular,
            FontSize = NVTokens.CaptionSize
        };

    public static Label Title() =>
        new()
        {
            FontFamily = NVTokens.FontSemiBold,
            FontSize = NVTokens.LabelSize
        };
}
