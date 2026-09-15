namespace NuvyntraLabs.UIKit;

public class NVIconButton : NVButton
{
    public static readonly BindableProperty KindProperty =
        BindableProperty.Create(nameof(Kind), typeof(NVIconKind), typeof(NVIconButton), NVIconKind.Plus, propertyChanged: OnKind);

    public NVIconButton()
    {
        Variant = NVButtonVariant.Ghost;
        Text = NVIcons.Glyph(Kind);
    }

    public NVIconKind Kind
    {
        get => (NVIconKind)GetValue(KindProperty);
        set => SetValue(KindProperty, value);
    }

    static void OnKind(BindableObject b, object o, object n)
    {
        if (b is NVIconButton button)
        {
            button.Text = NVIcons.Glyph(button.Kind);
        }
    }
}

public class NVToggleButton : ThemeAwareView
{
    public static readonly BindableProperty IsOnProperty =
        BindableProperty.Create(nameof(IsOn), typeof(bool), typeof(NVToggleButton), false, BindingMode.TwoWay, propertyChanged: Refresh);

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(NVToggleButton), "", propertyChanged: Refresh);

    readonly NVButton _button = new();

    public NVToggleButton()
    {
        _button.Command = new Command(() => IsOn = !IsOn);
        Content = _button;
        ApplyTheme();
    }

    public bool IsOn
    {
        get => (bool)GetValue(IsOnProperty);
        set => SetValue(IsOnProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    static void Refresh(BindableObject b, object o, object n) => ((NVToggleButton)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _button.Text = Text;
        _button.Variant = IsOn ? NVButtonVariant.Filled : NVButtonVariant.Outline;
    }
}

public class NVDropDownButton : ThemeAwareView
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(NVDropDownButton), "Menu", propertyChanged: Refresh);

    public static readonly BindableProperty ItemsProperty =
        BindableProperty.Create(nameof(Items), typeof(IList<string>), typeof(NVDropDownButton), new List<string>());

    readonly NVButton _button = new() { Variant = NVButtonVariant.Tonal };

    public NVDropDownButton()
    {
        _button.Command = new Command(async () =>
        {
            if (Application.Current?.Windows.FirstOrDefault()?.Page is not Page page)
            {
                return;
            }

            var pick = await page.DisplayActionSheetAsync(Text, "Cancel", null, Items?.ToArray() ?? []);
            if (!string.IsNullOrEmpty(pick) && pick != "Cancel")
            {
                Text = pick;
            }
        });
        Content = _button;
        ApplyTheme();
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public IList<string> Items
    {
        get => (IList<string>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    static void Refresh(BindableObject b, object o, object n) => ((NVDropDownButton)b).ApplyTheme();

    protected override void ApplyTheme() => _button.Text = $"{Text} ▾";
}

public class NVSwitch : ThemeAwareView
{
    public static readonly BindableProperty IsOnProperty =
        BindableProperty.Create(nameof(IsOn), typeof(bool), typeof(NVSwitch), false, BindingMode.TwoWay, propertyChanged: Refresh);

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(NVSwitch), "", propertyChanged: Refresh);

    readonly Switch _switch = new();
    readonly Label _label = new();

    public NVSwitch()
    {
        _switch.Toggled += (_, e) => IsOn = e.Value;
        Content = new HorizontalStackLayout { Spacing = NVTokens.Space2, Children = { _switch, _label } };
        ApplyTheme();
    }

    public bool IsOn
    {
        get => (bool)GetValue(IsOnProperty);
        set => SetValue(IsOnProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    static void Refresh(BindableObject b, object o, object n) => ((NVSwitch)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _switch.IsToggled = IsOn;
        _switch.OnColor = NVTheme.Current.Accent;
        _label.Text = Text;
        _label.TextColor = NVTheme.Current.Ink;
        _label.FontFamily = NVTokens.FontRegular;
    }
}

public class NVChip : ThemeAwareView
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(NVChip), "", propertyChanged: Refresh);

    public static readonly BindableProperty KindProperty =
        BindableProperty.Create(nameof(Kind), typeof(NVChipKind), typeof(NVChip), NVChipKind.Filter, propertyChanged: Refresh);

    public static readonly BindableProperty IsSelectedProperty =
        BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(NVChip), false, BindingMode.TwoWay, propertyChanged: Refresh);

    readonly Label _label = new() { Padding = new Thickness(NVTokens.Space3, NVTokens.Space1) };

    public NVChip()
    {
        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, _) => IsSelected = !IsSelected;
        _label.GestureRecognizers.Add(tap);
        Content = _label;
        ApplyTheme();
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public NVChipKind Kind
    {
        get => (NVChipKind)GetValue(KindProperty);
        set => SetValue(KindProperty, value);
    }

    public bool IsSelected
    {
        get => (bool)GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    static void Refresh(BindableObject b, object o, object n) => ((NVChip)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        var theme = NVTheme.Current;
        _label.Text = Text;
        _label.FontFamily = NVTokens.FontRegular;
        _label.BackgroundColor = IsSelected ? theme.Accent : theme.Mist;
        _label.TextColor = IsSelected ? theme.OnAccent : theme.Ink;
    }
}

public class NVSegmentedControl : ThemeAwareView
{
    public static readonly BindableProperty ItemsProperty =
        BindableProperty.Create(nameof(Items), typeof(IList<string>), typeof(NVSegmentedControl), new List<string> { "A", "B" }, propertyChanged: Refresh);

    public static readonly BindableProperty SelectedIndexProperty =
        BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(NVSegmentedControl), 0, BindingMode.TwoWay, propertyChanged: Refresh);

    readonly HorizontalStackLayout _row = new() { Spacing = 0 };

    public NVSegmentedControl()
    {
        Content = _row;
        ApplyTheme();
    }

    public IList<string> Items
    {
        get => (IList<string>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public int SelectedIndex
    {
        get => (int)GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    static void Refresh(BindableObject b, object o, object n) => ((NVSegmentedControl)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _row.Children.Clear();
        var items = Items ?? [];
        var index = items.Count == 0 ? 0 : Math.Clamp(SelectedIndex, 0, items.Count - 1);
        for (var i = 0; i < items.Count; i++)
        {
            var captured = i;
            var chip = new NVChip { Text = items[i], IsSelected = i == index };
            chip.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(NVChip.IsSelected) && chip.IsSelected)
                {
                    SelectedIndex = captured;
                }
            };
            _row.Children.Add(chip);
        }
    }
}

public class NVSpeechToTextButton : NVIconButton
{
    public NVSpeechToTextButton()
    {
        Kind = NVIconKind.Mic;
        Variant = NVButtonVariant.Tonal;
    }
}
