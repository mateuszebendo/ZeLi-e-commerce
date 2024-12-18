using Microsoft.AspNetCore.Mvc;
using ProductCatalogService.Application.Contracts;
using ProductCatalogService.Application.Dtos;

namespace ProductCatalogService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _service;

        public CategoriasController(ICategoriaService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<DetailsCategoriaDto>> PostCategoria([FromBody] CreateCategoriaDto categoriaDto)
        {
            DetailsCategoriaDto categoriaResult = await _service.RegisterNewCategoriaAsync(categoriaDto);

            return new CreatedAtActionResult(nameof(GetCategoriaById), "categorias", new { id = categoriaResult.Id }, categoriaResult);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DetailsCategoriaDto>> GetCategoriaById([FromRoute] int id)
        {
            DetailsCategoriaDto categoriaDto = await _service.GetCategoriaByIdAsync(id);

            return Ok(categoriaDto);
        }

        [HttpGet]
        public async Task<ActionResult<List<DetailsCategoriaDto>>> GetAllCategorias()
        {
            List<DetailsCategoriaDto> categorias = await _service.GetAllCategoriasAtivasAsync();

            return Ok(categorias);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DetailsCategoriaDto>> PutCategoria([FromBody] UpdateCategoriaDto categoriaDto, [FromRoute] int id)
        {
            var categoriaAtualizada = await _service.UpdateCategoriaAsync(categoriaDto, id);

            return Ok(categoriaAtualizada);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ReadCategoriaDto>> RemoveCategoria([FromRoute] int id)
        {
            var categoriaDesativada = await _service.DisableCategoriaByIdAsync(id);

            return Ok(categoriaDesativada);
        }
    }
}
