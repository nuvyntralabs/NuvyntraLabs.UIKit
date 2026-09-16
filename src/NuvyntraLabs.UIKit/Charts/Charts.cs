namespace NuvyntraLabs.UIKit;

public class NVChart : ThemeAwareView
{
    public static readonly BindableProperty SeriesProperty = BindableProperty.Create(nameof(Series), typeof(IList<NVChartSeries>), typeof(NVChart), new List<NVChartSeries>(), propertyChanged: Refresh);
    readonly GraphicsView _canvas = new() { HeightRequest = 180, Drawable = new NVChartDrawable() };

    public NVChart() { Content = _canvas; ApplyTheme(); }
    public IList<NVChartSeries> Series { get => (IList<NVChartSeries>)GetValue(SeriesProperty); set => SetValue(SeriesProperty, value); }
    public bool IsEmpty => NVChartLogic.IsEmpty(Series);
    static void Refresh(BindableObject b, object o, object n) => ((NVChart)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        if (_canvas.Drawable is NVChartDrawable drawable)
        {
            drawable.Series = Series;
        }

        _canvas.Invalidate();
    }
}

public class NVSparkline : NVChart { }

public class NVGauge : ThemeAwareView
{
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(double), typeof(NVGauge), 42d, propertyChanged: Refresh);
    readonly NVCircularProgress _arc = new();
    readonly NVCaptionText _caption = new();
    public NVGauge()
    {
        Content = new VerticalStackLayout { Spacing = NVTokens.Space2, HorizontalOptions = LayoutOptions.Center, Children = { _arc, _caption } };
        ApplyTheme();
    }
    public double Value { get => (double)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVGauge)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _arc.Value = Math.Clamp(Value / 100d, 0, 1);
        _caption.Text = $"{Value:0} / 100";
    }
}

public class NVLinearGauge : ThemeAwareView
{
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(double), typeof(NVLinearGauge), 0.42, propertyChanged: Refresh);
    readonly NVProgressBar _bar = new();
    readonly NVCaptionText _caption = new();
    public NVLinearGauge()
    {
        Content = new VerticalStackLayout { Spacing = NVTokens.Space2, Children = { new NVHeading { Text = "Linear", Role = NVTextRole.Caption }, _bar, _caption } };
        ApplyTheme();
    }
    public double Value { get => (double)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVLinearGauge)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _bar.Value = Math.Clamp(Value, 0, 1);
        _caption.Text = $"{Value:P0}";
    }
}

public class NVBarcode : ThemeAwareView
{
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(string), typeof(NVBarcode), "NUVEXA", propertyChanged: Refresh);
    public static readonly BindableProperty FormatProperty = BindableProperty.Create(nameof(Format), typeof(NVBarcodeFormat), typeof(NVBarcode), NVBarcodeFormat.Code128, propertyChanged: Refresh);
    readonly GraphicsView _canvas = new() { HeightRequest = 96, Drawable = new NVBarcodeDrawable() };
    public NVBarcode() { Content = _canvas; ApplyTheme(); }
    public string Value { get => (string)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }
    public NVBarcodeFormat Format { get => (NVBarcodeFormat)GetValue(FormatProperty); set => SetValue(FormatProperty, value); }
    public IReadOnlyList<int> Bars => NVBarcodeCodec.Code128Bars(Value);
    public bool[,] Matrix => NVBarcodeCodec.QrMatrix(Value);
    static void Refresh(BindableObject b, object o, object n) => ((NVBarcode)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        if (_canvas.Drawable is NVBarcodeDrawable drawable)
        {
            drawable.Value = Value;
            drawable.Format = Format;
        }

        _canvas.HeightRequest = Format == NVBarcodeFormat.Qr ? 160 : 96;
        _canvas.Invalidate();
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
        if (Content is not Border)
        {
            Content = new Border
            {
                Padding = new Thickness(NVTokens.Space4, NVTokens.Space2),
                StrokeThickness = 1,
                Content = _digits
            };
        }

        if (Content is Border border)
        {
            border.BackgroundColor = NVTheme.Current.Ink;
            border.Stroke = NVTheme.Current.Fog;
            border.StrokeShape = new RoundRectangle { CornerRadius = NVTokens.RadiusSmall };
            _digits.TextColor = NVTheme.Current.Accent;
        }
    }
}

