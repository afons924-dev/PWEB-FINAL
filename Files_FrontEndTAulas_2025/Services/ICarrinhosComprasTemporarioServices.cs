using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RCLAPI.DTO;

namespace RCLAPI.Services;

public interface ICarrinhosComprasTemporarioServices
{
    Task AdicionarProdutoCarrinho(IApiServices apiServices, IUserSessionService userSessionService, ItemCarrinhoDTO itemCarrinhoCompra);
    Task RemoveProdutoCarrinho(IApiServices apiServices, IUserSessionService userSessionService, ItemCarrinhoDTO itemCarrinhoCompra);
    Task RemoveProdutoCarrinhoPorId(IApiServices apiServices, IUserSessionService userSessionService, int idProduto);
    Task<IEnumerable<ItemCarrinhoDTO>> GetProdutosCarrinho();
    Task LimparProdutosCarrinho();
    Task RestoreCarrinhoDesatualizado();
    int getNumeroProdutosCarrinho();
    Task IncrementarQuantidade(IApiServices apiServices, IUserSessionService userSessionService, int produtoId);
    Task DecrementarQuantidade(IApiServices apiServices, IUserSessionService userSessionService, int produtoId);
    Task SincronizarCarrinhoComprasComAPI(IApiServices apiServices, IUserSessionService userSessionService, bool isUserLoggedIn);
}

