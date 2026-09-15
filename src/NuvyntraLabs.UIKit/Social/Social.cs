namespace NuvyntraLabs.UIKit;

public class NVProfileHeader : ThemeAwareView
{
    public static readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(NVProfileHeader), "Niladri", propertyChanged: Refresh);
    readonly NVAvatar _avatar = new() { Initials = "NP", StatusOn = true };
    readonly NVHeading _name = new();
    readonly NVButton _follow = new() { Text = "Follow", Variant = NVButtonVariant.Tonal };
    public NVProfileHeader()
    {
        Content = new HorizontalStackLayout { Spacing = NVTokens.Space3, Children = { _avatar, _name, _follow } };
        ApplyTheme();
    }
    public string Name { get => (string)GetValue(NameProperty); set => SetValue(NameProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVProfileHeader)b).ApplyTheme();
    protected override void ApplyTheme() => _name.Text = Name;
}

public class NVFeedCard : ThemeAwareView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(NVFeedCard), "Update", propertyChanged: Refresh);
    public static readonly BindableProperty BodyProperty = BindableProperty.Create(nameof(Body), typeof(string), typeof(NVFeedCard), "", propertyChanged: Refresh);
    readonly NVCard _card = new();
    readonly NVReactionBar _react = new();
    public NVFeedCard()
    {
        Content = new VerticalStackLayout { Spacing = NVTokens.Space2, Children = { _card, _react } };
        ApplyTheme();
    }
    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    public string Body { get => (string)GetValue(BodyProperty); set => SetValue(BodyProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVFeedCard)b).ApplyTheme();
    protected override void ApplyTheme() { _card.Title = Title; _card.Body = Body; }
}

public class NVComposer : ThemeAwareView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(NVComposer), "", BindingMode.TwoWay, propertyChanged: Refresh);
    readonly NVEditor _editor = new() { Label = "Message" };
    readonly NVIconButton _attach = new() { Kind = NVIconKind.Image };
    readonly NVButton _send = new() { Text = "Send", Variant = NVButtonVariant.Filled };
    public NVComposer()
    {
        _editor.PropertyChanged += (_, e) => { if (e.PropertyName == nameof(NVEditor.Text)) Text = _editor.Text; };
        Content = new VerticalStackLayout
        {
            Spacing = NVTokens.Space2,
            Children = { _editor, new HorizontalStackLayout { Spacing = NVTokens.Space2, Children = { _attach, _send } } }
        };
        ApplyTheme();
    }
    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVComposer)b).ApplyTheme();
    protected override void ApplyTheme() => _editor.Text = Text;
}

public class NVBubble : ThemeAwareView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(NVBubble), "", propertyChanged: Refresh);
    public static readonly BindableProperty IsMineProperty = BindableProperty.Create(nameof(IsMine), typeof(bool), typeof(NVBubble), false, propertyChanged: Refresh);
    readonly NVCard _card = new();
    public NVBubble() { Content = _card; ApplyTheme(); }
    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
    public bool IsMine { get => (bool)GetValue(IsMineProperty); set => SetValue(IsMineProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVBubble)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _card.Title = IsMine ? "You" : "Them";
        _card.Body = Text;
        HorizontalOptions = IsMine ? LayoutOptions.End : LayoutOptions.Start;
    }
}

public class NVTypingIndicator : ThemeAwareView
{
    readonly NVCaptionText _label = new() { Text = "Typing…" };
    public NVTypingIndicator() { Content = _label; ApplyTheme(); }
    protected override void ApplyTheme() { }
}

public class NVStoryRing : ThemeAwareView
{
    readonly NVAvatar _avatar = new() { Initials = "ST", StatusOn = true };
    public NVStoryRing() { Content = _avatar; ApplyTheme(); }
    protected override void ApplyTheme() { }
}

public class NVReactionBar : ThemeAwareView
{
    public NVReactionBar()
    {
        Content = new HorizontalStackLayout
        {
            Spacing = NVTokens.Space2,
            Children =
            {
                new NVIconButton { Kind = NVIconKind.Heart },
                new NVIconButton { Kind = NVIconKind.Chat },
                new NVIconButton { Kind = NVIconKind.Share }
            }
        };
        ApplyTheme();
    }
    protected override void ApplyTheme() { }
}

public class NVNotificationRow : NVListTile
{
    public NVNotificationRow()
    {
        Title = "Lumina";
        Subtitle = "New reply";
        Kind = NVIconKind.Bell;
    }
}

public class NVContactTile : NVListTile
{
    public NVContactTile()
    {
        Title = "Ada";
        Subtitle = "Available";
        Kind = NVIconKind.User;
    }
}
