namespace NuvyntraLabs.UIKit.Tests;

[Collection("UIKit")]
public class NVApp13Tests
{
    public static TheoryData<Type> NextControls =>
    [
        typeof(NVSpeedDial),
        typeof(NVSubscriptionCard),
        typeof(NVEmojiPicker),
        typeof(NVPivotGrid),
        typeof(NVPropertyGrid),
        typeof(NVJsonTree),
        typeof(NVDiffView),
        typeof(NVCodeEditor),
        typeof(NVCallBar),
        typeof(NVInCallView),
        typeof(NVSyncConflictCard),
        typeof(NVUploadTile),
        typeof(NVDeviceSheet),
        typeof(NVPrintPreview),
        typeof(NVNfcPrompt),
        typeof(NVReviewPrompt)
    ];

    [Theory]
    [MemberData(nameof(NextControls))]
    public void Default_ctor_does_not_throw(Type type)
    {
        var view = (View)Activator.CreateInstance(type)!;
        Assert.NotNull(view);
        NVAccessibility.Name(view, type.Name);
        Assert.Equal(type.Name, AutomationProperties.GetName(view));
        view.IsEnabled = false;
        Assert.False(view.IsEnabled);
        Assert.True(NVAccessibility.BodyContrastOk(NVTheme.Current.Ink, NVTheme.Current.Paper));
    }

    [Fact]
    public void Speed_dial_clamps_to_five_and_fans_when_open()
    {
        var dial = new NVSpeedDial
        {
            Actions = Enumerable.Range(1, 7).Select(i => new NVSpeedDialAction { Text = $"A{i}" }).ToList()
        };
        Assert.Equal(5, dial.VisibleActions.Count);
        dial.IsOpen = true;
        Assert.True(dial.IsOpen);
        Assert.Equal(5, dial.VisibleActions.Count);
    }

    [Fact]
    public void Emoji_filter_is_case_insensitive()
    {
        var hits = NVEmojiPicker.Filter(["😀 grin", "🔥 fire"], "FIRE");
        Assert.Equal("🔥 fire", Assert.Single(hits));
        var picker = new NVEmojiPicker { Glyphs = ["😀", "🔥"], Query = "" };
        Assert.Equal(2, picker.VisibleGlyphs.Count);
        picker.Selected = "🔥";
        Assert.Equal("🔥", picker.Selected);
    }

    [Fact]
    public void Pivot_empty_source_renders_without_throw()
    {
        var pivot = new NVPivotGrid { Facts = [] };
        Assert.Empty(pivot.RowLabels);
        Assert.Empty(pivot.ColumnLabels);
        pivot.Facts =
        [
            new NVPivotFact { Row = "Ada", Column = "Q1", Value = 2 },
            new NVPivotFact { Row = "Ada", Column = "Q1", Value = 3 }
        ];
        Assert.Equal(5, NVPivotGrid.Sum(pivot.Facts, "Ada", "Q1"));
        Assert.Single(pivot.RowLabels);
    }

    [Fact]
    public void Json_tree_parses_and_empty_is_safe()
    {
        Assert.Empty(NVJsonTree.Parse(""));
        Assert.Equal("Invalid JSON", Assert.Single(NVJsonTree.Parse("{")).Title);
        var tree = new NVJsonTree { Json = """{"ok":true}""" };
        Assert.Equal("root: { }", Assert.Single(tree.Roots).Title);
        Assert.Equal("ok: true", Assert.Single(tree.Roots[0].Children).Title);
    }

    [Fact]
    public void Diff_unified_marks_add_and_remove()
    {
        var lines = NVDiffView.Unified("one\ntwo", "one\nthree");
        Assert.Contains("  one", lines);
        Assert.Contains("- two", lines);
        Assert.Contains("+ three", lines);
        var view = new NVDiffView { Left = "a", Right = "b", Mode = NVDiffMode.SideBySide };
        Assert.Equal(NVDiffMode.SideBySide, view.Mode);
        view.Mode = NVDiffMode.Unified;
        Assert.Contains("- a", view.Lines);
    }

    [Fact]
    public void Code_editor_round_trips_text()
    {
        var editor = new NVCodeEditor { Text = "hi" };
        Assert.Equal("hi", editor.Text);
        editor.IsEnabled = false;
        Assert.False(editor.IsEnabled);
    }

    [Fact]
    public void Host_chrome_constructs_with_null_commands()
    {
        var bar = new NVCallBar { MuteCommand = null, EndCommand = null };
        var call = new NVInCallView { MuteCommand = null, EndCommand = null, Keypad = null };
        var conflict = new NVSyncConflictCard { KeepCommand = null, TakeRemoteCommand = null, Local = "L", Remote = "R" };
        var upload = new NVUploadTile { RetryCommand = null, FileName = "a.bin", Bytes = 512 };
        var sheet = new NVDeviceSheet { ConnectCommand = null, Devices = [] };
        var print = new NVPrintPreview { PrintCommand = null, ShareCommand = null, Page = null };
        var nfc = new NVNfcPrompt { Status = "Ready" };
        var review = new NVReviewPrompt { NotNowCommand = null, ReviewCommand = null, Rating = 4 };
        Assert.Equal("On a call", bar.Title);
        Assert.Equal("Ada", call.Name);
        Assert.Equal("L", conflict.Local);
        Assert.Equal(512, upload.Bytes);
        Assert.False(sheet.IsOpen);
        Assert.Null(print.PrintCommand);
        Assert.Equal("Ready", nfc.Status);
        Assert.Equal(4, review.Rating);
        var card = new NVSubscriptionCard { CtaCommand = null, Name = "Yearly", Price = "$72" };
        Assert.Equal("Yearly", card.Name);
    }
}
