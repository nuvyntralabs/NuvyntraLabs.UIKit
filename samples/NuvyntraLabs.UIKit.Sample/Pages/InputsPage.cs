namespace NuvyntraLabs.UIKit.Sample.Pages;

public sealed class InputsPage : CatalogSectionPage
{
    public InputsPage() : base("04  Inputs",
        "Fields in catalog order. NV-INP-01 … NV-INP-22.",
        () =>
        [
            Gallery.Sample(1, "NVTextField", "NV-INP-01  ·  Label, helper", new NVTextField { Label = "Email", Placeholder = "you@studio.dev", Helper = "We never share this." }),
            Gallery.Sample(2, "NVEditor", "NV-INP-02  ·  Multi-line", new NVEditor { Label = "Notes" }),
            Gallery.Sample(3, "NVSearchBar", "NV-INP-03", new NVSearchBar()),
            Gallery.Sample(4, "NVMaskedEntry", "NV-INP-04  ·  Card mask", new NVMaskedEntry { Label = "Card", Mask = "0000 0000 0000 0000" }),
            Gallery.Sample(5, "NVNumericEntry", "NV-INP-05", new NVNumericEntry { Label = "Number", Value = 12 }),
            Gallery.Sample(6, "NVNumericUpDown", "NV-INP-06", new NVNumericUpDown { Value = 3 }),
            Gallery.Sample(7, "NVOtpInput", "NV-INP-07  ·  Length 4", new NVOtpInput { Length = 4, Code = "1042" }),
            Gallery.Sample(8, "NVAutoComplete", "NV-INP-08", new NVAutoComplete { Label = "City", Suggestions = new List<string> { "Aurora", "Austin" } }),
            Gallery.Sample(9, "NVComboBox", "NV-INP-09", new NVComboBox { Label = "Choice", Items = new List<string> { "Aurora", "Ink" }, SelectedItem = "Aurora" }),
            Gallery.Sample(10, "NVPicker", "NV-INP-10", new NVPicker { Label = "Picker", Items = new List<string> { "One", "Two" } }),
            Gallery.Sample(11, "NVDatePicker", "NV-INP-11", new NVDatePicker()),
            Gallery.Sample(12, "NVTimePicker", "NV-INP-12", new NVTimePicker()),
            Gallery.Sample(13, "NVDateTimePicker", "NV-INP-13", new NVDateTimePicker()),
            Gallery.Sample(14, "NVTimeSpanPicker", "NV-INP-14  ·  Minutes", new NVTimeSpanPicker { Value = 30 }),
            Gallery.Sample(15, "NVTemplatedPicker", "NV-INP-15", new NVTemplatedPicker { Label = "Template", Items = new List<string> { "A", "B" } }),
            Gallery.Sample(16, "NVColorPicker", "NV-INP-16", new NVColorPicker()),
            Gallery.Sample(17, "NVSlider", "NV-INP-17  ·  40", new NVSlider { Value = 40 }),
            Gallery.Sample(18, "NVRangeSlider", "NV-INP-18", new NVRangeSlider()),
            Gallery.Sample(19, "NVCircularSlider", "NV-INP-19", new NVCircularSlider { Value = 25 }),
            Gallery.Sample(20, "NVRangeSelector", "NV-INP-20", new NVRangeSelector()),
            Gallery.Sample(21, "NVSignaturePad", "NV-INP-21", new NVSignaturePad()),
            Gallery.Sample(22, "NVRating", "NV-INP-22  ·  4 of 5", new NVRating { Value = 4 })
        ]) { }
}
