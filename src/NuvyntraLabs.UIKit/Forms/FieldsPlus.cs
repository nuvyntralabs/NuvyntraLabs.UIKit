namespace NuvyntraLabs.UIKit;

/// <summary>Labeled single-line input. Same surface as <see cref="NVTextField"/>; set <see cref="NVTextField.Label"/> for email, name, or any other field.</summary>
public class NVInputField : NVTextField
{
}

public class NVPhoneField : NVMaskedEntry
{
    public NVPhoneField()
    {
        Label = "Phone";
        Mask = "+00 00000 00000";
        Placeholder = "+91";
    }
}

public class NVPasswordField : NVTextField
{
    public NVPasswordField()
    {
        Label = "Password";
        IsPassword = true;
        Placeholder = "••••••••";
    }
}

public class NVPasswordStrength : ThemeAwareView
{
    public static readonly BindableProperty PasswordProperty = BindableProperty.Create(nameof(Password), typeof(string), typeof(NVPasswordStrength), "", propertyChanged: Refresh);
    readonly NVProgressBar _bar = new();
    readonly NVCaptionText _caption = new();
    public NVPasswordStrength()
    {
        Content = new VerticalStackLayout { Spacing = NVTokens.Space1, Children = { _bar, _caption } };
        ApplyTheme();
    }
    public string Password { get => (string)GetValue(PasswordProperty); set => SetValue(PasswordProperty, value); }
    public int Score => NVPasswordRules.Score(Password);
    static void Refresh(BindableObject b, object o, object n) => ((NVPasswordStrength)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _bar.Value = Score / 4d;
        _caption.Text = NVPasswordRules.Caption(Score);
    }
}

public class NVQuantityStepper : NVNumericUpDown
{
    public NVQuantityStepper() => Value = 1;
}

public class NVDateRangePicker : ThemeAwareView
{
    public static readonly BindableProperty StartProperty = BindableProperty.Create(nameof(Start), typeof(DateTime), typeof(NVDateRangePicker), DateTime.Today, BindingMode.TwoWay, propertyChanged: Refresh);
    public static readonly BindableProperty EndProperty = BindableProperty.Create(nameof(End), typeof(DateTime), typeof(NVDateRangePicker), DateTime.Today.AddDays(2), BindingMode.TwoWay, propertyChanged: Refresh);
    readonly NVDatePicker _start = new() { Label = "From" };
    readonly NVDatePicker _end = new() { Label = "To" };
    public NVDateRangePicker()
    {
        _start.PropertyChanged += (_, e) => { if (e.PropertyName == nameof(NVDatePicker.Date)) Start = _start.Date; };
        _end.PropertyChanged += (_, e) => { if (e.PropertyName == nameof(NVDatePicker.Date)) End = _end.Date; };
        Content = new HorizontalStackLayout { Spacing = NVTokens.Space2, Children = { _start, _end } };
        ApplyTheme();
    }
    public DateTime Start { get => (DateTime)GetValue(StartProperty); set => SetValue(StartProperty, value); }
    public DateTime End { get => (DateTime)GetValue(EndProperty); set => SetValue(EndProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVDateRangePicker)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _start.Date = Start;
        _end.Date = End < Start ? Start : End;
    }
}

public class NVMonthYearPicker : NVDatePicker
{
    public NVMonthYearPicker() => Label = "Month";
}

public class NVFilterBar : NVChipGroup
{
    public NVFilterBar() => Items = new List<string> { "All", "Open", "Done" };
}

public class NVTagInput : NVChipGroup
{
    public NVTagInput() => Items = new List<string> { "maui", "lumina" };
}

