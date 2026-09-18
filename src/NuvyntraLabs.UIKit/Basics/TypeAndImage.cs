namespace NuvyntraLabs.UIKit;

public class NVHeading : ThemeAwareView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(NVHeading), "", propertyChanged: Refresh);
    public static readonly BindableProperty RoleProperty = BindableProperty.Create(nameof(Role), typeof(NVTextRole), typeof(NVHeading), NVTextRole.Title, propertyChanged: Refresh);
    readonly Label _label = new();
    public NVHeading() { Content = _label; ApplyTheme(); }
    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
    public NVTextRole Role { get => (NVTextRole)GetValue(RoleProperty); set => SetValue(RoleProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVHeading)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _label.Text = Text;
        _label.TextColor = NVTheme.Current.Ink;
        _label.FontFamily = Role is NVTextRole.Caption or NVTextRole.Body ? NVTokens.FontRegular : NVTokens.FontSemiBold;
        _label.FontSize = Role switch
        {
            NVTextRole.Display => NVTokens.Type(NVTokens.DisplaySize),
            NVTextRole.Body => NVTokens.Type(NVTokens.BodySize),
            NVTextRole.Caption => NVTokens.Type(NVTokens.CaptionSize),
            _ => NVTokens.Type(NVTokens.TitleSize)
        };
    }
}

public class NVBodyText : NVHeading
{
    public NVBodyText() => Role = NVTextRole.Body;
}

public class NVCaptionText : NVHeading
{
    public NVCaptionText() => Role = NVTextRole.Caption;
}

public class NVImage : ThemeAwareView
{
    public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(ImageSource), typeof(NVImage), null, propertyChanged: Refresh);
    public static readonly BindableProperty CaptionProperty = BindableProperty.Create(nameof(Caption), typeof(string), typeof(NVImage), "", propertyChanged: Refresh);
    readonly Image _image = new() { Aspect = Aspect.AspectFill, HeightRequest = 160 };
    readonly Label _caption = new() { FontFamily = NVTokens.FontRegular, FontSize = NVTokens.CaptionSize };
    public NVImage()
    {
        Content = new VerticalStackLayout { Spacing = NVTokens.Space1, Children = { _image, _caption } };
        ApplyTheme();
    }
    public ImageSource? Source { get => (ImageSource?)GetValue(SourceProperty); set => SetValue(SourceProperty, value); }
    public string Caption { get => (string)GetValue(CaptionProperty); set => SetValue(CaptionProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVImage)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _image.Source = Source;
        _image.BackgroundColor = NVTheme.Current.Mist;
        if (Source is null)
        {
            _image.Source = null;
        }
        _caption.Text = string.IsNullOrWhiteSpace(Caption) ? (Source is null ? "Image placeholder" : "") : Caption;
        _caption.TextColor = NVTheme.Current.Muted;
        _caption.FontSize = NVTokens.Type(NVTokens.CaptionSize);
        _caption.IsVisible = !string.IsNullOrWhiteSpace(_caption.Text);
    }
}

public class NVSafeArea : ThemeAwareView
{
    public NVSafeArea()
    {
        Padding = new Thickness(NVTokens.Space4);
        ApplyTheme();
    }

    public View? Child
    {
        get => Content;
        set => Content = value;
    }

    protected override void ApplyTheme() => BackgroundColor = Colors.Transparent;
}
