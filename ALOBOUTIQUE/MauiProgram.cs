using Microsoft.Extensions.Logging;


namespace ALOBOUTIQUE
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
               
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                    fonts.AddFont("CormorantGaramond-Regular.ttf", "CormorantGaramond-Regular");
                    fonts.AddFont("CormorantGaramond-Italic.ttf", "CormorantGaramond-Italic");
                    fonts.AddFont("CormorantGaramond-Bold.ttf", "CormorantGaramond-Bold");
                    fonts.AddFont("CormorantGaramond-BoldItalic.ttf", "CormorantGaramond-BoldItalic");
                });

    		builder.Logging.AddDebug();


            return builder.Build();
        }
    }
}
