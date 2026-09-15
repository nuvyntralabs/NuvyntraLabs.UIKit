namespace NuvyntraLabs.UIKit;

public class NVChart : ThemeAwareView
{
    public static readonly BindableProperty SeriesProperty = BindableProperty.Create(nameof(Series), typeof(IList<NVChartSeries>), typeof(NVChart), new List<NVChartSeries>(), propertyChanged: Refresh);
    readonly VerticalStackLayout _stack = new() { Spacing = NVTokens.Space2 };

    public NVChart() { Content = _stack; ApplyTheme(); }
    public IList<NVChartSeries> Series { get => (IList<NVChartSeries>)GetValue(SeriesProperty); set => SetValue(SeriesProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVChart)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _stack.Children.Clear();
        foreach (var series in Series ?? [])
        {
            _stack.Children.Add(new Label { Text = $"{series.Kind}: {series.Title}", FontFamily = NVTokens.FontSemiBold, TextColor = NVTheme.Current.Ink });
            var max = series.Points.Select(p => p.Y).DefaultIfEmpty(1).Max();
            foreach (var point in series.Points)
            {
                var bar = new BoxView
                {
                    Color = NVTheme.Current.Accent,
                    HeightRequest = 10,
                    WidthRequest = 40 + (max <= 0 ? 0 : 160 * (point.Y / max)),
                    HorizontalOptions = LayoutOptions.Start
                };
                _stack.Children.Add(new HorizontalStackLayout
                {
                    Spacing = NVTokens.Space2,
                    Children =
                    {
                        new Label { Text = point.Label, WidthRequest = 72, TextColor = NVTheme.Current.Muted },
                        bar
                    }
                });
            }
        }
    }
}

public class NVSparkline : NVChart { }

public class NVGauge : ThemeAwareView
{
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(double), typeof(NVGauge), 42d, propertyChanged: Refresh);
    readonly NVCircularProgress _arc = new();
    public NVGauge() { Content = _arc; ApplyTheme(); }
    public double Value { get => (double)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVGauge)b).ApplyTheme();
    protected override void ApplyTheme() => _arc.Value = Math.Clamp(Value / 100d, 0, 1);
}

public class NVLinearGauge : NVProgressBar
{
    public NVLinearGauge() => Value = 0.42;
}

public class NVBarcode : ThemeAwareView
{
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(string), typeof(NVBarcode), "NUVEXA", propertyChanged: Refresh);
    public static readonly BindableProperty FormatProperty = BindableProperty.Create(nameof(Format), typeof(NVBarcodeFormat), typeof(NVBarcode), NVBarcodeFormat.Code128, propertyChanged: Refresh);
    readonly Label _label = new() { FontFamily = NVTokens.FontRegular, HorizontalTextAlignment = TextAlignment.Center };
    public NVBarcode() { Content = _label; ApplyTheme(); }
    public string Value { get => (string)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }
    public NVBarcodeFormat Format { get => (NVBarcodeFormat)GetValue(FormatProperty); set => SetValue(FormatProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVBarcode)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _label.Text = NVBarcodeCodec.Encode(Value, Format);
        _label.TextColor = NVTheme.Current.Ink;
        _label.FontSize = 18;
    }
}

public class NVTreeMap : ThemeAwareView
{
    public static readonly BindableProperty NodesProperty = BindableProperty.Create(nameof(Nodes), typeof(IList<NVTreeMapNode>), typeof(NVTreeMap), new List<NVTreeMapNode>(), propertyChanged: Refresh);
    readonly FlexLayout _flex = new() { Wrap = Microsoft.Maui.Layouts.FlexWrap.Wrap };
    public NVTreeMap() { Content = _flex; ApplyTheme(); }
    public IList<NVTreeMapNode> Nodes { get => (IList<NVTreeMapNode>)GetValue(NodesProperty); set => SetValue(NodesProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVTreeMap)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _flex.Children.Clear();
        foreach (var node in Nodes ?? [])
        {
            _flex.Children.Add(new Border
            {
                Padding = NVTokens.Space2,
                WidthRequest = 48 + node.Weight * 4,
                HeightRequest = 36 + node.Weight * 2,
                BackgroundColor = NVTheme.Current.Mist,
                StrokeThickness = 0,
                Content = new Label { Text = node.Title, TextColor = NVTheme.Current.Ink, FontFamily = NVTokens.FontRegular }
            });
        }
    }
}

public class NVHeatMap : NVTreeMap { }

public class NVRadialGauge : NVGauge { }

public class NVDigitalGauge : ThemeAwareView
{
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(int), typeof(NVDigitalGauge), 42, propertyChanged: Refresh);
    readonly Label _digits = new() { FontFamily = NVTokens.FontSemiBold, FontSize = NVTokens.DisplaySize, HorizontalTextAlignment = TextAlignment.Center };
    public NVDigitalGauge() { Content = _digits; ApplyTheme(); }
    public int Value { get => (int)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVDigitalGauge)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _digits.Text = Value.ToString("0000");
        _digits.TextColor = NVTheme.Current.Accent;
    }
}
