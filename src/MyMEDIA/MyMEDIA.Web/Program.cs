using MyMEDIA.Web.Components;
using MyMEDIA.Client.Pages;
using MyMEDIA.Client.Services;
using Blazored.LocalStorage;
using MyMEDIA.Client.Services.Interfaces; // Assuming interfaces are here

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IApiServices, ApiService>();
builder.Services.AddScoped<IUserSessionService, UserSessionService>();
builder.Services.AddScoped<IFavoritosTemporariosServices, FavoritosTemporariosService>();
builder.Services.AddScoped<ICarrinhosComprasTemporarioServices, CarrinhoComprasTemporarioService>();
builder.Services.AddScoped<ISliderUtilsServices, SliderUtilsServices>();
builder.Services.AddScoped<IUtilsTamanhoServices, UtilsTamanhoServices>();
builder.Services.AddScoped<ICardsUtilsServices, CardsUtilsServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(MyMEDIA.Client.Pages.Products).Assembly);

app.Run();
