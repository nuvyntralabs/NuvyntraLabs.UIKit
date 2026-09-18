namespace NuvyntraLabs.UIKit;

public class NVCard : ThemeAwareView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(NVCard), "", propertyChanged: Refresh);
    public static readonly BindableProperty BodyProperty = BindableProperty.Create(nameof(Body), typeof(string), typeof(NVCard), "", propertyChanged: Refresh);
    readonly Label _title = new() { FontFamily = NVTokens.FontSemiBold, FontSize = 18 };
    readonly Label _body = new() { FontFamily = NVTokens.FontRegular };

    public NVCard()
    {
        _title.FontFamily = NVTokens.FontSemiBold;
        _body.FontFamily = NVTokens.FontRegular;
        Content = new Border
        {
            Padding = NVTokens.Space4,
            StrokeThickness = 0,
            Content = new VerticalStackLayout { Spacing = NVTokens.Space2, Children = { _title, _body } }
        };
        ApplyTheme();
    }

    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    public string Body { get => (string)GetValue(BodyProperty); set => SetValue(BodyProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVCard)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _title.Text = Title;
        _body.Text = Body;
        _title.TextColor = NVTheme.Current.Ink;
        _title.FontSize = NVTokens.Type(NVTokens.TitleSize);
        _body.TextColor = NVTheme.Current.Muted;
        _body.FontSize = NVTokens.Type(NVTokens.BodySize);
        if (Content is Border border)
        {
            border.BackgroundColor = NVTheme.Current.Surface;
            border.StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = NVTokens.RadiusLarge };
        }
    }
}

public class NVExpander : ThemeAwareView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(NVExpander), "Section", propertyChanged: Refresh);
    public static readonly BindableProperty IsExpandedProperty = BindableProperty.Create(nameof(IsExpanded), typeof(bool), typeof(NVExpander), false, BindingMode.TwoWay, propertyChanged: Refresh);
    readonly Label _title = new() { FontFamily = NVTokens.FontSemiBold };
    readonly ContentView _panel = new();
    readonly Label _fallback = new() { FontFamily = NVTokens.FontRegular, Text = "Expanded content" };

    public NVExpander()
    {
        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, _) => IsExpanded = !IsExpanded;
        _title.GestureRecognizers.Add(tap);
        _panel.Content = _fallback;
        Content = new VerticalStackLayout { Spacing = NVTokens.Space2, Children = { _title, _panel } };
        ApplyTheme();
    }

    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    public bool IsExpanded { get => (bool)GetValue(IsExpandedProperty); set => SetValue(IsExpandedProperty, value); }
    public View? Panel
    {
        get => ReferenceEquals(_panel.Content, _fallback) ? null : _panel.Content;
        set => _panel.Content = value ?? _fallback;
    }
    static void Refresh(BindableObject b, object o, object n) => ((NVExpander)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _title.Text = NVIcons.Glyph(IsExpanded ? NVIconKind.ChevronDown : NVIconKind.ChevronRight, NVTheme.Current.FlowDirection) + " " + Title;
        _title.FontSize = NVTokens.Type(NVTokens.TitleSize);
        _title.TextColor = NVTheme.Current.Ink;
        _fallback.TextColor = NVTheme.Current.Muted;
        _panel.IsVisible = IsExpanded;
    }
}

public class NVAccordion : NVExpander { }

public class NVTabView : ThemeAwareView
{
    public static readonly BindableProperty TabsProperty = BindableProperty.Create(nameof(Tabs), typeof(IList<string>), typeof(NVTabView), new List<string> { "One", "Two" }, propertyChanged: Refresh);
    public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(NVTabView), 0, BindingMode.TwoWay, propertyChanged: Refresh);
    readonly NVSegmentedControl _tabs = new();
    readonly Label _body = new() { FontFamily = NVTokens.FontRegular };

    public NVTabView()
    {
        _tabs.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(NVSegmentedControl.SelectedIndex))
            {
                SelectedIndex = _tabs.SelectedIndex;
            }
        };
        Content = new VerticalStackLayout { Spacing = NVTokens.Space3, Children = { _tabs, _body } };
        ApplyTheme();
    }

    public IList<string> Tabs { get => (IList<string>)GetValue(TabsProperty); set => SetValue(TabsProperty, value); }
    public int SelectedIndex { get => (int)GetValue(SelectedIndexProperty); set => SetValue(SelectedIndexProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVTabView)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _tabs.Items = Tabs;
        _tabs.SelectedIndex = SelectedIndex;
        var name = Tabs is { Count: > 0 } ? Tabs[Math.Clamp(SelectedIndex, 0, Tabs.Count - 1)] : "";
        _body.Text = string.IsNullOrEmpty(name) ? "" : $"{name} panel";
        _body.TextColor = NVTheme.Current.Ink;
        _body.Padding = new Thickness(NVTokens.Space3);
        _body.BackgroundColor = NVTheme.Current.Mist;
    }
}

