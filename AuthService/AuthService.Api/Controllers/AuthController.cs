using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Application.Services;
using AuthService.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;

    public AuthController(IUsuarioService usuarioService, IMapper mapper, ITokenService tokenService)
    {
        _usuarioService = usuarioService;
        _mapper = mapper;
        _tokenService = tokenService;
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UsuarioDto>> RegistrarUsuario([FromBody] UsuarioCreateDto usuarioCreateDto)
    {
        if (usuarioCreateDto is null)
        {
            return BadRequest("Dados inválidos");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var usuarioCriado = await _usuarioService.CreateUsuarioAsync(usuarioCreateDto);
        return CreatedAtAction(nameof(GetUsuarioById), new { id = usuarioCriado.Id }, usuarioCriado);
    }

    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUsuarioById(int id)
    {
        var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
        if (usuario == null)
            return NotFound();

        return Ok(usuario);
    }

    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<ActionResult<UsuarioLoginResponseDto>> Authenticate([FromBody] UsuarioLoginDto usuarioLoginDto)
    {
        var usuarioRetornado = await _tokenService.LoginAsync(usuarioLoginDto.Email, usuarioLoginDto.Senha);

        if (usuarioRetornado == null)
            return NotFound(new { message = "Usuário ou senha inválidos" });

        var token = _tokenService.GenerateToken(usuarioRetornado);
        var UsuarioLoginResponseDto = _mapper.Map<UsuarioLoginResponseDto>(usuarioRetornado);
        UsuarioLoginResponseDto.Token = token;


        return UsuarioLoginResponseDto;
    }
}
