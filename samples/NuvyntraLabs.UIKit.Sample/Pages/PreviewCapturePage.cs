namespace NuvyntraLabs.UIKit.Sample.Pages;

/// <summary>
/// Headless capture host. Set <c>NUVYNTRA_CAPTURE_PREVIEWS</c> to a directory and launch the sample.
/// Each catalog view is staged, captured, and written as <c>{Name}.png</c>.
/// </summary>
public sealed class PreviewCapturePage : ContentPage
{
    readonly string _output;
    readonly Grid _stage;
    readonly NVCaptionText _status = new();

    public PreviewCapturePage(string output)
    {
        _output = output;
        Title = "Preview capture";
        BackgroundColor = NVTheme.Current.Paper;
        NVTheme.Current.SetMode(NVThemeMode.Light);

        _stage = new Grid
        {
            BackgroundColor = NVTheme.Current.Paper,
            Padding = NVTokens.Space5,
            MinimumHeightRequest = 280
        };

        Content = new VerticalStackLayout
        {
            Spacing = NVTokens.Space3,
            Padding = NVTokens.Space4,
            Children =
            {
                new NVHeading { Text = "Capturing Lumina previews", Role = NVTextRole.Title },
                _status,
                new NVSurface { Elevation = 1, Content = _stage }
            }
        };

        Loaded += async (_, _) => await RunAsync();
    }

    async Task RunAsync()
    {
        Directory.CreateDirectory(_output);
        var items = PreviewCatalog.All();
        var index = 0;
        foreach (var (name, demo) in items)
        {
            index++;
            _status.Text = $"{index}/{items.Count}  {name}";
            _stage.Children.Clear();
            demo.HorizontalOptions = LayoutOptions.Fill;
            _stage.Children.Add(demo);
            await Task.Delay(120);

            var path = Path.Combine(_output, $"{name}.png");
            try
            {
                var shot = await _stage.CaptureAsync();
                if (shot is null)
                {
                    continue;
                }

                await using var stream = await shot.OpenReadAsync();
                await using var file = File.Create(path);
                await stream.CopyToAsync(file);
            }
            catch
            {
                // Keep going — one control must not abort the catalog export.
            }
        }

        _status.Text = $"Wrote {Directory.GetFiles(_output, "*.png").Length} PNGs to {_output}";
        await Task.Delay(400);
        Application.Current?.Quit();
    }
}