public class NVPinPad : ThemeAwareView
{
    public static readonly BindableProperty CodeProperty = BindableProperty.Create(nameof(Code), typeof(string), typeof(NVPinPad), "", BindingMode.TwoWay, propertyChanged: Refresh);
    readonly NVOtpInput _otp = new() { Length = 4 };
    readonly NVWrapLayout _keys = new();
    public NVPinPad()
    {
        _otp.PropertyChanged += (_, e) => { if (e.PropertyName == nameof(NVOtpInput.Code)) Code = _otp.Code; };
        _keys.Items = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "⌫", "0", "OK" };
        _keys.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(NVWrapLayout.Items))
            {
                WireKeys();
            }
        };
        Content = new VerticalStackLayout { Spacing = NVTokens.Space3, Children = { _otp, _keys } };
        ApplyTheme();
        WireKeys();
    }
    public string Code { get => (string)GetValue(CodeProperty); set => SetValue(CodeProperty, value); }
    public void Press(string key)
    {
        if (key == "⌫")
        {
            Code = Code.Length == 0 ? "" : Code[..^1];
            return;
        }

        if (key == "OK" || key.Length != 1 || !char.IsDigit(key[0]) || Code.Length >= 4)
        {
            return;
        }

        Code += key;
    }
    static void Refresh(BindableObject b, object o, object n) => ((NVPinPad)b).ApplyTheme();
    protected override void ApplyTheme() => _otp.Code = Code;
    void WireKeys()
    {
        if (_keys.Content is not FlexLayout flex)
        {
            return;
        }

        foreach (var child in flex.Children.OfType<NVChip>())
        {
            var pick = child.Text;
            child.GestureRecognizers.Clear();
            var tap = new TapGestureRecognizer();
            tap.Tapped += (_, _) => Press(pick);
            child.GestureRecognizers.Add(tap);
        }
    }
}

public class NVCopyable : ThemeAwareView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(NVCopyable), "", propertyChanged: Refresh);
    public static readonly BindableProperty CopiedProperty = BindableProperty.Create(nameof(Copied), typeof(bool), typeof(NVCopyable), false, BindingMode.TwoWay, propertyChanged: Refresh);
    readonly NVHeading _text = new() { Role = NVTextRole.Body };
    readonly NVButton _copy = new() { Variant = NVButtonVariant.Ghost };
    public NVCopyable()
    {
        _copy.Command = new Command(async () =>
        {
            Copied = true;
            try
            {
                await Clipboard.Default.SetTextAsync(Text ?? "");
            }
            catch
            {
                // Host clipboard may be unavailable in tests.
            }
        });
        Content = new HorizontalStackLayout { Spacing = NVTokens.Space2, Children = { _text, _copy } };
        ApplyTheme();
    }
    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
    public bool Copied { get => (bool)GetValue(CopiedProperty); set => SetValue(CopiedProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVCopyable)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _text.Text = Text;
        _copy.Text = Copied ? "Copied" : "Copy";
    }
}

public class NVLink : ThemeAwareView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(NVLink), "Learn more", propertyChanged: Refresh);
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(NVLink));
    readonly Label _label = new() { FontFamily = NVTokens.FontSemiBold, FontSize = NVTokens.BodySize };
    public NVLink()
    {
        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, _) =>
        {
            if (Command?.CanExecute(null) == true)
            {
                Command.Execute(null);
            }
        };
        _label.GestureRecognizers.Add(tap);
        Content = _label;
        ApplyTheme();
    }
    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
    public ICommand? Command { get => (ICommand?)GetValue(CommandProperty); set => SetValue(CommandProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVLink)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _label.Text = Text;
        _label.TextColor = NVTheme.Current.Accent;
        _label.TextDecorations = TextDecorations.Underline;
    }
}

public class NVCountryPicker : NVComboBox
{
    public NVCountryPicker()
    {
        Label = "Country";
        Items = new List<string> { "India", "United States", "Germany", "Japan" };
    }
}

public class NVLanguagePicker : NVComboBox
{
    public NVLanguagePicker()
    {
        Label = "Language";
        Items = new List<string> { "English", "हिन्दी", "Español" };
    }
}

public class NVThemePicker : NVSegmentedControl
{
    public NVThemePicker()
    {
        Items = new List<string> { "Light", "Dark", "System" };
        PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(SelectedIndex))
            {
                NVTheme.Current.SetMode(SelectedIndex switch
                {
                    1 => NVThemeMode.Dark,
                    2 => NVThemeMode.System,
                    _ => NVThemeMode.Light
                });
            }
        };
    }
}