sealed class NVChartDrawable : IDrawable
{
    public IList<NVChartSeries>? Series { get; set; }

    public void Draw(ICanvas canvas, RectF dirty)
    {
        canvas.FillColor = NVTheme.Current.Paper;
        canvas.FillRectangle(dirty);
        if (NVChartLogic.IsEmpty(Series))
        {
            return;
        }

        var pad = 16f;
        var plot = new RectF(dirty.X + pad, dirty.Y + pad, Math.Max(8, dirty.Width - pad * 2), Math.Max(8, dirty.Height - pad * 2));
        foreach (var series in Series!)
        {
            DrawSeries(canvas, plot, series);
        }
    }

    static void DrawSeries(ICanvas canvas, RectF plot, NVChartSeries series)
    {
        var points = NVChartLogic.Map(series);
        if (points.Count == 0)
        {
            return;
        }

        var maxY = Math.Max(1, points.Max(p => p.Y));
        canvas.StrokeColor = NVTheme.Current.Accent;
        canvas.FillColor = NVTheme.Current.Accent.WithAlpha(0.85f);
        canvas.StrokeSize = 2;

        switch (series.Kind)
        {
            case NVChartSeriesKind.Pie:
            case NVChartSeriesKind.Donut:
            case NVChartSeriesKind.Sunburst:
                DrawPie(canvas, plot, points, series.Kind == NVChartSeriesKind.Donut || series.Kind == NVChartSeriesKind.Sunburst);
                break;
            case NVChartSeriesKind.Scatter:
            case NVChartSeriesKind.Bubble:
                DrawDots(canvas, plot, points, maxY, series.Kind == NVChartSeriesKind.Bubble);
                break;
            case NVChartSeriesKind.Candle:
            case NVChartSeriesKind.Ohlc:
                DrawStems(canvas, plot, series, maxY);
                break;
            case NVChartSeriesKind.Polar:
            case NVChartSeriesKind.Radar:
                DrawRadar(canvas, plot, points, maxY);
                break;
            case NVChartSeriesKind.Bar:
            case NVChartSeriesKind.Column:
            case NVChartSeriesKind.Funnel:
            case NVChartSeriesKind.Pyramid:
                DrawBars(canvas, plot, points, maxY);
                break;
            default:
                DrawLine(canvas, plot, points, maxY, series.Kind is NVChartSeriesKind.Area);
                break;
        }
    }

    static void DrawBars(ICanvas canvas, RectF plot, IReadOnlyList<NVChartLogic.Point> points, double maxY)
    {
        var width = plot.Width / Math.Max(1, points.Count);
        for (var i = 0; i < points.Count; i++)
        {
            var height = (float)(plot.Height * (points[i].Y / maxY));
            canvas.FillRectangle(plot.X + i * width + 2, plot.Bottom - height, Math.Max(2, width - 4), height);
        }
    }

    static void DrawLine(ICanvas canvas, RectF plot, IReadOnlyList<NVChartLogic.Point> points, double maxY, bool fill)
    {
        if (points.Count == 1)
        {
            var y = plot.Bottom - (float)(plot.Height * (points[0].Y / maxY));
            canvas.FillCircle(plot.X + plot.Width / 2, y, 3);
            return;
        }

        var path = new PathF();
        for (var i = 0; i < points.Count; i++)
        {
            var x = plot.X + plot.Width * i / (points.Count - 1);
            var y = plot.Bottom - (float)(plot.Height * (points[i].Y / maxY));
            if (i == 0)
            {
                path.MoveTo(x, y);
            }
            else
            {
                path.LineTo(x, y);
            }
        }

        if (fill)
        {
            path.LineTo(plot.Right, plot.Bottom);
            path.LineTo(plot.Left, plot.Bottom);
            path.Close();
            canvas.FillColor = NVTheme.Current.Accent.WithAlpha(0.28f);
            canvas.FillPath(path);
        }

        canvas.DrawPath(path);
    }

