namespace NuvyntraLabs.UIKit;

public class NVCollectionView : ThemeAwareView
{
    public static readonly BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(IList<NVListItem>), typeof(NVCollectionView), new List<NVListItem>(), propertyChanged: Refresh);
    public static readonly BindableProperty LayoutModeProperty = BindableProperty.Create(nameof(LayoutMode), typeof(NVLayoutMode), typeof(NVCollectionView), NVLayoutMode.List, propertyChanged: Refresh);
    readonly VerticalStackLayout _list = new() { Spacing = NVTokens.Space2 };

    public NVCollectionView() { Content = _list; ApplyTheme(); }
    public IList<NVListItem> Items { get => (IList<NVListItem>)GetValue(ItemsProperty); set => SetValue(ItemsProperty, value); }
    public NVLayoutMode LayoutMode { get => (NVLayoutMode)GetValue(LayoutModeProperty); set => SetValue(LayoutModeProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVCollectionView)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _list.Children.Clear();
        foreach (var item in Items ?? [])
        {
            _list.Children.Add(new NVCard { Title = item.Title, Body = item.Subtitle ?? "" });
        }
    }
}

public class NVListView : NVCollectionView { }
public class NVCardsView : NVCollectionView
{
    public NVCardsView() => LayoutMode = NVLayoutMode.Card;
}

public class NVDataPager : ThemeAwareView
{
    public static readonly BindableProperty TotalCountProperty = BindableProperty.Create(nameof(TotalCount), typeof(int), typeof(NVDataPager), 0, propertyChanged: Refresh);
    public static readonly BindableProperty PageSizeProperty = BindableProperty.Create(nameof(PageSize), typeof(int), typeof(NVDataPager), 10, propertyChanged: Refresh);
    public static readonly BindableProperty PageIndexProperty = BindableProperty.Create(nameof(PageIndex), typeof(int), typeof(NVDataPager), 0, BindingMode.TwoWay, propertyChanged: Refresh);
    readonly Label _label = new() { FontFamily = NVTokens.FontRegular };
    readonly NVButton _prev = new() { Text = "Prev", Variant = NVButtonVariant.Ghost };
    readonly NVButton _next = new() { Text = "Next", Variant = NVButtonVariant.Ghost };

    public NVDataPager()
    {
        _prev.Command = new Command(() => PageIndex = Math.Max(0, PageIndex - 1));
        _next.Command = new Command(() => PageIndex = Math.Min(PageCount - 1, PageIndex + 1));
        Content = new HorizontalStackLayout { Spacing = NVTokens.Space2, Children = { _prev, _label, _next } };
        ApplyTheme();
    }

    public int TotalCount { get => (int)GetValue(TotalCountProperty); set => SetValue(TotalCountProperty, value); }
    public int PageSize { get => (int)GetValue(PageSizeProperty); set => SetValue(PageSizeProperty, value); }
    public int PageIndex { get => (int)GetValue(PageIndexProperty); set => SetValue(PageIndexProperty, value); }
    public int PageCount => NVDataPagerLogic.PageCount(TotalCount, PageSize);
    static void Refresh(BindableObject b, object o, object n) => ((NVDataPager)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _label.Text = NVDataPagerLogic.Caption(PageIndex, TotalCount, PageSize);
        _label.TextColor = NVTheme.Current.Muted;
    }
}

public class NVDataGrid : ThemeAwareView
{
    public static readonly BindableProperty ColumnsProperty = BindableProperty.Create(nameof(Columns), typeof(IList<NVGridColumn>), typeof(NVDataGrid), new List<NVGridColumn>(), propertyChanged: Refresh);
    public static readonly BindableProperty RowsProperty = BindableProperty.Create(nameof(Rows), typeof(IList<IDictionary<string, object?>>), typeof(NVDataGrid), new List<IDictionary<string, object?>>(), propertyChanged: Refresh);
    readonly Grid _grid = new() { ColumnSpacing = 8, RowSpacing = 6 };

    public NVDataGrid() { Content = _grid; ApplyTheme(); }
    public IList<NVGridColumn> Columns { get => (IList<NVGridColumn>)GetValue(ColumnsProperty); set => SetValue(ColumnsProperty, value); }
    public IList<IDictionary<string, object?>> Rows { get => (IList<IDictionary<string, object?>>)GetValue(RowsProperty); set => SetValue(RowsProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVDataGrid)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _grid.Children.Clear();
        _grid.ColumnDefinitions.Clear();
        _grid.RowDefinitions.Clear();
        var cols = Columns ?? [];
        var rows = Rows ?? [];
        foreach (var _ in cols)
        {
            _grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        }
        _grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        for (var r = 0; r < rows.Count; r++)
        {
            _grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        }

        for (var c = 0; c < cols.Count; c++)
        {
            var header = new Label { Text = cols[c].Header, FontFamily = NVTokens.FontSemiBold, TextColor = NVTheme.Current.Ink };
            Grid.SetColumn(header, c);
            Grid.SetRow(header, 0);
            _grid.Add(header);
        }

        for (var r = 0; r < rows.Count; r++)
        {
            for (var c = 0; c < cols.Count; c++)
            {
                rows[r].TryGetValue(cols[c].Key, out var value);
                var cell = new Label { Text = value?.ToString() ?? "", FontFamily = NVTokens.FontRegular, TextColor = NVTheme.Current.Muted };
                Grid.SetColumn(cell, c);
                Grid.SetRow(cell, r + 1);
                _grid.Add(cell);
            }
        }
    }
}

