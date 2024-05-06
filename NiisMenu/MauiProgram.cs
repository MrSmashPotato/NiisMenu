using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;

namespace NiisMenu
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseLocalNotification()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("LarosaBlack.ttf", "LarosaBlack");
                    fonts.AddFont("LarosaBold.ttf", "LarosaBold");
                    fonts.AddFont("LarosaLight.ttf", "LarosaLight");
                    fonts.AddFont("LarosaRegular.ttf", "LarosaRegular");
                    fonts.AddFont("LarosaMedium.ttf", "LarosaMedium");
                    fonts.AddFont("LarosaThin.ttf", "LarosaThin");

                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
