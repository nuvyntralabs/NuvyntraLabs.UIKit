namespace NuvyntraLabs.UIKit;

public class NVCurrencyLabel : ThemeAwareView
{
    public static readonly BindableProperty AmountProperty = BindableProperty.Create(nameof(Amount), typeof(double), typeof(NVCurrencyLabel), 0d, propertyChanged: Refresh);
    readonly NVHeading _label = new();
    public NVCurrencyLabel() { Content = _label; ApplyTheme(); }
    public double Amount { get => (double)GetValue(AmountProperty); set => SetValue(AmountProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVCurrencyLabel)b).ApplyTheme();
    protected override void ApplyTheme() => _label.Text = Amount.ToString("C", System.Globalization.CultureInfo.CurrentCulture);
}

public class NVCountdown : ThemeAwareView
{
    public static readonly BindableProperty SecondsProperty = BindableProperty.Create(nameof(Seconds), typeof(int), typeof(NVCountdown), 90, propertyChanged: Refresh);
    readonly NVHeading _label = new();
    public NVCountdown() { Content = _label; ApplyTheme(); }
    public int Seconds { get => (int)GetValue(SecondsProperty); set => SetValue(SecondsProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVCountdown)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        var span = TimeSpan.FromSeconds(Math.Max(0, Seconds));
        _label.Text = $"{(int)span.TotalMinutes:00}:{span.Seconds:00}";
    }
}

public class NVQuote : ThemeAwareView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(NVQuote), "", propertyChanged: Refresh);
    readonly NVBodyText _body = new();
    public NVQuote()
    {
        Content = new Border { Padding = NVTokens.Space3, StrokeThickness = 0, Content = _body };
        ApplyTheme();
    }
    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVQuote)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _body.Text = Text;
        if (Content is Border border)
        {
            border.BackgroundColor = NVTheme.Current.Mist;
            border.StrokeShape = new RoundRectangle { CornerRadius = NVTokens.RadiusSmall };
        }
    }
}

public class NVCodeBlock : ThemeAwareView
{
    public static readonly BindableProperty CodeProperty = BindableProperty.Create(nameof(Code), typeof(string), typeof(NVCodeBlock), "UseNuvyntraUIKit();", propertyChanged: Refresh);
    readonly Label _label = new() { FontFamily = NVTokens.FontRegular, FontSize = NVTokens.CaptionSize };
    public NVCodeBlock()
    {
        Content = new Border { Padding = NVTokens.Space3, Content = _label, StrokeThickness = 1 };
        ApplyTheme();
    }
    public string Code { get => (string)GetValue(CodeProperty); set => SetValue(CodeProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVCodeBlock)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _label.Text = Code;
        _label.TextColor = NVTheme.Current.Ink;
        if (Content is Border border)
        {
            border.BackgroundColor = NVTheme.Current.Surface;
            border.Stroke = NVTheme.Current.Fog;
        }
    }
}

public class NVBulletList : ThemeAwareView
{
    public static readonly BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(IList<string>), typeof(NVBulletList), new List<string> { "One", "Two" }, propertyChanged: Refresh);
    readonly VerticalStackLayout _list = new() { Spacing = NVTokens.Space1 };
    public NVBulletList() { Content = _list; ApplyTheme(); }
    public IList<string> Items { get => (IList<string>)GetValue(ItemsProperty); set => SetValue(ItemsProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVBulletList)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _list.Children.Clear();
        foreach (var item in Items ?? [])
        {
            _list.Children.Add(new NVBodyText { Text = "• " + item });
        }
    }
}

public class NVStatCard : ThemeAwareView
{
    public static readonly BindableProperty LabelProperty = BindableProperty.Create(nameof(Label), typeof(string), typeof(NVStatCard), "KPI", propertyChanged: Refresh);
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(string), typeof(NVStatCard), "0", propertyChanged: Refresh);
    readonly NVCard _card = new();
    public NVStatCard() { Content = _card; ApplyTheme(); }
    public string Label { get => (string)GetValue(LabelProperty); set => SetValue(LabelProperty, value); }
    public string Value { get => (string)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVStatCard)b).ApplyTheme();
    protected override void ApplyTheme() { _card.Title = Value; _card.Body = Label; }
}

public class NVTimeline : ThemeAwareView
{
    public static readonly BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(IList<NVTimelineItem>), typeof(NVTimeline), new List<NVTimelineItem>(), propertyChanged: Refresh);
    readonly VerticalStackLayout _list = new() { Spacing = NVTokens.Space2 };
    public NVTimeline() { Content = _list; ApplyTheme(); }
    public IList<NVTimelineItem> Items { get => (IList<NVTimelineItem>)GetValue(ItemsProperty); set => SetValue(ItemsProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVTimeline)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _list.Children.Clear();
        foreach (var item in (Items ?? []).OrderBy(i => i.At))
        {
            _list.Children.Add(new NVListTile { Title = item.Title, Subtitle = $"{item.At:t} · {item.Detail}", Kind = NVIconKind.Check });
        }
    }
}

