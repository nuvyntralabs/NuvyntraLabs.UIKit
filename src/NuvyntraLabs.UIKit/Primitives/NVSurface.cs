namespace NuvyntraLabs.UIKit;

/// <summary>Themed paper surface with Lumina radius and hairline stroke.</summary>
public class NVSurface : ThemeAwareView
{
    public static readonly BindableProperty ElevationProperty = BindableProperty.Create(
        nameof(Elevation), typeof(int), typeof(NVSurface), 1,
        propertyChanged: OnChromeChanged);

    readonly Border _border = new();

    public NVSurface()
    {
        _border.Padding = NVTokens.Space4;
        _border.StrokeThickness = 1;
        base.Content = _border;
        ApplyTheme();
    }

    public int Elevation
    {
        get => (int)GetValue(ElevationProperty);
        set => SetValue(ElevationProperty, value);
    }

    public new View? Content
    {
        get => _border.Content;
        set => _border.Content = value;
    }

    static void OnChromeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is NVSurface surface)
        {
            surface.ApplyTheme();
        }
    }

    protected override void ApplyTheme()
    {
        var theme = NVTheme.Current;
        _border.BackgroundColor = theme.Surface;
        _border.Stroke = theme.Fog;
        _border.StrokeShape = new RoundRectangle { CornerRadius = NVTokens.RadiusMedium };
        _border.Shadow = Elevation <= 0
            ? new Shadow { Opacity = 0 }
            : new Shadow
            {
                Brush = theme.Ink,
                Opacity = theme.IsDark ? 0.2f : 0.08f,
                Radius = 4 + Elevation,
                Offset = new Point(0, 2)
            };
    }
}
