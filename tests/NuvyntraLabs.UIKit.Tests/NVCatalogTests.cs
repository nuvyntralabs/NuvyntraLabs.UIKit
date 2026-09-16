namespace NuvyntraLabs.UIKit.Tests;

[Collection("UIKit")]
public class NVCatalogTests
{
    [Fact]
    public void Controls_cover_the_1_3_surface()
    {
        Assert.Equal(201, NVCatalog.Controls.Count);
        Assert.Equal(201, NVCatalog.ControlNames.Distinct().Count());
        Assert.All(NVCatalog.ControlNames, name => Assert.StartsWith("NV", name));
        Assert.Contains(typeof(NVCommandPalette), NVCatalog.Controls);
        Assert.Contains(typeof(NVHeatCalendar), NVCatalog.Controls);
        Assert.Contains(typeof(NVSpeedDial), NVCatalog.Controls);
        Assert.Contains(typeof(NVReviewPrompt), NVCatalog.Controls);
    }

    [Fact]
    public void Pages_cover_the_1_3_recipes()
    {
        Assert.Equal(66, NVCatalog.Pages.Count);
        Assert.Equal(66, NVCatalog.PageNames.Distinct().Count());
        Assert.All(NVCatalog.PageNames, name => Assert.EndsWith("View", name));
        Assert.Contains(typeof(NVInvoiceView), NVCatalog.Pages);
        Assert.Contains(typeof(NVWhatsNewView), NVCatalog.Pages);
        Assert.Contains(typeof(NVAddressFormView), NVCatalog.Pages);
    }

    [Fact]
    public void Foundation_and_helpers_are_present()
    {
        Assert.Equal(8, NVCatalog.Foundation.Count);
        Assert.Contains(typeof(NVTheme), NVCatalog.Foundation);
        Assert.Contains(typeof(NVRadioGroup), NVCatalog.Helpers);
        Assert.Contains(typeof(NVFormField), NVCatalog.Helpers);
    }

    [Fact]
    public void Sample_gallery_names_every_catalog_type()
    {
        var pages = FindSamplePages();
        var source = string.Join('\n', Directory.GetFiles(pages, "*.cs").Select(File.ReadAllText));
        foreach (var name in NVCatalog.ControlNames.Concat(NVCatalog.Foundation.Select(t => t.Name)))
        {
            Assert.Contains(name, source);
        }

        Assert.Contains("NVCatalog.Pages", source);
        Assert.Contains(nameof(NVRadioGroup), source);
        Assert.Contains(nameof(NVFormField), source);
    }

    static string FindSamplePages()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            var pages = Path.Combine(dir.FullName, "samples", "NuvyntraLabs.UIKit.Sample", "Pages");
            if (Directory.Exists(pages))
            {
                return pages;
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate samples/NuvyntraLabs.UIKit.Sample/Pages.");
    }
}
