namespace NuvyntraLabs.UIKit;

public class NVCalendar : ThemeAwareView
{
    public static readonly BindableProperty MonthProperty = BindableProperty.Create(nameof(Month), typeof(DateTime), typeof(NVCalendar), DateTime.Today, BindingMode.TwoWay, propertyChanged: Refresh);
    public static readonly BindableProperty SelectedDateProperty = BindableProperty.Create(nameof(SelectedDate), typeof(DateTime?), typeof(NVCalendar), null, BindingMode.TwoWay, propertyChanged: Refresh);
    public static readonly BindableProperty SelectedDatesProperty = BindableProperty.Create(nameof(SelectedDates), typeof(IList<DateTime>), typeof(NVCalendar), new List<DateTime>(), BindingMode.TwoWay, propertyChanged: Refresh);
    public static readonly BindableProperty AllowMultipleProperty = BindableProperty.Create(nameof(AllowMultiple), typeof(bool), typeof(NVCalendar), false, propertyChanged: Refresh);
    readonly Grid _grid = new() { ColumnSpacing = 4, RowSpacing = 4 };
    readonly Label _title = new() { FontFamily = NVTokens.FontSemiBold, HorizontalOptions = LayoutOptions.Center };

    public NVCalendar()
    {
        var prev = new NVButton { Text = "‹", Variant = NVButtonVariant.Ghost, Command = new Command(PreviousMonth) };
        var next = new NVButton { Text = "›", Variant = NVButtonVariant.Ghost, Command = new Command(NextMonth) };
        Content = new VerticalStackLayout
        {
            Spacing = NVTokens.Space2,
            Children =
            {
                new HorizontalStackLayout { Spacing = NVTokens.Space2, Children = { prev, _title, next } },
                _grid
            }
        };
        ApplyTheme();
    }

    public DateTime Month { get => (DateTime)GetValue(MonthProperty); set => SetValue(MonthProperty, value); }
    public DateTime? SelectedDate { get => (DateTime?)GetValue(SelectedDateProperty); set => SetValue(SelectedDateProperty, value); }
    public IList<DateTime> SelectedDates { get => (IList<DateTime>)GetValue(SelectedDatesProperty); set => SetValue(SelectedDatesProperty, value); }
    public bool AllowMultiple { get => (bool)GetValue(AllowMultipleProperty); set => SetValue(AllowMultipleProperty, value); }

    public void NextMonth() => Month = Month.AddMonths(1);
    public void PreviousMonth() => Month = Month.AddMonths(-1);

    public void Toggle(DateTime date)
    {
        SelectedDate = date;
        var next = SelectedDates?.ToList() ?? [];
        if (AllowMultiple)
        {
            var hit = next.FindIndex(d => d.Date == date.Date);
            if (hit >= 0)
            {
                next.RemoveAt(hit);
            }
            else
            {
                next.Add(date);
            }

            SelectedDates = next;
            return;
        }

        SelectedDates = [date];
    }

    static void Refresh(BindableObject b, object o, object n) => ((NVCalendar)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _title.Text = Month.ToString("MMMM yyyy");
        _title.TextColor = NVTheme.Current.Ink;
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
            var chosen = IsSelected(date);
            var label = new Label
            {
                Text = day.ToString(),
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = chosen ? NVTheme.Current.Paper : NVTheme.Current.Ink,
                BackgroundColor = chosen ? NVTheme.Current.Accent : Colors.Transparent,
                FontFamily = NVTokens.FontRegular
            };
            var tap = new TapGestureRecognizer();
            tap.Tapped += (_, _) => Toggle(date);
            label.GestureRecognizers.Add(tap);
            Grid.SetColumn(label, index % 7);
            Grid.SetRow(label, 1 + (index / 7));
            _grid.Add(label);
        }
    }

    bool IsSelected(DateTime date) =>
        SelectedDate?.Date == date.Date || (SelectedDates ?? []).Any(d => d.Date == date.Date);
}

/// <summary>Contribution / habit day cells. Missing values draw empty. Not a <see cref="NVChart"/> series.</summary>
public class NVHeatCalendar : ThemeAwareView
{
    public static readonly BindableProperty MonthProperty = BindableProperty.Create(nameof(Month), typeof(DateTime), typeof(NVHeatCalendar), DateTime.Today, BindingMode.TwoWay, propertyChanged: Refresh);
    public static readonly BindableProperty ValuesProperty = BindableProperty.Create(nameof(Values), typeof(IList<NVHeatDay>), typeof(NVHeatCalendar), new List<NVHeatDay>(), propertyChanged: Refresh);

    readonly Grid _grid = new() { ColumnSpacing = 4, RowSpacing = 4 };

    public NVHeatCalendar()
    {
        Content = _grid;
        NVAccessibility.Name(this, "Heat calendar", "Daily activity");
        ApplyTheme();
    }

