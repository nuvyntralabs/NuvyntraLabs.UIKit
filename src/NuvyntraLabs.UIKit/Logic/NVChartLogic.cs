namespace NuvyntraLabs.UIKit;

/// <summary>Maps <see cref="NVChartSeriesKind"/> to drawable x/y values. Empty series is safe.</summary>
public static class NVChartLogic
{
    public readonly record struct Point(double X, double Y, string Label);

    public static IReadOnlyList<Point> Map(NVChartSeries? series)
    {
        if (series?.Points is null || series.Points.Count == 0)
        {
            return [];
        }

        return series.Points.Select((point, index) =>
        {
            var y = series.Kind is NVChartSeriesKind.Candle or NVChartSeriesKind.Ohlc
                ? point.Close ?? point.Y
                : point.Y;
            return new Point(index, y, string.IsNullOrWhiteSpace(point.Label) ? index.ToString() : point.Label);
        }).ToList();
    }

    public static bool IsEmpty(IEnumerable<NVChartSeries>? series) =>
        series is null || !series.Any(item => item.Points is { Count: > 0 });
}
