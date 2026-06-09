using Microsoft.Extensions.Logging;
using ALOBOUTIQUE.Views;
using ALOBOUTIQUE.ViewModels;

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

            // 1. REGISTRO DE VIEWMODELS
            builder.Services.AddSingleton<PosViewModel>();
            builder.Services.AddSingleton<CustomersViewModel>();
            // builder.Services.AddSingleton<InventoryViewModel>(); // Descomenta cuando crees la clase
            // builder.Services.AddSingleton<HistoryViewModel>(); // Descomenta cuando crees la clase

            // 2. REGISTRO DE VISTAS (PÁGINAS)
            builder.Services.AddSingleton<PosPage>();
            builder.Services.AddSingleton<CustomersPage>();
            // builder.Services.AddSingleton<InventoryPage>(); // Descomenta cuando crees el XAML
            // builder.Services.AddSingleton<HistoryPage>(); // Descomenta cuando crees el XAML

            builder.Logging.AddDebug();

            return builder.Build();
        }
    }
}