public class NVWizard : ThemeAwareView
{
    public static readonly BindableProperty StepsProperty = BindableProperty.Create(nameof(Steps), typeof(IList<string>), typeof(NVWizard), new List<string> { "Account", "Address", "Pay" }, propertyChanged: Refresh);
    public static readonly BindableProperty IndexProperty = BindableProperty.Create(nameof(Index), typeof(int), typeof(NVWizard), 0, BindingMode.TwoWay, propertyChanged: Refresh);
    readonly NVStepProgressBar _steps = new();
    readonly NVButton _back = new() { Text = "Back", Variant = NVButtonVariant.Ghost };
    readonly NVButton _next = new() { Text = "Next", Variant = NVButtonVariant.Filled };
    public NVWizard()
    {
        _back.Command = new Command(() => Index = Math.Max(0, Index - 1));
        _next.Command = new Command(() => Index = Math.Min((Steps?.Count ?? 1) - 1, Index + 1));
        Content = new VerticalStackLayout
        {
            Spacing = NVTokens.Space3,
            Children = { _steps, new HorizontalStackLayout { Spacing = NVTokens.Space2, Children = { _back, _next } } }
        };
        ApplyTheme();
    }
    public IList<string> Steps { get => (IList<string>)GetValue(StepsProperty); set => SetValue(StepsProperty, value); }
    public int Index { get => (int)GetValue(IndexProperty); set => SetValue(IndexProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVWizard)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _steps.Steps = Steps;
        _steps.Index = Index;
    }
}

public class NVStickyBar : ThemeAwareView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(NVStickyBar), "Continue", propertyChanged: Refresh);
    readonly NVButton _button = new() { Variant = NVButtonVariant.Filled };
    public NVStickyBar() { Content = _button; ApplyTheme(); }
    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVStickyBar)b).ApplyTheme();
    protected override void ApplyTheme() => _button.Text = Text;
}

public class NVCartBar : ThemeAwareView
{
    public static readonly BindableProperty TotalProperty = BindableProperty.Create(nameof(Total), typeof(double), typeof(NVCartBar), 0d, propertyChanged: Refresh);
    readonly NVCurrencyLabel _total = new();
    readonly NVButton _pay = new() { Text = "Checkout", Variant = NVButtonVariant.Filled };
    public NVCartBar()
    {
        Content = new HorizontalStackLayout { Spacing = NVTokens.Space3, Children = { _total, _pay } };
        ApplyTheme();
    }
    public double Total { get => (double)GetValue(TotalProperty); set => SetValue(TotalProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVCartBar)b).ApplyTheme();
    protected override void ApplyTheme() => _total.Amount = Total;
}

public class NVPriceTag : NVCurrencyLabel { }

public class NVVariantPicker : NVSegmentedControl
{
    public NVVariantPicker() => Items = new List<string> { "S", "M", "L" };
}

public class NVCouponField : NVTextField
{
    public NVCouponField()
    {
        Label = "Coupon";
        Placeholder = "AURORA10";
    }
}

public class NVTicket : ThemeAwareView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(NVTicket), "Boarding pass", propertyChanged: Refresh);
    public static readonly BindableProperty CodeProperty = BindableProperty.Create(nameof(Code), typeof(string), typeof(NVTicket), "NUV-1042", propertyChanged: Refresh);
    readonly NVCard _card = new();
    readonly NVBarcode _code = new();
    public NVTicket()
    {
        Content = new VerticalStackLayout { Spacing = NVTokens.Space2, Children = { _card, _code } };
        ApplyTheme();
    }
    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    public string Code { get => (string)GetValue(CodeProperty); set => SetValue(CodeProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVTicket)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _card.Title = Title;
        _card.Body = Code;
        _code.Value = Code;
    }
}

public class NVTimeSlotPicker : NVChipGroup
{
    public NVTimeSlotPicker() => Items = new List<string> { "09:00", "11:30", "16:00" };
}

public class NVSeatPicker : ThemeAwareView
{
    public static readonly BindableProperty SeatsProperty = BindableProperty.Create(nameof(Seats), typeof(IList<NVSeatCell>), typeof(NVSeatPicker), new List<NVSeatCell>(), propertyChanged: Refresh);
    readonly NVWrapLayout _wrap = new();
    public NVSeatPicker() { Content = _wrap; ApplyTheme(); }
    public IList<NVSeatCell> Seats { get => (IList<NVSeatCell>)GetValue(SeatsProperty); set => SetValue(SeatsProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVSeatPicker)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        var seats = Seats is { Count: > 0 } ? Seats : new List<NVSeatCell> { new() { Label = "A1" }, new() { Label = "A2", Taken = true }, new() { Label = "A3" } };
        _wrap.Items = seats.Select(s => s.Taken ? $"{s.Label}·" : s.Label).ToList();
    }
}
