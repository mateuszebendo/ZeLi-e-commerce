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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DetailsCategoriaDto>> PostCategoria([FromBody] CreateCategoriaDto categoriaDto)
        {
            if (categoriaDto == null) return BadRequest("Categoria invalida");

            DetailsCategoriaDto categoriaResult = await _service.RegisterNewCategoriaAsync(categoriaDto);

            return new CreatedAtActionResult(nameof(GetCategoriaById), "categorias", new { id = categoriaResult.Id }, categoriaResult);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<DetailsCategoriaDto>> GetCategoriaById([FromRoute] int id)
        {
            if (id <= 0) return BadRequest();

            DetailsCategoriaDto? categoriaDto = await _service.GetCategoriaByIdAsync(id);

            if(categoriaDto == null) return NotFound();

            return Ok(categoriaDto);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<DetailsCategoriaDto>>> GetAllCategorias()
        {
            List<DetailsCategoriaDto> categorias = await _service.GetAllCategoriasAtivasAsync();

            return Ok(categorias);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<DetailsCategoriaDto>> PutCategoria([FromBody] UpdateCategoriaDto categoriaDto, [FromRoute] int id)
        {
            if(id <= 0 || categoriaDto == null) return BadRequest();

            DetailsCategoriaDto? categoriaAtualizada = await _service.UpdateCategoriaAsync(categoriaDto, id);

            if(categoriaAtualizada == null) return NotFound();

            return Ok(categoriaAtualizada);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ReadCategoriaDto>> RemoveCategoria([FromRoute] int id)
        {
            if(id <= 0) return BadRequest();

            ReadCategoriaDto? categoriaDesativada = await _service.DisableCategoriaByIdAsync(id);

            if(categoriaDesativada == null) return NotFound();

            return Ok(categoriaDesativada);
        }
    }
}
