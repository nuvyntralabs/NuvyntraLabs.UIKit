namespace NuvyntraLabs.UIKit.Tests;

public class NVCatalogTests
{
    [Fact]
    public void Controls_cover_the_1_0_surface()
    {
        Assert.Equal(177, NVCatalog.Controls.Count);
        Assert.Equal(177, NVCatalog.ControlNames.Distinct().Count());
        Assert.All(NVCatalog.ControlNames, name => Assert.StartsWith("NV", name));
    }

    [Fact]
    public void Pages_cover_the_1_0_recipes()
    {
        Assert.Equal(57, NVCatalog.Pages.Count);
        Assert.Equal(57, NVCatalog.PageNames.Distinct().Count());
        Assert.All(NVCatalog.PageNames, name => Assert.EndsWith("View", name));
    }

    [Fact]
    public void Foundation_and_helpers_are_present()
    {
        Assert.Equal(8, NVCatalog.Foundation.Count);
        Assert.Contains(typeof(NVTheme), NVCatalog.Foundation);
        Assert.Contains(typeof(NVRadioGroup), NVCatalog.Helpers);
        Assert.Contains(typeof(NVFormField), NVCatalog.Helpers);
    }
}
