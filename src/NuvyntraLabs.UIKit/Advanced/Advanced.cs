namespace NuvyntraLabs.UIKit;

public class NVMasterDetail : ThemeAwareView
{
    readonly NVNavigationDrawer _master = new() { Items = new List<string> { "Inbox", "Starred", "Sent" } };
    readonly NVCard _detail = new() { Title = "Detail", Body = "Two-pane / phone stack" };
    public NVMasterDetail()
    {
        Content = new Grid
        {
            ColumnDefinitions = new ColumnDefinitionCollection { new(new GridLength(200)), new(GridLength.Star) },
            Children = { _master }
        };
        Grid.SetColumn(_detail, 1);
        ((Grid)Content).Add(_detail);
        ApplyTheme();
    }
    protected override void ApplyTheme() { }
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
        Content = new NVWrapLayout
        {
            Items = new List<string> { "Revenue", "Orders", "NPS", "Uptime" }
        };
        ApplyTheme();
    }
    protected override void ApplyTheme() { }
}

public class NVGantt : ThemeAwareView
{
    readonly NVTimeline _timeline = new();
    public NVGantt()
    {
        _timeline.Items =
        [
            new NVTimelineItem { Title = "Design", Detail = "3d", At = DateTime.Today },
            new NVTimelineItem { Title = "Build", Detail = "5d", At = DateTime.Today.AddDays(3) }
        ];
        Content = _timeline;
        ApplyTheme();
    }
    protected override void ApplyTheme() { }
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
