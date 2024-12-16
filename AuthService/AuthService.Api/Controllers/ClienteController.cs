using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ClienteController : ControllerBase
{
    private readonly IClienteService _clienteService;
    private readonly IMapper _mapper;

    public ClienteController(IClienteService clienteService, IMapper mapper)
    {
        _clienteService = clienteService;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ClienteDto>> RegistrarCliente([FromBody] ClienteCreateDto clienteCreateDto)
    {
        if (clienteCreateDto is null)
        {
            return BadRequest("Dados inválidos");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var clienteCriado = await _clienteService.CreateClienteAsync(clienteCreateDto);
        return CreatedAtAction(nameof(GetClienteById), new { id = clienteCriado.ClienteId }, clienteCriado);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ClienteDto>>> GetAllClientes()
    {
        var clientes = await _clienteService.GetAllClientesAsync();

        if (clientes == null || !clientes.Any())
            return NotFound("Nenhum cliente encontrado.");

        return Ok(clientes);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetClienteById(int id)
    {
        var cliente = await _clienteService.GetClienteByIdAsync(id);
        if (cliente == null)
            return NotFound();

        return Ok(cliente);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCliente(int id, [FromBody] ClienteUpdateDto clienteUpdateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var clienteAtualizado = await _clienteService.UpdateCliente(clienteUpdateDto, id);
            return Ok(clienteAtualizado);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{id}/senha")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSenha(int id, [FromBody] string novaSenha)
    {
        try
        {
            var resultado = await _clienteService.UpdateSenha(id, novaSenha);

            if (resultado)
                return NoContent();
            
            return BadRequest("Falha ao atualizar a senha.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCliente(int id)
    {
        try
        {
            var clienteDesativado = await _clienteService.DeleteCliente(id);
            return Ok(clienteDesativado);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
