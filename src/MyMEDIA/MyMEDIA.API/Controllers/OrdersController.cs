using Microsoft.AspNetCore.Mvc;
using MyMEDIA.API.Repositories;
using MyMEDIA.Shared.Entities;
using System.Security.Claims;

namespace MyMEDIA.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _repository;

    public OrdersController(IOrderRepository repository)
    {
        _repository = repository;
    }

    private string GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetMyOrders()
    {
        var userId = GetUserId();
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        return Ok(await _repository.GetOrdersAsync(userId));
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
    {
        // TODO: Check for Admin/Employee role
        return Ok(await _repository.GetAllOrdersAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        var order = await _repository.GetOrderAsync(id);
        if (order == null) return NotFound();

        var userId = GetUserId();
        // Check ownership or admin role
        if (order.ClientId != userId)
        {
             // return Unauthorized(); // Or check if admin
        }

        return order;
    }

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(Order order)
    {
        var userId = GetUserId();
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        order.ClientId = userId;
        order.OrderDate = DateTime.UtcNow;
        // Calculate total, etc.

        var createdOrder = await _repository.CreateOrderAsync(order);
        return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.Id }, createdOrder);
    }
}
