namespace NuvyntraLabs.UIKit;

/// <summary>Expands a tiny recurrence language: DAILY, WEEKLY, optional COUNT.</summary>
public static class NVRecurrence
{
    public const int DefaultCap = 64;

    public static IReadOnlyList<DateTime> Expand(DateTime start, string? rule, int cap = DefaultCap)
    {
        cap = Math.Clamp(cap, 1, 365);
        if (string.IsNullOrWhiteSpace(rule))
        {
            return [start];
        }

        var daily = rule.Contains("WEEKLY", StringComparison.OrdinalIgnoreCase)
            ? 7
            : 1;
        var count = cap;
        var match = System.Text.RegularExpressions.Regex.Match(rule, @"COUNT=(\d+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        if (match.Success && int.TryParse(match.Groups[1].Value, out var parsed))
        {
            count = Math.Min(cap, parsed);
        }

        return Enumerable.Range(0, count).Select(i => start.AddDays(i * daily)).ToArray();
    }
}
