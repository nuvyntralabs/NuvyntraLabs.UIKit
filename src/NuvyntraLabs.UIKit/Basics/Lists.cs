namespace NuvyntraLabs.UIKit;

public class NVListTile : ThemeAwareView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(NVListTile), "", propertyChanged: Refresh);
    public static readonly BindableProperty SubtitleProperty = BindableProperty.Create(nameof(Subtitle), typeof(string), typeof(NVListTile), "", propertyChanged: Refresh);
    public static readonly BindableProperty KindProperty = BindableProperty.Create(nameof(Kind), typeof(NVIconKind), typeof(NVListTile), NVIconKind.ChevronRight, propertyChanged: Refresh);
    readonly NVIcon _leading = new();
    readonly NVHeading _title = new() { Role = NVTextRole.Body };
    readonly NVCaptionText _sub = new();
    readonly NVIcon _trailing = new() { Kind = NVIconKind.ChevronRight };
    public NVListTile()
    {
        var text = new VerticalStackLayout { Spacing = 2, Children = { _title, _sub } };
        Content = new HorizontalStackLayout { Spacing = NVTokens.Space3, Children = { _leading, text, _trailing } };
        ApplyTheme();
    }
    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    public string Subtitle { get => (string)GetValue(SubtitleProperty); set => SetValue(SubtitleProperty, value); }
    public NVIconKind Kind { get => (NVIconKind)GetValue(KindProperty); set => SetValue(KindProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVListTile)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _leading.Kind = Kind;
        _title.Text = Title;
        _sub.Text = Subtitle;
        _sub.IsVisible = !string.IsNullOrWhiteSpace(Subtitle);
    }
}

public class NVSettingsTile : ThemeAwareView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(NVSettingsTile), "Setting", propertyChanged: Refresh);
    public static readonly BindableProperty IsOnProperty = BindableProperty.Create(nameof(IsOn), typeof(bool), typeof(NVSettingsTile), false, BindingMode.TwoWay, propertyChanged: Refresh);
    readonly NVSwitch _switch = new();
    public NVSettingsTile()
    {
        _switch.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(NVSwitch.IsOn))
            {
                IsOn = _switch.IsOn;
            }
        };
        Content = _switch;
        ApplyTheme();
    }
    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
    public bool IsOn { get => (bool)GetValue(IsOnProperty); set => SetValue(IsOnProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVSettingsTile)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _switch.Text = Text;
        _switch.IsOn = IsOn;
    }
}

public class NVChipGroup : ThemeAwareView
{
    public static readonly BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(IList<string>), typeof(NVChipGroup), new List<string> { "All", "New" }, propertyChanged: Refresh);
    readonly NVWrapLayout _wrap = new();
    public NVChipGroup() { Content = _wrap; ApplyTheme(); }
    public IList<string> Items { get => (IList<string>)GetValue(ItemsProperty); set => SetValue(ItemsProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVChipGroup)b).ApplyTheme();
    protected override void ApplyTheme() => _wrap.Items = Items;
}

public class NVCheckList : ThemeAwareView
{
    public static readonly BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(IList<string>), typeof(NVCheckList), new List<string> { "One", "Two" }, propertyChanged: Refresh);
    readonly VerticalStackLayout _list = new() { Spacing = NVTokens.Space2 };
    public NVCheckList() { Content = _list; ApplyTheme(); }
    public IList<string> Items { get => (IList<string>)GetValue(ItemsProperty); set => SetValue(ItemsProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVCheckList)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _list.Children.Clear();
        foreach (var item in Items ?? [])
        {
            _list.Children.Add(new NVCheckBox { Text = item });
        }
    }
}

public class NVGroupedList : ThemeAwareView
{
    public static readonly BindableProperty GroupsProperty = BindableProperty.Create(nameof(Groups), typeof(IList<string>), typeof(NVGroupedList), new List<string> { "A", "B" }, propertyChanged: Refresh);
    readonly VerticalStackLayout _list = new() { Spacing = NVTokens.Space3 };
    public NVGroupedList() { Content = _list; ApplyTheme(); }
    public IList<string> Groups { get => (IList<string>)GetValue(GroupsProperty); set => SetValue(GroupsProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVGroupedList)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _list.Children.Clear();
        foreach (var group in Groups ?? [])
        {
            _list.Children.Add(new NVSectionHeader { Text = group });
            _list.Children.Add(new NVListTile { Title = $"{group} item", Kind = NVIconKind.User });
        }
    }
}

public class NVIndexBar : ThemeAwareView
{
    public static readonly BindableProperty SelectedProperty = BindableProperty.Create(nameof(Selected), typeof(string), typeof(NVIndexBar), "A", BindingMode.TwoWay, propertyChanged: Refresh);
    readonly VerticalStackLayout _col = new() { Spacing = 0 };
    public NVIndexBar() { Content = _col; ApplyTheme(); }
    public string Selected { get => (string)GetValue(SelectedProperty); set => SetValue(SelectedProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVIndexBar)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _col.Children.Clear();
        foreach (var letter in "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
        {
            var pick = letter.ToString();
            var label = new Label
            {
                Text = pick,
                FontFamily = NVTokens.FontSemiBold,
                FontSize = NVTokens.CaptionSize,
                TextColor = pick == Selected ? NVTheme.Current.Accent : NVTheme.Current.Muted,
                HorizontalTextAlignment = TextAlignment.Center
            };
            var tap = new TapGestureRecognizer();
            tap.Tapped += (_, _) => Selected = pick;
            label.GestureRecognizers.Add(tap);
            _col.Children.Add(label);
        }
    }
}

public class NVSwipeTile : NVListTile
{
    public NVSwipeTile()
    {
        Title = "Swipe";
        Subtitle = "Leading / trailing actions";
    }
}

public class NVSelectionBar : ThemeAwareView
{
    public static readonly BindableProperty CountProperty = BindableProperty.Create(nameof(Count), typeof(int), typeof(NVSelectionBar), 0, propertyChanged: Refresh);
    readonly NVHeading _label = new() { Role = NVTextRole.Body };
    readonly NVButton _clear = new() { Text = "Clear", Variant = NVButtonVariant.Ghost };
    public NVSelectionBar()
    {
        Content = new HorizontalStackLayout { Spacing = NVTokens.Space3, Children = { _label, _clear } };
        ApplyTheme();
    }
    public int Count { get => (int)GetValue(CountProperty); set => SetValue(CountProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVSelectionBar)b).ApplyTheme();
    protected override void ApplyTheme() => _label.Text = $"{Count} selected";
}

public class NVSkeletonList : ThemeAwareView
{
    public NVSkeletonList()
    {
        Content = new VerticalStackLayout
        {
            Spacing = NVTokens.Space2,
            Children = { new NVSkeleton(), new NVSkeleton(), new NVSkeleton() }
        };
        ApplyTheme();
    }
    protected override void ApplyTheme() { }
}

public class NVInfiniteFooter : ThemeAwareView
{
    readonly NVBusyIndicator _busy = new();
    readonly NVCaptionText _caption = new() { Text = "Loading more" };
    public NVInfiniteFooter()
    {
        Content = new HorizontalStackLayout { Spacing = NVTokens.Space2, Children = { _busy, _caption } };
        ApplyTheme();
    }
    protected override void ApplyTheme() { }
}
