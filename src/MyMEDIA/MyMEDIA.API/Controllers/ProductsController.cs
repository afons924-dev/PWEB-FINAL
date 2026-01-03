using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMEDIA.Shared.Data;
using MyMEDIA.Shared.Entities;

namespace MyMEDIA.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return await _context.Products.Include(p => p.Category).Where(p => p.IsActive).ToListAsync();
    }

    [HttpGet("my")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Product>>> GetMyProducts()
    {
        // Ideally verify User.Identity.Name or ClaimTypes.NameIdentifier
        // For PoC, returning all for demo purposes or filter by a dummy ID if Claim is empty
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        // If no auth, return empty
        if(string.IsNullOrEmpty(userId)) return new List<Product>();

        return await _context.Products.Where(p => p.SupplierId == userId).ToListAsync();
    }

    [HttpGet("all")] // For management
    [Authorize]
    public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
    {
        return await _context.Products.Include(p => p.Category).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Product>> PostProduct(Product product)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            product.SupplierId = userId;
        }

        // Suppliers create pending products
        product.IsActive = false;

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetProduct", new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutProduct(int id, Product product)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }

        _context.Entry(product).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Products.Any(e => e.Id == id))
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
    [Authorize]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
