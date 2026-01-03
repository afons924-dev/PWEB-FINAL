using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using RestfulAPIWeb.Repositories;
using RestfulAPIWeb.Entities;
using RestfulAPIWeb.Data;
using RestfulAPIWeb.DTO;

namespace RestfulAPIWeb.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Cliente", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class EncomendasController : ControllerBase
{
    private readonly IEncomendaRepository _encomendaRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    // Controller das encomendas
    public EncomendasController(IEncomendaRepository encomendaRepository, IProdutoRepository produtoRepository, UserManager<ApplicationUser> userManager)
    {
        _encomendaRepository = encomendaRepository;
        _produtoRepository = produtoRepository;
        _userManager = userManager;
    }

    // Obter as encomendas deste user
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        // Obtém o token referente ao user
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("O Utilizador não se encontra autenticado.");
        }

        // Obter o utilizador da base de dados
        var user = await _userManager.FindByIdAsync(userId);

        //// Regra de negócios: User tem de ter o estado "ativo" para poder gerir o carrinho
        //if (user.Estado != "Activo")
        //{
        //    return Forbid("O CLiente não se encontra Ativo.");
        //}

        var encomendas = await _encomendaRepository.GetEncomendasByUserIdAsync(userId);
        return Ok(encomendas);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEncomendaWithItems([FromBody] EncomendaDTO encomenda)
    {
        if (encomenda == null || !encomenda.Itens.Any())
            return BadRequest("A lista de itens não pode estar vazia.");

        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("O Utilizador não se encontra autenticado.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user.Estado != "Activo")
            {
                return Forbid("O Cliente não se encontra ativo.");
            }

            // Validação da existência de produto em stock
            foreach (var dto in encomenda.Itens)
            {
                var produto = await _produtoRepository.ObterDetalheProdutoAsync(dto.ProdutoId);

                if (produto == null)
                    return NotFound($"O produto com ID {dto.ProdutoId} não foi encontrado.");

                if (!produto.Disponivel)
                    return BadRequest($"O produto {produto.Nome} não está disponível para encomenda.");

                if (produto.EmStock < dto.Quantidade)
                    return BadRequest($"O produto {produto.Nome} não tem stock suficiente.");
            }

            var novaEncomenda = new Encomenda
            {
                Data = DateTime.UtcNow,
                Estado = EstadoEncomenda.Pendente,
                pagamentoConfirmadoInternamente = false,
                pagamentoEfetuado = false,
                ClienteId = userId,
                Rua = encomenda.Morada.Rua,
                Localidade1 = encomenda.Morada.Localidade1,
                CodigoPostal = encomenda.Morada.CodigoPostal,
                Cidade = encomenda.Morada.Cidade,
                Pais = encomenda.Morada.Pais
            };

            novaEncomenda = await _encomendaRepository.AddEncomendaAsync(novaEncomenda);

            var itens = new List<EncomendaItem>();

            foreach (var dto in encomenda.Itens)
            {
                var produto = await _produtoRepository.ObterDetalheProdutoAsync(dto.ProdutoId);

                itens.Add(new EncomendaItem
                {
                    EncomendaId = novaEncomenda.Id,
                    ProdutoId = produto.Id,
                    NomeProduto = produto.Nome,
                    Preco = produto.Preco ?? 0,
                    Quantidade = dto.Quantidade,
                    DetalheProduto = produto.Detalhe,
                    NomeCategoria = produto.categoria?.Nome ?? "",
                    DetalheModoDisponibilizacao = produto.modoentrega?.Detalhe ?? "",
                });

                // Actualizar o stock
                await _produtoRepository.DarBaixaDeStockAsync(dto.ProdutoId, dto.Quantidade);
            }

            await _encomendaRepository.AddEncomendaItensAsync(itens);

            return Created("", new
            {
                message = "Encomenda criada com sucesso!",
                encomendaId = novaEncomenda.Id
            });

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao criar encomenda: {ex.Message}");
            return StatusCode(500, "Erro interno ao criar encomenda.");
        }
    }

    // Cancelar encomenda
    [HttpPost("{idEncomenda}/cancelar")]
    public async Task<IActionResult> CancelarEncomenda(int idEncomenda)
    {
        try
        {
            // Obter a encomenda com os itens associados
            var encomenda = await _encomendaRepository.GetEncomendaByIdAsync(idEncomenda);

            if (encomenda == null)
            {
                return NotFound(new
                {
                    ErrorMessage = $"A encomenda com ID {idEncomenda} não foi encontrada."
                });
            }

            // Validar se a encomenda já foi cancelada ou enviada
            if (encomenda.Estado == EstadoEncomenda.Cancelada)
            {
                return BadRequest(new
                {
                    ErrorMessage = "A encomenda já está cancelada."
                });
            }

            if (encomenda.Estado == EstadoEncomenda.Enviada)
            {
                return BadRequest(new
                {
                    ErrorMessage = "A encomenda já foi enviada e não pode ser cancelada."
                });
            }

            // Reverter o stock dos produtos
            foreach (var item in encomenda.ProdutosEncomendados)
            {
                var sucesso = await _produtoRepository.AumentarStockAsync(item.ProdutoId, item.Quantidade);

                if (!sucesso)
                {
                    return BadRequest(new
                    {
                        ErrorMessage = $"Erro ao reverter o stock do produto com ID {item.ProdutoId}."
                    });
                }
            }

            // Atualizar o estado da encomenda
            encomenda.Estado = EstadoEncomenda.Cancelada;
            await _encomendaRepository.UpdateEncomendaAsync(encomenda);

            // Retornar sucesso
            return Ok(new
            {
                Message = "Encomenda cancelada e stock revertido com sucesso.",
                EncomendaId = encomenda.Id
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao cancelar encomenda: {ex.Message}");
            return StatusCode(500, new
            {
                ErrorMessage = "Erro interno ao cancelar encomenda."
            });
        }
    }

    // Efectuar o paganmento da encomenda
    [HttpPut("{idEncomenda}/pagar")]
    public async Task<IActionResult> PagarEncomenda(int idEncomenda)
    {
        try
        {
            //  Procurar a encomenda pelo ID
            var encomenda = await _encomendaRepository.GetEncomendaByIdAsync(idEncomenda);

            if (encomenda == null)
            {
                return NotFound($"Encomenda com ID {idEncomenda} não encontrada.");
            }

            // Verificar se a encomenda já foi paga
            if (encomenda.pagamentoEfetuado)
            {
                return BadRequest("A encomenda já foi paga.");
            }

            // Atualizar o estado do pagamento
            encomenda.pagamentoEfetuado = true;
            encomenda.pagamentoConfirmadoInternamente = false;

            // Atualizar a encomenda no repositório
            var sucesso = await _encomendaRepository.UpdateEncomendaAsync(encomenda);
            if (!sucesso)
            {
                return StatusCode(500, "Erro ao atualizar o estado do pagamento da encomenda.");
            }

            return Ok(new { mensagem = "Pagamento realizado com sucesso.", encomendaId = encomenda.Id });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao pagar encomenda: {ex.Message}");
            return StatusCode(500, "Erro interno ao processar o pagamento.");
        }
    }


}
