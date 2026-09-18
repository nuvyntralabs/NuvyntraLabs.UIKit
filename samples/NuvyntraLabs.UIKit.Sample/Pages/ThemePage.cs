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
        var compact = new NVButton { Text = "Compact", Variant = NVButtonVariant.Outline };
        var cozy = new NVButton { Text = "Comfortable", Variant = NVButtonVariant.Tonal };
        var roomy = new NVButton { Text = "Spacious", Variant = NVButtonVariant.Outline };
        compact.Command = new Command(() => NVTheme.Current.Density = NVDensity.Compact);
        cozy.Command = new Command(() => NVTheme.Current.Density = NVDensity.Comfortable);
        roomy.Command = new Command(() => NVTheme.Current.Density = NVDensity.Spacious);
        yield return Gallery.Sample(5, "NVMotion", "NV-FND-05  ·  Fast / normal / slow; honor reduce-motion",
            new NVStepProgressBar { Steps = new List<string> { "Fast 180", "Normal 280", "Slow 420" }, Index = 1 });
        yield return Gallery.Sample(6, "NVDensity", "NV-FND-06  ·  Compact / comfortable / spacious",
            new HorizontalStackLayout { Spacing = NVTokens.Space2, Children = { compact, cozy, roomy } });
        yield return Gallery.Sample(7, "NVVisualState", "NV-FND-07  ·  Rest / press / disabled",
            new HorizontalStackLayout
            {
                Spacing = NVTokens.Space2,
                Children =
                {
                    new NVButton { Text = "Rest", Variant = NVButtonVariant.Filled },
                    new NVButton { Text = "Disabled", Variant = NVButtonVariant.Filled, IsEnabled = false },
                    new NVTextField { Label = "Error", Error = "Required" }
                }
            });
        yield return Gallery.Sample(8, "NVAccessibility", "NV-FND-08  ·  Body contrast on paper / muted / danger / warn / ok",
            new NVCaptionText
            {
                Text = string.Join("  ·  ",
                    ContrastLine("ink", NVTheme.Current.Ink),
                    ContrastLine("muted", NVTheme.Current.Muted),
                    ContrastLine("danger", NVTheme.Current.Danger),
                    ContrastLine("warn", NVTheme.Current.Warn),
                    ContrastLine("ok", NVTheme.Current.Ok),
                    ContrastLine("onAccent", NVTheme.Current.OnAccent, NVTheme.Current.Accent))
            });
        var scale80 = new NVButton { Text = "80%", Variant = NVButtonVariant.Outline };
        var scale100 = new NVButton { Text = "100%", Variant = NVButtonVariant.Tonal };
        var scale200 = new NVButton { Text = "200%", Variant = NVButtonVariant.Outline };
        scale80.Command = new Command(() => NVTheme.Current.SetTypeScale(0.8));
        scale100.Command = new Command(() => NVTheme.Current.SetTypeScale(1));
        scale200.Command = new Command(() => NVTheme.Current.SetTypeScale(2));
        yield return Gallery.Sample(9, "NVTokens", "TypeScale 80–200%. Toolbar Type cycles the same values.",
            new HorizontalStackLayout { Spacing = NVTokens.Space2, Children = { scale80, scale100, scale200 } });
        var ltr = new NVButton { Text = "LTR", Variant = NVButtonVariant.Outline };
        var rtl = new NVButton { Text = "RTL", Variant = NVButtonVariant.Outline };
        ltr.Command = new Command(() => NVTheme.Current.SetFlowDirection(FlowDirection.LeftToRight));
        rtl.Command = new Command(() => NVTheme.Current.SetFlowDirection(FlowDirection.RightToLeft));
        yield return Gallery.Sample(10, "NVTheme", "FlowDirection. Default MatchParent — playground hosts stay LTR until they set this.",
            new HorizontalStackLayout { Spacing = NVTokens.Space2, Children = { ltr, rtl } });
        yield return Gallery.Sample(11, "NVCommandPalette", "Windows / Catalyst: host calls TryHandleShortcut(\"Control+K\") or Escape.",
            new NVCaptionText { Text = "OverlayHost.TryHandleKey / NVCommandPalette.TryHandleShortcut — no new catalog type." });
    }

    static string ContrastLine(string name, Color text, Color? background = null) =>
        $"{name} {(NVAccessibility.BodyContrastOk(text, background ?? NVTheme.Current.Paper) ? "pass" : "fail")}";

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
