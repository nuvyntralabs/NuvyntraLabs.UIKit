namespace NuvyntraLabs.UIKit.Sample.Pages;

public sealed class ThemePage : ContentPage
{
    public ThemePage()
    {
        Title = "Theme";
        var light = new NVButton { Text = "Light", Variant = NVButtonVariant.Outline };
        var dark = new NVButton { Text = "Dark", Variant = NVButtonVariant.Outline };
        var system = new NVButton { Text = "System", Variant = NVButtonVariant.Tonal };
        light.Command = new Command(() => NVTheme.Current.SetMode(NVThemeMode.Light));
        dark.Command = new Command(() => NVTheme.Current.SetMode(NVThemeMode.Dark));
        system.Command = new Command(() => NVTheme.Current.SetMode(NVThemeMode.System));

        Content = Wrap(new VerticalStackLayout
        {
            Spacing = NVTokens.Space4,
            Children =
            {
                Heading("Lumina"),
                Body("Warm paper, aurora accent. Not Material, Fluent, or a vendor default."),
                new HorizontalStackLayout
                {
                    Spacing = NVTokens.Space2,
                    Children = { light, dark, system }
                },
                SwatchRow()
            }
        });

        NVTheme.Current.Changed += (_, _) => ApplyPage();
        ApplyPage();
    }

    void ApplyPage()
    {
        BackgroundColor = NVTheme.Current.Paper;
    }

    static View Wrap(View inner) => new ScrollView
    {
        Padding = NVTokens.Space5,
        Content = inner
    };

    static Label Heading(string text) => new()
    {
        Text = text,
        FontFamily = NVTokens.FontSemiBold,
        FontSize = NVTokens.TitleSize,
        TextColor = NVTheme.Current.Ink
    };

    static Label Body(string text) => new()
    {
        Text = text,
        FontFamily = NVTokens.FontRegular,
        FontSize = NVTokens.BodySize,
        TextColor = NVTheme.Current.Muted
    };

    static View SwatchRow()
    {
        var theme = NVTheme.Current;
        return new HorizontalStackLayout
        {
            Spacing = NVTokens.Space2,
            Children =
            {
                Swatch(theme.Paper),
                Swatch(theme.Surface),
                Swatch(theme.Accent),
                Swatch(theme.Ink),
                Swatch(theme.Danger)
            }
        };
    }

    static View Swatch(Color color) => new BoxView
    {
        Color = color,
        WidthRequest = 36,
        HeightRequest = 36,
        CornerRadius = 8
    };
}
