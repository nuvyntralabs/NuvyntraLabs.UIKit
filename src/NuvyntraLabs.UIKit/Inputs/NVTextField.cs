namespace NuvyntraLabs.UIKit;

/// <summary>Labeled text field with helper and error slots (FieldChrome).</summary>
public class NVTextField : ThemeAwareView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text), typeof(string), typeof(NVTextField), string.Empty,
        BindingMode.TwoWay, propertyChanged: OnTextChanged);

    public static readonly BindableProperty LabelProperty = BindableProperty.Create(
        nameof(Label), typeof(string), typeof(NVTextField), string.Empty,
        propertyChanged: OnChromeChanged);

    public static readonly BindableProperty HelperProperty = BindableProperty.Create(
        nameof(Helper), typeof(string), typeof(NVTextField), string.Empty,
        propertyChanged: OnChromeChanged);

    public static readonly BindableProperty ErrorProperty = BindableProperty.Create(
        nameof(Error), typeof(string), typeof(NVTextField), string.Empty,
        propertyChanged: OnChromeChanged);

    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
        nameof(Placeholder), typeof(string), typeof(NVTextField), string.Empty,
        propertyChanged: OnPlaceholderChanged);

    public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(
        nameof(IsPassword), typeof(bool), typeof(NVTextField), false,
        propertyChanged: OnPasswordChanged);

    readonly Label _label = new();
    readonly Entry _entry = new();
    readonly Label _hint = new();
    readonly Border _chrome = new();

    public NVTextField()
    {
        _label.FontFamily = NVTokens.FontSemiBold;
        _label.FontSize = NVTokens.LabelSize;
        _entry.FontFamily = NVTokens.FontRegular;
        _entry.FontSize = NVTokens.BodySize;
        _entry.BackgroundColor = Colors.Transparent;
        _entry.TextChanged += (_, e) => Text = e.NewTextValue;
        _hint.FontFamily = NVTokens.FontRegular;
        _hint.FontSize = NVTokens.CaptionSize;
        _chrome.Padding = new Thickness(NVTokens.Space3, NVTokens.Space2);
        _chrome.StrokeThickness = 1;
        _chrome.Content = _entry;
        Content = new VerticalStackLayout
        {
            Spacing = NVTokens.Space1,
            Children = { _label, _chrome, _hint }
        };
        ApplyTheme();
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public string Helper
    {
        get => (string)GetValue(HelperProperty);
        set => SetValue(HelperProperty, value);
    }

    public string Error
    {
        get => (string)GetValue(ErrorProperty);
        set => SetValue(ErrorProperty, value);
    }

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }

    public bool HasError => !string.IsNullOrWhiteSpace(Error);

    static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is NVTextField field && field._entry.Text != (newValue as string ?? string.Empty))
        {
            field._entry.Text = newValue as string ?? string.Empty;
        }
    }

    static void OnPlaceholderChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is NVTextField field)
        {
            field._entry.Placeholder = newValue as string ?? string.Empty;
        }
    }

    static void OnPasswordChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is NVTextField field)
        {
            field._entry.IsPassword = newValue is true;
        }
    }

    static void OnChromeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is NVTextField field)
        {
            field.ApplyTheme();
        }
    }

    protected override void ApplyTheme()
    {
        var theme = NVTheme.Current;
        var error = HasError;
        _label.Text = Label;
        _label.IsVisible = !string.IsNullOrWhiteSpace(Label);
        _label.TextColor = theme.Muted;
        _entry.TextColor = theme.Ink;
        _entry.PlaceholderColor = theme.Fog;
        _entry.Placeholder = Placeholder;
        _entry.IsPassword = IsPassword;
        if (_entry.Text != Text)
        {
            _entry.Text = Text;
        }

        _chrome.BackgroundColor = theme.Surface;
        _chrome.Stroke = error ? theme.Danger : theme.Fog;
        _chrome.StrokeShape = new RoundRectangle { CornerRadius = NVTokens.RadiusSmall };
        _hint.Text = error ? Error : Helper;
        _hint.TextColor = error ? theme.Danger : theme.Muted;
        _hint.IsVisible = !string.IsNullOrWhiteSpace(_hint.Text);
    }
}
