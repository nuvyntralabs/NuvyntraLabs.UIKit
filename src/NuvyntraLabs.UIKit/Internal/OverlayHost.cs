namespace NuvyntraLabs.UIKit;

/// <summary>Shared scrim + card used by popup, sheet, drawer, and pickers.</summary>
public class OverlayHost : ThemeAwareView
{
    public static readonly BindableProperty IsOpenProperty = BindableProperty.Create(
        nameof(IsOpen), typeof(bool), typeof(OverlayHost), false,
        BindingMode.TwoWay, propertyChanged: OnOpen);

    public static readonly BindableProperty DismissOnScrimProperty = BindableProperty.Create(
        nameof(DismissOnScrim), typeof(bool), typeof(OverlayHost), true);

    public static readonly BindableProperty DismissOnEscapeProperty = BindableProperty.Create(
        nameof(DismissOnEscape), typeof(bool), typeof(OverlayHost), true);

    public static readonly BindableProperty PlacementProperty = BindableProperty.Create(
        nameof(Placement), typeof(OverlayPlacement), typeof(OverlayHost), OverlayPlacement.Center,
        propertyChanged: OnOpen);

    readonly Grid _root = new();
    readonly BoxView _scrim = new();
    readonly Border _panel = new();

    public OverlayHost()
    {
        _scrim.Opacity = 0.45;
        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, _) =>
        {
            if (DismissOnScrim)
            {
                IsOpen = false;
            }
        };
        _scrim.GestureRecognizers.Add(tap);
        _panel.Padding = NVTokens.Space4;
        _root.Children.Add(_scrim);
        _root.Children.Add(_panel);
        Content = _root;
        IsVisible = false;
        ApplyTheme();
    }

    public View? PanelContent
    {
        get => _panel.Content;
        set => _panel.Content = value;
    }

    public bool IsOpen
    {
        get => (bool)GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    public bool DismissOnScrim
    {
        get => (bool)GetValue(DismissOnScrimProperty);
        set => SetValue(DismissOnScrimProperty, value);
    }

    public bool DismissOnEscape
    {
        get => (bool)GetValue(DismissOnEscapeProperty);
        set => SetValue(DismissOnEscapeProperty, value);
    }

    /// <summary>Host pages call this from a window key handler. Escape dismisses when <see cref="DismissOnEscape"/>.</summary>
    public bool TryHandleKey(string key)
    {
        if (!IsOpen || !DismissOnEscape || !IsEscape(key))
        {
            return false;
        }

        IsOpen = false;
        return true;
    }

    public bool FocusPanel()
    {
        if (!IsOpen)
        {
            return false;
        }

        return _panel.Focus() || (PanelContent as VisualElement)?.Focus() == true;
    }

    static bool IsEscape(string key) =>
        key.Equals("Escape", StringComparison.OrdinalIgnoreCase)
        || key.Equals("Esc", StringComparison.OrdinalIgnoreCase);

    public OverlayPlacement Placement
    {
        get => (OverlayPlacement)GetValue(PlacementProperty);
        set => SetValue(PlacementProperty, value);
    }

    static void OnOpen(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is OverlayHost host)
        {
            host.IsVisible = host.IsOpen;
            host.ApplyPlacement();
            if (host.IsOpen)
            {
                host.FocusPanel();
            }
        }
    }

    void ApplyPlacement()
    {
        _panel.VerticalOptions = Placement switch
        {
            OverlayPlacement.Bottom => LayoutOptions.End,
            OverlayPlacement.Top => LayoutOptions.Start,
            OverlayPlacement.Start => LayoutOptions.Fill,
            OverlayPlacement.End => LayoutOptions.Fill,
            _ => LayoutOptions.Center
        };
        _panel.HorizontalOptions = Placement switch
        {
            OverlayPlacement.Start => LayoutOptions.Start,
            OverlayPlacement.End => LayoutOptions.End,
            OverlayPlacement.Bottom or OverlayPlacement.Top => LayoutOptions.Fill,
            _ => LayoutOptions.Center
        };
        if (Placement is OverlayPlacement.Bottom or OverlayPlacement.Top)
        {
            _panel.Margin = new Thickness(0);
        }
    }

    protected override void ApplyTheme()
    {
        var theme = NVTheme.Current;
        _scrim.Color = theme.Ink;
        _panel.BackgroundColor = theme.Surface;
        _panel.Stroke = theme.Fog;
        _panel.StrokeShape = new RoundRectangle { CornerRadius = NVTokens.RadiusLarge };
        ApplyPlacement();
    }
}

public enum OverlayPlacement
{
    Center,
    Bottom,
    Top,
    Start,
    End
}
