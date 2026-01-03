using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using RestfulAPIWeb.Entities;
using RestfulAPIWeb.Repositories;
using RestfulAPIWeb.Data;

namespace RestfulAPIWeb.Controllers;

// Controller dos produtos favoritos

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Cliente", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class FavoritosController : ControllerBase
{
    private readonly IFavoritoRepository _favoritoRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public FavoritosController(IFavoritoRepository favoritoRepository, UserManager<ApplicationUser> userManager)
    {
        _favoritoRepository = favoritoRepository;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserFavoritos()
    {
        // Obtém o token referente ao user
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("O Utilizador não se encontra autenticado.");
        }

        // Obter o utilizador da base de dados
        var user = await _userManager.FindByIdAsync(userId);

        //// Regra de negócios: User tem de ter o estado "ativo" para poder gerir os favoritos
        //if (user.Estado != "Activo")
        //{
        //    return Forbid("O Cliente não se encontra Ativo.");
        //}

        IEnumerable<Favorito> favoritos = await _favoritoRepository.GetFavoritosByClienteIdAsync(userId);

        if (favoritos.Count() == 0)
        {
            return NotFound("O Cliente não tem produtos nos favoritos.");
        }

        return Ok(favoritos);
    }

    // Adicionar produto favorito
    [HttpPost("{produtoId}")]
    public async Task<IActionResult> AddToFavorites(int produtoId)
    {

        if (produtoId <= 0)
        {
            return BadRequest("O ID do produto é obrigatório.");
        }

        // Obtém o token referente ao user
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("O Utilizador não se encontra autenticado.");
        }

        // Obter os dados do utilizador da base de dados
        var user = await _userManager.FindByIdAsync(userId);

        //// Regra de negócios: User tem de ter o estado "ativo" para poder gerir os favoritos
        //if (user.Estado != "Activo")
        //{
        //    return Forbid("O Cliente não se encontra Ativo.");
        //}

        // Testar se é um produto favorito
        var favoritos = await _favoritoRepository.GetFavoritosByClienteIdAsync(userId);

        if (favoritos.Any(f => f.ProdutoId == produtoId && f.ClienteId == userId))
        {
            return BadRequest("O Produto já se encontra nos favoritos.");
        }

        // Adicionar o produto aos favoritos
        await _favoritoRepository.AddFavoritoAsync(new Favorito
        {
            ClienteId = userId,
            ProdutoId = produtoId
        });

        return Ok("O Produto foi adicionado aos favoritos com sucesso.");
    }

    // Remover um produto favorito
    [HttpDelete("{produtoId}")]
    public async Task<IActionResult> RemoveFromFavorites(int produtoId)
    {
        // Obtém o token referente ao user
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("O Utilizador não se encontra autenticado.");
        }

        // Obter os dados do utilizador da base de dados
        var user = await _userManager.FindByIdAsync(userId);

        //// Regra de negócios: User tem de ter o estado "ativo" para poder gerir os favoritos
        //if (user.Estado != "Activo")
        //{
        //    return Forbid("O Cliente não se encontra Ativo.");
        //}

        // Verificar se é um produto favorito deste user
        var favoritos = await _favoritoRepository.GetFavoritosByClienteIdAsync(userId);

        if(!favoritos.Any(f => f.ProdutoId == produtoId && f.ClienteId == userId))
        {
            return BadRequest("O Produto que pretende remover não se encontra nos favoritos.");
        }

        // Apagar a referência de produto favorito
        await _favoritoRepository.RemoveFavoritoAsync(userId, produtoId);
        return Ok("O Produto foi removido dos favoritos com sucesso.");
    }
}
