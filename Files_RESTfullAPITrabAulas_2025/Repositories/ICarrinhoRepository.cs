using RestfulAPIWeb.DTO;
using RestfulAPIWeb.Entities;

namespace RestfulAPIWeb.Repositories;

public interface ICarrinhoRepository
{
    Task<IEnumerable<CarrinhoCompras>> GetCarrinhoItemsByClienteIdAsync(string clienteId);
    Task AddItemToCarrinhoAsync(CarrinhoCompras item);
    Task UpdateItemQuantityInCarrinhoAsync(int itemId, double novaQuantidade);
    Task ClearCarrinhoItemsByClientIdAsync(string clienteId);
    Task<CarrinhoCompras> GetCarrinhoItemAsync(ItemCarrinhoDTO itemCarrinhoDTO);
    Task<CarrinhoCompras> GetCarrinhoItemByProdutoIdClienteId(int produtoId, string clienteId);
    Task<CarrinhoCompras> GetCarrinhoItemByIdAsync(int itemId);
    Task RemoveItemFromCarrinhoAsync(int itemId);
}
