using MyMEDIA.Shared.Entities;

namespace MyMEDIA.API.Repositories;

public interface IFavoriteRepository
{
    Task<IEnumerable<Favorite>> GetFavoritesAsync(string userId);
    Task<Favorite> AddFavoriteAsync(Favorite favorite);
    Task DeleteFavoriteAsync(int id);
    Task<bool> IsFavoriteAsync(string userId, int productId);
}
