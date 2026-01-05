using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyMEDIA.Shared.DTO;

namespace MyMEDIA.Client.Services;

// SERVICO QUE MANTÉM OS ITENS DO CARRINHO DO UTILIZADOR EM MODO ANONIMO
// VAMOS INJETAR ITO COMO SINGLETON NO Program.cs
public class CarrinhoComprasTemporarioService : ICarrinhosComprasTemporarioServices
{

    private HashSet<ItemCarrinhoDTO> _produtosCarrinho;

    private readonly ILocalStorageService _localStorageService;

    public CarrinhoComprasTemporarioService(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    private async Task<HashSet<ItemCarrinhoDTO>> GetLocalProdutosCarrinho()
    {
        string? result = null;
        try
        {
            result = await _localStorageService.GetItemAsync<string>("carrinho");
        }
        catch (Exception)
        {

        }

        if (string.IsNullOrEmpty(result))
        {
            return new HashSet<ItemCarrinhoDTO>();
        }

        return JsonSerializer.Deserialize<HashSet<ItemCarrinhoDTO>>(result, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task AdicionarProdutoCarrinho(IApiServices apiServices, IUserSessionService userSessionService, ItemCarrinhoDTO itemCarrinhoCompra)
    {
        if (userSessionService == null || apiServices == null)
        {
            throw new ArgumentNullException("Os serviços de API e sessão de utilizador não podem ser nulos.");
        }

        if (!await userSessionService.IsUserLoggedIn())
        {
            await AtualizarCarrinhoLocal(itemCarrinhoCompra);
        }
        else
        {
            // Sincronizar com a API
            var respostaComCarrinho = await apiServices.GetCarrinhoCompras();
            List<ItemCarrinhoDTO> carrinhoApi;
            ItemCarrinhoDTO ?produtoExistente = null;

            if (respostaComCarrinho?.Data != null)
            {
                carrinhoApi = respostaComCarrinho.Data;
                produtoExistente = carrinhoApi.FirstOrDefault(p => p.ProdutoId == itemCarrinhoCompra.ProdutoId);
            }

            if (produtoExistente != null)
            {
                var novaQuantidade = produtoExistente.Quantidade + itemCarrinhoCompra.Quantidade;
                var resultado = await apiServices.AtualizarQuantidadeProdutoCarrinho(itemCarrinhoCompra.ProdutoId, novaQuantidade);

                if (!resultado.Data)
                {
                    throw new Exception($"Erro ao atualizar a quantidade do produto na API: {resultado.ErrorMessage}");
                }
            }
            else
            {
                var resultado = await apiServices.AdicionarProdutoCarrinho(itemCarrinhoCompra);

                if (!resultado.Data)
                {
                    throw new Exception($"Erro ao adicionar produto ao carrinho na API: {resultado.ErrorMessage}");
                }
            }

            await AtualizarCarrinhoLocal(itemCarrinhoCompra);
        }
    }

    public async Task RestoreCarrinhoDesatualizado()
    {
        await LimparProdutosCarrinho();
    }

    private async Task AtualizarCarrinhoLocal(ItemCarrinhoDTO itemCarrinhoCompra)
    {
        if(_produtosCarrinho == null)
        {
            _produtosCarrinho = await GetLocalProdutosCarrinho();
        }

        var produtoExistente = _produtosCarrinho.FirstOrDefault(p => p.ProdutoId == itemCarrinhoCompra.ProdutoId);

        if (produtoExistente != null)
        {
            produtoExistente.Quantidade += itemCarrinhoCompra.Quantidade;
        }
        else
        {
            _produtosCarrinho.Add(itemCarrinhoCompra);
        }

        await GuardarCarrinho(_produtosCarrinho);
    }

    public async Task RemoveProdutoCarrinho(IApiServices apiServices, IUserSessionService userSessionService, ItemCarrinhoDTO itemCarrinhoCompra)
    {
        if (userSessionService == null || apiServices == null)
        {
            throw new ArgumentNullException("Os serviços de API e sessão de utilizador não podem ser nulos.");
        }

        if (!_produtosCarrinho.Contains(itemCarrinhoCompra))
        {
            throw new ArgumentException("O item não existe no carrinho local.");
        }

        if (await userSessionService.IsUserLoggedIn())
        {

            var resultado = await apiServices.RemoverProdutoCarrinho(itemCarrinhoCompra.ProdutoId);

            if (!resultado.Data)
            {
                throw new Exception($"Erro ao remover o produto da API: {resultado.ErrorMessage}");
            }
        }

        _produtosCarrinho.Remove(itemCarrinhoCompra);
        await GuardarCarrinho(_produtosCarrinho);
    }

    public async Task RemoveProdutoCarrinhoPorId(IApiServices apiServices, IUserSessionService userSessionService, int idProduto)
    {
        if (userSessionService == null || apiServices == null)
        {
            throw new ArgumentNullException("Os serviços de API e sessão de utilizador não podem ser nulos.");
        }

        var produto = _produtosCarrinho.FirstOrDefault(p => p.ProdutoId == idProduto);

        if (produto == null)
        {
            throw new ArgumentException("O item com o ID especificado não existe no carrinho local.");
        }

        if (await userSessionService.IsUserLoggedIn())
        {
            // Sincronizar com a API
            var resultado = await apiServices.RemoverProdutoCarrinho(idProduto);

            if (!resultado.Data)
            {
                throw new Exception($"Erro ao remover o produto da API: {resultado.ErrorMessage}");
            }
        }

        // Remover do carrinho local
        _produtosCarrinho.Remove(produto);
        await GuardarCarrinho(_produtosCarrinho);
    }

    public async Task<IEnumerable<ItemCarrinhoDTO>> GetProdutosCarrinho()
    {
        _produtosCarrinho = await GetLocalProdutosCarrinho();
        return _produtosCarrinho;
    }

    public async Task LimparProdutosCarrinho()
    {
        _produtosCarrinho.Clear();
        await _localStorageService.RemoveItemAsync("carrinho");
    }

    public int getNumeroProdutosCarrinho()
    {
        return _produtosCarrinho.Count;
    }

    public async Task IncrementarQuantidade(IApiServices apiServices, IUserSessionService userSessionService, int produtoId)
    {
        if (userSessionService == null || apiServices == null)
        {
            throw new ArgumentNullException("Os serviços de API e sessão de utilizador não podem ser nulos.");
        }

        var produto = _produtosCarrinho.FirstOrDefault(p => p.ProdutoId == produtoId);

        if (produto != null)
        {
            produto.Quantidade++;

            if (await userSessionService.IsUserLoggedIn())
            {
                // Sincronizar com a API
                var resultado = await apiServices.AtualizarQuantidadeProdutoCarrinho(produtoId, produto.Quantidade);

                if (!resultado.Data)
                {
                    throw new Exception($"Erro ao atualizar a quantidade do produto na API: {resultado.ErrorMessage}");
                }
            }
        }
        else
        {
            throw new Exception("Produto não encontrado no carrinho.");
        }

        await GuardarCarrinho(_produtosCarrinho);
    }

    public async Task DecrementarQuantidade(IApiServices apiServices, IUserSessionService userSessionService, int produtoId)
    {
        if (userSessionService == null || apiServices == null)
        {
            throw new ArgumentNullException("Os serviços de API e sessão de utilizador não podem ser nulos.");
        }

        var produto = _produtosCarrinho.FirstOrDefault(p => p.ProdutoId == produtoId);

        foreach (var item in _produtosCarrinho)
        {
            Console.WriteLine(item.ProdutoId);
        }

        if (produto != null)
        {
            if (produto.Quantidade > 1)
            {
                produto.Quantidade--;

                if (await userSessionService.IsUserLoggedIn())
                {
                    // Sincronizar com a API
                    var resultado = await apiServices.AtualizarQuantidadeProdutoCarrinho(produtoId, produto.Quantidade);

                    if (!resultado.Data)
                    {
                        throw new Exception($"Erro ao atualizar a quantidade do produto na API: {resultado.ErrorMessage}");
                    }
                }
            }
            else
            {
                // Remover do carrinho se a quantidade atingir 0
                if (await userSessionService.IsUserLoggedIn())
                {
                    var resultado = await apiServices.RemoverProdutoCarrinho(produtoId);

                    if (!resultado.Data)
                    {
                        throw new Exception($"Erro ao remover o produto da API: {resultado.ErrorMessage}");
                    }
                }

                _produtosCarrinho.Remove(produto);
            }
        }
        else
        {
            throw new Exception("Produto não encontrado no carrinho.");
        }

        await GuardarCarrinho(_produtosCarrinho);
    }

    public async Task SincronizarCarrinhoComprasComAPI(IApiServices _apiServices, IUserSessionService _userSessionService, bool isUserLoggedIn)
    {

        if (!isUserLoggedIn)
        {
            return;
        }

        // carrinho local
        var carrinhoLocal = (await GetProdutosCarrinho()).ToList();

        // carrinho da API
        var respostaComCarrinho = await _apiServices.GetCarrinhoCompras();

        if (respostaComCarrinho?.Data == null)
        {
            return;
        }

        var carrinhoApi = respostaComCarrinho.Data;

        // Adicionar carrinho local à API, se não existirem lá ou quantidade for diferente
        foreach (var itemCarrinho in carrinhoLocal)
        {
            if (!carrinhoApi.Any(p => p.ProdutoId == itemCarrinho.ProdutoId))
            {
                await _apiServices.AdicionarProdutoCarrinho(itemCarrinho);
            }
            else
            {
                if (carrinhoApi.First(p => p.ProdutoId == itemCarrinho.ProdutoId).Quantidade != itemCarrinho.Quantidade)
                {
                    await _apiServices.AtualizarQuantidadeProdutoCarrinho(itemCarrinho.ProdutoId, itemCarrinho.Quantidade);
                }
            }
        }

        // ============ ATUALIZAR CARRINHO LOCAL ============ //
        await LimparProdutosCarrinho();

        // voltar a obter dados atualizados da API apos sincronizacao
        // Obter carrinho da API
        var carrinhoAtualizadosResposta = await _apiServices.GetCarrinhoCompras();

        if (carrinhoAtualizadosResposta != null && carrinhoAtualizadosResposta.Data != null)
        {
            // CARRINHO OBTIDO DA API
            var carrinhoAtualizados = carrinhoAtualizadosResposta.Data;

            foreach (var item in carrinhoAtualizados)
            {
                _produtosCarrinho.Add(new ItemCarrinhoDTO
                {
                    ProdutoId = item.ProdutoId,
                    ClienteId = item.ClienteId,
                    Quantidade = item.Quantidade
                });
            }
        }

        await GuardarCarrinho(_produtosCarrinho);
    }

    private async Task GuardarCarrinho(HashSet<ItemCarrinhoDTO> carrinho)
    {
        var json = JsonSerializer.Serialize(carrinho);
        await _localStorageService.SetItemAsync("carrinho", json);
    }

}
