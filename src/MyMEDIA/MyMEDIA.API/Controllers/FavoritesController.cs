using Microsoft.AspNetCore.Mvc;
using MyMEDIA.API.Repositories;
using MyMEDIA.Shared.Entities;
using System.Security.Claims;

namespace MyMEDIA.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FavoritesController : ControllerBase
{
    private readonly IFavoriteRepository _repository;

    public FavoritesController(IFavoriteRepository repository)
    {
        _repository = repository;
    }

    private string GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Favorite>>> GetFavorites()
    {
        var userId = GetUserId();
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        return Ok(await _repository.GetFavoritesAsync(userId));
    }

    [HttpPost]
    public async Task<ActionResult<Favorite>> AddFavorite(Favorite favorite)
    {
        var userId = GetUserId();
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        if (await _repository.IsFavoriteAsync(userId, favorite.ProductId))
        {
            return Conflict("Already a favorite");
        }

        favorite.ClientId = userId;
        var newFav = await _repository.AddFavoriteAsync(favorite);
        return CreatedAtAction(nameof(GetFavorites), new { id = newFav.Id }, newFav);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveFavorite(int id)
    {
        // Ideally verify ownership
        await _repository.DeleteFavoriteAsync(id);
        return NoContent();
    }
}
