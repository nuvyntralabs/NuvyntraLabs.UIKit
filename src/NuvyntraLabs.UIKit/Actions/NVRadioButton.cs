namespace NuvyntraLabs.UIKit;

/// <summary>Lumina radio button. Use <see cref="GroupName"/> or wrap in <see cref="NVRadioGroup"/>.</summary>
public class NVRadioButton : ThemeAwareView
{
    public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
        nameof(IsChecked), typeof(bool), typeof(NVRadioButton), false,
        BindingMode.TwoWay, propertyChanged: OnCheckedChanged);

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text), typeof(string), typeof(NVRadioButton), string.Empty,
        propertyChanged: OnTextChanged);

    public static readonly BindableProperty GroupNameProperty = BindableProperty.Create(
        nameof(GroupName), typeof(string), typeof(NVRadioButton), string.Empty,
        propertyChanged: OnGroupChanged);

    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        nameof(Value), typeof(object), typeof(NVRadioButton));

    readonly RadioButton _radio = new();

    public NVRadioButton()
    {
        _radio.CheckedChanged += (_, e) => IsChecked = e.Value;
        _radio.FontFamily = NVTokens.FontRegular;
        _radio.FontSize = NVTokens.BodySize;
        Content = _radio;
        ApplyTheme();
    }

    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string GroupName
    {
        get => (string)GetValue(GroupNameProperty);
        set => SetValue(GroupNameProperty, value);
    }

    public object? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    static void OnCheckedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is NVRadioButton radio)
        {
            radio._radio.IsChecked = newValue is true;
        }
    }

    static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is NVRadioButton radio)
        {
            radio._radio.Content = newValue as string ?? string.Empty;
        }
    }

    static void OnGroupChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is NVRadioButton radio)
        {
            radio._radio.GroupName = newValue as string ?? string.Empty;
        }
    }

    protected override void ApplyTheme()
    {
        var theme = NVTheme.Current;
        _radio.TextColor = theme.Ink;
        _radio.FontSize = NVTokens.Type(NVTokens.BodySize);
        _radio.Content = Text;
        _radio.GroupName = GroupName;
        _radio.IsChecked = IsChecked;
    }
}
