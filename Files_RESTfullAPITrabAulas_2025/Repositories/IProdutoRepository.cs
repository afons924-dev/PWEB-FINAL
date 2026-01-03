using RestfulAPIWeb.Entities;

namespace RestfulAPIWeb.Repositories;

public interface IProdutoRepository
{
    Task<IEnumerable<Produto>> ObterTodosProdutosAsync();
    Task<IEnumerable<Produto>> ObterProdutosPorCategoriaAsync(int categoriaId);
    Task<IEnumerable<Produto>> ObterProdutosMaisVendidosAsync();
    Task<IEnumerable<Produto>> ObterProdutosPromocaoAsync();
    Task<Produto> ObterDetalheProdutoAsync(int id);
    Task<bool> DarBaixaDeStockAsync(int produtoId, decimal quantidade);
    Task<bool> AumentarStockAsync(int produtoId, decimal quantidade);
}
