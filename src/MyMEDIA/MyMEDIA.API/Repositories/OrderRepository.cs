using Microsoft.EntityFrameworkCore;
using MyMEDIA.Shared.Data;
using MyMEDIA.Shared.Entities;

namespace MyMEDIA.API.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync(string userId)
    {
        // For client, get their orders.
        return await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .Where(o => o.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Order?> UpdateOrderAsync(Order order)
    {
        var existing = await _context.Orders.FindAsync(order.Id);
        if (existing == null) return null;

        _context.Entry(existing).CurrentValues.SetValues(order);
        await _context.SaveChangesAsync();
        return existing;
    }
}
