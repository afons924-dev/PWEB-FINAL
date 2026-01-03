using Microsoft.EntityFrameworkCore;

using RestfulAPIWeb.Data;
using RestfulAPIWeb.DTO;
using RestfulAPIWeb.Entities;

namespace RestfulAPIWeb.Repositories;

public class CarrinhoRepository : ICarrinhoRepository
{
    private readonly ApplicationDbContext _context;

    public CarrinhoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddItemToCarrinhoAsync(CarrinhoCompras item)
    {
        var existingItem = await _context.CarrinhosCompras
            .FirstOrDefaultAsync(c => c.ClienteId == item.ClienteId && c.ProdutoId == item.ProdutoId);

        // Caso o item já exista no carrinho, aumentar a Qtd
        if (existingItem != null)
        {
            existingItem.Quantidade += item.Quantidade;
        }
        else
        {
            await _context.CarrinhosCompras.AddAsync(item);
        }

        await _context.SaveChangesAsync();
    }

    public async Task ClearCarrinhoItemsByClientIdAsync(string clienteId)
    {
        var carrinhoItems = _context.CarrinhosCompras
            .Where(c => c.ClienteId == clienteId);

        if (carrinhoItems.Any())
        {
            _context.CarrinhosCompras.RemoveRange(carrinhoItems);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<CarrinhoCompras>> GetCarrinhoItemsByClienteIdAsync(string clienteId)
    {
        return await _context.CarrinhosCompras
            .Where(c => c.ClienteId == clienteId)
            .ToListAsync();
    }

    public async Task<CarrinhoCompras> GetCarrinhoItemAsync(ItemCarrinhoDTO itemCarrinhoDTO)
    {
        return await _context.CarrinhosCompras.FirstOrDefaultAsync(c => c.ProdutoId == itemCarrinhoDTO.ProdutoId && c.ClienteId == itemCarrinhoDTO.ClienteId);
    }

    public async Task<CarrinhoCompras> GetCarrinhoItemByProdutoIdClienteId(int produtoId, string clienteId)
    {
        return await _context.CarrinhosCompras.FirstOrDefaultAsync(c => c.ProdutoId == produtoId && c.ClienteId == clienteId);
    }

    public async Task<CarrinhoCompras> GetCarrinhoItemByIdAsync(int itemId)
    {
        return await _context.CarrinhosCompras.FirstOrDefaultAsync(c => c.Id == itemId);
    }

    public async Task UpdateItemQuantityInCarrinhoAsync(int itemId, double novaQuantidade)
    {
        // Verificar se o item existe jo carrinhop
        var item = await _context.CarrinhosCompras.FirstOrDefaultAsync(c => c.Id == itemId);

        if (item != null)
        {
            item.Quantidade = novaQuantidade;
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveItemFromCarrinhoAsync(int itemId)
    {
        var item = await _context.CarrinhosCompras.FirstOrDefaultAsync(c => c.Id == itemId);
        if (item != null)
        {
            _context.CarrinhosCompras.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
