namespace NuvyntraLabs.UIKit.Sample;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var dest = Environment.GetEnvironmentVariable("NUVYNTRA_CAPTURE_PREVIEWS");
        if (!string.IsNullOrWhiteSpace(dest))
        {
            return new Window(new Pages.PreviewCapturePage(dest))
            {
                Width = 420,
                Height = 780,
                Title = "UIKit preview capture"
            };
        }

        return new Window(new AppShell());
    }
}
