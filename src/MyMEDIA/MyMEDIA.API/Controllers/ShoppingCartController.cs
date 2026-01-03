using Microsoft.AspNetCore.Mvc;
using MyMEDIA.API.Repositories;
using MyMEDIA.Shared.Entities;
using System.Security.Claims;

namespace MyMEDIA.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShoppingCartController : ControllerBase
{
    private readonly IShoppingCartRepository _repository;

    public ShoppingCartController(IShoppingCartRepository repository)
    {
        _repository = repository;
    }

    private string GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShoppingCartItem>>> GetItems()
    {
        var userId = GetUserId();
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var items = await _repository.GetItemsAsync(userId);
        return Ok(items);
    }

    [HttpPost]
    public async Task<ActionResult<ShoppingCartItem>> AddItem(ShoppingCartItem item)
    {
        var userId = GetUserId();
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        item.ClientId = userId;

        var existingItem = await _repository.GetItemByProductAsync(userId, item.ProductId);
        if (existingItem != null)
        {
            existingItem.Quantity += item.Quantity;
            await _repository.UpdateItemAsync(existingItem);
            return Ok(existingItem);
        }

        var newItem = await _repository.AddItemAsync(item);
        return CreatedAtAction(nameof(GetItems), new { id = newItem.Id }, newItem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItem(int id, ShoppingCartItem item)
    {
        if (id != item.Id) return BadRequest();

        var userId = GetUserId();
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        // Ensure user owns the item
        var existing = await _repository.GetItemAsync(id);
        if (existing == null) return NotFound();
        if (existing.ClientId != userId) return Unauthorized();

        existing.Quantity = item.Quantity;
        await _repository.UpdateItemAsync(existing);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        var userId = GetUserId();
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var existing = await _repository.GetItemAsync(id);
        if (existing == null) return NotFound();
        if (existing.ClientId != userId) return Unauthorized();

        await _repository.DeleteItemAsync(id);
        return NoContent();
    }
}
