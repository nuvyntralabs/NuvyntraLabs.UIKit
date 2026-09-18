namespace NuvyntraLabs.UIKit;

public abstract class ThemeAwareView : ContentView
{
    protected ThemeAwareView()
    {
        NVTheme.Current.Changed += OnThemeChanged;
        Unloaded += (_, _) => NVTheme.Current.Changed -= OnThemeChanged;
        FlowDirection = NVTheme.Current.FlowDirection;
    }

    void OnThemeChanged(object? sender, EventArgs e)
    {
        FlowDirection = NVTheme.Current.FlowDirection;
        ApplyTheme();
    }

    protected abstract void ApplyTheme();
}
