namespace NuvyntraLabs.UIKit;

public class NVEditor : ThemeAwareView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(NVEditor), "", BindingMode.TwoWay, propertyChanged: Refresh);
    public static readonly BindableProperty LabelProperty = BindableProperty.Create(nameof(Label), typeof(string), typeof(NVEditor), "", propertyChanged: Refresh);
    readonly Editor _editor = new() { AutoSize = EditorAutoSizeOption.TextChanges, HeightRequest = 96 };
    readonly Label _label = FieldChrome.Title();
    readonly Border _box;

    public NVEditor()
    {
        _editor.TextChanged += (_, e) => Text = e.NewTextValue ?? "";
        _box = FieldChrome.Box(_editor);
        Content = FieldChrome.Stack(_label, _box, FieldChrome.Caption());
        ApplyTheme();
    }

    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
    public string Label { get => (string)GetValue(LabelProperty); set => SetValue(LabelProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVEditor)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _label.Text = Label;
        _label.TextColor = NVTheme.Current.Muted;
        _editor.Text = Text;
        _editor.TextColor = NVTheme.Current.Ink;
        FieldChrome.Paint(_box, false);
    }
}

public class NVSearchBar : NVTextField
{
    public NVSearchBar()
    {
        Label = "Search";
        Placeholder = "Search";
    }
}

public class NVMaskedEntry : NVTextField
{
    public static readonly BindableProperty MaskProperty = BindableProperty.Create(nameof(Mask), typeof(string), typeof(NVMaskedEntry), "0000", propertyChanged: OnMask);
    public string Mask { get => (string)GetValue(MaskProperty); set => SetValue(MaskProperty, value); }
    protected override string NormalizeText(string value) => NVMaskLogic.Apply(Mask, value);
    static void OnMask(BindableObject b, object o, object n)
    {
        if (b is NVMaskedEntry field)
        {
            field.Text = NVMaskLogic.Apply(field.Mask, field.Text);
        }
    }
}

public class NVNumericEntry : NVTextField
{
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(double), typeof(NVNumericEntry), 0d, BindingMode.TwoWay, propertyChanged: OnValue);
    public static readonly BindableProperty MinimumProperty = BindableProperty.Create(nameof(Minimum), typeof(double), typeof(NVNumericEntry), double.MinValue);
    public static readonly BindableProperty MaximumProperty = BindableProperty.Create(nameof(Maximum), typeof(double), typeof(NVNumericEntry), double.MaxValue);

    public NVNumericEntry() => Label = "Number";

    public double Value { get => (double)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }
    public double Minimum { get => (double)GetValue(MinimumProperty); set => SetValue(MinimumProperty, value); }
    public double Maximum { get => (double)GetValue(MaximumProperty); set => SetValue(MaximumProperty, value); }

    static void OnValue(BindableObject b, object o, object n)
    {
        if (b is NVNumericEntry e)
        {
            var v = Math.Clamp(e.Value, e.Minimum, e.Maximum);
            e.Text = v.ToString(System.Globalization.CultureInfo.CurrentCulture);
        }
    }
}

public class NVNumericUpDown : ThemeAwareView
{
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(double), typeof(NVNumericUpDown), 0d, BindingMode.TwoWay, propertyChanged: Refresh);
    readonly NVNumericEntry _entry = new();
    readonly NVButton _down = new() { Text = "−", Variant = NVButtonVariant.Tonal };
    readonly NVButton _up = new() { Text = "+", Variant = NVButtonVariant.Tonal };

    public NVNumericUpDown()
    {
        _down.Command = new Command(() => Value -= 1);
        _up.Command = new Command(() => Value += 1);
        Content = new HorizontalStackLayout { Spacing = NVTokens.Space2, Children = { _down, _entry, _up } };
        ApplyTheme();
    }

    public double Value { get => (double)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVNumericUpDown)b).ApplyTheme();
    protected override void ApplyTheme() => _entry.Value = Value;
}

public class NVOtpInput : ThemeAwareView
{
    public static readonly BindableProperty LengthProperty = BindableProperty.Create(nameof(Length), typeof(int), typeof(NVOtpInput), 6, propertyChanged: Refresh);
    public static readonly BindableProperty CodeProperty = BindableProperty.Create(nameof(Code), typeof(string), typeof(NVOtpInput), "", BindingMode.TwoWay, propertyChanged: Refresh);
    readonly HorizontalStackLayout _row = new() { Spacing = NVTokens.Space2 };

