namespace NuvyntraLabs.UIKit;

public abstract class ThemeAwareView : ContentView
{
    protected ThemeAwareView()
    {
        NVTheme.Current.Changed += OnThemeChanged;
        Unloaded += (_, _) => NVTheme.Current.Changed -= OnThemeChanged;
    }

    void OnThemeChanged(object? sender, EventArgs e) => ApplyTheme();

    protected abstract void ApplyTheme();
}
