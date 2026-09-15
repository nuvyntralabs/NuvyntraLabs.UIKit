using Microsoft.Extensions.Logging;

namespace NuvyntraLabs.UIKit.Sample;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseNuvyntraUIKit();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
