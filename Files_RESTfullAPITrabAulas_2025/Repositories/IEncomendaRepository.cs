using RestfulAPIWeb.Entities;

namespace RestfulAPIWeb.Repositories;

public interface IEncomendaRepository
{
    Task<Encomenda> AddEncomendaAsync(Encomenda encomenda);
    Task AddEncomendaItensAsync(IEnumerable<EncomendaItem> itens);
    Task<IEnumerable<Encomenda>> GetEncomendasByUserIdAsync(string userId);
    Task<Encomenda?> GetEncomendaByIdAsync(int encomendaId);
    Task<bool> UpdateEncomendaAsync(Encomenda encomenda);
}
