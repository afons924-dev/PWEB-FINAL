using RCLAPI.DTO;
using SQLite;

namespace RCLAPI.Services;
public class LocalServices
{
    private readonly SQLiteAsyncConnection _database;
    public LocalServices()
    {
        var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "local.db");
        _database= new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<ProdutoFavorito>().Wait();
    }

    public async Task<ProdutoFavorito> ReadAsync(int id)
    {
        try
        {
            return await _database.Table<ProdutoFavorito>().Where(p => p.ProdutoId == id).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public async Task<List<ProdutoFavorito>> ReadAllAsync()
    {
        try
        {
            return await _database.Table<ProdutoFavorito>().ToListAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public  async Task CreateAsync(ProdutoFavorito produtoFavorito)
    {
        try
        {
            var insere = await _database.InsertAsync(produtoFavorito);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public async Task DeleteAsync(ProdutoFavorito produtoFavorito)
    {
        try
        {
            await _database.DeleteAsync(produtoFavorito);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