    static void DrawDots(ICanvas canvas, RectF plot, IReadOnlyList<NVChartLogic.Point> points, double maxY, bool bubble)
    {
        var span = Math.Max(1, points.Count - 1);
        for (var i = 0; i < points.Count; i++)
        {
            var x = plot.X + plot.Width * i / span;
            var y = plot.Bottom - (float)(plot.Height * (points[i].Y / maxY));
            canvas.FillCircle(x, y, bubble ? 8 : 3);
        }
    }

    static void DrawStems(ICanvas canvas, RectF plot, NVChartSeries series, double maxY)
    {
        var width = plot.Width / Math.Max(1, series.Points.Count);
        for (var i = 0; i < series.Points.Count; i++)
        {
            var point = series.Points[i];
            var high = (float)((point.High ?? point.Y) / maxY * plot.Height);
            var low = (float)((point.Low ?? 0) / maxY * plot.Height);
            var close = (float)((point.Close ?? point.Y) / maxY * plot.Height);
            var x = plot.X + i * width + width / 2;
            canvas.DrawLine(x, plot.Bottom - high, x, plot.Bottom - low);
            canvas.FillRectangle(x - 3, plot.Bottom - close, 6, 6);
        }
    }

    static void DrawRadar(ICanvas canvas, RectF plot, IReadOnlyList<NVChartLogic.Point> points, double maxY)
    {
        var cx = plot.Center.X;
        var cy = plot.Center.Y;
        var radius = Math.Min(plot.Width, plot.Height) / 2;
        var path = new PathF();
        for (var i = 0; i < points.Count; i++)
        {
            var angle = i * Math.PI * 2 / points.Count - Math.PI / 2;
            var r = radius * (float)(points[i].Y / maxY);
            var x = cx + r * (float)Math.Cos(angle);
            var y = cy + r * (float)Math.Sin(angle);
            if (i == 0)
            {
                path.MoveTo(x, y);
            }
            else
            {
                path.LineTo(x, y);
            }
        }

        path.Close();
        canvas.DrawPath(path);
    }

    static void DrawPie(ICanvas canvas, RectF plot, IReadOnlyList<NVChartLogic.Point> points, bool donut)
    {
        var total = Math.Max(1, points.Sum(p => p.Y));
        var start = 0f;
        var box = new RectF(plot.Center.X - plot.Height / 2, plot.Y, plot.Height, plot.Height);
        for (var i = 0; i < points.Count; i++)
        {
            var sweep = (float)(360 * (points[i].Y / total));
            canvas.FillColor = NVTheme.Current.Accent.WithAlpha((float)(0.35 + 0.5 * (i / (double)Math.Max(1, points.Count - 1))));
            canvas.FillArc(box.X, box.Y, box.Width, box.Height, start, start + sweep, false);
            start += sweep;
        }

        if (donut)
        {
            canvas.FillColor = NVTheme.Current.Paper;
            var inset = box.Width * 0.28f;
            canvas.FillEllipse(box.X + inset, box.Y + inset, box.Width - inset * 2, box.Height - inset * 2);
        }
    }
}

sealed class NVBarcodeDrawable : IDrawable
{
    public string Value { get; set; } = "NUVEXA";
    public NVBarcodeFormat Format { get; set; } = NVBarcodeFormat.Code128;

    public void Draw(ICanvas canvas, RectF dirty)
    {
        canvas.FillColor = NVTheme.Current.Paper;
        canvas.FillRectangle(dirty);
        canvas.FillColor = NVTheme.Current.Ink;
        if (Format == NVBarcodeFormat.Qr)
        {
            var matrix = NVBarcodeCodec.QrMatrix(Value);
            var n = matrix.GetLength(0);
            var cell = Math.Min(dirty.Width, dirty.Height) / n;
            for (var r = 0; r < n; r++)
            {
                for (var c = 0; c < n; c++)
                {
                    if (matrix[r, c])
                    {
                        canvas.FillRectangle(dirty.X + c * cell, dirty.Y + r * cell, cell, cell);
                    }
                }
            }

            return;
        }

        var bars = NVBarcodeCodec.Code128Bars(Value);
        var unit = dirty.Width / Math.Max(1, bars.Sum());
        var x = dirty.X;
        var black = true;
        foreach (var width in bars)
        {
            if (black)
            {
                canvas.FillRectangle(x, dirty.Y + 8, width * unit, dirty.Height - 16);
            }

            x += width * unit;
            black = !black;
        }
    }
}
