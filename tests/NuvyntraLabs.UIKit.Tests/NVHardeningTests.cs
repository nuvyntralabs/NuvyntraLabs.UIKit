namespace NuvyntraLabs.UIKit.Tests;

[Collection("UIKit")]
public class NVHardeningTests : IDisposable
{
    public NVHardeningTests() => NVTheme.Current.UseLumina();

    public void Dispose() => NVTheme.Current.UseLumina();

    [Fact]
    public void Type_scale_clamps_and_scales_tokens()
    {
        NVTheme.Current.SetTypeScale(0.5);
        Assert.Equal(0.8, NVTheme.Current.TypeScale);
        Assert.Equal(NVTokens.BodySize * 0.8, NVTokens.Type(NVTokens.BodySize));
        Assert.Equal(44, NVTokens.MinTap);

        NVTheme.Current.SetTypeScale(4);
        Assert.Equal(2, NVTheme.Current.TypeScale);
        Assert.Equal(NVTokens.BodySize * 2, NVTokens.Type(NVTokens.BodySize));
        Assert.Equal(88, NVTokens.MinTap);
    }

    [Fact]
    public void Heading_font_size_follows_type_scale()
    {
        var heading = new NVHeading { Text = "Title", Role = NVTextRole.Title };
        Assert.Equal(NVTokens.TitleSize, heading.Content is Label label ? label.FontSize : -1);
        NVTheme.Current.SetTypeScale(2);
        Assert.Equal(NVTokens.TitleSize * 2, heading.Content is Label scaled ? scaled.FontSize : -1);
    }

    [Theory]
    [InlineData(NVThemeMode.Light)]
    [InlineData(NVThemeMode.Dark)]
    public void Semantic_text_on_paper_meets_contrast(NVThemeMode mode)
    {
        NVTheme.Current.SetMode(mode);
        var paper = NVTheme.Current.Paper;
        Assert.True(NVAccessibility.BodyContrastOk(NVTheme.Current.Ink, paper));
        Assert.True(NVAccessibility.BodyContrastOk(NVTheme.Current.Muted, paper));
        Assert.True(NVAccessibility.BodyContrastOk(NVTheme.Current.Danger, paper));
        Assert.True(NVAccessibility.BodyContrastOk(NVTheme.Current.Warn, paper));
        Assert.True(NVAccessibility.BodyContrastOk(NVTheme.Current.Ok, paper));
        Assert.True(NVAccessibility.BodyContrastOk(NVTheme.Current.OnAccent, NVTheme.Current.Accent));
        Assert.True(NVAccessibility.BodyContrastOk(NVTheme.Current.On(NVTheme.Current.Danger), NVTheme.Current.Danger));
    }

    [Fact]
    public void Start_pad_and_advance_swipe_flip_in_rtl()
    {
        NVTheme.Current.SetFlowDirection(FlowDirection.LeftToRight);
        Assert.False(NVTokens.IsRtl);
        Assert.Equal(16, NVTokens.StartPad(16).Left);
        Assert.Equal(0, NVTokens.StartPad(16).Right);
        Assert.Equal(SwipeDirection.Left, NVTokens.AdvanceSwipe);

        NVTheme.Current.SetFlowDirection(FlowDirection.RightToLeft);
        Assert.True(NVTokens.IsRtl);
        Assert.Equal(0, NVTokens.StartPad(16).Left);
        Assert.Equal(16, NVTokens.StartPad(16).Right);
        Assert.Equal(SwipeDirection.Right, NVTokens.AdvanceSwipe);
        Assert.Equal("◂", NVIcons.Glyph(NVIconKind.ChevronRight, FlowDirection.RightToLeft));
        Assert.Equal("▸", NVIcons.Glyph(NVIconKind.ChevronRight, FlowDirection.LeftToRight));
    }

    [Fact]
    public void Tree_indent_uses_start_edge()
    {
        var tree = new NVTreeView
        {
            Roots =
            [
                new NVTreeNode
                {
                    Title = "Root",
                    IsExpanded = true,
                    Children = { new NVTreeNode { Title = "Child" } }
                }
            ]
        };
        NVTheme.Current.SetFlowDirection(FlowDirection.RightToLeft);
        var child = Assert.IsType<NVListTile>(((VerticalStackLayout)tree.Content!).Children[1]);
        Assert.Equal(NVTokens.Space4, child.Margin.Right);
        Assert.Equal(0, child.Margin.Left);
    }

    [Fact]
    public void Overlay_escape_dismisses_unless_paywall_blocks()
    {
        var dialog = new NVDialog { IsOpen = true };
        Assert.True(dialog.TryHandleKey("Escape"));
        Assert.False(dialog.IsOpen);
        Assert.False(dialog.TryHandleKey("Escape"));

        var wall = new NVPaywall { IsOpen = true, IsBlocking = true };
        Assert.False(wall.TryHandleKey("Esc"));
        Assert.True(wall.IsOpen);
        wall.IsBlocking = false;
        Assert.True(wall.TryHandleKey("escape"));
        Assert.False(wall.IsOpen);
    }

    [Fact]
    public void Palette_control_k_toggles_and_escape_closes()
    {
        var palette = new NVCommandPalette();
        Assert.True(NVCommandPalette.MatchesOpenShortcut("Control+K"));
        Assert.True(NVCommandPalette.MatchesOpenShortcut("cmd+k"));
        Assert.True(palette.TryHandleShortcut("Ctrl+K"));
        Assert.True(palette.IsOpen);
        Assert.True(palette.TryHandleShortcut("Escape"));
        Assert.False(palette.IsOpen);
    }

    [Fact]
    public void Use_lumina_resets_scale_and_flow()
    {
        NVTheme.Current.SetTypeScale(2);
        NVTheme.Current.SetFlowDirection(FlowDirection.RightToLeft);
        NVTheme.Current.UseLumina();
        Assert.Equal(1, NVTheme.Current.TypeScale);
        Assert.Equal(FlowDirection.MatchParent, NVTheme.Current.FlowDirection);
        Assert.False(NVTheme.Current.IsRtl);
    }

    [Fact]
    public void Catalog_count_is_unchanged()
    {
        Assert.Equal(201, NVCatalog.Controls.Count);
        Assert.Equal(66, NVCatalog.Pages.Count);
    }
}
