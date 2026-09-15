namespace NuvyntraLabs.UIKit;

public class NVCalendar : ThemeAwareView
{
    public static readonly BindableProperty MonthProperty = BindableProperty.Create(nameof(Month), typeof(DateTime), typeof(NVCalendar), DateTime.Today, BindingMode.TwoWay, propertyChanged: Refresh);
    public static readonly BindableProperty SelectedDateProperty = BindableProperty.Create(nameof(SelectedDate), typeof(DateTime?), typeof(NVCalendar), null, BindingMode.TwoWay, propertyChanged: Refresh);
    readonly Grid _grid = new() { ColumnSpacing = 4, RowSpacing = 4 };

    public NVCalendar() { Content = _grid; ApplyTheme(); }
    public DateTime Month { get => (DateTime)GetValue(MonthProperty); set => SetValue(MonthProperty, value); }
    public DateTime? SelectedDate { get => (DateTime?)GetValue(SelectedDateProperty); set => SetValue(SelectedDateProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVCalendar)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _grid.Children.Clear();
        _grid.ColumnDefinitions.Clear();
        _grid.RowDefinitions.Clear();
        for (var i = 0; i < 7; i++)
        {
            _grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        }

        var first = new DateTime(Month.Year, Month.Month, 1);
        var start = (int)first.DayOfWeek;
        var days = DateTime.DaysInMonth(Month.Year, Month.Month);
        var cells = start + days;
        var rows = (int)Math.Ceiling(cells / 7d) + 1;
        for (var r = 0; r < rows; r++)
        {
            _grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        }

        var headers = new[] { "S", "M", "T", "W", "T", "F", "S" };
        for (var c = 0; c < 7; c++)
        {
            var header = new Label { Text = headers[c], HorizontalTextAlignment = TextAlignment.Center, TextColor = NVTheme.Current.Muted, FontFamily = NVTokens.FontRegular };
            Grid.SetColumn(header, c);
            Grid.SetRow(header, 0);
            _grid.Add(header);
        }

        for (var day = 1; day <= days; day++)
        {
            var date = new DateTime(Month.Year, Month.Month, day);
            var index = start + day - 1;
            var label = new Label
            {
                Text = day.ToString(),
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = SelectedDate?.Date == date.Date ? NVTheme.Current.Paper : NVTheme.Current.Ink,
                BackgroundColor = SelectedDate?.Date == date.Date ? NVTheme.Current.Accent : Colors.Transparent,
                FontFamily = NVTokens.FontRegular
            };
            var tap = new TapGestureRecognizer();
            tap.Tapped += (_, _) => SelectedDate = date;
            label.GestureRecognizers.Add(tap);
            Grid.SetColumn(label, index % 7);
            Grid.SetRow(label, 1 + (index / 7));
            _grid.Add(label);
        }
    }
}

public class NVScheduler : ThemeAwareView
{
    public static readonly BindableProperty AppointmentsProperty = BindableProperty.Create(nameof(Appointments), typeof(IList<NVAppointment>), typeof(NVScheduler), new List<NVAppointment>(), propertyChanged: Refresh);
    readonly VerticalStackLayout _list = new() { Spacing = NVTokens.Space2 };
    public NVScheduler() { Content = _list; ApplyTheme(); }
    public IList<NVAppointment> Appointments { get => (IList<NVAppointment>)GetValue(AppointmentsProperty); set => SetValue(AppointmentsProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVScheduler)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _list.Children.Clear();
        foreach (var appt in (Appointments ?? []).OrderBy(a => a.Start))
        {
            _list.Children.Add(new NVCard
            {
                Title = appt.Title,
                Body = $"{appt.Start:t} – {appt.End:t}"
            });
        }
    }
}
