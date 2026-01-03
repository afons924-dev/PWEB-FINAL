using RestfulAPIWeb.Data;
using Microsoft.EntityFrameworkCore;
using RestfulAPIWeb.Entities;

namespace RestfulAPIWeb.Repositories;
public class CategoriaRepository : ICategoriaRepository
{
    private readonly ApplicationDbContext dbContext;
    public CategoriaRepository(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public async Task<IEnumerable<Categoria>> GetCategorias()
    {
        var categorias = await dbContext.Categorias
            .Where(x => x.Imagem.Length > 0)
            .OrderBy(O => O.Ordem)
            .ThenBy(p => p.Nome)
            .ToListAsync();

        return categorias;
    }
}
