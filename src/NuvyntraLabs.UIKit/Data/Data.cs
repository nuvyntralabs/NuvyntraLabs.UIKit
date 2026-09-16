namespace NuvyntraLabs.UIKit;

public class NVCollectionView : ThemeAwareView
{
    public static readonly BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(IList<NVListItem>), typeof(NVCollectionView), new List<NVListItem>(), propertyChanged: Refresh);
    public static readonly BindableProperty LayoutModeProperty = BindableProperty.Create(nameof(LayoutMode), typeof(NVLayoutMode), typeof(NVCollectionView), NVLayoutMode.List, propertyChanged: Refresh);
    public static readonly BindableProperty SelectionModeProperty = BindableProperty.Create(nameof(SelectionMode), typeof(NVSelectionKind), typeof(NVCollectionView), NVSelectionKind.None, propertyChanged: Refresh);
    public static readonly BindableProperty SelectedItemsProperty = BindableProperty.Create(nameof(SelectedItems), typeof(IList<NVListItem>), typeof(NVCollectionView), new List<NVListItem>(), BindingMode.TwoWay);
    public static readonly BindableProperty GroupedProperty = BindableProperty.Create(nameof(Grouped), typeof(bool), typeof(NVCollectionView), false, propertyChanged: Refresh);
    public static readonly BindableProperty AllowSwipeProperty = BindableProperty.Create(nameof(AllowSwipe), typeof(bool), typeof(NVCollectionView), false, propertyChanged: Refresh);

    readonly CollectionView _list = new();

    public NVCollectionView()
    {
        _list.SelectionChanged += (_, e) =>
        {
            SelectedItems = e.CurrentSelection.OfType<NVListItem>().ToList();
        };
        Content = _list;
        ApplyTheme();
    }

    public IList<NVListItem> Items { get => (IList<NVListItem>)GetValue(ItemsProperty); set => SetValue(ItemsProperty, value); }
    public NVLayoutMode LayoutMode { get => (NVLayoutMode)GetValue(LayoutModeProperty); set => SetValue(LayoutModeProperty, value); }
    public NVSelectionKind SelectionMode { get => (NVSelectionKind)GetValue(SelectionModeProperty); set => SetValue(SelectionModeProperty, value); }
    public IList<NVListItem> SelectedItems { get => (IList<NVListItem>)GetValue(SelectedItemsProperty); set => SetValue(SelectedItemsProperty, value); }
    public bool Grouped { get => (bool)GetValue(GroupedProperty); set => SetValue(GroupedProperty, value); }
    public bool AllowSwipe { get => (bool)GetValue(AllowSwipeProperty); set => SetValue(AllowSwipeProperty, value); }
    public IReadOnlyList<NVListGroup> Groups => Group(Items);

    public static IReadOnlyList<NVListGroup> Group(IEnumerable<NVListItem>? items) =>
        (items ?? [])
            .GroupBy(item => string.IsNullOrWhiteSpace(item.Title) ? "#" : item.Title[..1].ToUpperInvariant())
            .OrderBy(g => g.Key)
            .Select(g =>
            {
                var group = new NVListGroup { Name = g.Key };
                group.AddRange(g);
                return group;
            })
            .ToList();

    public void Select(NVListItem item)
    {
        if (SelectionMode == NVSelectionKind.None)
        {
            return;
        }

        var next = SelectedItems?.ToList() ?? [];
        if (SelectionMode == NVSelectionKind.Single)
        {
            next = [item];
        }
        else if (!next.Contains(item))
        {
            next.Add(item);
        }

        SelectedItems = next;
    }

    static void Refresh(BindableObject b, object o, object n) => ((NVCollectionView)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        _list.ItemsLayout = LayoutMode == NVLayoutMode.List
            ? LinearItemsLayout.Vertical
            : new GridItemsLayout(2, ItemsLayoutOrientation.Vertical);
        _list.SelectionMode = SelectionMode switch
        {
            NVSelectionKind.Single => Microsoft.Maui.Controls.SelectionMode.Single,
            NVSelectionKind.Multiple => Microsoft.Maui.Controls.SelectionMode.Multiple,
            _ => Microsoft.Maui.Controls.SelectionMode.None
        };
        _list.IsGrouped = Grouped;
        _list.ItemsSource = Grouped ? Groups : Items;
        _list.GroupHeaderTemplate = null;
        _list.ItemTemplate = new DataTemplate(() =>
        {
            var card = new NVCard();
            card.SetBinding(NVCard.TitleProperty, nameof(NVListItem.Title));
            card.SetBinding(NVCard.BodyProperty, nameof(NVListItem.Subtitle));
            if (!AllowSwipe)
            {
                return card;
            }

            return new SwipeView
            {
                RightItems = [new SwipeItem { Text = "More", BackgroundColor = NVTheme.Current.Mist }],
                Content = card
            };
        });
        if (Grouped)
        {
            _list.GroupHeaderTemplate = new DataTemplate(() =>
            {
                var header = new NVSectionHeader();
                header.SetBinding(NVSectionHeader.TextProperty, nameof(NVListGroup.Name));
                return header;
            });
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
    public static readonly BindableProperty FilterProperty = BindableProperty.Create(nameof(Filter), typeof(string), typeof(NVDataGrid), "", propertyChanged: Refresh);
    public static readonly BindableProperty SortKeyProperty = BindableProperty.Create(nameof(SortKey), typeof(string), typeof(NVDataGrid), "", propertyChanged: Refresh);
    public static readonly BindableProperty SortDirectionProperty = BindableProperty.Create(nameof(SortDirection), typeof(NVSortDirection), typeof(NVDataGrid), NVSortDirection.None, propertyChanged: Refresh);
    public static readonly BindableProperty PageSizeProperty = BindableProperty.Create(nameof(PageSize), typeof(int), typeof(NVDataGrid), 0, propertyChanged: Refresh);
    public static readonly BindableProperty PageIndexProperty = BindableProperty.Create(nameof(PageIndex), typeof(int), typeof(NVDataGrid), 0, BindingMode.TwoWay, propertyChanged: Refresh);
    public static readonly BindableProperty FrozenColumnCountProperty = BindableProperty.Create(nameof(FrozenColumnCount), typeof(int), typeof(NVDataGrid), 0, propertyChanged: Refresh);

    readonly Grid _grid = new() { ColumnSpacing = 8, RowSpacing = 6 };
    readonly NVDataPager _pager = new();
    IDictionary<string, object?>? _draft;
    int _editIndex = -1;

    public NVDataGrid()
    {
        _pager.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(NVDataPager.PageIndex) && PageIndex != _pager.PageIndex)
            {
                PageIndex = _pager.PageIndex;
            }
        };
        Content = new VerticalStackLayout { Spacing = NVTokens.Space2, Children = { _grid, _pager } };
        ApplyTheme();
    }

    public IList<NVGridColumn> Columns { get => (IList<NVGridColumn>)GetValue(ColumnsProperty); set => SetValue(ColumnsProperty, value); }
    public IList<IDictionary<string, object?>> Rows { get => (IList<IDictionary<string, object?>>)GetValue(RowsProperty); set => SetValue(RowsProperty, value); }
    public string Filter { get => (string)GetValue(FilterProperty); set => SetValue(FilterProperty, value); }
    public string SortKey { get => (string)GetValue(SortKeyProperty); set => SetValue(SortKeyProperty, value); }
    public NVSortDirection SortDirection { get => (NVSortDirection)GetValue(SortDirectionProperty); set => SetValue(SortDirectionProperty, value); }
    public int PageSize { get => (int)GetValue(PageSizeProperty); set => SetValue(PageSizeProperty, value); }
    public int PageIndex { get => (int)GetValue(PageIndexProperty); set => SetValue(PageIndexProperty, value); }
    public int FrozenColumnCount { get => (int)GetValue(FrozenColumnCountProperty); set => SetValue(FrozenColumnCountProperty, value); }
    public bool IsEditing => _editIndex >= 0;
    public IReadOnlyList<IDictionary<string, object?>> VisibleRows { get; private set; } = [];

    public void CycleSort(string key)
    {
        if (!string.Equals(SortKey, key, StringComparison.Ordinal))
        {
            SortKey = key;
            SortDirection = NVSortDirection.Ascending;
            return;
        }

        SortDirection = NVGridLogic.Cycle(SortDirection);
    }

    public void BeginEdit(int index)
    {
        var rows = Rows ?? [];
        if (index < 0 || index >= rows.Count)
        {
            return;
        }

        _editIndex = index;
        _draft = NVGridLogic.Copy(rows[index]);
    }

    public void CommitEdit()
    {
        if (_draft is null || _editIndex < 0 || Rows is null || _editIndex >= Rows.Count)
        {
            CancelEdit();
            return;
        }

        Rows[_editIndex] = _draft;
        CancelEdit();
        ApplyTheme();
    }

    public void CancelEdit()
    {
        _editIndex = -1;
        _draft = null;
    }

    static void Refresh(BindableObject b, object o, object n) => ((NVDataGrid)b).ApplyTheme();

    protected override void ApplyTheme()
    {
        var prepared = NVGridLogic.Page(
            NVGridLogic.Sort(NVGridLogic.Filter(Rows, Filter), SortKey, SortDirection),
            PageIndex,
            PageSize);
        VisibleRows = prepared.ToList();
        _pager.IsVisible = PageSize > 0;
        _pager.TotalCount = NVGridLogic.Filter(Rows, Filter).Count;
        _pager.PageSize = PageSize <= 0 ? 10 : PageSize;
        _pager.PageIndex = PageIndex;
        _grid.Children.Clear();
        _grid.ColumnDefinitions.Clear();
        _grid.RowDefinitions.Clear();
        var cols = Columns ?? [];
        foreach (var _ in cols)
        {
            _grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        }

        _grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        for (var r = 0; r < prepared.Count; r++)
        {
            _grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        }

        for (var c = 0; c < cols.Count; c++)
        {
            var key = cols[c].Key;
            var frozen = c < FrozenColumnCount || cols[c].Frozen;
            var header = new Label
            {
                Text = cols[c].Header + (SortKey == key && SortDirection != NVSortDirection.None ? (SortDirection == NVSortDirection.Ascending ? " ↑" : " ↓") : ""),
                FontFamily = NVTokens.FontSemiBold,
                TextColor = NVTheme.Current.Ink,
                BackgroundColor = frozen ? NVTheme.Current.Mist : Colors.Transparent
            };
            var tap = new TapGestureRecognizer();
            tap.Tapped += (_, _) =>
            {
                if (cols.ElementAtOrDefault(c)?.Sortable != false)
                {
                    CycleSort(key);
                }
            };
            header.GestureRecognizers.Add(tap);
            Grid.SetColumn(header, c);
            _grid.Add(header);
        }

        for (var r = 0; r < prepared.Count; r++)
        {
            for (var c = 0; c < cols.Count; c++)
            {
                prepared[r].TryGetValue(cols[c].Key, out var value);
                var cell = new Label
                {
                    Text = value?.ToString() ?? "",
                    FontFamily = NVTokens.FontRegular,
                    TextColor = NVTheme.Current.Muted,
                    BackgroundColor = c < FrozenColumnCount || cols[c].Frozen ? NVTheme.Current.Mist : Colors.Transparent
                };
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

    public void Toggle(NVTreeNode node)
    {
        node.IsExpanded = !node.IsExpanded;
        ApplyTheme();
    }

    protected override void ApplyTheme()
    {
        _list.Children.Clear();
        void Add(NVTreeNode node, int depth)
        {
            var row = new NVListTile
            {
                Title = node.Title,
                Subtitle = node.Children.Count == 0 ? "" : $"{node.Children.Count} child(ren)",
                Kind = node.IsExpanded ? NVIconKind.ChevronDown : NVIconKind.ChevronRight,
                Margin = new Thickness(depth * NVTokens.Space4, 0, 0, 0)
            };
            var tap = new TapGestureRecognizer();
            tap.Tapped += (_, _) => Toggle(node);
            row.GestureRecognizers.Add(tap);
            _list.Children.Add(row);
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

public class NVTreeDataGrid : NVDataGrid
{
    public static readonly BindableProperty RootsProperty = BindableProperty.Create(nameof(Roots), typeof(IList<NVTreeNode>), typeof(NVTreeDataGrid), new List<NVTreeNode>(), propertyChanged: OnRoots);

    public IList<NVTreeNode> Roots { get => (IList<NVTreeNode>)GetValue(RootsProperty); set => SetValue(RootsProperty, value); }

    public void Expand(NVTreeNode node, Func<NVTreeNode, IEnumerable<NVTreeNode>>? loader = null)
    {
        NVGridLogic.ExpandOnce(node, loader);
        Rows = NVGridLogic.Flatten(Roots);
    }

    static void OnRoots(BindableObject b, object o, object n)
    {
        if (b is NVTreeDataGrid grid)
        {
            grid.Rows = NVGridLogic.Flatten(grid.Roots);
            grid.ApplyPublicTheme();
        }
    }

    void ApplyPublicTheme() => ApplyTheme();
}

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
