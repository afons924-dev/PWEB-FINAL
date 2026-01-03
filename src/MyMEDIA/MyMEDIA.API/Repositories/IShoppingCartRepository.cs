using MyMEDIA.Shared.Entities;

namespace MyMEDIA.API.Repositories;

public interface IShoppingCartRepository
{
    Task<IEnumerable<ShoppingCartItem>> GetItemsAsync(string userId);
    Task<ShoppingCartItem?> GetItemAsync(int id);
    Task<ShoppingCartItem?> GetItemByProductAsync(string userId, int productId);
    Task<ShoppingCartItem> AddItemAsync(ShoppingCartItem item);
    Task<ShoppingCartItem?> UpdateItemAsync(ShoppingCartItem item);
    Task DeleteItemAsync(int id);
    Task ClearCartAsync(string userId);
}
