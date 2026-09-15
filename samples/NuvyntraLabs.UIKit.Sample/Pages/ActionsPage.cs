namespace NuvyntraLabs.UIKit.Sample.Pages;

public sealed class ActionsPage : CatalogSectionPage
{
    public ActionsPage() : base("03  Actions",
        "Buttons and selection. NV-ACT-01 … NV-ACT-10, then NVRadioGroup.",
        () =>
        [
            Gallery.Sample(1, "NVButton", "NV-ACT-01  ·  Filled / tonal / outline / ghost / danger",
                new VerticalStackLayout
                {
                    Spacing = NVTokens.Space2,
                    Children =
                    {
                        new NVButton { Text = "Filled", Variant = NVButtonVariant.Filled },
                        new NVButton { Text = "Tonal", Variant = NVButtonVariant.Tonal },
                        new NVButton { Text = "Outline", Variant = NVButtonVariant.Outline },
                        new NVButton { Text = "Ghost", Variant = NVButtonVariant.Ghost },
                        new NVButton { Text = "Danger", Variant = NVButtonVariant.Danger }
                    }
                }),
            Gallery.Sample(2, "NVIconButton", "NV-ACT-02  ·  Plus", new NVIconButton { Kind = NVIconKind.Plus }),
            Gallery.Sample(3, "NVToggleButton", "NV-ACT-03  ·  On / off", new NVToggleButton { Text = "Toggle" }),
            Gallery.Sample(4, "NVDropDownButton", "NV-ACT-04  ·  Menu", new NVDropDownButton { Text = "Menu", Items = new List<string> { "Edit", "Share" } }),
            Gallery.Sample(5, "NVCheckBox", "NV-ACT-05  ·  Checked", new NVCheckBox { Text = "Accept terms", IsChecked = true }),
            Gallery.Sample(6, "NVRadioButton", "NV-ACT-06  ·  Grouped",
                new NVRadioGroup
                {
                    GroupName = "plan",
                    Children =
                    {
                        new NVRadioButton { Text = "Monthly", IsChecked = true },
                        new NVRadioButton { Text = "Yearly" }
                    }
                }),
            Gallery.Sample(7, "NVSwitch", "NV-ACT-07", new NVSwitch { Text = "Aurora", IsOn = true }),
            Gallery.Sample(8, "NVChip", "NV-ACT-08  ·  Filter", new NVChip { Text = "Filter" }),
            Gallery.Sample(9, "NVSegmentedControl", "NV-ACT-09", new NVSegmentedControl { Items = new List<string> { "Day", "Week" } }),
            Gallery.Sample(10, "NVSpeechToTextButton", "NV-ACT-10  ·  Host supplies recognizer", new NVSpeechToTextButton()),
            Gallery.Sample(11, "NVRadioGroup", "Helper  ·  Owns NVRadioButton grouping", new NVCaptionText { Text = "See sample 06" })
        ]) { }
}