    public NVOtpInput()
    {
        Content = _row;
        ApplyTheme();
    }

    public int Length { get => (int)GetValue(LengthProperty); set => SetValue(LengthProperty, value); }
    public string Code { get => (string)GetValue(CodeProperty); set => SetValue(CodeProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVOtpInput)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _row.Children.Clear();
        var len = Math.Clamp(Length, 1, 12);
        var chars = (Code ?? "").PadRight(len);
        for (var i = 0; i < len; i++)
        {
            var idx = i;
            var field = new NVTextField { Text = chars[i] == ' ' ? "" : chars[i].ToString() };
            field.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(NVTextField.Text))
                {
                    var buffer = (Code ?? "").PadRight(len).ToCharArray();
                    buffer[idx] = string.IsNullOrEmpty(field.Text) ? ' ' : field.Text[0];
                    Code = new string(buffer).TrimEnd();
                }
            };
            _row.Children.Add(field);
        }
    }
}

public class NVAutoComplete : NVTextField
{
    public static readonly BindableProperty SuggestionsProperty = BindableProperty.Create(nameof(Suggestions), typeof(IList<string>), typeof(NVAutoComplete), new List<string>(), propertyChanged: OnHits);
    readonly VerticalStackLayout _hits = new() { Spacing = NVTokens.Space1 };
    public NVAutoComplete()
    {
        var field = Content;
        Content = new VerticalStackLayout { Spacing = NVTokens.Space2, Children = { field, _hits } };
        PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(Text))
            {
                PaintHits();
            }
        };
        PaintHits();
    }
    public IList<string> Suggestions { get => (IList<string>)GetValue(SuggestionsProperty); set => SetValue(SuggestionsProperty, value); }
    public IEnumerable<string> Filter() => FilterSuggestions(Suggestions, Text);
    public static IEnumerable<string> FilterSuggestions(IEnumerable<string>? suggestions, string? text) =>
        (suggestions ?? []).Where(s => s.Contains(text ?? "", StringComparison.OrdinalIgnoreCase));
    static void OnHits(BindableObject b, object o, object n)
    {
        if (b is NVAutoComplete field)
        {
            field.PaintHits();
        }
    }
    void PaintHits()
    {
        _hits.Children.Clear();
        foreach (var hit in Filter().Take(5))
        {
            var pick = hit;
            var chip = new NVChip { Text = pick, Kind = NVChipKind.Assist };
            var tap = new TapGestureRecognizer();
            tap.Tapped += (_, _) => Text = pick;
            chip.GestureRecognizers.Add(tap);
            _hits.Children.Add(chip);
        }
    }
}

public class NVComboBox : ThemeAwareView
{
    public static readonly BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(IList<string>), typeof(NVComboBox), new List<string>(), propertyChanged: Refresh);
    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(string), typeof(NVComboBox), "", BindingMode.TwoWay, propertyChanged: Refresh);
    readonly Picker _picker = new();
    readonly Label _label = FieldChrome.Title();
    readonly Border _box;

    public NVComboBox()
    {
        _picker.SelectedIndexChanged += (_, _) => SelectedItem = _picker.SelectedItem as string ?? "";
        _box = FieldChrome.Box(_picker);
        Content = FieldChrome.Stack(_label, _box, FieldChrome.Caption());
        ApplyTheme();
    }

    public string Label { get; set; } = "Choice";
    public IList<string> Items { get => (IList<string>)GetValue(ItemsProperty); set => SetValue(ItemsProperty, value); }
    public string SelectedItem { get => (string)GetValue(SelectedItemProperty); set => SetValue(SelectedItemProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVComboBox)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _label.Text = Label;
        _label.TextColor = NVTheme.Current.Muted;
        _picker.ItemsSource = Items?.ToList();
        _picker.SelectedItem = SelectedItem;
        _picker.TextColor = NVTheme.Current.Ink;
        FieldChrome.Paint(_box, false);
    }
}

public class NVPicker : NVComboBox { }
public class NVTemplatedPicker : NVPicker { }

public class NVDatePicker : ThemeAwareView
{
    public static readonly BindableProperty DateProperty = BindableProperty.Create(nameof(Date), typeof(DateTime), typeof(NVDatePicker), DateTime.Today, BindingMode.TwoWay, propertyChanged: Refresh);
    readonly DatePicker _picker = new();
    readonly Label _label = FieldChrome.Title();
    readonly Border _box;

