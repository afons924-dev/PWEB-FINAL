using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMEDIA.Shared.Data;
using MyMEDIA.Shared.Entities;

namespace MyMEDIA.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CategoriesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        // Return only root categories, including subcategories
        return await _context.Categories
            .Where(c => c.ParentId == null)
            .Include(c => c.SubCategories)
            .ToListAsync();
    }

    [HttpGet("flat")]
    public async Task<ActionResult<IEnumerable<Category>>> GetAllCategoriesFlat()
    {
        return await _context.Categories.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var category = await _context.Categories
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
        {
            return NotFound();
        }

        return category;
    }

    [HttpPost]
    // [Authorize(Roles = "Admin,Employee")] // TODO: Enable Auth
    public async Task<ActionResult<Category>> PostCategory(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCategory", new { id = category.Id }, category);
    }

    [HttpPut("{id}")]
    // [Authorize(Roles = "Admin,Employee")] // TODO: Enable Auth
    public async Task<IActionResult> PutCategory(int id, Category category)
    {
        if (id != category.Id)
        {
            return BadRequest();
        }

        _context.Entry(category).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Categories.Any(e => e.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    // [Authorize(Roles = "Admin,Employee")] // TODO: Enable Auth
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
