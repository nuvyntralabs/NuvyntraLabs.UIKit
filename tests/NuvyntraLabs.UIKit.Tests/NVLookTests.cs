namespace NuvyntraLabs.UIKit.Tests;

[Collection("UIKit")]
public class NVLookTests
{
    [Fact]
    public void Mask_keeps_digits_and_literals()
    {
        Assert.Equal("+91 98765", NVMaskLogic.Apply("+00 00000 00000", "9198765"));
        Assert.Equal("1234", NVMaskLogic.Apply("0000 0000 0000 0000", "1234"));
        Assert.Equal("", NVMaskLogic.Apply("0000", ""));
    }

    [Fact]
    public void Markdown_renders_heading_and_body()
    {
        var blocks = NVMarkdownLogic.Parse("# Lumina\nWarm paper.\n- Tokens");
        Assert.Equal(NVTextRole.Display, blocks[0].Role);
        Assert.Equal("Lumina", blocks[0].Text);
        Assert.Equal("Warm paper.", blocks[1].Text);
        Assert.StartsWith("• ", blocks[2].Text);
    }

    [Fact]
    public void TreeView_toggles_expand()
    {
        var leaf = new NVTreeNode { Title = "Leaf" };
        var root = new NVTreeNode { Title = "Root", Children = { leaf } };
        var tree = new NVTreeView { Roots = [root] };
        Assert.False(root.IsExpanded);
        tree.Toggle(root);
        Assert.True(root.IsExpanded);
        tree.Toggle(root);
        Assert.False(root.IsExpanded);
    }

    [Fact]
    public void PinPad_presses_digits_and_backspace()
    {
        var pad = new NVPinPad();
        pad.Press("1");
        pad.Press("0");
        pad.Press("4");
        pad.Press("2");
        pad.Press("9");
        Assert.Equal("1042", pad.Code);
        pad.Press("⌫");
        Assert.Equal("104", pad.Code);
    }

    [Fact]
    public void Copyable_marks_copied()
    {
        var copy = new NVCopyable { Text = "NUV-2048" };
        Assert.False(copy.Copied);
        copy.Copied = true;
        Assert.True(copy.Copied);
    }

    [Fact]
    public void Emoji_chip_sets_selected()
    {
        var picker = new NVEmojiPicker();
        picker.Selected = "🔥";
        Assert.Equal("🔥", picker.Selected);
        Assert.Contains("🔥", picker.VisibleGlyphs);
    }

    [Fact]
    public void Expander_hosts_panel()
    {
        var body = new NVBodyText { Text = "Inside" };
        var expander = new NVExpander { Title = "More", IsExpanded = true, Panel = body };
        Assert.Same(body, expander.Panel);
        expander.IsExpanded = false;
        Assert.False(expander.IsExpanded);
    }

    [Fact]
    public void AutoComplete_shows_filtered_hits()
    {
        var field = new NVAutoComplete
        {
            Suggestions = new List<string> { "Aurora", "Austin", "Ink" },
            Text = "au"
        };
        Assert.Equal(["Aurora", "Austin"], field.Filter().ToList());
    }

    [Fact]
    public void Map_and_video_construct_with_chrome()
    {
        var map = new NVMap { Place = "Studio" };
        Assert.Equal("Studio", map.Place);
        var video = new NVVideoPlayer();
        video.IsPlaying = true;
        Assert.True(video.IsPlaying);
    }
}
