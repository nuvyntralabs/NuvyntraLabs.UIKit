namespace NuvyntraLabs.UIKit;

/// <summary>Sort / filter / page / edit helpers for <see cref="NVDataGrid"/>.</summary>
public static class NVGridLogic
{
    public static NVSortDirection Cycle(NVSortDirection current) =>
        current switch
        {
            NVSortDirection.None => NVSortDirection.Ascending,
            NVSortDirection.Ascending => NVSortDirection.Descending,
            _ => NVSortDirection.None
        };

    public static IList<IDictionary<string, object?>> Filter(IEnumerable<IDictionary<string, object?>>? rows, string? query)
    {
        var source = (rows ?? []).ToList();
        if (string.IsNullOrWhiteSpace(query))
        {
            return source;
        }

        return source.Where(row => row.Values.Any(value =>
            (value?.ToString() ?? "").Contains(query, StringComparison.OrdinalIgnoreCase))).ToList();
    }

    public static IList<IDictionary<string, object?>> Sort(IEnumerable<IDictionary<string, object?>>? rows, string? key, NVSortDirection direction)
    {
        var source = (rows ?? []).ToList();
        if (string.IsNullOrWhiteSpace(key) || direction == NVSortDirection.None)
        {
            return source;
        }

        IComparable? Key(IDictionary<string, object?> row)
        {
            row.TryGetValue(key, out var value);
            return value as IComparable ?? value?.ToString();
        }

        return direction == NVSortDirection.Ascending
            ? source.OrderBy(Key).ToList()
            : source.OrderByDescending(Key).ToList();
    }

    public static IList<IDictionary<string, object?>> Page(IEnumerable<IDictionary<string, object?>>? rows, int pageIndex, int pageSize)
    {
        var source = (rows ?? []).ToList();
        if (pageSize <= 0)
        {
            return source;
        }

        var skip = Math.Max(0, pageIndex) * pageSize;
        return source.Skip(skip).Take(pageSize).ToList();
    }

    public static IDictionary<string, object?> Copy(IDictionary<string, object?> row) =>
        new Dictionary<string, object?>(row);

    public static void ExpandOnce(NVTreeNode node, Func<NVTreeNode, IEnumerable<NVTreeNode>>? loader)
    {
        if (!node.ChildrenLoaded)
        {
            if (loader is not null)
            {
                foreach (var child in loader(node))
                {
                    node.Children.Add(child);
                }
            }

            node.ChildrenLoaded = true;
        }

        node.IsExpanded = true;
    }

    public static IList<IDictionary<string, object?>> Flatten(IEnumerable<NVTreeNode>? roots, string titleKey = "name")
    {
        var rows = new List<IDictionary<string, object?>>();
        void Walk(NVTreeNode node, int depth)
        {
            rows.Add(new Dictionary<string, object?> { [titleKey] = new string(' ', depth * 2) + node.Title });
            if (node.IsExpanded)
            {
                foreach (var child in node.Children)
                {
                    Walk(child, depth + 1);
                }
            }
        }

        foreach (var root in roots ?? [])
        {
            Walk(root, 0);
        }

        return rows;
    }
}
