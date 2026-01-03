using MyMEDIA.Shared.Entities;

namespace MyMEDIA.API.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<IEnumerable<Product>> GetMyProductsAsync(string userId);
    Task<IEnumerable<Product>> GetAllProductsAsync(); // Management
    Task<Product?> GetProductAsync(int id);
    Task<Product> AddProductAsync(Product product);
    Task<Product?> UpdateProductAsync(Product product);
    Task DeleteProductAsync(int id);
    Task<bool> ProductExistsAsync(int id);
}
