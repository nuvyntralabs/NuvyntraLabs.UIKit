namespace NuvyntraLabs.UIKit;

public class NVProgressBar : ThemeAwareView
{
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(double), typeof(NVProgressBar), 0d, propertyChanged: Refresh);
    readonly ProgressBar _bar = new();
    public NVProgressBar() { Content = _bar; ApplyTheme(); }
    public double Value { get => (double)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVProgressBar)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _bar.Progress = Math.Clamp(Value, 0, 1);
        _bar.ProgressColor = NVTheme.Current.Accent;
    }
}

public class NVCircularProgress : ThemeAwareView
{
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(double), typeof(NVCircularProgress), 0.4, propertyChanged: Refresh);
    readonly Label _label = new() { HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center, FontFamily = NVTokens.FontSemiBold };
    public NVCircularProgress()
    {
        Content = new Border
        {
            StrokeShape = new Ellipse(),
            WidthRequest = 72,
            HeightRequest = 72,
            StrokeThickness = 4,
            Content = _label
        };
        ApplyTheme();
    }
    public double Value { get => (double)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVCircularProgress)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        if (Content is Border border)
        {
            border.Stroke = NVTheme.Current.Accent;
            border.BackgroundColor = NVTheme.Current.Surface;
        }
        _label.Text = $"{(int)(Math.Clamp(Value, 0, 1) * 100)}%";
        _label.TextColor = NVTheme.Current.Ink;
    }
}

public class NVCircularProgressBar : NVCircularProgress { }

public class NVStepProgressBar : ThemeAwareView
{
    public static readonly BindableProperty StepsProperty = BindableProperty.Create(nameof(Steps), typeof(IList<string>), typeof(NVStepProgressBar), new List<string> { "One", "Two", "Three" }, propertyChanged: Refresh);
    public static readonly BindableProperty IndexProperty = BindableProperty.Create(nameof(Index), typeof(int), typeof(NVStepProgressBar), 0, propertyChanged: Refresh);
    readonly Label _label = new() { FontFamily = NVTokens.FontRegular };
    readonly NVProgressBar _bar = new();
    public NVStepProgressBar()
    {
        Content = new VerticalStackLayout { Spacing = NVTokens.Space2, Children = { _label, _bar } };
        ApplyTheme();
    }
    public IList<string> Steps { get => (IList<string>)GetValue(StepsProperty); set => SetValue(StepsProperty, value); }
    public int Index { get => (int)GetValue(IndexProperty); set => SetValue(IndexProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVStepProgressBar)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        var items = Steps ?? [];
        var i = items.Count == 0 ? 0 : Math.Clamp(Index, 0, items.Count - 1);
        _label.Text = items.Count == 0 ? "" : $"Step {i + 1}: {items[i]}";
        _label.TextColor = NVTheme.Current.Ink;
        _bar.Value = items.Count == 0 ? 0 : (i + 1) / (double)items.Count;
    }
}

public class NVBusyIndicator : ThemeAwareView
{
    readonly ActivityIndicator _spin = new() { IsRunning = true };
    public NVBusyIndicator() { Content = _spin; ApplyTheme(); }
    protected override void ApplyTheme() => _spin.Color = NVTheme.Current.Accent;
}

public class NVShimmer : NVSkeleton { }

public class NVPullToRefresh : ThemeAwareView
{
    public static readonly BindableProperty IsRefreshingProperty = BindableProperty.Create(nameof(IsRefreshing), typeof(bool), typeof(NVPullToRefresh), false, BindingMode.TwoWay);
    readonly RefreshView _refresh = new();
    public NVPullToRefresh()
    {
        _refresh.Content = new ScrollView
        {
            Content = new Label { Text = "Pull to refresh", FontFamily = NVTokens.FontRegular }
        };
        _refresh.Refreshing += (_, _) =>
        {
            IsRefreshing = false;
            _refresh.IsRefreshing = false;
        };
        Content = _refresh;
        ApplyTheme();
    }
    public bool IsRefreshing { get => (bool)GetValue(IsRefreshingProperty); set => SetValue(IsRefreshingProperty, value); }
    protected override void ApplyTheme() => _refresh.RefreshColor = NVTheme.Current.Accent;
}

public class NVToast
{
    public static Task ShowAsync(string message) =>
        Application.Current?.Windows.FirstOrDefault()?.Page?.DisplayAlertAsync("", message, "OK") ?? Task.CompletedTask;
}

public class NVBanner : ThemeAwareView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(NVBanner), "", propertyChanged: Refresh);
    public static readonly BindableProperty ToneProperty = BindableProperty.Create(nameof(Tone), typeof(NVBannerTone), typeof(NVBanner), NVBannerTone.Info, propertyChanged: Refresh);
    readonly Label _label = new() { FontFamily = NVTokens.FontRegular };

    public NVBanner()
    {
        Content = new Border { Padding = NVTokens.Space3, Content = _label, StrokeThickness = 0 };
        ApplyTheme();
    }

    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
    public NVBannerTone Tone { get => (NVBannerTone)GetValue(ToneProperty); set => SetValue(ToneProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVBanner)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _label.Text = Text;
        _label.TextColor = NVTheme.Current.Ink;
        if (Content is Border border)
        {
            border.BackgroundColor = Tone switch
            {
                NVBannerTone.Success => NVTheme.Current.Mist,
                NVBannerTone.Warning => NVTheme.Current.Warn,
                NVBannerTone.Danger => NVTheme.Current.Danger,
                _ => NVTheme.Current.Paper
            };
            border.StrokeShape = new RoundRectangle { CornerRadius = NVTokens.RadiusMedium };
        }
    }
}

public class NVAlert : NVBanner
{
    public NVAlert() => Tone = NVBannerTone.Warning;
}

public class NVPopup : OverlayHost
{
    public NVPopup() => Placement = OverlayPlacement.Center;
}

public class NVEmptyView : ThemeAwareView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(NVEmptyView), "Nothing here", propertyChanged: Refresh);
    public static readonly BindableProperty ReasonProperty = BindableProperty.Create(nameof(Reason), typeof(NVStatusReason), typeof(NVEmptyView), NVStatusReason.Empty, propertyChanged: Refresh);
    readonly Label _title = new() { FontFamily = NVTokens.FontSemiBold, FontSize = NVTokens.TitleSize };
    readonly Label _body = new() { FontFamily = NVTokens.FontRegular };
    public NVEmptyView()
    {
        Content = new VerticalStackLayout { Spacing = NVTokens.Space2, Children = { _title, _body } };
        ApplyTheme();
    }
    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    public NVStatusReason Reason { get => (NVStatusReason)GetValue(ReasonProperty); set => SetValue(ReasonProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVEmptyView)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _title.Text = Title;
        _title.TextColor = NVTheme.Current.Ink;
        _body.Text = Reason.ToString();
        _body.TextColor = NVTheme.Current.Muted;
    }
}

public class NVTooltip : ThemeAwareView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(NVTooltip), "", propertyChanged: Refresh);
    readonly Label _label = new() { FontFamily = NVTokens.FontRegular, FontSize = NVTokens.CaptionSize };
    public NVTooltip()
    {
        Content = new Border { Padding = new Thickness(NVTokens.Space2), Content = _label, StrokeThickness = 0 };
        ApplyTheme();
    }
    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVTooltip)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _label.Text = Text;
        _label.TextColor = NVTheme.Current.Paper;
        if (Content is Border border)
        {
            border.BackgroundColor = NVTheme.Current.Ink;
            border.StrokeShape = new RoundRectangle { CornerRadius = NVTokens.RadiusSmall };
        }
    }
}
