using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using RestfulAPIWeb.Entities;
using RestfulAPIWeb.Repositories;
using RestfulAPIWeb.Data;
using RestfulAPIWeb.DTO;

namespace RestfulAPIWeb.Controllers;

// Controller dos carrinhos de compras

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Cliente", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CarrinhosController : ControllerBase
{
    private readonly ICarrinhoRepository _carrinhoRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    // Métodos dos carrinhos
    public CarrinhosController(ICarrinhoRepository carrinhoRepository, UserManager<ApplicationUser> userManager)
    {
        _carrinhoRepository = carrinhoRepository;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserCarrinhoItems()
    {
        // Obtém o token referente ao user
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if(string.IsNullOrEmpty(userId))
        {
            return Unauthorized("O Utilizador não se encontra autenticado.");
        }

        // Get User data
        var user = await _userManager.FindByIdAsync(userId);

        // Só se o user estiver no estado activo é que pode efectur compras e etc
        //if (user.Estado != "Activo")
        //{
        //    return Forbid("O CLiente não se encontra Ativo.");
        //}

        // Itens do carrinho
        var carrinhoItems = await _carrinhoRepository.GetCarrinhoItemsByClienteIdAsync(userId);

        if (carrinhoItems.Count() > 0)
        {
            return Ok(carrinhoItems);
        }
        else
        {
            return NoContent();
        }
    }

    // Adicionar itens ao carrinho
    [HttpPost]
    public async Task<IActionResult> AddItemToCarrinho([FromBody] ItemCarrinhoDTO itemCarrinhoDTO)
    {

        // Obter o token referente a este user
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("O Utilizador não se encontra autenticado.");
        }

        // Obter o utilizador da base de dados
        // 
        var user = await _userManager.FindByIdAsync(userId);

        // Regra de negócios: User tem de ter o estado "ativo" para poder gerir o carrinho
        //if (user.Estado != "Activo")
        //{
        //    return Forbid("O CLiente não se encontra Ativo.");
        //}

        // Add itens ao carrinho
        try
        {
            await _carrinhoRepository.AddItemToCarrinhoAsync(new CarrinhoCompras{
                ClienteId = userId,
                ProdutoId = itemCarrinhoDTO.ProdutoId,
                Quantidade = itemCarrinhoDTO.Quantidade
            });

            return Ok("O item foi adicionado ao carrinho com sucesso.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao adicionar item ao carrinho: {ex.Message}");
            return StatusCode(500, "Erro ao processar o pedido.");
        }
    }

    // Actualizar a Qtd de um item no carrinho
    [HttpPut("{produtoId}")]
    public async Task<IActionResult> UpdateItemQuantityInCarrinho(int produtoId, double novaQuantidade)
    {
        // Obter o token referente a este user
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("O Utilizador não se encontra autenticado.");
        }

        if(novaQuantidade <= 0)
        {
            return Forbid("A Quantidade tem de ser positiva");
        }

        // Obter os dados do utilizador da base de dados      
        var user = await _userManager.FindByIdAsync(userId);

        // Regra de negócio: O Utilizador tem de estar ativo para poder gerir o carrinho
        //if (user.Estado != "Activo")
        //{
        //    return Forbid("O CLiente não se encontra Ativo.");
        //}

        // Verificar se o item já existe no carrinho
        var carrinhoItem = await _carrinhoRepository.GetCarrinhoItemByProdutoIdClienteId(produtoId, userId);

        if (carrinhoItem == null || carrinhoItem.ClienteId != userId)
        {
            return NotFound("O item não foi encontrado no carrinho do cliente.");
        }

        // Actualizar a Qtd deste itemk
        await _carrinhoRepository.UpdateItemQuantityInCarrinhoAsync(carrinhoItem.Id, novaQuantidade);

        return Ok("A quantidade do item foi atualizada com sucesso.");
    }

    // Remover itens do carrinho
    [HttpDelete("{produtoId}")]
    public async Task<IActionResult> RemoveItemFromCarrinho(int produtoId)
    {
        // Obter o token referente a este user
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("O Utilizador não se encontra autenticado.");
        }

        // Obter os dados do utilizador da base de dados
        var user = await _userManager.FindByIdAsync(userId);

        // Regra de negócio: O Utilizador tem de estar ativo para poder gerir o carrinho
        //if (user.Estado != "Activo")
        //{
        //    return Forbid("O CLiente não se encontra Ativo.");
        //}

        // Verificar se o item existe no carrinho
        var carrinhoItem = await _carrinhoRepository.GetCarrinhoItemByProdutoIdClienteId(produtoId, userId);

        if (carrinhoItem == null || carrinhoItem.ClienteId != userId)
        {
            return NotFound("O item não foi encontrado no carrinho do cliente.");
        }

        // Remover o item do carrinho
        await _carrinhoRepository.RemoveItemFromCarrinhoAsync(carrinhoItem.Id);

        return Ok("O item foi removido do carrinho com sucesso.");
    }

    // Apagar todos os itens do carrinho
    [HttpDelete]
    public async Task<IActionResult> ClearAllItemsFromCarrinho()
    {
        // Obter o token referente a este user
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("O Utilizador não se encontra autenticado.");
        }

        // Obter o utilizador da base de dados
        var user = await _userManager.FindByIdAsync(userId);

        // Regra de negócio: O Utilizador tem de estar ativo para poder gerir o carrinho
        //if (user.Estado != "Activo")
        //{
        //    return Forbid("O CLiente não se encontra Ativo.");
        //}

        // Esvaziar o carrinho
        await _carrinhoRepository.ClearCarrinhoItemsByClientIdAsync(userId);
        return Ok("O Carrinho foi esvaziado com sucesso.");
    }
}
