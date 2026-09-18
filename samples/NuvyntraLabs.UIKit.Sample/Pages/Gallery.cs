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

    public static View Sample(int number, string typeName, string note, View demo)
    {
        var surface = new NVSurface { Elevation = 1 };
        surface.Content = new VerticalStackLayout
        {
            Spacing = NVTokens.Space3,
            Children =
            {
                new NVHeading { Text = $"{number:00}  {typeName}", Role = NVTextRole.Title },
                new NVCaptionText { Text = note },
                demo
            }
        };
        return surface;
    }

    public static View Overlay(int number, string typeName, string note, OverlayHost overlay)
    {
        overlay.IsOpen = false;
        var show = new NVButton { Text = $"Show {typeName}", Variant = NVButtonVariant.Tonal };
        var stage = new Grid { MinimumHeightRequest = 8 };
        show.Command = new Command(() => overlay.IsOpen = !overlay.IsOpen);
        overlay.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName != nameof(OverlayHost.IsOpen))
            {
                return;
            }

            show.Text = overlay.IsOpen ? "Dismiss" : $"Show {typeName}";
            stage.HeightRequest = overlay.IsOpen ? 280 : -1;
        };
        stage.Children.Add(overlay);
        return Sample(number, typeName, note, new VerticalStackLayout
        {
            Spacing = NVTokens.Space2,
            Children = { show, stage }
        });
    }
}

public abstract class CatalogSectionPage : ContentPage
{
    protected CatalogSectionPage(string title, string intro, Func<IEnumerable<View>> build)
    {
        Title = title;
        ToolbarItems.Add(new ToolbarItem
        {
            Text = "Theme",
            Command = new Command(() =>
                NVTheme.Current.SetMode(NVTheme.Current.IsDark ? NVThemeMode.Light : NVThemeMode.Dark))
        });
        ToolbarItems.Add(new ToolbarItem
        {
            Text = "RTL",
            Command = new Command(() =>
                NVTheme.Current.SetFlowDirection(
                    NVTheme.Current.IsRtl ? FlowDirection.LeftToRight : FlowDirection.RightToLeft))
        });
        ToolbarItems.Add(new ToolbarItem
        {
            Text = "Type",
            Command = new Command(() =>
            {
                var next = NVTheme.Current.TypeScale switch
                {
                    < 1 => 1,
                    < 2 => 2,
                    _ => 0.8
                };
                NVTheme.Current.SetTypeScale(next);
            })
        });
        Content = Gallery.Page(title, intro, build());
        NVTheme.Current.Changed += (_, _) =>
        {
            FlowDirection = NVTheme.Current.FlowDirection;
            BackgroundColor = NVTheme.Current.Paper;
        };
        FlowDirection = NVTheme.Current.FlowDirection;
        BackgroundColor = NVTheme.Current.Paper;
    }
}
