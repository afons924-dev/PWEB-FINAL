using Microsoft.Extensions.Logging;
using Blazored.LocalStorage;
using MyMEDIA.Client.Services;
using MyMEDIA.Client.Services.Interfaces;

namespace MyMEDIA.Maui;

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

        builder.Services.AddBlazoredLocalStorage();
        builder.Services.AddHttpClient();
        builder.Services.AddScoped<IApiServices, ApiService>();
        builder.Services.AddScoped<IUserSessionService, UserSessionService>();
        builder.Services.AddScoped<IFavoritosTemporariosServices, FavoritosTemporariosService>();
        builder.Services.AddScoped<ICarrinhosComprasTemporarioServices, CarrinhoComprasTemporarioService>();
        builder.Services.AddScoped<ISliderUtilsServices, SliderUtilsServices>();
        builder.Services.AddScoped<IUtilsTamanhoServices, UtilsTamanhoServices>();
        builder.Services.AddScoped<ICardsUtilsServices, CardsUtilsServices>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
