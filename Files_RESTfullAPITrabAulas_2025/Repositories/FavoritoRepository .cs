using Microsoft.EntityFrameworkCore;

using RestfulAPIWeb.Data;
using RestfulAPIWeb.Entities;

namespace RestfulAPIWeb.Repositories;

public class FavoritoRepository : IFavoritoRepository
{
    private readonly ApplicationDbContext _context;

    public FavoritoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddFavoritoAsync(Favorito favorito)
    {
        await _context.Favoritos.AddAsync(favorito);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveFavoritoAsync(string clienteId, int produtoId)
    {
        var favorito = await _context.Favoritos
            .FirstOrDefaultAsync(f => f.ClienteId == clienteId && f.ProdutoId == produtoId);

        if (favorito != null)
        {
            _context.Favoritos.Remove(favorito);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Favorito>> GetFavoritosByClienteIdAsync(string clienteId)
    {
        return await _context.Favoritos
            .Where(f => f.ClienteId == clienteId)
            .ToListAsync();
    }
}
