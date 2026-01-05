using System.Text.Json;

using MyMEDIA.Shared.DTO;

namespace MyMEDIA.Client.Services;
public class UserSessionService : IUserSessionService
{
    // Variáveis de sessão
    public event Action? OnLoginStateChanged;

    private readonly ILocalStorageService _localStorage;

    public UserSessionService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    // Guardar na local storage o token recebido do controller
    public async Task Login(Token token)
    {
        var json = JsonSerializer.Serialize(token);
        await _localStorage.SetItemAsync("userToken", json);
    }

    // Lê da local storage o token
    public async Task<Token?> GetToken()
    {
        string? resultado = null;

        try
        {
            resultado = await _localStorage.GetItemAsync<string>("userToken");
        }
        catch (Exception)
        {

        }

        if (string.IsNullOrEmpty(resultado))
        {
            return null;
        }

        // Deserealiza o token
        return JsonSerializer.Deserialize<Token>(resultado, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });
    }

    // Apagar o token da local storage
    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("userToken");
    }

    public async Task<bool> IsUserLoggedIn()
    {
        return await GetToken() != null;
    }
}
