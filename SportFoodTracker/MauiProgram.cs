using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SportFoodTracker.Context.ErnaehrungsplanDatabase;
using SportFoodTracker.Context.Sportplan;
using SportFoodTracker.Context.TrainingDatabase;
using SportFoodTracker.Models.Ernähungsplan;
using SportFoodTracker.Service;

namespace SportFoodTracker
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
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton<SportsPlanDatabase>();
            builder.Services.AddSingleton<TrainingDatabase>();
            builder.Services.AddSingleton<NutritionPlanDatabase>();
            builder.Services.AddSingleton<SportsplanService>();
            builder.Services.AddSingleton<TrainingService>();




#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
