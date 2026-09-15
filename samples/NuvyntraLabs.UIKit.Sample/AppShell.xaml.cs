namespace NuvyntraLabs.UIKit.Sample;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        ApplyShellChrome();
        NVTheme.Current.Changed += (_, _) => ApplyShellChrome();
    }

    void ApplyShellChrome()
    {
        var theme = NVTheme.Current;
        FlyoutBackgroundColor = theme.Paper;
        BackgroundColor = theme.Paper;
    }
}