public class NVBottomNavigation : NVTabView { }
public class NVNavigationDrawer : ThemeAwareView
{
    public static readonly BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(IList<string>), typeof(NVNavigationDrawer), new List<string> { "Home", "Settings" }, propertyChanged: Refresh);
    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(string), typeof(NVNavigationDrawer), "Home", BindingMode.TwoWay, propertyChanged: Refresh);
    readonly VerticalStackLayout _list = new() { Spacing = NVTokens.Space1 };
    public NVNavigationDrawer() { Content = _list; ApplyTheme(); }
    public IList<string> Items { get => (IList<string>)GetValue(ItemsProperty); set => SetValue(ItemsProperty, value); }
    public string SelectedItem { get => (string)GetValue(SelectedItemProperty); set => SetValue(SelectedItemProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVNavigationDrawer)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _list.Children.Clear();
        foreach (var item in Items ?? [])
        {
            var pick = item;
            var tile = new NVListTile { Title = pick, Kind = NVIconKind.Home };
            tile.BackgroundColor = pick == SelectedItem ? NVTheme.Current.Mist : Colors.Transparent;
            var tap = new TapGestureRecognizer();
            tap.Tapped += (_, _) => SelectedItem = pick;
            tile.GestureRecognizers.Add(tap);
            _list.Children.Add(tile);
        }
    }
}

public class NVSideDrawer : NVNavigationDrawer { }

public class NVToolbar : ThemeAwareView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(NVToolbar), "Lumina", propertyChanged: Refresh);
    readonly Label _title = new() { FontFamily = NVTokens.FontSemiBold, FontSize = 20 };
    public NVToolbar()
    {
        Content = new HorizontalStackLayout { Spacing = NVTokens.Space3, Children = { new NVIcon { Kind = NVIconKind.Menu }, _title } };
        ApplyTheme();
    }
    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVToolbar)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _title.Text = Title;
        _title.TextColor = NVTheme.Current.Ink;
        _title.FontSize = NVTokens.Type(NVTokens.TitleSize);
    }
}

public class NVAppBar : NVToolbar { }

public class NVBreadcrumb : ThemeAwareView
{
    public static readonly BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(IList<string>), typeof(NVBreadcrumb), new List<string> { "Home", "Kit" }, propertyChanged: Refresh);
    readonly Label _label = new() { FontFamily = NVTokens.FontRegular };
    public NVBreadcrumb() { Content = _label; ApplyTheme(); }
    public IList<string> Items { get => (IList<string>)GetValue(ItemsProperty); set => SetValue(ItemsProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVBreadcrumb)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _label.Text = string.Join(" / ", Items ?? []);
        _label.TextColor = NVTheme.Current.Muted;
    }
}

public class NVStepper : ThemeAwareView
{
    public static readonly BindableProperty StepsProperty = BindableProperty.Create(nameof(Steps), typeof(IList<string>), typeof(NVStepper), new List<string> { "One", "Two", "Three" }, propertyChanged: Refresh);
    public static readonly BindableProperty IndexProperty = BindableProperty.Create(nameof(Index), typeof(int), typeof(NVStepper), 0, propertyChanged: Refresh);
    readonly Label _label = new() { FontFamily = NVTokens.FontRegular };
    public NVStepper() { Content = _label; ApplyTheme(); }
    public IList<string> Steps { get => (IList<string>)GetValue(StepsProperty); set => SetValue(StepsProperty, value); }
    public int Index { get => (int)GetValue(IndexProperty); set => SetValue(IndexProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVStepper)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        var i = Math.Clamp(Index, 0, Math.Max(0, (Steps?.Count ?? 1) - 1));
        _label.Text = Steps is { Count: > 0 } ? $"Step {i + 1}: {Steps[i]}" : "";
        _label.TextColor = NVTheme.Current.Ink;
    }
}

