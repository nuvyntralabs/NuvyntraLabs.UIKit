namespace NuvyntraLabs.UIKit;

public enum NVLayoutMode
{
    List,
    Tile,
    Card
}

public enum NVSelectionKind
{
    None,
    Single,
    Multiple
}

public enum NVSortDirection
{
    None,
    Ascending,
    Descending
}

public enum NVChipKind
{
    Filter,
    Input,
    Assist,
    Choice
}

public enum NVFormFieldKind
{
    Text,
    Number,
    Boolean,
    Date,
    Enum
}

public enum NVChartSeriesKind
{
    Line,
    Spline,
    Area,
    Bar,
    Column,
    Pie,
    Donut,
    Scatter,
    Bubble,
    Candle,
    Ohlc,
    Funnel,
    Pyramid,
    Polar,
    Radar,
    Sunburst,
    Spark
}

public enum NVBarcodeFormat
{
    Code128,
    Qr
}

public enum NVStatusReason
{
    Empty,
    Offline,
    EmptyCart,
    NoPhotos,
    NoVideos,
    NoTasks,
    LocationDenied,
    PaymentFailed,
    NoCredits,
    Generic
}

public enum NVBannerTone
{
    Info,
    Success,
    Warning,
    Danger
}

public enum NVTextRole
{
    Display,
    Title,
    Body,
    Caption
}

public sealed class NVTimelineItem
{
    public string Title { get; set; } = "";
    public string Detail { get; set; } = "";
    public DateTime At { get; set; } = DateTime.Now;
}

public sealed class NVSeatCell
{
    public string Label { get; set; } = "";
    public bool Taken { get; set; }
}

public sealed class NVListItem
{
    public string Title { get; set; } = "";
    public string? Subtitle { get; set; }
    public string? Detail { get; set; }
    public NVIconKind Icon { get; set; }
}

public sealed class NVFormField
{
    public string Name { get; set; } = "";
    public string Label { get; set; } = "";
    public NVFormFieldKind Kind { get; set; } = NVFormFieldKind.Text;
    public object? Value { get; set; }
    public string? Error { get; set; }

    public static NVFormField For(string name, Type type, object? value = null)
    {
        var kind = type == typeof(bool) || type == typeof(bool?)
            ? NVFormFieldKind.Boolean
            : type == typeof(int) || type == typeof(double) || type == typeof(decimal)
                ? NVFormFieldKind.Number
                : type == typeof(DateTime) || type == typeof(DateTime?)
                    ? NVFormFieldKind.Date
                    : type.IsEnum
                        ? NVFormFieldKind.Enum
                        : NVFormFieldKind.Text;
        return new NVFormField { Name = name, Label = name, Kind = kind, Value = value };
    }
}

public sealed class NVGridColumn
{
    public string Header { get; set; } = "";
    public string Binding { get; set; } = "";
    public string Key { get => Binding; set => Binding = value; }
    public bool Sortable { get; set; } = true;
    public bool Frozen { get; set; }
}

public sealed class NVTreeNode
{
    public string Title { get; set; } = "";
    public bool IsExpanded { get; set; }
    public bool IsChecked { get; set; }
    public bool ChildrenLoaded { get; set; }
    public List<NVTreeNode> Children { get; } = [];
}

public sealed class NVListGroup : List<NVListItem>
{
    public string Name { get; set; } = "";
}

public sealed class NVChartPoint
{
    public string Category { get; set; } = "";
    public string Label { get => Category; set => Category = value; }
    public double Value { get; set; }
    public double Y { get => Value; set => Value = value; }
    public double? Open { get; set; }
    public double? High { get; set; }
    public double? Low { get; set; }
    public double? Close { get; set; }
}

public sealed class NVChartSeries
{
    public string Title { get; set; } = "";
    public NVChartSeriesKind Kind { get; set; } = NVChartSeriesKind.Line;
    public IList<NVChartPoint> Points { get; set; } = [];
}

public sealed class NVAppointment
{
    public string Title { get; set; } = "";
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string? Recurrence { get; set; }
}

public sealed class NVChatMessage
{
    public string Author { get; set; } = "";
    public string Text { get; set; } = "";
    public bool IsMine { get; set; }
    public DateTime At { get; set; } = DateTime.Now;
}

public sealed class NVKanbanColumn
{
    public string Title { get; set; } = "";
    public List<NVListItem> Cards { get; } = [];
}

public sealed class NVTreeMapNode
{
    public string Title { get; set; } = "";
    public double Value { get; set; }
    public double Weight { get => Value; set => Value = value; }
    public List<NVTreeMapNode> Children { get; } = [];
}

public sealed class NVSpreadsheetCell
{
    public int Row { get; set; }
    public int Column { get; set; }
    public string Text { get; set; } = "";
    public string Value { get => Text; set => Text = value; }
}

public sealed class NVCommandItem
{
    public string Title { get; set; } = "";
    public string? Group { get; set; }
    public ICommand? Command { get; set; }
}

public sealed class NVCoachStep
{
    public string Title { get; set; } = "";
    public string Body { get; set; } = "";
    public View? Target { get; set; }
}

public sealed class NVMenuAction
{
    public string Text { get; set; } = "";
    public ICommand? Command { get; set; }
}

public sealed class NVFileChip
{
    public string Name { get; set; } = "";
    public long Size { get; set; }
}

public sealed class NVHeatDay
{
    public DateTime Date { get; set; }
    public double Value { get; set; }
}

public sealed class NVSpeedDialAction
{
    public string Text { get; set; } = "";
    public ICommand? Command { get; set; }
}

public sealed class NVPivotFact
{
    public string Row { get; set; } = "";
    public string Column { get; set; } = "";
    public double Value { get; set; }
}

public sealed class NVPropertyItem
{
    public string Name { get; set; } = "";
    public object? Value { get; set; }
    public NVFormFieldKind Kind { get; set; } = NVFormFieldKind.Text;
}

public enum NVDiffMode
{
    Unified,
    SideBySide
}

public sealed class NVDeviceItem
{
    public string Name { get; set; } = "";
    public bool IsConnected { get; set; }
}
