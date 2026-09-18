namespace NuvyntraLabs.UIKit;

/// <summary>Lumina button. Variants are styles, not extra types.</summary>
public class NVButton : ThemeAwareView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text), typeof(string), typeof(NVButton), string.Empty,
        propertyChanged: OnTextChanged);

    public static readonly BindableProperty VariantProperty = BindableProperty.Create(
        nameof(Variant), typeof(NVButtonVariant), typeof(NVButton), NVButtonVariant.Filled,
        propertyChanged: OnChromeChanged);

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command), typeof(ICommand), typeof(NVButton), null,
        propertyChanged: OnCommandChanged);

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
        nameof(CommandParameter), typeof(object), typeof(NVButton));

    readonly Button _button = new();

    public NVButton()
    {
        _button.FontFamily = NVTokens.FontSemiBold;
        _button.FontSize = NVTokens.BodySize;
        _button.CornerRadius = (int)NVTokens.RadiusMedium;
        _button.Padding = new Thickness(NVTokens.Space4, NVTokens.Space3);
        _button.Clicked += (_, _) =>
        {
            if (Command?.CanExecute(CommandParameter) == true)
            {
                Command.Execute(CommandParameter);
            }
        };
        Content = _button;
        ApplyTheme();
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public NVButtonVariant Variant
    {
        get => (NVButtonVariant)GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is NVButton button)
        {
            button._button.Text = newValue as string ?? string.Empty;
        }
    }

    static void OnChromeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is NVButton button)
        {
            button.ApplyTheme();
        }
    }

    static void OnCommandChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not NVButton button)
        {
            return;
        }

        if (oldValue is ICommand oldCommand)
        {
            oldCommand.CanExecuteChanged -= button.OnCanExecuteChanged;
        }

        if (newValue is ICommand newCommand)
        {
            newCommand.CanExecuteChanged += button.OnCanExecuteChanged;
        }

        button.ApplyEnabled();
    }

    void OnCanExecuteChanged(object? sender, EventArgs e) => ApplyEnabled();

    void ApplyEnabled()
    {
        var enabled = IsEnabled && (Command is null || Command.CanExecute(CommandParameter));
        _button.IsEnabled = enabled;
        Opacity = enabled ? 1 : 0.45;
    }

    protected override void ApplyTheme()
    {
        var theme = NVTheme.Current;
        _button.Text = Text;
        _button.FontSize = NVTokens.Type(NVTokens.BodySize);
        _button.MinimumHeightRequest = NVTokens.MinTap;
        switch (Variant)
        {
            case NVButtonVariant.Tonal:
                _button.BackgroundColor = theme.Mist;
                _button.TextColor = theme.Ink;
                _button.BorderColor = Colors.Transparent;
                _button.BorderWidth = 0;
                break;
            case NVButtonVariant.Outline:
                _button.BackgroundColor = Colors.Transparent;
                _button.TextColor = theme.Accent;
                _button.BorderColor = theme.Accent;
                _button.BorderWidth = 1;
                break;
            case NVButtonVariant.Ghost:
                _button.BackgroundColor = Colors.Transparent;
                _button.TextColor = theme.Ink;
                _button.BorderColor = Colors.Transparent;
                _button.BorderWidth = 0;
                break;
            case NVButtonVariant.Danger:
                _button.BackgroundColor = theme.Danger;
                _button.TextColor = theme.On(theme.Danger);
                _button.BorderColor = Colors.Transparent;
                _button.BorderWidth = 0;
                break;
            default:
                _button.BackgroundColor = theme.Accent;
                _button.TextColor = theme.OnAccent;
                _button.BorderColor = Colors.Transparent;
                _button.BorderWidth = 0;
                break;
        }

        ApplyEnabled();
    }
}