    public NVDatePicker()
    {
        _picker.DateSelected += (_, e) => Date = e.NewDate ?? DateTime.Today;
        _box = FieldChrome.Box(_picker);
        Content = FieldChrome.Stack(_label, _box, FieldChrome.Caption());
        ApplyTheme();
    }

    public string Label { get; set; } = "Date";
    public DateTime Date { get => (DateTime)GetValue(DateProperty); set => SetValue(DateProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVDatePicker)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _label.Text = Label;
        _label.TextColor = NVTheme.Current.Muted;
        _picker.Date = Date;
        FieldChrome.Paint(_box, false);
    }
}

public class NVTimePicker : ThemeAwareView
{
    public static readonly BindableProperty TimeProperty = BindableProperty.Create(nameof(Time), typeof(TimeSpan), typeof(NVTimePicker), TimeSpan.Zero, BindingMode.TwoWay, propertyChanged: Refresh);
    readonly TimePicker _picker = new();
    readonly Label _label = FieldChrome.Title();
    readonly Border _box;

    public NVTimePicker()
    {
        _picker.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(TimePicker.Time))
            {
                Time = _picker.Time ?? TimeSpan.Zero;
            }
        };
        _box = FieldChrome.Box(_picker);
        Content = FieldChrome.Stack(_label, _box, FieldChrome.Caption());
        ApplyTheme();
    }

    public string Label { get; set; } = "Time";
    public TimeSpan Time { get => (TimeSpan)GetValue(TimeProperty); set => SetValue(TimeProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVTimePicker)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _label.Text = Label;
        _label.TextColor = NVTheme.Current.Muted;
        _picker.Time = Time;
        FieldChrome.Paint(_box, false);
    }
}

public class NVDateTimePicker : ThemeAwareView
{
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(DateTime), typeof(NVDateTimePicker), DateTime.Now, BindingMode.TwoWay, propertyChanged: Refresh);
    readonly NVDatePicker _date = new();
    readonly NVTimePicker _time = new();

    public NVDateTimePicker()
    {
        _date.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(NVDatePicker.Date))
            {
                Value = _date.Date.Date + Value.TimeOfDay;
            }
        };
        _time.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(NVTimePicker.Time))
            {
                Value = Value.Date + _time.Time;
            }
        };
        Content = new HorizontalStackLayout { Spacing = NVTokens.Space2, Children = { _date, _time } };
        ApplyTheme();
    }

    public DateTime Value { get => (DateTime)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVDateTimePicker)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _date.Date = Value.Date;
        _time.Time = Value.TimeOfDay;
    }
}

public class NVTimeSpanPicker : NVNumericUpDown
{
    public NVTimeSpanPicker() { }
    public TimeSpan Duration { get => TimeSpan.FromMinutes(Value); set => Value = value.TotalMinutes; }
}

public class NVColorPicker : ThemeAwareView
{
    public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(NVColorPicker), Colors.Teal, BindingMode.TwoWay, propertyChanged: Refresh);
    readonly BoxView _swatch = new() { HeightRequest = 36, WidthRequest = 36 };
    readonly Slider _r = new() { Maximum = 1 };
    readonly Slider _g = new() { Maximum = 1 };
    readonly Slider _b = new() { Maximum = 1 };

    public NVColorPicker()
    {
        void Bind(Slider s) => s.ValueChanged += (_, _) => Color = Color.FromRgb((float)_r.Value, (float)_g.Value, (float)_b.Value);
        Bind(_r); Bind(_g); Bind(_b);
        Content = new HorizontalStackLayout { Spacing = NVTokens.Space2, Children = { _swatch, _r, _g, _b } };
        ApplyTheme();
    }

    public Color Color { get => (Color)GetValue(ColorProperty); set => SetValue(ColorProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVColorPicker)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _swatch.Color = Color;
        _r.Value = Color.Red;
        _g.Value = Color.Green;
        _b.Value = Color.Blue;
    }
}

public class NVSlider : ThemeAwareView
{
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(double), typeof(NVSlider), 0d, BindingMode.TwoWay, propertyChanged: Refresh);
    readonly Slider _slider = new() { Maximum = 100 };
    public NVSlider()
    {
        _slider.ValueChanged += (_, e) => Value = e.NewValue;
        Content = _slider;
        ApplyTheme();
    }
    public double Value { get => (double)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVSlider)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _slider.Value = Value;
        _slider.MinimumTrackColor = NVTheme.Current.Accent;
    }
}

