using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductCatalogService.Application.Contracts;
using ProductCatalogService.Application.Dtos;
using ProductCatalogService.Domain.Pagination;
using ProductCatalogService.Domain.ValueObjects;

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

        [HttpGet("filter/nome/pagination")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DetailsCategoriaDto>>> GetCategoriasFiltradas([FromQuery] CategoriasFiltroNome categoriasFiltro)
        {
            var tuplaRetornoService = await _service.GetCategoriasFiltroNomeAsync(categoriasFiltro);

            MetaData metaData = tuplaRetornoService.Item1;
            IEnumerable<DetailsCategoriaDto> categorias = tuplaRetornoService.Item2;

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metaData));

            return Ok(categorias);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DetailsCategoriaDto>>> GetAllCategoriasPaged([FromQuery] CategoriaParameters categoriaParameters)
        {
            var tuplaRetornoService = await _service.GetAllCategoriasAtivasPagedAsync(categoriaParameters);

            MetaData metaData = tuplaRetornoService.Item1;
            IEnumerable<DetailsCategoriaDto> categorias = tuplaRetornoService.Item2;

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metaData));

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
