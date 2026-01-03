using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMEDIA.Shared.Data;
using MyMEDIA.Shared.Entities;

namespace MyMEDIA.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public OrdersController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<Order>> PostOrder(Order order)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(string.IsNullOrEmpty(userId)) return Unauthorized();

        order.UserId = userId;
        order.OrderDate = DateTime.UtcNow;
        order.Status = "Pending";
        order.TotalAmount = 0;

        foreach(var item in order.Items)
        {
            var product = await _context.Products.FindAsync(item.ProductId);

            if(product == null) return BadRequest($"Product {item.ProductId} not found");
            if(product.StockQuantity < item.Quantity) return BadRequest($"Not enough stock for {product.Title}");

            item.UnitPrice = product.FinalPrice;
            order.TotalAmount += item.UnitPrice * item.Quantity;

            // Decrement Stock
            product.StockQuantity -= item.Quantity;
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetOrder", new { id = order.Id }, order);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        var order = await _context.Orders.Include(o => o.Items).ThenInclude(i => i.Product).FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return NotFound();

        // Security check: Only owner or Admin/Employee can see
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

        if(order.UserId != userId && userRole != "Admin" && userRole != "Employee")
            return Forbid();

        return order;
    }

    [HttpGet("my")]
    public async Task<ActionResult<IEnumerable<Order>>> GetMyOrders()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(string.IsNullOrEmpty(userId)) return Unauthorized();

        return await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    [HttpGet("sales")]
    [Authorize(Roles = "Supplier")]
    public async Task<ActionResult<IEnumerable<OrderItem>>> GetMySales()
    {
        var supplierId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(string.IsNullOrEmpty(supplierId)) return Unauthorized();

        // Get all order items where the product belongs to this supplier
        return await _context.OrderItems
            .Include(i => i.Order)
            .Include(i => i.Product)
            .Where(i => i.Product.SupplierId == supplierId)
            .OrderByDescending(i => i.Order.OrderDate)
            .ToListAsync();
    }

    [HttpGet("all")] // For Management
    [Authorize(Roles = "Admin,Employee")]
    public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
    {
        return await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
    {
        var order = await _context.Orders.FindAsync(id);
        if(order == null) return NotFound();

        order.Status = status;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
