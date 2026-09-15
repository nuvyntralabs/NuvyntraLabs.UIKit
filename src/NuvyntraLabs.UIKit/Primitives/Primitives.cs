namespace NuvyntraLabs.UIKit;

public class NVDivider : ThemeAwareView
{
    public static readonly BindableProperty IsVerticalProperty =
        BindableProperty.Create(nameof(IsVertical), typeof(bool), typeof(NVDivider), false, propertyChanged: Refresh);

    readonly BoxView _line = new();

    public NVDivider()
    {
        Content = _line;
        ApplyTheme();
    }

    public bool IsVertical
    {
        get => (bool)GetValue(IsVerticalProperty);
        set => SetValue(IsVerticalProperty, value);
    }

    static void Refresh(BindableObject b, object o, object n) => ((NVDivider)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _line.Color = NVTheme.Current.Fog;
        _line.HeightRequest = IsVertical ? -1 : 1;
        _line.WidthRequest = IsVertical ? 1 : -1;
        _line.HorizontalOptions = IsVertical ? LayoutOptions.Center : LayoutOptions.Fill;
        _line.VerticalOptions = IsVertical ? LayoutOptions.Fill : LayoutOptions.Center;
    }
}

public class NVIcon : ThemeAwareView
{
    public static readonly BindableProperty KindProperty =
        BindableProperty.Create(nameof(Kind), typeof(NVIconKind), typeof(NVIcon), NVIconKind.None, propertyChanged: Refresh);

    public static readonly BindableProperty SpinProperty =
        BindableProperty.Create(nameof(Spin), typeof(bool), typeof(NVIcon), false);

    readonly Label _glyph = new() { HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };

    public NVIcon()
    {
        _glyph.FontSize = 18;
        Content = _glyph;
        ApplyTheme();
    }

    public NVIconKind Kind
    {
        get => (NVIconKind)GetValue(KindProperty);
        set => SetValue(KindProperty, value);
    }

    public bool Spin
    {
        get => (bool)GetValue(SpinProperty);
        set => SetValue(SpinProperty, value);
    }

    static void Refresh(BindableObject b, object o, object n) => ((NVIcon)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _glyph.Text = NVIcons.Glyph(Kind);
        _glyph.TextColor = NVTheme.Current.Ink;
    }
}

public class NVAvatar : ThemeAwareView
{
    public static readonly BindableProperty InitialsProperty =
        BindableProperty.Create(nameof(Initials), typeof(string), typeof(NVAvatar), "NV", propertyChanged: Refresh);

    public static readonly BindableProperty ImageSourceProperty =
        BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(NVAvatar), null, propertyChanged: Refresh);

    public static readonly BindableProperty StatusOnProperty =
        BindableProperty.Create(nameof(StatusOn), typeof(bool), typeof(NVAvatar), false, propertyChanged: Refresh);

    readonly Grid _root = new() { WidthRequest = 40, HeightRequest = 40 };
    readonly Image _image = new() { Aspect = Aspect.AspectFill };
    readonly Label _initials = new() { HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };
    readonly Ellipse _pip = new() { WidthRequest = 10, HeightRequest = 10, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.End };

    public NVAvatar()
    {
        _root.Children.Add(_image);
        _root.Children.Add(_initials);
        _root.Children.Add(_pip);
        Content = _root;
        ApplyTheme();
    }

    public string Initials
    {
        get => (string)GetValue(InitialsProperty);
        set => SetValue(InitialsProperty, value);
    }

    public ImageSource? ImageSource
    {
        get => (ImageSource?)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    public bool StatusOn
    {
        get => (bool)GetValue(StatusOnProperty);
        set => SetValue(StatusOnProperty, value);
    }

    static void Refresh(BindableObject b, object o, object n) => ((NVAvatar)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        var theme = NVTheme.Current;
        _root.BackgroundColor = theme.Mist;
        _initials.Text = Initials;
        _initials.TextColor = theme.Ink;
        _initials.IsVisible = ImageSource is null;
        _image.Source = ImageSource;
        _image.IsVisible = ImageSource is not null;
        _pip.Fill = StatusOn ? theme.Ok : theme.Fog;
    }
}

public class NVBadge : ThemeAwareView
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(NVBadge), "", propertyChanged: Refresh);

    public static readonly BindableProperty DotProperty =
        BindableProperty.Create(nameof(Dot), typeof(bool), typeof(NVBadge), false, propertyChanged: Refresh);

    readonly Label _label = new() { Padding = new Thickness(6, 2) };

    public NVBadge()
    {
        Content = _label;
        ApplyTheme();
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public bool Dot
    {
        get => (bool)GetValue(DotProperty);
        set => SetValue(DotProperty, value);
    }

    static void Refresh(BindableObject b, object o, object n) => ((NVBadge)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        var theme = NVTheme.Current;
        _label.Text = Dot ? "●" : Text;
        _label.TextColor = theme.OnAccent;
        _label.BackgroundColor = theme.Accent;
        _label.FontSize = NVTokens.CaptionSize;
        _label.FontFamily = NVTokens.FontSemiBold;
    }
}

public class NVSkeleton : ThemeAwareView
{
    readonly BoxView _bar = new() { HeightRequest = 14 };

    public NVSkeleton()
    {
        Content = _bar;
        ApplyTheme();
    }

    protected override void ApplyTheme() => _bar.Color = NVTheme.Current.Mist;
}

public class NVEffects : ThemeAwareView
{
    public NVEffects()
    {
        Content = new ContentView();
        ApplyTheme();
    }

    public View? Target
    {
        get => Content as View;
        set => Content = value;
    }

    protected override void ApplyTheme() => Opacity = 1;
}

public static class NVElevation
{
    public static void Apply(VisualElement view, int level)
    {
        view.Shadow = new Shadow
        {
            Brush = NVTheme.Current.Ink,
            Opacity = NVTheme.Current.IsDark ? 0.2f : 0.08f,
            Radius = 4 + Math.Clamp(level, 0, 5),
            Offset = new Point(0, 2)
        };
    }
}

public class NVOverlay : OverlayHost
{
}

public class NVInteractiveViewer : ThemeAwareView
{
    readonly Grid _host = new();

    public NVInteractiveViewer()
    {
        Content = _host;
        ApplyTheme();
    }

    public View? Viewport
    {
        get => _host.Children.Count > 0 ? _host.Children[0] as View : null;
        set
        {
            _host.Children.Clear();
            if (value is not null)
            {
                _host.Children.Add(value);
            }
        }
    }

    protected override void ApplyTheme() => _host.BackgroundColor = NVTheme.Current.Paper;
}

public class NVSpacer : ContentView
{
    public static readonly BindableProperty SizeProperty =
        BindableProperty.Create(nameof(Size), typeof(double), typeof(NVSpacer), NVTokens.Space4, propertyChanged: OnSize);

    public NVSpacer() => HeightRequest = Size;

    public double Size
    {
        get => (double)GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    static void OnSize(BindableObject b, object o, object n)
    {
        if (b is NVSpacer s)
        {
            s.HeightRequest = s.Size;
        }
    }
}

public class NVHighlight : ThemeAwareView
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(NVHighlight), "", propertyChanged: Refresh);

    readonly Label _label = new();

    public NVHighlight()
    {
        Content = _label;
        ApplyTheme();
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    static void Refresh(BindableObject b, object o, object n) => ((NVHighlight)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _label.Text = Text;
        _label.BackgroundColor = NVTheme.Current.Mist;
        _label.TextColor = NVTheme.Current.Ink;
        _label.Padding = new Thickness(4, 0);
    }
}
