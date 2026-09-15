namespace NuvyntraLabs.UIKit.Sample.Pages;

public sealed class InputsPage : ContentPage
{
    public InputsPage()
    {
        Title = "Inputs";
        Content = new ScrollView
        {
            Padding = NVTokens.Space5,
            Content = new VerticalStackLayout
            {
                Spacing = NVTokens.Space4,
                Children =
                {
                    new NVTextField
                    {
                        Label = "Email",
                        Placeholder = "you@studio.dev",
                        Helper = "We never share this."
                    },
                    new NVTextField
                    {
                        Label = "Password",
                        IsPassword = true,
                        Placeholder = "••••••••"
                    },
                    new NVTextField
                    {
                        Label = "Team name",
                        Error = "Required",
                        Text = ""
                    },
                    new NVSurface
                    {
                        Elevation = 1
                    }
                }
            }
        };

        if (Content is ScrollView scroll && scroll.Content is VerticalStackLayout stack)
        {
            var surface = (NVSurface)stack.Children[^1];
            surface.Content = new Label
            {
                Text = "NVSurface hosts any child. Pages compose these controls; they do not invent chrome.",
                FontFamily = NVTokens.FontRegular,
                FontSize = NVTokens.BodySize,
                TextColor = NVTheme.Current.Ink
            };
        }

        NVTheme.Current.Changed += (_, _) => BackgroundColor = NVTheme.Current.Paper;
        BackgroundColor = NVTheme.Current.Paper;
    }
}
