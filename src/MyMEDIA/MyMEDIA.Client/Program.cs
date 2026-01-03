using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using MyMEDIA.Client;
using MyMEDIA.Client.Services;
using MyMEDIA.Client.Auth;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Configuração do HttpClient para falar com a API (Porta 5000)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5000/") });

builder.Services.AddScoped<CartService>();

// Se a classe BrowserLocalStorage não existir, este passo dará erro a seguir.
// Por agora, vamos manter como está no original para corrigir a sintaxe primeiro.
builder.Services.AddScoped<ILocalStorageService, BrowserLocalStorage>();

builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>());
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();