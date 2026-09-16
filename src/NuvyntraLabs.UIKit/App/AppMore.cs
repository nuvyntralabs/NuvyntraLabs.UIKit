namespace NuvyntraLabs.UIKit;

/// <summary>FAB that fans out 2–5 actions.</summary>
public class NVSpeedDial : ThemeAwareView
{
    public static readonly BindableProperty ActionsProperty = BindableProperty.Create(nameof(Actions), typeof(IList<NVSpeedDialAction>), typeof(NVSpeedDial), new List<NVSpeedDialAction>(), propertyChanged: Refresh);
    public static readonly BindableProperty IsOpenProperty = BindableProperty.Create(nameof(IsOpen), typeof(bool), typeof(NVSpeedDial), false, BindingMode.TwoWay, propertyChanged: Refresh);

    readonly VerticalStackLayout _fan = new() { Spacing = NVTokens.Space2 };
    readonly NVFloatingActionButton _fab = new();

    public NVSpeedDial()
    {
        _fab.Command = new Command(() => IsOpen = !IsOpen, () => IsEnabled);
        Content = new VerticalStackLayout { Spacing = NVTokens.Space2, HorizontalOptions = LayoutOptions.End, Children = { _fan, _fab } };
        NVAccessibility.Name(this, "Speed dial", "Fan out actions");
        ApplyTheme();
    }

    public IList<NVSpeedDialAction> Actions { get => (IList<NVSpeedDialAction>)GetValue(ActionsProperty); set => SetValue(ActionsProperty, value); }
    public bool IsOpen { get => (bool)GetValue(IsOpenProperty); set => SetValue(IsOpenProperty, value); }
    public IReadOnlyList<NVSpeedDialAction> VisibleActions { get; private set; } = [];

    public static IReadOnlyList<NVSpeedDialAction> Clamp(IEnumerable<NVSpeedDialAction>? actions) =>
        (actions ?? []).Take(5).ToList();

    static void Refresh(BindableObject b, object o, object n) => ((NVSpeedDial)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        VisibleActions = Clamp(Actions);
        _fan.Children.Clear();
        _fan.IsVisible = IsOpen;
        if (IsOpen)
        {
            foreach (var action in VisibleActions)
            {
                _fan.Children.Add(new NVButton
                {
                    Text = action.Text,
                    Variant = NVButtonVariant.Tonal,
                    Command = action.Command,
                    IsEnabled = IsEnabled
                });
            }
        }

        _fab.IsEnabled = IsEnabled;
    }
}

/// <summary>One plan: name, price, feature bullets, CTA. Hosted by <see cref="NVPaywall"/>.</summary>
public class NVSubscriptionCard : ThemeAwareView
{
    public static readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(NVSubscriptionCard), "Plus", propertyChanged: Refresh);
    public static readonly BindableProperty PriceProperty = BindableProperty.Create(nameof(Price), typeof(string), typeof(NVSubscriptionCard), "$8 / mo", propertyChanged: Refresh);
    public static readonly BindableProperty FeaturesProperty = BindableProperty.Create(nameof(Features), typeof(IList<string>), typeof(NVSubscriptionCard), new List<string> { "Palette", "Coach" }, propertyChanged: Refresh);
    public static readonly BindableProperty CtaTextProperty = BindableProperty.Create(nameof(CtaText), typeof(string), typeof(NVSubscriptionCard), "Choose", propertyChanged: Refresh);
    public static readonly BindableProperty CtaCommandProperty = BindableProperty.Create(nameof(CtaCommand), typeof(ICommand), typeof(NVSubscriptionCard));

    readonly NVHeading _name = new();
    readonly NVHeading _price = new() { Role = NVTextRole.Body };
    readonly NVBulletList _bullets = new();
    readonly NVButton _cta = new() { Variant = NVButtonVariant.Filled };

    public NVSubscriptionCard()
    {
        _cta.Command = new Command(
            () =>
            {
                if (IsEnabled && CtaCommand?.CanExecute(null) == true)
                {
                    CtaCommand.Execute(null);
                }
            },
            () => IsEnabled && (CtaCommand is null || CtaCommand.CanExecute(null)));
        Content = new Border
        {
            Padding = NVTokens.Space4,
            StrokeThickness = 1,
            Content = new VerticalStackLayout { Spacing = NVTokens.Space3, Children = { _name, _price, _bullets, _cta } }
        };
        NVAccessibility.Name(this, "Subscription card", "Choose a plan");
        ApplyTheme();
    }

    public string Name { get => (string)GetValue(NameProperty); set => SetValue(NameProperty, value); }
    public string Price { get => (string)GetValue(PriceProperty); set => SetValue(PriceProperty, value); }
    public IList<string> Features { get => (IList<string>)GetValue(FeaturesProperty); set => SetValue(FeaturesProperty, value); }
    public string CtaText { get => (string)GetValue(CtaTextProperty); set => SetValue(CtaTextProperty, value); }
    public ICommand? CtaCommand { get => (ICommand?)GetValue(CtaCommandProperty); set => SetValue(CtaCommandProperty, value); }

    static void Refresh(BindableObject b, object o, object n) => ((NVSubscriptionCard)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _name.Text = Name;
        _price.Text = Price;
        _bullets.Items = Features ?? [];
        _cta.Text = CtaText;
        _cta.IsEnabled = IsEnabled && (CtaCommand is null || CtaCommand.CanExecute(null));
        if (Content is Border border)
        {
            border.Stroke = NVTheme.Current.Fog;
            border.BackgroundColor = NVTheme.Current.Surface;
            border.StrokeShape = new RoundRectangle { CornerRadius = NVTokens.RadiusMedium };
        }
    }
}

