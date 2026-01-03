using Microsoft.EntityFrameworkCore;

using RestfulAPIWeb.Data;
using RestfulAPIWeb.Entities;

namespace RestfulAPIWeb.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly ApplicationDbContext _context;

    public ProdutoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Produto>> ObterProdutosPorCategoriaAsync(int categoriaId)
    {
        // Regra de negócio: Não mostrar produtos sem imagem associada
        return await _context.Produtos
            .Where(p => p.CategoriaId == categoriaId)
                .Where(x => x.Imagem.Length > 0)
                .Include("modoentrega")
                .Include("categoria")
                .OrderBy(p => p.categoria.Ordem)
                .ThenBy(p => p.Nome)
                .ToListAsync();
    }

    public async Task<IEnumerable<Produto>> ObterProdutosPromocaoAsync()
    {
        // Regra de negócio: Não mostrar produtos sem imagem associada
        return await _context.Produtos
            .Where(p => p.Promocao == true)
                .Where(x => x.Imagem!.Length > 0) 
                .Include("modoentrega")
                .Include("categoria")
                .OrderBy(p => p.categoria.Ordem)
                .ThenBy(p => p.Nome)
                .ToListAsync();
    }

    public async Task<IEnumerable<Produto>> ObterProdutosMaisVendidosAsync()
    {
        // Regra de negócio: Não mostrar produtos sem imagem associada
        return await _context.Produtos
            .Where(p => p.MaisVendido)
                .Where(x => x.Imagem.Length > 0)
                .Include("modoentrega")
                .Include("categoria")
                .OrderBy(p => p.categoria.Ordem)
                .ThenBy(p => p.Nome)
                .ToListAsync();
    }

    public async Task<Produto> ObterDetalheProdutoAsync(int id)
    {
        // Regra de negócio: Não mostrar produtos sem imagem associada
        var detalheProduto = await _context.Produtos
            .Where(x => x.Imagem.Length > 0)
                .Include("modoentrega")
                .Include("categoria")
                .FirstOrDefaultAsync(p => p.Id == id);

        if (detalheProduto is null)
        {
            throw new InvalidOperationException();
        }
        return detalheProduto;
    }

    public async Task<IEnumerable<Produto>> ObterTodosProdutosAsync()
    {
        // Regra de negócio: Não mostrar produtos sem imagem associada
        return await _context.Produtos
                .Where(x => x.Imagem.Length > 0) 
                .Include("modoentrega")
                .Include("categoria")
                .OrderBy(p => p.categoria.Ordem)
                .ThenBy(p => p.Nome)
                .ToListAsync();
    }

    public async Task<bool> DarBaixaDeStockAsync(int produtoId, decimal quantidade)
    {
        var produto = await _context.Produtos.FindAsync(produtoId);

        if (produto == null)
        {
            return false;
        }

        if (produto.EmStock < quantidade)
        {
            throw new InvalidOperationException($"O produto {produto.Nome} não possui stock suficiente.");
        }

        produto.EmStock -= quantidade;

        if(produto.EmStock == 0)
        {
            produto.Disponivel = false;
        }

        _context.Produtos.Update(produto);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> AumentarStockAsync(int produtoId, decimal quantidade)
    {
        var produto = await _context.Produtos.FindAsync(produtoId);

        if (produto == null)
        {
            return false;
        }

        produto.EmStock += quantidade;

        _context.Produtos.Update(produto);
        await _context.SaveChangesAsync();

        return true;
    }
}
