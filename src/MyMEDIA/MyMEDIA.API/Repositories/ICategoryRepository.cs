using MyMEDIA.Shared.Entities;

namespace MyMEDIA.API.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetCategoriesAsync();
    Task<Category?> GetCategoryAsync(int id);
    Task<Category> AddCategoryAsync(Category category);
    Task<Category?> UpdateCategoryAsync(Category category);
    Task DeleteCategoryAsync(int id);
}
