using MyMEDIA.Web.Components;
using MyMEDIA.Client.Pages; // Para aceder aos componentes do Cliente

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contentor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Configurar o pipeline de pedidos HTTP
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