    public DateTime Month { get => (DateTime)GetValue(MonthProperty); set => SetValue(MonthProperty, value); }
    public IList<NVHeatDay> Values { get => (IList<NVHeatDay>)GetValue(ValuesProperty); set => SetValue(ValuesProperty, value); }
    public int DayCount => DateTime.DaysInMonth(Month.Year, Month.Month);
    public IReadOnlyList<View> DayCells { get; private set; } = [];

    public static NVHeatDay? Find(IEnumerable<NVHeatDay>? values, DateTime date) =>
        values?.FirstOrDefault(day => day.Date.Date == date.Date);

    public double? ValueFor(DateTime date) => Find(Values, date)?.Value;

    static void Refresh(BindableObject b, object o, object n) => ((NVHeatCalendar)b).ApplyTheme();

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
        var days = DayCount;
        var rows = (int)Math.Ceiling((start + days) / 7d);
        for (var r = 0; r < rows; r++)
        {
            _grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        }

        var cells = new List<View>(days);
        var peak = (Values ?? []).Select(v => v.Value).DefaultIfEmpty(0).Max();
        for (var day = 1; day <= days; day++)
        {
            var date = new DateTime(Month.Year, Month.Month, day);
            var value = ValueFor(date);
            var box = new BoxView
            {
                HeightRequest = 18,
                CornerRadius = 4,
                Color = value is null
                    ? Colors.Transparent
                    : peak <= 0
                        ? NVTheme.Current.Mist
                        : NVTheme.Current.Accent.WithAlpha((float)Math.Clamp(0.2 + (value.Value / peak) * 0.8, 0.2, 1))
            };
            if (value is null && Content is not null)
            {
                box.BackgroundColor = Colors.Transparent;
                box.Color = NVTheme.Current.Fog.WithAlpha(0.25f);
            }

            var index = start + day - 1;
            Grid.SetColumn(box, index % 7);
            Grid.SetRow(box, index / 7);
            _grid.Add(box);
            cells.Add(box);
        }

        DayCells = cells;
    }
}

public class NVScheduler : ThemeAwareView
{
    public static readonly BindableProperty AppointmentsProperty = BindableProperty.Create(nameof(Appointments), typeof(IList<NVAppointment>), typeof(NVScheduler), new List<NVAppointment>(), propertyChanged: Refresh);
    public static readonly BindableProperty AgendaDateProperty = BindableProperty.Create(nameof(AgendaDate), typeof(DateTime?), typeof(NVScheduler), null, propertyChanged: Refresh);
    public static readonly BindableProperty RecurrenceCapProperty = BindableProperty.Create(nameof(RecurrenceCap), typeof(int), typeof(NVScheduler), NVRecurrence.DefaultCap, propertyChanged: Refresh);
    readonly VerticalStackLayout _list = new() { Spacing = NVTokens.Space2 };
    public NVScheduler() { Content = _list; ApplyTheme(); }
    public IList<NVAppointment> Appointments { get => (IList<NVAppointment>)GetValue(AppointmentsProperty); set => SetValue(AppointmentsProperty, value); }
    public DateTime? AgendaDate { get => (DateTime?)GetValue(AgendaDateProperty); set => SetValue(AgendaDateProperty, value); }
    public int RecurrenceCap { get => (int)GetValue(RecurrenceCapProperty); set => SetValue(RecurrenceCapProperty, value); }
    public IReadOnlyList<NVAppointment> Agenda => Expand(Appointments, AgendaDate, RecurrenceCap);
    static void Refresh(BindableObject b, object o, object n) => ((NVScheduler)b).ApplyTheme();

    public static IReadOnlyList<NVAppointment> Expand(IEnumerable<NVAppointment>? appointments, DateTime? agendaDate, int cap)
    {
        var expanded = new List<NVAppointment>();
        foreach (var appt in appointments ?? [])
        {
            var duration = appt.End - appt.Start;
            foreach (var start in NVRecurrence.Expand(appt.Start, appt.Recurrence, cap))
            {
                expanded.Add(new NVAppointment
                {
                    Title = appt.Title,
                    Start = start,
                    End = start + duration,
                    Recurrence = appt.Recurrence
                });
            }
        }

        var filtered = agendaDate is DateTime day
            ? expanded.Where(item => item.Start.Date == day.Date)
            : expanded;
        return filtered.OrderBy(item => item.Start).ToList();
    }

    protected override void ApplyTheme()
    {
        _list.Children.Clear();
        foreach (var appt in Agenda)
        {
            _list.Children.Add(new NVCard
            {
                Title = appt.Title,
                Body = $"{appt.Start:d}  {appt.Start:t} – {appt.End:t}"
            });
        }
    }
}
