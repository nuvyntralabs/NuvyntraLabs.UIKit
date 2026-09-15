namespace NuvyntraLabs.UIKit.Sample.Pages;

static class Gallery
{
    public static View Page(string title, string intro, IEnumerable<View> blocks)
    {
        var stack = new VerticalStackLayout { Spacing = NVTokens.Space4 };
        stack.Children.Add(new NVHeading { Text = title, Role = NVTextRole.Title });
        stack.Children.Add(new NVBodyText { Text = intro });
        foreach (var block in blocks)
        {
            stack.Children.Add(block);
        }

        return new ScrollView
        {
            Padding = NVTokens.Space5,
            Content = stack
        };
    }

    public static View Chapter(string title) =>
        new NVSectionHeader { Text = title };

    public static View Sample(int number, string typeName, string note, View demo) =>
        new VerticalStackLayout
        {
            Spacing = NVTokens.Space2,
            Children =
            {
                new NVHeading { Text = $"{number:00}  {typeName}", Role = NVTextRole.Body },
                new NVCaptionText { Text = note },
                demo,
                new NVDivider()
            }
        };
}

public abstract class CatalogSectionPage : ContentPage
{
    protected CatalogSectionPage(string title, string intro, Func<IEnumerable<View>> build)
    {
        Title = title;
        Content = Gallery.Page(title, intro, build());
        NVTheme.Current.Changed += (_, _) => BackgroundColor = NVTheme.Current.Paper;
        BackgroundColor = NVTheme.Current.Paper;
    }
}
