using Microsoft.AspNetCore.Mvc;
using ProductCatalogService.Application.Contracts;
using ProductCatalogService.Application.Dtos;

namespace ProductCatalogService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoService _service;

        public ProdutoController(IProdutoService service)
        {
            _service = service;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DetailsProdutoDto>> PostProduto([FromBody] CreateProdutoDto produtoDto)
        {
            if (produtoDto == null) return BadRequest("Produto invalida");

            DetailsProdutoDto produtoResult = await _service.RegisterNewProdutoAsync(produtoDto);

            return new CreatedAtActionResult(nameof(GetProdutoById), "produtos", new { id = produtoResult.Id }, produtoResult);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<DetailsProdutoDto>> GetProdutoById([FromRoute] int id)
        {
            if (id <= 0) return BadRequest();

            DetailsProdutoDto? produtoDto = await _service.GetProdutoByIdAsync(id);

            if (produtoDto == null) return NotFound();

            return Ok(produtoDto);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<DetailsProdutoDto>>> GetAllProdutos()
        {
            List<DetailsProdutoDto> produtos = await _service.GetAllProdutosAtivosAsync();

            return Ok(produtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<DetailsProdutoDto>> PutProduto([FromBody] UpdateProdutoDto produtoDto, [FromRoute] int id)
        {
            if (id <= 0 || produtoDto == null) return BadRequest();

            DetailsProdutoDto? produtoAtualizada = await _service.UpdateProdutoAsync(produtoDto, id);

            if (produtoAtualizada == null) return NotFound();

            return Ok(produtoAtualizada);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ReadProdutoDto>> RemoveProduto([FromRoute] int id)
        {
            if (id <= 0) return BadRequest();

            ReadProdutoDto? produtoDesativada = await _service.DisableProdutoByIdAsync(id);

            if (produtoDesativada == null) return NotFound();

            return Ok(produtoDesativada);
        }
    }
}
