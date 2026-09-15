namespace NuvyntraLabs.UIKit.Tests;

public class NVThemeTests
{
    public NVThemeTests()
    {
        NVTheme.Current.UseLumina();
        NVTheme.Current.SetMode(NVThemeMode.Light);
    }

    [Fact]
    public void Light_paper_is_warm_not_blue_gray()
    {
        var paper = NVTheme.Current.Paper;
        Assert.True(paper.Red > paper.Blue);
    }

    [Fact]
    public void SetMode_dark_flips_ink_lighter_than_paper()
    {
        NVTheme.Current.SetMode(NVThemeMode.Dark);
        Assert.True(NVTheme.Current.IsDark);
        Assert.True(NVTheme.Current.Ink.Red > NVTheme.Current.Paper.Red);
    }

    [Fact]
    public void SetAccent_round_trips()
    {
        var accent = Color.FromArgb("#0E7C66");
        NVTheme.Current.SetAccent(accent);
        Assert.Equal(accent.ToArgbHex(), NVTheme.Current.Accent.ToArgbHex());
    }

    [Fact]
    public void Token_known_keys_resolve()
    {
        Assert.Equal(NVTheme.Current.Paper, NVTheme.Current.Token("paper"));
        Assert.Equal(NVTheme.Current.Accent, NVTheme.Current.Token("accent"));
        Assert.Equal(NVTheme.Current.Danger, NVTheme.Current.Token("danger"));
    }

    [Fact]
    public void Token_unknown_key_throws()
    {
        Assert.Throws<NVThemeException>(() => NVTheme.Current.Token("magenta"));
    }

    [Fact]
    public void Changed_fires_on_mode()
    {
        var fired = 0;
        void Handler(object? _, EventArgs e) => fired++;
        NVTheme.Current.Changed += Handler;
        try
        {
            NVTheme.Current.SetMode(NVThemeMode.Dark);
            Assert.True(fired >= 1);
        }
        finally
        {
            NVTheme.Current.Changed -= Handler;
            NVTheme.Current.UseLumina();
        }
    }
}
