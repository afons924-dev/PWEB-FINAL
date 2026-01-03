using MyMEDIA.Shared.DTO;

namespace MyMEDIA.Client.Services.Interfaces
{
    public interface IProdutosRestServices
    {
        Task<List<ProdutoDTO>> GetAllProdutos();

        public Task<List<Categoria>> GetAllCategorias();
    }
}
