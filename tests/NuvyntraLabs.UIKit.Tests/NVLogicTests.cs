namespace NuvyntraLabs.UIKit.Tests;

public class NVLogicTests
{
    [Theory]
    [InlineData(0, 10, 0)]
    [InlineData(10, 10, 1)]
    [InlineData(11, 10, 2)]
    [InlineData(25, 10, 3)]
    public void Pager_page_count(int items, int size, int expected) =>
        Assert.Equal(expected, NVDataPagerLogic.PageCount(items, size));

    [Fact]
    public void Pager_clamp_and_skip_are_one_based()
    {
        Assert.Equal(1, NVDataPagerLogic.ClampPage(0, 4));
        Assert.Equal(4, NVDataPagerLogic.ClampPage(99, 4));
        Assert.Equal(20, NVDataPagerLogic.Skip(3, 10));
        Assert.Equal("2 / 3", NVDataPagerLogic.Caption(1, 25, 10));
        Assert.Equal("0 / 0", NVDataPagerLogic.Caption(0, 0, 10));
    }

    [Theory]
    [InlineData("NUVEXA", NVBarcodeFormat.Code128)]
    [InlineData("hello", NVBarcodeFormat.Qr)]
    public void Barcode_round_trips(string payload, NVBarcodeFormat format)
    {
        var encoded = NVBarcodeCodec.Encode(payload, format);
        Assert.Equal(payload, NVBarcodeCodec.Decode(encoded));
        Assert.Equal(format, NVBarcodeCodec.FormatOf(encoded));
    }

    [Fact]
    public void Barcode_rejects_bad_payload()
    {
        Assert.Throws<FormatException>(() => NVBarcodeCodec.Decode("nope"));
    }

    [Fact]
    public void Recurrence_expands_daily_and_weekly()
    {
        var start = new DateTime(2026, 9, 1);
        Assert.Equal(new[] { start }, NVRecurrence.Expand(start, null));
        Assert.Equal(3, NVRecurrence.Expand(start, "DAILY;COUNT=3").Count);
        var weekly = NVRecurrence.Expand(start, "WEEKLY;COUNT=2");
        Assert.Equal(start.AddDays(7), weekly[1]);
    }

    [Fact]
    public void Form_field_factory_maps_clr_types()
    {
        Assert.Equal(NVFormFieldKind.Boolean, NVFormField.For("On", typeof(bool)).Kind);
        Assert.Equal(NVFormFieldKind.Number, NVFormField.For("N", typeof(int)).Kind);
        Assert.Equal(NVFormFieldKind.Date, NVFormField.For("When", typeof(DateTime)).Kind);
        Assert.Equal(NVFormFieldKind.Enum, NVFormField.For("Tone", typeof(NVBannerTone)).Kind);
        Assert.Equal(NVFormFieldKind.Text, NVFormField.For("Name", typeof(string)).Kind);
    }

    [Fact]
    public void AutoComplete_filter_is_case_insensitive()
    {
        var hits = NVAutoComplete.FilterSuggestions(["Aurora", "Ink", "Paper"], "au").ToList();
        Assert.Single(hits);
        Assert.Equal("Aurora", hits[0]);
    }

    [Fact]
    public void Status_reasons_cover_empty_variants()
    {
        Assert.Equal(10, Enum.GetValues<NVStatusReason>().Length);
    }

    [Theory]
    [InlineData(null, 0)]
    [InlineData("", 0)]
    [InlineData("short", 0)]
    [InlineData("longenough", 1)]
    [InlineData("Longenough1", 3)]
    [InlineData("Longenough1!", 4)]
    public void Password_score(string? password, int expected) =>
        Assert.Equal(expected, NVPasswordRules.Score(password));

    [Fact]
    public void Chart_series_kinds_are_complete()
    {
        Assert.Equal(17, Enum.GetValues<NVChartSeriesKind>().Length);
    }
}
