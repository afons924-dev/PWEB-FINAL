using RestfulAPIWeb.Entities;

namespace RestfulAPIWeb.Repositories;

public interface ICategoriaRepository
{
    Task<IEnumerable<Categoria>> GetCategorias();
}
