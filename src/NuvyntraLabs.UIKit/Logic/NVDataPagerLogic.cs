namespace NuvyntraLabs.UIKit;

/// <summary>Shared page math for <see cref="NVDataPager"/>, grid, and list.</summary>
public static class NVDataPagerLogic
{
    public static int PageCount(int itemCount, int pageSize)
    {
        if (itemCount <= 0 || pageSize <= 0)
        {
            return 0;
        }

        return (int)Math.Ceiling(itemCount / (double)pageSize);
    }

    public static int ClampPage(int page, int pageCount)
    {
        if (pageCount <= 0)
        {
            return 0;
        }

        return Math.Clamp(page, 1, pageCount);
    }

    public static int Skip(int page, int pageSize) => Math.Max(0, (page - 1) * pageSize);

    public static string Caption(int pageIndexZeroBased, int itemCount, int pageSize)
    {
        var pages = PageCount(itemCount, pageSize);
        if (pages == 0)
        {
            return "0 / 0";
        }

        return $"{pageIndexZeroBased + 1} / {pages}";
    }
}
