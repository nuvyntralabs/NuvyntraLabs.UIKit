namespace NuvyntraLabs.UIKit.Tests;

public class NVFoundationTests
{
    [Fact]
    public void Icon_glyphs_are_unique_except_none()
    {
        var glyphs = Enum.GetValues<NVIconKind>()
            .Where(k => k != NVIconKind.None)
            .Select(NVIcons.Glyph)
            .ToList();
        Assert.Equal(glyphs.Count, glyphs.Distinct().Count());
        Assert.Equal("", NVIcons.Glyph(NVIconKind.None));
    }

    [Fact]
    public void Reduce_motion_zeroes_durations()
    {
        NVMotion.ReduceMotion = true;
        try
        {
            Assert.Equal(0u, NVMotion.Fast);
            Assert.Equal(0u, NVMotion.Normal);
        }
        finally
        {
            NVMotion.ReduceMotion = false;
        }

        Assert.Equal(NVTokens.MotionFast, NVMotion.Fast);
    }

    [Fact]
    public void Contrast_helpers_pass_lumina_body_on_paper()
    {
        NVTheme.Current.UseLumina();
        NVTheme.Current.SetMode(NVThemeMode.Light);
        Assert.True(NVAccessibility.BodyContrastOk(NVTheme.Current.Ink, NVTheme.Current.Paper));
        NVTheme.Current.SetMode(NVThemeMode.Dark);
        Assert.True(NVAccessibility.BodyContrastOk(NVTheme.Current.Ink, NVTheme.Current.Paper));
        NVTheme.Current.UseLumina();
    }

    [Fact]
    public void Visual_state_names_match_the_enum()
    {
        Assert.Equal(nameof(NVVisualStateKind.Rest), NVVisualState.Rest);
        Assert.Equal(nameof(NVVisualStateKind.Error), NVVisualState.Error);
    }

    [Fact]
    public void Typography_roles_are_distinct_sizes()
    {
        Assert.True(NVTokens.DisplaySize > NVTokens.TitleSize);
        Assert.True(NVTokens.TitleSize > NVTokens.BodySize);
        Assert.True(NVTokens.BodySize > NVTokens.CaptionSize);
    }
}
