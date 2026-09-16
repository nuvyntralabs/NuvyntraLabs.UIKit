namespace NuvyntraLabs.UIKit;

public class NVMasterDetail : ThemeAwareView
{
    readonly NVNavigationDrawer _master = new() { Items = new List<string> { "Inbox", "Starred", "Sent" }, SelectedItem = "Inbox" };
    readonly NVCard _detail = new();
    public NVMasterDetail()
    {
        _master.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(NVNavigationDrawer.SelectedItem))
            {
                ApplyTheme();
            }
        };
        Content = new Grid
        {
            ColumnDefinitions = new ColumnDefinitionCollection { new(new GridLength(200)), new(GridLength.Star) },
            Children = { _master }
        };
        Grid.SetColumn(_detail, 1);
        ((Grid)Content).Add(_detail);
        ApplyTheme();
    }
    protected override void ApplyTheme()
    {
        _detail.Title = _master.SelectedItem;
        _detail.Body = $"Showing {_master.SelectedItem.ToLowerInvariant()} in the detail pane.";
    }
}

public class NVRetryView : NVEmptyView
{
    public NVRetryView()
    {
        Title = "Couldn’t load";
        Reason = NVStatusReason.Generic;
    }
}

public class NVOfflineBanner : NVBanner
{
    public NVOfflineBanner()
    {
        Text = "You’re offline";
        Tone = NVBannerTone.Warning;
    }
}

public class NVPermissionCard : ThemeAwareView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(NVPermissionCard), "Location", propertyChanged: Refresh);
    readonly NVCard _card = new();
    readonly NVButton _allow = new() { Text = "Allow", Variant = NVButtonVariant.Filled };
    public NVPermissionCard()
    {
        Content = new VerticalStackLayout { Spacing = NVTokens.Space2, Children = { _card, _allow } };
        ApplyTheme();
    }
    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVPermissionCard)b).ApplyTheme();
    protected override void ApplyTheme() { _card.Title = Title; _card.Body = "Host attaches PermissionFlow."; }
}

public class NVForceUpdate : ThemeAwareView
{
    readonly NVEmptyView _empty = new() { Title = "Update required", Reason = NVStatusReason.Generic };
    readonly NVButton _store = new() { Text = "Update", Variant = NVButtonVariant.Filled };
    public NVForceUpdate()
    {
        Content = new VerticalStackLayout { Spacing = NVTokens.Space3, Children = { _empty, _store } };
        ApplyTheme();
    }
    protected override void ApplyTheme() { }
}

public class NVLockPad : ThemeAwareView
{
    readonly NVHeading _title = new() { Text = "Enter PIN" };
    readonly NVPinPad _pad = new();
    public NVLockPad()
    {
        Content = new VerticalStackLayout { Spacing = NVTokens.Space4, Children = { _title, _pad } };
        ApplyTheme();
    }
    protected override void ApplyTheme() { }
}

public class NVBiometricGate : ThemeAwareView
{
    readonly NVIcon _icon = new() { Kind = NVIconKind.Lock };
    readonly NVHeading _title = new() { Text = "Unlock" };
    readonly NVButton _prompt = new() { Text = "Use biometrics", Variant = NVButtonVariant.Tonal };
    public NVBiometricGate()
    {
        Content = new VerticalStackLayout { Spacing = NVTokens.Space3, Children = { _icon, _title, _prompt } };
        ApplyTheme();
    }
    protected override void ApplyTheme() { }
}

public class NVDashboardGrid : ThemeAwareView
{
    public NVDashboardGrid()
    {
        var grid = new FlexLayout { Wrap = Microsoft.Maui.Layouts.FlexWrap.Wrap };
        foreach (var (label, value) in new[] { ("Revenue", "$48k"), ("Orders", "128"), ("NPS", "72"), ("Uptime", "99.9%") })
        {
            grid.Children.Add(new NVStatCard { Label = label, Value = value, WidthRequest = 140 });
        }
        Content = grid;
        ApplyTheme();
    }
    protected override void ApplyTheme() { }
}

public class NVGantt : ThemeAwareView
{
    readonly VerticalStackLayout _rows = new() { Spacing = NVTokens.Space2 };
    public NVGantt() { Content = _rows; ApplyTheme(); }
    protected override void ApplyTheme()
    {
        _rows.Children.Clear();
        foreach (var (title, start, width) in new[] { ("Design", 0, 90), ("Build", 80, 140), ("Ship", 200, 70) })
        {
            var bar = new BoxView { HeightRequest = 16, WidthRequest = width, Color = NVTheme.Current.Accent, HorizontalOptions = LayoutOptions.Start };
            _rows.Children.Add(new VerticalStackLayout
            {
                Spacing = 2,
                Children =
                {
                    new NVCaptionText { Text = title },
                    new Grid { Children = { new BoxView { Color = NVTheme.Current.Mist, HeightRequest = 16 }, new HorizontalStackLayout { Children = { new BoxView { WidthRequest = start, Color = Colors.Transparent }, bar } } } }
                }
            });
        }
    }
}

public class NVOrgChart : NVTreeView
{
    public NVOrgChart()
    {
        Roots =
        [
            new NVTreeNode
            {
                Title = "Studio",
                IsExpanded = true,
                Children = { new NVTreeNode { Title = "Design" }, new NVTreeNode { Title = "Engineering" } }
            }
        ];
    }
}
