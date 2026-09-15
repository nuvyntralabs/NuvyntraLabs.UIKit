namespace NuvyntraLabs.UIKit.Sample.Pages;

public sealed class ActionsPage : ContentPage
{
    public ActionsPage()
    {
        Title = "Actions";
        Content = new ScrollView
        {
            Padding = NVTokens.Space5,
            Content = new VerticalStackLayout
            {
                Spacing = NVTokens.Space4,
                Children =
                {
                    new NVButton { Text = "Filled", Variant = NVButtonVariant.Filled },
                    new NVButton { Text = "Tonal", Variant = NVButtonVariant.Tonal },
                    new NVButton { Text = "Outline", Variant = NVButtonVariant.Outline },
                    new NVButton { Text = "Ghost", Variant = NVButtonVariant.Ghost },
                    new NVButton { Text = "Danger", Variant = NVButtonVariant.Danger },
                    new NVCheckBox { Text = "Accept terms", IsChecked = true },
                    new NVRadioGroup
                    {
                        GroupName = "plan",
                        Children =
                        {
                            new NVRadioButton { Text = "Monthly", IsChecked = true },
                            new NVRadioButton { Text = "Yearly" }
                        }
                    }
                }
            }
        };

        NVTheme.Current.Changed += (_, _) => BackgroundColor = NVTheme.Current.Paper;
        BackgroundColor = NVTheme.Current.Paper;
    }
}
