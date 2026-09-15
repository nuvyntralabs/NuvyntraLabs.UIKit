namespace NuvyntraLabs.UIKit.Sample.Pages;

public sealed class ThemePage : CatalogSectionPage
{
    public ThemePage() : base("01  Theme",
        "Foundation first. NV-FND-01 … NV-FND-08. Mode applies to every later page.",
        Build) { }

    static IEnumerable<View> Build()
    {
        var light = new NVButton { Text = "Light", Variant = NVButtonVariant.Outline };
        var dark = new NVButton { Text = "Dark", Variant = NVButtonVariant.Outline };
        var system = new NVButton { Text = "System", Variant = NVButtonVariant.Tonal };
        light.Command = new Command(() => NVTheme.Current.SetMode(NVThemeMode.Light));
        dark.Command = new Command(() => NVTheme.Current.SetMode(NVThemeMode.Dark));
        system.Command = new Command(() => NVTheme.Current.SetMode(NVThemeMode.System));

        yield return Gallery.Sample(1, "NVTheme", "NV-FND-01  ·  Light / dark / system",
            new HorizontalStackLayout { Spacing = NVTokens.Space2, Children = { light, dark, system } });
        yield return Gallery.Sample(2, "NVTokens", "NV-FND-02  ·  Paper, surface, accent, ink, danger", SwatchRow());
        yield return Gallery.Sample(3, "NVTypography", "NV-FND-03  ·  Display / title / body / caption",
            new VerticalStackLayout
            {
                Spacing = NVTokens.Space1,
                Children =
                {
                    new NVHeading { Text = "Display", Role = NVTextRole.Display },
                    new NVHeading { Text = "Title", Role = NVTextRole.Title },
                    new NVBodyText { Text = "Body" },
                    new NVCaptionText { Text = "Caption" }
                }
            });
        yield return Gallery.Sample(4, "NVIcons", "NV-FND-04  ·  Stroke glyphs",
            new HorizontalStackLayout
            {
                Spacing = NVTokens.Space3,
                Children =
                {
                    new NVIcon { Kind = NVIconKind.Home },
                    new NVIcon { Kind = NVIconKind.Search },
                    new NVIcon { Kind = NVIconKind.Settings },
                    new NVIcon { Kind = NVIconKind.Star }
                }
            });
        yield return Gallery.Sample(5, "NVMotion", "NV-FND-05  ·  Fast / normal / slow; honor reduce-motion",
            new NVCaptionText { Text = $"Normal duration {NVMotion.Normal} ms" });
        yield return Gallery.Sample(6, "NVDensity", "NV-FND-06  ·  Comfortable default",
            new NVCaptionText { Text = NVTheme.Current.Density.ToString() });
        yield return Gallery.Sample(7, "NVVisualState", "NV-FND-07  ·  Rest / press / focus / disabled / error",
            new NVCaptionText { Text = string.Join(" · ", new[] { NVVisualState.Rest, NVVisualState.Press, NVVisualState.Error }) });
        yield return Gallery.Sample(8, "NVAccessibility", "NV-FND-08  ·  Body contrast on paper",
            new NVCaptionText { Text = NVAccessibility.BodyContrastOk(NVTheme.Current.Ink, NVTheme.Current.Paper) ? "Pass ≥ 4.5:1" : "Fail" });
    }

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