/// <summary>Searchable glyph grid for composer / chat.</summary>
public class NVEmojiPicker : ThemeAwareView
{
    public static readonly BindableProperty QueryProperty = BindableProperty.Create(nameof(Query), typeof(string), typeof(NVEmojiPicker), "", BindingMode.TwoWay, propertyChanged: Refresh);
    public static readonly BindableProperty GlyphsProperty = BindableProperty.Create(nameof(Glyphs), typeof(IList<string>), typeof(NVEmojiPicker), DefaultGlyphs(), propertyChanged: Refresh);
    public static readonly BindableProperty SelectedProperty = BindableProperty.Create(nameof(Selected), typeof(string), typeof(NVEmojiPicker), "", BindingMode.TwoWay, propertyChanged: Refresh);

    readonly NVSearchBar _search = new() { Label = "Search emoji" };
    readonly HorizontalStackLayout _grid = new() { Spacing = NVTokens.Space2 };

    public NVEmojiPicker()
    {
        _search.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(NVTextField.Text))
            {
                Query = _search.Text;
            }
        };
        Content = new VerticalStackLayout { Spacing = NVTokens.Space3, Children = { _search, _grid } };
        NVAccessibility.Name(this, "Emoji picker", "Search and pick a glyph");
        ApplyTheme();
    }

    public string Query { get => (string)GetValue(QueryProperty); set => SetValue(QueryProperty, value); }
    public IList<string> Glyphs { get => (IList<string>)GetValue(GlyphsProperty); set => SetValue(GlyphsProperty, value); }
    public string Selected { get => (string)GetValue(SelectedProperty); set => SetValue(SelectedProperty, value); }
    public IReadOnlyList<string> VisibleGlyphs { get; private set; } = [];

    public static IReadOnlyList<string> Filter(IEnumerable<string>? glyphs, string? query)
    {
        var items = (glyphs ?? []).ToList();
        if (string.IsNullOrWhiteSpace(query))
        {
            return items;
        }

        return items.Where(g => g.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    static List<string> DefaultGlyphs() =>
        ["😀", "😂", "😍", "👍", "🎉", "🔥", "💡", "❤️", "✨", "🙌"];

    static void Refresh(BindableObject b, object o, object n) => ((NVEmojiPicker)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _search.Text = Query;
        VisibleGlyphs = Filter(Glyphs, Query);
        _grid.Children.Clear();
        foreach (var glyph in VisibleGlyphs)
        {
            var pick = glyph;
            var chip = new NVChip { Text = pick, Kind = NVChipKind.Choice, IsSelected = pick == Selected };
            var tap = new TapGestureRecognizer();
            tap.Tapped += (_, _) => Selected = pick;
            chip.GestureRecognizers.Add(tap);
            _grid.Children.Add(chip);
        }
    }
}
