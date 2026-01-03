using Microsoft.AspNetCore.Mvc;
using MyMEDIA.API.Repositories;
using MyMEDIA.Shared.Entities;

namespace MyMEDIA.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _repository;

    public CategoriesController(ICategoryRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        return Ok(await _repository.GetCategoriesAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var category = await _repository.GetCategoryAsync(id);
        if (category == null) return NotFound();
        return category;
    }

    // Add Post, Put, Delete if needed (mostly for Backoffice, but API can support it)
}
