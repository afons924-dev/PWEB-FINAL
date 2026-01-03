using Microsoft.EntityFrameworkCore;

using RestfulAPIWeb.Data;
using RestfulAPIWeb.Entities;

namespace RestfulAPIWeb.Repositories;

public class EncomendaRepository : IEncomendaRepository
{
    private readonly ApplicationDbContext _context;

    public EncomendaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Encomenda>> GetEncomendasByUserIdAsync(string userId)
    {
        return await _context.Encomendas
            .Include(e => e.ProdutosEncomendados)
            .Where(e => e.ClienteId == userId)
            .ToListAsync();
    }

    public async Task<Encomenda?> GetEncomendaByIdAsync(int id)
    {
        return await _context.Encomendas
            .Where(e => e.Id == id)
            .Include(e => e.ProdutosEncomendados)
            .FirstOrDefaultAsync();
    }

    public async Task<Encomenda> AddEncomendaAsync(Encomenda encomenda)
    {
        _context.Encomendas.Add(encomenda); 
        await _context.SaveChangesAsync();
        return encomenda;
    }

    public async Task AddEncomendaItensAsync(IEnumerable<EncomendaItem> itens)
    {
        _context.EncomendaItems.AddRange(itens);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateEncomendaAsync(Encomenda encomenda)
    {
        try
        {
            _context.Encomendas.Update(encomenda);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao atualizar a encomenda com ID {encomenda.Id}: {ex.Message}");
            return false;
        }
    }

}
