using Microsoft.AspNetCore.Mvc;

using RestfulAPIWeb.Entities;
using RestfulAPIWeb.Repositories;

namespace RestfulAPIWeb.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaRepository categoriaRepository;

    public CategoriasController(ICategoriaRepository categoriaRepository)
    {
        this.categoriaRepository = categoriaRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        IEnumerable<Categoria> categorias;

        categorias = await categoriaRepository.GetCategorias();
        return Ok(categorias);
    }
}