public class NVCarousel : ThemeAwareView
{
    public static readonly BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(IList<string>), typeof(NVCarousel), new List<string> { "A", "B", "C" }, propertyChanged: Refresh);
    public static readonly BindableProperty IndexProperty = BindableProperty.Create(nameof(Index), typeof(int), typeof(NVCarousel), 0, BindingMode.TwoWay, propertyChanged: Refresh);
    readonly NVCard _card = new();
    readonly NVDotIndicator _dots = new();
    readonly SwipeGestureRecognizer _advanceSwipe;
    public NVCarousel()
    {
        var prev = new NVIconButton { Kind = NVIconKind.ChevronRight, Rotation = 180 };
        var next = new NVIconButton { Kind = NVIconKind.ChevronRight };
        prev.Command = new Command(() => Index = Math.Max(0, Index - 1));
        next.Command = new Command(() => Index = Math.Min(Index + 1, Math.Max(0, (Items?.Count ?? 1) - 1)));
        var swipe = new SwipeGestureRecognizer { Direction = NVTokens.AdvanceSwipe };
        swipe.Swiped += (_, _) => Index = Math.Min(Index + 1, Math.Max(0, (Items?.Count ?? 1) - 1));
        GestureRecognizers.Add(swipe);
        _advanceSwipe = swipe;
        Content = new VerticalStackLayout
        {
            Spacing = NVTokens.Space2,
            Children =
            {
                new Grid
                {
                    ColumnDefinitions = new ColumnDefinitionCollection { new(GridLength.Auto), new(GridLength.Star), new(GridLength.Auto) },
                    Children = { prev, _card, next }
                },
                _dots
            }
        };
        Grid.SetColumn(_card, 1);
        Grid.SetColumn(next, 2);
        ApplyTheme();
    }
    public IList<string> Items { get => (IList<string>)GetValue(ItemsProperty); set => SetValue(ItemsProperty, value); }
    public int Index { get => (int)GetValue(IndexProperty); set => SetValue(IndexProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVCarousel)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        var text = Items is { Count: > 0 } ? Items[Math.Clamp(Index, 0, Items.Count - 1)] : "";
        _card.Title = text;
        _card.Body = $"{Index + 1} / {Items?.Count ?? 0}";
        _dots.Count = Math.Max(1, Items?.Count ?? 1);
        _dots.Index = Index;
        _advanceSwipe.Direction = NVTokens.AdvanceSwipe;
    }
}

public class NVSlideView : NVCarousel { }
public class NVParallaxView : NVCarousel { }

public class NVBackdrop : ThemeAwareView
{
    public NVBackdrop()
    {
        Content = new Grid
        {
            Children =
            {
                new BoxView { Color = NVTheme.Current.Mist },
                new NVCard { Title = "Front", Body = "Backdrop layer" }
            }
        };
        ApplyTheme();
    }

    protected override void ApplyTheme()
    {
        if (Content is Grid grid && grid.Children.FirstOrDefault() is BoxView back)
        {
            back.Color = NVTheme.Current.Mist;
        }
    }
}

public class NVDockLayout : ThemeAwareView
{
    public NVDockLayout()
    {
        Content = new Grid
        {
            RowDefinitions = new RowDefinitionCollection
            {
                new(GridLength.Auto),
                new(GridLength.Star),
                new(GridLength.Auto)
            }
        };
        var top = new NVToolbar();
        var body = new NVSurface { Content = new Label { Text = "Dock body", TextColor = NVTheme.Current.Muted } };
        var bottom = new NVBottomNavigation();
        Grid.SetRow(top, 0);
        Grid.SetRow(body, 1);
        Grid.SetRow(bottom, 2);
        ((Grid)Content).Add(top);
        ((Grid)Content).Add(body);
        ((Grid)Content).Add(bottom);
        ApplyTheme();
    }

    protected override void ApplyTheme() { }
}

