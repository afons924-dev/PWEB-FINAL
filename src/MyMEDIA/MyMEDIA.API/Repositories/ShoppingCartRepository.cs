using Microsoft.EntityFrameworkCore;
using MyMEDIA.Shared.Data;
using MyMEDIA.Shared.Entities;

namespace MyMEDIA.API.Repositories;

public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly ApplicationDbContext _context;

    public ShoppingCartRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ShoppingCartItem>> GetItemsAsync(string userId)
    {
        return await _context.ShoppingCartItems
            .Include(i => i.Product)
            .Where(i => i.ClientId == userId)
            .ToListAsync();
    }

    public async Task<ShoppingCartItem?> GetItemAsync(int id)
    {
        return await _context.ShoppingCartItems
            .Include(i => i.Product)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<ShoppingCartItem?> GetItemByProductAsync(string userId, int productId)
    {
        return await _context.ShoppingCartItems
            .FirstOrDefaultAsync(i => i.ClientId == userId && i.ProductId == productId);
    }

    public async Task<ShoppingCartItem> AddItemAsync(ShoppingCartItem item)
    {
        _context.ShoppingCartItems.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<ShoppingCartItem?> UpdateItemAsync(ShoppingCartItem item)
    {
        var existingItem = await _context.ShoppingCartItems.FindAsync(item.Id);
        if (existingItem == null) return null;

        _context.Entry(existingItem).CurrentValues.SetValues(item);
        await _context.SaveChangesAsync();
        return existingItem;
    }

    public async Task DeleteItemAsync(int id)
    {
        var item = await _context.ShoppingCartItems.FindAsync(id);
        if (item != null)
        {
            _context.ShoppingCartItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ClearCartAsync(string userId)
    {
        var items = await _context.ShoppingCartItems.Where(i => i.ClientId == userId).ToListAsync();
        _context.ShoppingCartItems.RemoveRange(items);
        await _context.SaveChangesAsync();
    }
}
