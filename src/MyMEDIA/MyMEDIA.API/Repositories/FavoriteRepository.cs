using Microsoft.EntityFrameworkCore;
using MyMEDIA.Shared.Data;
using MyMEDIA.Shared.Entities;

namespace MyMEDIA.API.Repositories;

public class FavoriteRepository : IFavoriteRepository
{
    private readonly ApplicationDbContext _context;

    public FavoriteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Favorite>> GetFavoritesAsync(string userId)
    {
        return await _context.Favorites
            .Include(f => f.Product)
            .Where(f => f.ClientId == userId)
            .ToListAsync();
    }

    public async Task<Favorite> AddFavoriteAsync(Favorite favorite)
    {
        _context.Favorites.Add(favorite);
        await _context.SaveChangesAsync();
        return favorite;
    }

    public async Task DeleteFavoriteAsync(int id)
    {
        var fav = await _context.Favorites.FindAsync(id);
        if (fav != null)
        {
            _context.Favorites.Remove(fav);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> IsFavoriteAsync(string userId, int productId)
    {
        return await _context.Favorites.AnyAsync(f => f.ClientId == userId && f.ProductId == productId);
    }
}
