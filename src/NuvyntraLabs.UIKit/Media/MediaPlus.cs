namespace NuvyntraLabs.UIKit;

public class NVImageGallery : ThemeAwareView
{
    public static readonly BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(IList<string>), typeof(NVImageGallery), new List<string> { "One", "Two", "Three" }, propertyChanged: Refresh);
    readonly FlexLayout _wrap = new() { Wrap = Microsoft.Maui.Layouts.FlexWrap.Wrap };
    public NVImageGallery() { Content = _wrap; ApplyTheme(); }
    public IList<string> Items { get => (IList<string>)GetValue(ItemsProperty); set => SetValue(ItemsProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVImageGallery)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _wrap.Children.Clear();
        foreach (var item in Items ?? [])
        {
            _wrap.Children.Add(new NVImage { Caption = item, WidthRequest = 96, HeightRequest = 96 });
        }
    }
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
    public static readonly BindableProperty IsPlayingProperty = BindableProperty.Create(nameof(IsPlaying), typeof(bool), typeof(NVVideoPlayer), false, BindingMode.TwoWay, propertyChanged: Refresh);
    readonly Grid _poster = new() { HeightRequest = 180 };
    readonly NVIconButton _play = new() { Kind = NVIconKind.Play, Variant = NVButtonVariant.Filled };
    readonly NVSlider _scrub = new() { Value = 18 };
    readonly NVCaptionText _time = new() { Text = "0:18 / 1:04" };
    public NVVideoPlayer()
    {
        _play.Command = new Command(() => IsPlaying = !IsPlaying);
        _play.HorizontalOptions = LayoutOptions.Center;
        _play.VerticalOptions = LayoutOptions.Center;
        _poster.Add(new BoxView());
        _poster.Add(_play);
        Content = new VerticalStackLayout { Spacing = NVTokens.Space2, Children = { _poster, new HorizontalStackLayout { Spacing = NVTokens.Space2, Children = { _time, _scrub } } } };
        ApplyTheme();
    }
    public bool IsPlaying { get => (bool)GetValue(IsPlayingProperty); set => SetValue(IsPlayingProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVVideoPlayer)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        if (_poster.Children[0] is BoxView paper)
        {
            paper.Color = NVTheme.Current.Ink;
        }
        _play.Kind = IsPlaying ? NVIconKind.Pause : NVIconKind.Play;
    }
}

public class NVAudioPlayer : ThemeAwareView
{
    public static readonly BindableProperty IsPlayingProperty = BindableProperty.Create(nameof(IsPlaying), typeof(bool), typeof(NVAudioPlayer), false, BindingMode.TwoWay, propertyChanged: Refresh);
    readonly NVSlider _scrub = new() { Value = 20 };
    readonly NVIconButton _play = new() { Kind = NVIconKind.Play };
    readonly NVCaptionText _time = new() { Text = "0:20" };
    public NVAudioPlayer()
    {
        _play.Command = new Command(() => IsPlaying = !IsPlaying);
        Content = new HorizontalStackLayout { Spacing = NVTokens.Space2, Children = { _play, _scrub, _time } };
        ApplyTheme();
    }
    public bool IsPlaying { get => (bool)GetValue(IsPlayingProperty); set => SetValue(IsPlayingProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVAudioPlayer)b).ApplyTheme();
    protected override void ApplyTheme() => _play.Kind = IsPlaying ? NVIconKind.Pause : NVIconKind.Play;
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
    readonly NVSlider _slider = new() { Value = 50 };
    readonly BoxView _before = new() { HeightRequest = 120 };
    readonly BoxView _after = new() { HeightRequest = 120 };
    readonly Grid _split = new() { HeightRequest = 120 };
    public NVBeforeAfter()
    {
        _slider.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(NVSlider.Value))
            {
                ApplyTheme();
            }
        };
        _split.Add(_after);
        _split.Add(_before);
        Content = new VerticalStackLayout
        {
            Spacing = NVTokens.Space2,
            Children = { _split, _slider, new NVCaptionText { Text = "Before · After" } }
        };
        ApplyTheme();
    }
    protected override void ApplyTheme()
    {
        _after.Color = NVTheme.Current.Accent;
        _before.Color = NVTheme.Current.Mist;
        _before.WidthRequest = 280 * (_slider.Value / 100);
        _before.HorizontalOptions = LayoutOptions.Start;
    }
}