public class NVRadialMenu : ThemeAwareView
{
    public static readonly BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(IList<string>), typeof(NVRadialMenu), new List<string> { "Edit", "Share", "Delete" }, propertyChanged: Refresh);
    readonly AbsoluteLayout _abs = new() { HeightRequest = 180, WidthRequest = 180 };
    public NVRadialMenu() { Content = _abs; ApplyTheme(); }
    public IList<string> Items { get => (IList<string>)GetValue(ItemsProperty); set => SetValue(ItemsProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVRadialMenu)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _abs.Children.Clear();
        var items = (Items ?? []).ToList();
        var center = new NVFloatingActionButton();
        AbsoluteLayout.SetLayoutBounds(center, new Rect(70, 70, 40, 40));
        _abs.Children.Add(center);
        for (var i = 0; i < items.Count; i++)
        {
            var angle = i * Math.PI * 2 / Math.Max(1, items.Count) - Math.PI / 2;
            var chip = new NVChip { Text = items[i] };
            AbsoluteLayout.SetLayoutBounds(chip, new Rect(70 + 62 * Math.Cos(angle), 70 + 62 * Math.Sin(angle), 80, 32));
            _abs.Children.Add(chip);
        }
    }
}

public class NVBottomSheet : OverlayHost
{
    public NVBottomSheet()
    {
        Placement = OverlayPlacement.Bottom;
        PanelContent = new VerticalStackLayout
        {
            Spacing = NVTokens.Space3,
            Children =
            {
                new NVHeading { Text = "Sheet" },
                new NVBodyText { Text = "Detent over the current page." },
                new NVButton { Text = "Done", Variant = NVButtonVariant.Filled, Command = new Command(() => IsOpen = false) }
            }
        };
    }
}

public class NVNavigationView : ThemeAwareView
{
    readonly NVToolbar _bar = new() { Title = "Lumina" };
    readonly NVNavigationDrawer _drawer = new();
    public NVNavigationView()
    {
        Content = new Grid
        {
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new(new GridLength(200)),
                new(GridLength.Star)
            },
            Children = { _drawer }
        };
        Grid.SetColumn(_drawer, 0);
        var body = new VerticalStackLayout { Children = { _bar, new NVCard { Title = "Home", Body = "Adaptive rail — drawer on phone, rail on tablet." } } };
        Grid.SetColumn(body, 1);
        ((Grid)Content).Add(body);
        ApplyTheme();
    }
    protected override void ApplyTheme() { }
}

public class NVWrapLayout : ThemeAwareView
{
    public static readonly BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(IList<string>), typeof(NVWrapLayout), new List<string> { "One", "Two", "Three" }, propertyChanged: Refresh);
    readonly FlexLayout _flex = new() { Wrap = Microsoft.Maui.Layouts.FlexWrap.Wrap };
    public NVWrapLayout() { Content = _flex; ApplyTheme(); }
    public IList<string> Items { get => (IList<string>)GetValue(ItemsProperty); set => SetValue(ItemsProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVWrapLayout)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _flex.Children.Clear();
        foreach (var item in Items ?? [])
        {
            _flex.Children.Add(new NVChip { Text = item });
        }
    }
}

public class NVGridSplitter : ThemeAwareView
{
    public static readonly BindableProperty OffsetProperty = BindableProperty.Create(nameof(Offset), typeof(double), typeof(NVGridSplitter), 140d, BindingMode.TwoWay, propertyChanged: Refresh);
    readonly BoxView _bar = new() { WidthRequest = 8 };
    readonly NVCard _left = new() { Title = "Left", Body = "Pane" };
    readonly NVCard _right = new() { Title = "Right", Body = "Pane" };
    readonly Grid _grid = new();
    public NVGridSplitter()
    {
        _grid.ColumnDefinitions = new ColumnDefinitionCollection { new(new GridLength(140)), new(GridLength.Auto), new(GridLength.Star) };
        var pan = new PanGestureRecognizer();
        pan.PanUpdated += (_, e) =>
        {
            if (e.StatusType == GestureStatus.Running)
            {
                Offset = Math.Clamp(Offset + e.TotalX / 8, 80, 240);
            }
        };
        _bar.GestureRecognizers.Add(pan);
        _grid.Add(_left);
        Grid.SetColumn(_bar, 1);
        _grid.Add(_bar);
        Grid.SetColumn(_right, 2);
        _grid.Add(_right);
        Content = _grid;
        ApplyTheme();
    }
    public double Offset { get => (double)GetValue(OffsetProperty); set => SetValue(OffsetProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVGridSplitter)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _bar.Color = NVTheme.Current.Fog;
        _grid.ColumnDefinitions[0] = new ColumnDefinition(new GridLength(Offset));
    }
}
