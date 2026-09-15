namespace NuvyntraLabs.UIKit;

public class NVImageGallery : ThemeAwareView
{
    public static readonly BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(IList<string>), typeof(NVImageGallery), new List<string> { "One", "Two", "Three" }, propertyChanged: Refresh);
    readonly NVWrapLayout _wrap = new();
    public NVImageGallery() { Content = _wrap; ApplyTheme(); }
    public IList<string> Items { get => (IList<string>)GetValue(ItemsProperty); set => SetValue(ItemsProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVImageGallery)b).ApplyTheme();
    protected override void ApplyTheme() => _wrap.Items = Items;
}

public class NVLightbox : OverlayHost
{
    public NVLightbox()
    {
        Placement = OverlayPlacement.Center;
        PanelContent = new NVImage { Caption = "Lightbox" };
    }
}

public class NVVideoPlayer : ThemeAwareView
{
    readonly NVCard _card = new() { Title = "Video", Body = "Host supplies a player. Kit chrome only." };
    readonly NVIconButton _play = new() { Kind = NVIconKind.Play, Variant = NVButtonVariant.Filled };
    public NVVideoPlayer()
    {
        Content = new Grid { Children = { _card, _play } };
        _play.HorizontalOptions = LayoutOptions.Center;
        _play.VerticalOptions = LayoutOptions.Center;
        ApplyTheme();
    }
    protected override void ApplyTheme() { }
}

public class NVAudioPlayer : ThemeAwareView
{
    readonly NVSlider _scrub = new() { Value = 20 };
    readonly NVIconButton _play = new() { Kind = NVIconKind.Play };
    public NVAudioPlayer()
    {
        Content = new HorizontalStackLayout { Spacing = NVTokens.Space2, Children = { _play, _scrub } };
        ApplyTheme();
    }
    protected override void ApplyTheme() { }
}

public class NVWebView : ThemeAwareView
{
    public static readonly BindableProperty UrlProperty = BindableProperty.Create(nameof(Url), typeof(string), typeof(NVWebView), "https://nuvyntralabs.github.io/", propertyChanged: Refresh);
    readonly WebView _web = new() { HeightRequest = 220 };
    public NVWebView() { Content = _web; ApplyTheme(); }
    public string Url { get => (string)GetValue(UrlProperty); set => SetValue(UrlProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVWebView)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        if (Uri.TryCreate(Url, UriKind.Absolute, out var uri) && uri.Scheme is "https" or "http")
        {
            _web.Source = uri.ToString();
        }
    }
}

public class NVVoiceNote : ThemeAwareView
{
    readonly NVWaveform _wave = new();
    readonly NVIconButton _mic = new() { Kind = NVIconKind.Mic, Variant = NVButtonVariant.Tonal };
    public NVVoiceNote()
    {
        Content = new HorizontalStackLayout { Spacing = NVTokens.Space2, Children = { _mic, _wave } };
        ApplyTheme();
    }
    protected override void ApplyTheme() { }
}

public class NVWaveform : ThemeAwareView
{
    readonly HorizontalStackLayout _bars = new() { Spacing = 3 };
    public NVWaveform() { Content = _bars; ApplyTheme(); }
    protected override void ApplyTheme()
    {
        _bars.Children.Clear();
        foreach (var h in new[] { 8, 16, 12, 20, 10, 18, 9 })
        {
            _bars.Children.Add(new BoxView { WidthRequest = 4, HeightRequest = h, Color = NVTheme.Current.Accent, VerticalOptions = LayoutOptions.Center });
        }
    }
}

public class NVBeforeAfter : ThemeAwareView
{
    readonly NVRangeSlider _slider = new() { Start = 40, End = 60 };
    readonly NVCaptionText _caption = new() { Text = "Before · After" };
    public NVBeforeAfter()
    {
        Content = new VerticalStackLayout { Spacing = NVTokens.Space2, Children = { new NVImage { Caption = "Compare" }, _slider, _caption } };
        ApplyTheme();
    }
    protected override void ApplyTheme() { }
}
