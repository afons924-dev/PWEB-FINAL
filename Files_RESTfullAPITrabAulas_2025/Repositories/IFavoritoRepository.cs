using RestfulAPIWeb.Entities;

namespace RestfulAPIWeb.Repositories;

public interface IFavoritoRepository
{
    Task AddFavoritoAsync(Favorito favorito);
    Task RemoveFavoritoAsync(string clienteId, int produtoId);
    Task<IEnumerable<Favorito>> GetFavoritosByClienteIdAsync(string clienteId);
}
