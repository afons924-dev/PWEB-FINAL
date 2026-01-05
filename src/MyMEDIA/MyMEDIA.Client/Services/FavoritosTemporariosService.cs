using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyMEDIA.Shared.DTO;

namespace MyMEDIA.Client.Services;

// SERVICO QUE MANTÉM OS PRODUTOS FAVORITOS DO UTILIZADOR EM MODO ANONIMO
// VAMOS INJETAR ITO COMO SINGLETON NO Program.cs
public class FavoritosTemporariosService : IFavoritosTemporariosServices
{
    private HashSet<ProdutoFavorito> _produtosFavoritos;
    private readonly ILocalStorageService _localStorageService;

    public FavoritosTemporariosService(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    private async Task<HashSet<ProdutoFavorito>> GetLocalProdutosFavoritos()
    {
        string? result = null;
        try
        {
            result = await _localStorageService.GetItemAsync<string>("favoritos");
        }
        catch (Exception)
        {

        }

        if (string.IsNullOrEmpty(result))
        {
            return new HashSet<ProdutoFavorito>();
        }

        return JsonSerializer.Deserialize<HashSet<ProdutoFavorito>>(result, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task AdicionarProdutoFavorito(ProdutoFavorito produtoFavorito)
    {
        _produtosFavoritos.Add(produtoFavorito);
        await GuardarFavoritos(_produtosFavoritos);
    }

    public async Task RemoveProdutoFavorito(ProdutoFavorito produtoFavorito)
    {
        _produtosFavoritos.Remove(produtoFavorito);
        await GuardarFavoritos(_produtosFavoritos);
    }

    public async Task RemoveProdutoFavoritoPorId(int idProduto)
    {
        var produtoFavorito = _produtosFavoritos.FirstOrDefault(p => p.ProdutoId == idProduto);

        if (produtoFavorito != null)
        {
            await RemoveProdutoFavorito(produtoFavorito);
        }
    }

    public async Task<IEnumerable<ProdutoFavorito>> GetProdutosFavoritos()
    {
        _produtosFavoritos = await GetLocalProdutosFavoritos();
        return _produtosFavoritos;
    }

    public async Task LimparProdutosFavoritos()
    {
        _produtosFavoritos.Clear();
        await _localStorageService.RemoveItemAsync("favoritos");
    }

    public int getNumeroProdutosFavoritos()
    {
        return _produtosFavoritos.Count;
    }


    // SINCRONIZAR FAVORITOS COM A API
    public async Task SincronizarFavoritosComAPI(IApiServices _apiServices, bool isUserLoggedIn)
    {
        if (!isUserLoggedIn)
        {
            return;
        }

        // favoritos locais
        var favoritosLocais = (await GetProdutosFavoritos()).ToList();

        // favoritos da API
        var respostaComFavoritos = await _apiServices.GetFavoritos();

        List<ProdutoFavorito> favoritosApi = new();

        if (respostaComFavoritos.Data != null)
        {
            favoritosApi = respostaComFavoritos.Data;
        }

        // Adicionar favoritos locais à API, se não existirem lá
        foreach (var favoritoLocal in favoritosLocais)
        {
            if (!favoritosApi.Any(f => f.ProdutoId == favoritoLocal.ProdutoId))
            {
                await _apiServices.AdicionarFavorito(favoritoLocal.ProdutoId);
            }
        }

        // ============ ATUALIZAR FAVORITOS LOCAIS ============ //
        await LimparProdutosFavoritos();

        // voltar a obter dados atualizados da API apos sincronizacao
        // Obter favoritos da API
        var favoritosAtualizadosResposta = await _apiServices!.GetFavoritos();

        if (favoritosAtualizadosResposta != null && favoritosAtualizadosResposta.Data != null)
        {
            // FAVORITOS OBTIDOS DA API
            var favoritosAPIAtualizados = favoritosAtualizadosResposta.Data;

            foreach (var favoritoAtualizado in favoritosAPIAtualizados)
            {
                await AdicionarProdutoFavorito(favoritoAtualizado);
            }
        }
    }

    // METODOS DE ADICAO E REMOCAO CHAMADOS PELO SLIDER COMPONENT
    public async Task<bool> AdicionarFavoritoServico(int produtoId, IApiServices apiServices, IUserSessionService _userSessionService)
    {
        bool isUserLoggedIn = await _userSessionService.IsUserLoggedIn();

        try
        {
            if (isUserLoggedIn)
            {
                var favoritosApi = await apiServices.GetFavoritos();

                if (favoritosApi.Data == null || !favoritosApi.Data.Any(f => f.ProdutoId == produtoId))
                {
                    var resposta = await apiServices.AdicionarFavorito(produtoId);
                    if (!resposta.Data)
                    {
                        Console.WriteLine($"Erro ao adicionar favorito na API: {resposta.ErrorMessage}");
                        return false;
                    }
                }
            }

            // Adicionar ao serviço local
            if (!_produtosFavoritos.Any(f => f.ProdutoId == produtoId))
            {
            }
            await AdicionarProdutoFavorito(new ProdutoFavorito { ProdutoId = produtoId });

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao adicionar favorito: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RemoverFavoritoServico(int produtoId, IApiServices apiServices, IUserSessionService _userSessionService)
    {
        bool isUserLoggedIn = await _userSessionService.IsUserLoggedIn();

        try
        {
            if (isUserLoggedIn)
            {
                var favoritosApi = await apiServices.GetFavoritos();

                if (favoritosApi.Data != null && favoritosApi.Data.Any(f => f.ProdutoId == produtoId))
                {
                    var resposta = await apiServices.RemoverFavorito(produtoId);
                    if (!resposta.Data)
                    {
                        Console.WriteLine($"Erro ao remover favorito na API: {resposta.ErrorMessage}");
                        return false;
                    }
                }
            }

            // Remover do serviço local
            await RemoveProdutoFavoritoPorId(produtoId);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao remover favorito: {ex.Message}");
            return false;
        }
    }

    private async Task GuardarFavoritos(HashSet<ProdutoFavorito> favoritos)
    {
        var json = JsonSerializer.Serialize(favoritos);
        await _localStorageService.SetItemAsync("favoritos", json);
    }

}
