namespace NuvyntraLabs.UIKit;

/// <summary>Lumina checkbox with optional label and indeterminate support.</summary>
public class NVCheckBox : ThemeAwareView
{
    public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
        nameof(IsChecked), typeof(bool?), typeof(NVCheckBox), false,
        BindingMode.TwoWay, propertyChanged: OnCheckedChanged);

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text), typeof(string), typeof(NVCheckBox), string.Empty,
        propertyChanged: OnTextChanged);

    readonly CheckBox _box = new();
    readonly Label _label = new();

    public NVCheckBox()
    {
        _box.CheckedChanged += (_, e) => IsChecked = e.Value;
        _label.VerticalTextAlignment = TextAlignment.Center;
        _label.FontFamily = NVTokens.FontRegular;
        _label.FontSize = NVTokens.BodySize;
        Content = new HorizontalStackLayout
        {
            Spacing = NVTokens.Space2,
            VerticalOptions = LayoutOptions.Center,
            Children = { _box, _label }
        };
        ApplyTheme();
    }

    public bool? IsChecked
    {
        get => (bool?)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    static void OnCheckedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is NVCheckBox box)
        {
            box._box.IsChecked = newValue as bool? ?? false;
        }
    }

    static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is NVCheckBox box)
        {
            box._label.Text = newValue as string ?? string.Empty;
            box._label.IsVisible = !string.IsNullOrWhiteSpace(box._label.Text);
        }
    }

    protected override void ApplyTheme()
    {
        var theme = NVTheme.Current;
        _box.Color = theme.Accent;
        _label.TextColor = theme.Ink;
        _label.FontSize = NVTokens.Type(NVTokens.BodySize);
        _label.Text = Text;
        _label.IsVisible = !string.IsNullOrWhiteSpace(Text);
        _box.IsChecked = IsChecked ?? false;
    }
}