public class NVRangeSlider : ThemeAwareView
{
    public static readonly BindableProperty StartProperty = BindableProperty.Create(nameof(Start), typeof(double), typeof(NVRangeSlider), 20d, BindingMode.TwoWay, propertyChanged: Refresh);
    public static readonly BindableProperty EndProperty = BindableProperty.Create(nameof(End), typeof(double), typeof(NVRangeSlider), 80d, BindingMode.TwoWay, propertyChanged: Refresh);
    readonly NVSlider _a = new();
    readonly NVSlider _b = new();
    public NVRangeSlider()
    {
        _a.PropertyChanged += (_, e) => { if (e.PropertyName == nameof(NVSlider.Value)) Start = Math.Min(_a.Value, End); };
        _b.PropertyChanged += (_, e) => { if (e.PropertyName == nameof(NVSlider.Value)) End = Math.Max(_b.Value, Start); };
        Content = new VerticalStackLayout { Children = { _a, _b } };
        ApplyTheme();
    }
    public double Start { get => (double)GetValue(StartProperty); set => SetValue(StartProperty, value); }
    public double End { get => (double)GetValue(EndProperty); set => SetValue(EndProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVRangeSlider)b).ApplyTheme();
    protected override void ApplyTheme() { _a.Value = Start; _b.Value = End; }
}

public class NVCircularSlider : NVSlider { }
public class NVRangeSelector : NVRangeSlider { }

public class NVSignaturePad : ThemeAwareView
{
    public static readonly BindableProperty HasStrokeProperty = BindableProperty.Create(nameof(HasStroke), typeof(bool), typeof(NVSignaturePad), false);
    readonly List<PointF> _points = [];
    readonly GraphicsView _canvas = new() { HeightRequest = 140 };
    public NVSignaturePad()
    {
        _canvas.Drawable = new NVSignatureDrawable(_points);
        _canvas.StartInteraction += (_, e) =>
        {
            _points.Clear();
            Add(e.Touches);
        };
        _canvas.DragInteraction += (_, e) => Add(e.Touches);
        Content = FieldChrome.Box(_canvas);
        ApplyTheme();
    }
    public bool HasStroke { get => (bool)GetValue(HasStrokeProperty); set => SetValue(HasStrokeProperty, value); }
    public string Export() => HasStroke ? "nv-signature" : "";
    void Add(PointF[]? touches)
    {
        if (touches is { Length: > 0 })
        {
            _points.Add(touches[0]);
            HasStroke = true;
            _canvas.Invalidate();
        }
    }
    protected override void ApplyTheme()
    {
        _canvas.BackgroundColor = NVTheme.Current.Surface;
        _canvas.Invalidate();
    }
}

sealed class NVSignatureDrawable(List<PointF> points) : IDrawable
{
    public void Draw(ICanvas canvas, RectF dirty)
    {
        canvas.FillColor = NVTheme.Current.Surface;
        canvas.FillRectangle(dirty);
        canvas.StrokeColor = NVTheme.Current.Ink;
        canvas.StrokeSize = 2;
        if (points.Count == 0)
        {
            canvas.FontColor = NVTheme.Current.Muted;
            canvas.FontSize = 14;
            canvas.DrawString("Sign here", dirty, HorizontalAlignment.Center, VerticalAlignment.Center);
            return;
        }

        var path = new PathF();
        path.MoveTo(points[0]);
        foreach (var point in points.Skip(1))
        {
            path.LineTo(point);
        }

        canvas.DrawPath(path);
    }
}

public class NVRating : ThemeAwareView
{
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(int), typeof(NVRating), 0, BindingMode.TwoWay, propertyChanged: Refresh);
    readonly HorizontalStackLayout _row = new();
    public NVRating()
    {
        Content = _row;
        ApplyTheme();
    }
    public int Value { get => (int)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVRating)b).ApplyTheme();
    protected override void ApplyTheme()
    {
        _row.Children.Clear();
        for (var i = 1; i <= 5; i++)
        {
            var star = i;
            var label = new Label { Text = i <= Value ? "★" : "☆", FontSize = 22, TextColor = NVTheme.Current.Accent };
            var tap = new TapGestureRecognizer();
            tap.Tapped += (_, _) => Value = star;
            label.GestureRecognizers.Add(tap);
            _row.Children.Add(label);
        }
    }
}
