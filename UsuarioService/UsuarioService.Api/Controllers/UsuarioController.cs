using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UsuarioService.Application.DTOs;
using UsuarioService.Application.Interfaces;

namespace UsuarioService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _clienteService;
    private readonly IMapper _mapper;

    public UsuarioController(IUsuarioService clienteService, IMapper mapper)
    {
        _clienteService = clienteService;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UsuarioDto>> RegistrarUsuario([FromBody] UsuarioCreateDto clienteCreateDto)
    {
        if (clienteCreateDto is null)
        {
            return BadRequest("Dados inválidos");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var clienteCriado = await _clienteService.CreateUsuarioAsync(clienteCreateDto);
        return CreatedAtAction(nameof(GetUsuarioById), new { id = clienteCriado.Id }, clienteCriado);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUsuarioById(int id)
    {
        var cliente = await _clienteService.GetUsuarioByIdAsync(id);
        if (cliente == null)
            return NotFound();

        return Ok(cliente);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUsuario(int id, [FromBody] UsuarioUpdateDto clienteUpdateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var clienteAtualizado = await _clienteService.UpdateUsuarioAsync(clienteUpdateDto, id);
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
            var resultado = await _clienteService.UpdateSenhaAsync(id, novaSenha);

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
    public async Task<IActionResult> DeleteUsuario(int id)
    {
        try
        {
            var clienteDesativado = await _clienteService.DeleteUsuarioAsync(id);
            return Ok(clienteDesativado);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