public class NVTreeView : ThemeAwareView
{
    public static readonly BindableProperty RootsProperty = BindableProperty.Create(nameof(Roots), typeof(IList<NVTreeNode>), typeof(NVTreeView), new List<NVTreeNode>(), propertyChanged: Refresh);
    readonly VerticalStackLayout _list = new() { Spacing = 4 };
    public NVTreeView() { Content = _list; ApplyTheme(); }
    public IList<NVTreeNode> Roots { get => (IList<NVTreeNode>)GetValue(RootsProperty); set => SetValue(RootsProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVTreeView)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _list.Children.Clear();
        void Add(NVTreeNode node, int depth)
        {
            _list.Children.Add(new Label
            {
                Text = new string(' ', depth * 2) + (node.IsExpanded ? "▾ " : "▸ ") + node.Title,
                TextColor = NVTheme.Current.Ink,
                FontFamily = NVTokens.FontRegular
            });
            if (node.IsExpanded)
            {
                foreach (var child in node.Children)
                {
                    Add(child, depth + 1);
                }
            }
        }
        foreach (var root in Roots ?? [])
        {
            Add(root, 0);
        }
    }
}

public class NVKanban : ThemeAwareView
{
    public static readonly BindableProperty ColumnsProperty = BindableProperty.Create(nameof(Columns), typeof(IList<NVKanbanColumn>), typeof(NVKanban), new List<NVKanbanColumn>(), propertyChanged: Refresh);
    readonly HorizontalStackLayout _row = new() { Spacing = NVTokens.Space3 };
    public NVKanban() { Content = new ScrollView { Orientation = ScrollOrientation.Horizontal, Content = _row }; ApplyTheme(); }
    public IList<NVKanbanColumn> Columns { get => (IList<NVKanbanColumn>)GetValue(ColumnsProperty); set => SetValue(ColumnsProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVKanban)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _row.Children.Clear();
        foreach (var column in Columns ?? [])
        {
            var stack = new VerticalStackLayout { Spacing = NVTokens.Space2, WidthRequest = 220 };
            stack.Children.Add(new Label { Text = column.Title, FontFamily = NVTokens.FontSemiBold, TextColor = NVTheme.Current.Ink });
            foreach (var card in column.Cards)
            {
                stack.Children.Add(new NVCard { Title = card.Title, Body = card.Subtitle ?? "" });
            }
            _row.Children.Add(stack);
        }
    }
}

public class NVTreeDataGrid : NVDataGrid { }

public class NVDataForm : ThemeAwareView
{
    public static readonly BindableProperty FieldsProperty = BindableProperty.Create(nameof(Fields), typeof(IList<NVFormField>), typeof(NVDataForm), new List<NVFormField>(), propertyChanged: Refresh);
    readonly VerticalStackLayout _stack = new() { Spacing = NVTokens.Space3 };
    public NVDataForm() { Content = _stack; ApplyTheme(); }
    public IList<NVFormField> Fields { get => (IList<NVFormField>)GetValue(FieldsProperty); set => SetValue(FieldsProperty, value); }
    static void Refresh(BindableObject b, object o, object n) => ((NVDataForm)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _stack.Children.Clear();
        foreach (var field in Fields ?? [])
        {
            View editor = field.Kind switch
            {
                NVFormFieldKind.Boolean => new NVSwitch { Text = field.Label, IsOn = field.Value is true },
                NVFormFieldKind.Number => new NVNumericEntry { Label = field.Label, Value = field.Value is double d ? d : 0 },
                NVFormFieldKind.Date => new NVDatePicker { Label = field.Label, Date = field.Value is DateTime dt ? dt : DateTime.Today },
                NVFormFieldKind.Enum => new NVComboBox { Label = field.Label, SelectedItem = field.Value?.ToString() ?? "" },
                _ => new NVTextField { Label = field.Label, Text = field.Value?.ToString() ?? "", Error = field.Error ?? "" }
            };
            _stack.Children.Add(editor);
        }
    }
}
