using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMEDIA.Shared.DTO;

namespace MyMEDIA.Client.Services;

public interface IFavoritosTemporariosServices
{
    Task AdicionarProdutoFavorito(ProdutoFavorito produtoFavorito);
    Task RemoveProdutoFavorito(ProdutoFavorito produtoFavorito);
    Task RemoveProdutoFavoritoPorId(int id);
    Task<IEnumerable<ProdutoFavorito>> GetProdutosFavoritos();
    Task LimparProdutosFavoritos();
    int getNumeroProdutosFavoritos();
    Task SincronizarFavoritosComAPI(IApiServices _apiServices, bool isUserLoggedIn);
    Task<bool> RemoverFavoritoServico(int produtoId, IApiServices apiServices, IUserSessionService _userSessionService);
    Task<bool> AdicionarFavoritoServico(int produtoId, IApiServices apiServices, IUserSessionService _userSessionService);
}
