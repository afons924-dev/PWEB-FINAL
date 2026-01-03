using RCLAPI.DTO;

namespace RCLProdutos.Services.Interfaces
{
    public interface IProdutosRestServices
    {
        Task<List<ProdutoDTO>> GetAllProdutos();

        public Task<List<Categoria>> GetAllCategorias();
    }
}
