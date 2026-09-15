namespace NuvyntraLabs.UIKit;

public class NVSectionHeader : ThemeAwareView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(NVSectionHeader), "Section", propertyChanged: Refresh);
    readonly Label _label = new() { FontFamily = NVTokens.FontSemiBold, FontSize = NVTokens.LabelSize };
    public NVSectionHeader() { Content = _label; ApplyTheme(); }
    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVSectionHeader)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _label.Text = Text.ToUpperInvariant();
        _label.TextColor = NVTheme.Current.Muted;
    }
}

public class NVFormSection : ThemeAwareView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(NVFormSection), "Details", propertyChanged: Refresh);
    readonly NVSectionHeader _header = new();
    readonly VerticalStackLayout _fields = new() { Spacing = NVTokens.Space3 };
    public NVFormSection()
    {
        Content = new VerticalStackLayout { Spacing = NVTokens.Space2, Children = { _header, _fields } };
        ApplyTheme();
    }
    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    public IList<View> Fields => _fields.Children.Cast<View>().ToList();
    public void Add(View field) => _fields.Children.Add(field);
    static void Refresh(BindableObject b, object o, object n) => ((NVFormSection)b).ApplyTheme();
    protected override void ApplyTheme() => _header.Text = Title;
}

public class NVFloatingActionButton : NVIconButton
{
    public NVFloatingActionButton()
    {
        Kind = NVIconKind.Plus;
        Variant = NVButtonVariant.Filled;
    }
}

public class NVDotIndicator : ThemeAwareView
{
    public static readonly BindableProperty CountProperty = BindableProperty.Create(nameof(Count), typeof(int), typeof(NVDotIndicator), 3, propertyChanged: Refresh);
    public static readonly BindableProperty IndexProperty = BindableProperty.Create(nameof(Index), typeof(int), typeof(NVDotIndicator), 0, BindingMode.TwoWay, propertyChanged: Refresh);
    readonly HorizontalStackLayout _row = new() { Spacing = 6, HorizontalOptions = LayoutOptions.Center };
    public NVDotIndicator() { Content = _row; ApplyTheme(); }
    public int Count { get => (int)GetValue(CountProperty); set => SetValue(CountProperty, value); }
    public int Index { get => (int)GetValue(IndexProperty); set => SetValue(IndexProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVDotIndicator)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _row.Children.Clear();
        var n = Math.Clamp(Count, 1, 12);
        var i = Math.Clamp(Index, 0, n - 1);
        for (var d = 0; d < n; d++)
        {
            _row.Children.Add(new BoxView
            {
                WidthRequest = d == i ? 16 : 8,
                HeightRequest = 8,
                CornerRadius = 4,
                Color = d == i ? NVTheme.Current.Accent : NVTheme.Current.Fog
            });
        }
    }
}

public class NVAppScaffold : ThemeAwareView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(NVAppScaffold), "Lumina", propertyChanged: Refresh);
    public static readonly BindableProperty ShowFabProperty = BindableProperty.Create(nameof(ShowFab), typeof(bool), typeof(NVAppScaffold), true, propertyChanged: Refresh);
    readonly NVToolbar _bar = new();
    readonly ContentView _body = new();
    readonly NVFloatingActionButton _fab = new();
    readonly NVBottomNavigation _tabs = new();
    public NVAppScaffold()
    {
        var grid = new Grid
        {
            RowDefinitions = new RowDefinitionCollection
            {
                new(GridLength.Auto),
                new(GridLength.Star),
                new(GridLength.Auto)
            }
        };
        Grid.SetRow(_bar, 0);
        Grid.SetRow(_body, 1);
        var bottom = new Grid();
        bottom.Add(_tabs);
        _fab.HorizontalOptions = LayoutOptions.End;
        _fab.VerticalOptions = LayoutOptions.End;
        _fab.Margin = new Thickness(0, 0, NVTokens.Space4, NVTokens.Space4);
        Grid.SetRow(_fab, 1);
        grid.Add(_bar);
        grid.Add(_body);
        Grid.SetRow(bottom, 2);
        grid.Add(bottom);
        grid.Add(_fab);
        Content = grid;
        ApplyTheme();
    }
    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    public bool ShowFab { get => (bool)GetValue(ShowFabProperty); set => SetValue(ShowFabProperty, value); }
    public View? Body { get => _body.Content; set => _body.Content = value; }
    static void Refresh(BindableObject b, object o, object n) => ((NVAppScaffold)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _bar.Title = Title;
        _fab.IsVisible = ShowFab;
    }
}

public class NVDialog : OverlayHost
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(NVDialog), "Confirm", propertyChanged: Refresh);
    public static readonly BindableProperty MessageProperty = BindableProperty.Create(nameof(Message), typeof(string), typeof(NVDialog), "", propertyChanged: Refresh);
    readonly NVHeading _title = new();
    readonly NVBodyText _body = new();
    readonly NVButton _ok = new() { Text = "OK", Variant = NVButtonVariant.Filled };
    readonly NVButton _cancel = new() { Text = "Cancel", Variant = NVButtonVariant.Ghost };
    public NVDialog()
    {
        Placement = OverlayPlacement.Center;
        _ok.Command = new Command(() => IsOpen = false);
        _cancel.Command = new Command(() => IsOpen = false);
        PanelContent = new VerticalStackLayout
        {
            Spacing = NVTokens.Space3,
            Children = { _title, _body, new HorizontalStackLayout { Spacing = NVTokens.Space2, Children = { _cancel, _ok } } }
        };
        ApplyTheme();
    }
    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    public string Message { get => (string)GetValue(MessageProperty); set => SetValue(MessageProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVDialog)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        base.ApplyTheme();
        _title.Text = Title;
        _body.Text = Message;
    }
}

public class NVActionSheet : OverlayHost
{
    public static readonly BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(IList<string>), typeof(NVActionSheet), new List<string> { "Edit", "Share", "Delete" }, propertyChanged: Refresh);
    readonly VerticalStackLayout _list = new() { Spacing = NVTokens.Space2 };
    public NVActionSheet()
    {
        Placement = OverlayPlacement.Bottom;
        PanelContent = _list;
        ApplyTheme();
    }
    public IList<string> Items { get => (IList<string>)GetValue(ItemsProperty); set => SetValue(ItemsProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVActionSheet)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        base.ApplyTheme();
        _list.Children.Clear();
        foreach (var item in Items ?? [])
        {
            var button = new NVButton { Text = item, Variant = NVButtonVariant.Ghost };
            button.Command = new Command(() => IsOpen = false);
            _list.Children.Add(button);
        }
    }
}

public class NVMenu : NVDropDownButton { }
