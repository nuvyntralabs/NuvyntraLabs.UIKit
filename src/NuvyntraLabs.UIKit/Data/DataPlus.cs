namespace NuvyntraLabs.UIKit;

/// <summary>Rows × columns aggregation. <see cref="NVDataGrid"/> stays flat.</summary>
public class NVPivotGrid : ThemeAwareView
{
    public static readonly BindableProperty FactsProperty = BindableProperty.Create(nameof(Facts), typeof(IList<NVPivotFact>), typeof(NVPivotGrid), new List<NVPivotFact>(), propertyChanged: Refresh);

    readonly Grid _grid = new() { ColumnSpacing = 8, RowSpacing = 6 };

    public NVPivotGrid()
    {
        Content = _grid;
        NVAccessibility.Name(this, "Pivot grid", "Aggregated rows and columns");
        ApplyTheme();
    }

    public IList<NVPivotFact> Facts { get => (IList<NVPivotFact>)GetValue(FactsProperty); set => SetValue(FactsProperty, value); }
    public IReadOnlyList<string> RowLabels { get; private set; } = [];
    public IReadOnlyList<string> ColumnLabels { get; private set; } = [];

    public static double Sum(IEnumerable<NVPivotFact>? facts, string row, string column) =>
        (facts ?? []).Where(f => f.Row == row && f.Column == column).Sum(f => f.Value);

    static void Refresh(BindableObject b, object o, object n) => ((NVPivotGrid)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _grid.Children.Clear();
        _grid.ColumnDefinitions.Clear();
        _grid.RowDefinitions.Clear();
        var facts = Facts ?? [];
        RowLabels = facts.Select(f => f.Row).Distinct().ToList();
        ColumnLabels = facts.Select(f => f.Column).Distinct().ToList();
        _grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
        foreach (var _ in ColumnLabels)
        {
            _grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        }

        _grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        foreach (var _ in RowLabels)
        {
            _grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        }

        for (var c = 0; c < ColumnLabels.Count; c++)
        {
            var header = new Label { Text = ColumnLabels[c], FontFamily = NVTokens.FontSemiBold, TextColor = NVTheme.Current.Ink };
            Grid.SetColumn(header, c + 1);
            _grid.Add(header);
        }

        for (var r = 0; r < RowLabels.Count; r++)
        {
            var label = new Label { Text = RowLabels[r], FontFamily = NVTokens.FontSemiBold, TextColor = NVTheme.Current.Ink };
            Grid.SetRow(label, r + 1);
            _grid.Add(label);
            for (var c = 0; c < ColumnLabels.Count; c++)
            {
                var cell = new Label
                {
                    Text = Sum(facts, RowLabels[r], ColumnLabels[c]).ToString("0.##"),
                    FontFamily = NVTokens.FontRegular,
                    TextColor = NVTheme.Current.Muted
                };
                Grid.SetColumn(cell, c + 1);
                Grid.SetRow(cell, r + 1);
                _grid.Add(cell);
            }
        }
    }
}

/// <summary>Inspect key/value with <c>NV*</c> editors.</summary>
public class NVPropertyGrid : ThemeAwareView
{
    public static readonly BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(IList<NVPropertyItem>), typeof(NVPropertyGrid), new List<NVPropertyItem>(), propertyChanged: Refresh);

    readonly NVDataForm _form = new();

    public NVPropertyGrid()
    {
        Content = _form;
        NVAccessibility.Name(this, "Property grid", "Inspect values");
        ApplyTheme();
    }

    public IList<NVPropertyItem> Items { get => (IList<NVPropertyItem>)GetValue(ItemsProperty); set => SetValue(ItemsProperty, value); }

    static void Refresh(BindableObject b, object o, object n) => ((NVPropertyGrid)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _form.Fields = (Items ?? []).Select(item => new NVFormField
        {
            Name = item.Name,
            Label = item.Name,
            Kind = item.Kind,
            Value = item.Value
        }).ToList();
    }
}

/// <summary>Expandable JSON. <see cref="NVTreeView"/> is the general hierarchy.</summary>
public class NVJsonTree : ThemeAwareView
{
    public static readonly BindableProperty JsonProperty = BindableProperty.Create(nameof(Json), typeof(string), typeof(NVJsonTree), "", propertyChanged: Refresh);

    readonly NVTreeView _tree = new();

    public NVJsonTree()
    {
        Content = _tree;
        NVAccessibility.Name(this, "JSON tree", "Expandable document");
        ApplyTheme();
    }

    public string Json { get => (string)GetValue(JsonProperty); set => SetValue(JsonProperty, value); }
    public IList<NVTreeNode> Roots => _tree.Roots;

    public static IList<NVTreeNode> Parse(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return [];
        }

        try
        {
            using var doc = System.Text.Json.JsonDocument.Parse(json);
            return [Walk("root", doc.RootElement)];
        }
        catch (System.Text.Json.JsonException)
        {
            return [new NVTreeNode { Title = "Invalid JSON" }];
        }
    }

    static NVTreeNode Walk(string name, System.Text.Json.JsonElement element)
    {
        var node = new NVTreeNode { Title = $"{name}: {Preview(element)}", IsExpanded = true };
        if (element.ValueKind == System.Text.Json.JsonValueKind.Object)
        {
            foreach (var prop in element.EnumerateObject())
            {
                node.Children.Add(Walk(prop.Name, prop.Value));
            }
        }
        else if (element.ValueKind == System.Text.Json.JsonValueKind.Array)
        {
            var i = 0;
            foreach (var item in element.EnumerateArray())
            {
                node.Children.Add(Walk($"[{i++}]", item));
            }
        }

        return node;
    }

    static string Preview(System.Text.Json.JsonElement element) =>
        element.ValueKind switch
        {
            System.Text.Json.JsonValueKind.Object => "{ }",
            System.Text.Json.JsonValueKind.Array => $"[ {element.GetArrayLength()} ]",
            System.Text.Json.JsonValueKind.String => element.GetString() ?? "",
            System.Text.Json.JsonValueKind.Number => element.GetRawText(),
            System.Text.Json.JsonValueKind.True => "true",
            System.Text.Json.JsonValueKind.False => "false",
            System.Text.Json.JsonValueKind.Null => "null",
            _ => element.GetRawText()
        };

    static void Refresh(BindableObject b, object o, object n) => ((NVJsonTree)b).ApplyTheme();

    protected override void ApplyTheme() => _tree.Roots = Parse(Json);
}
