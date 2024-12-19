using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Services;

public class TokenService : ITokenService
{
    private readonly Settings.Settings _settings;
    private readonly IAuthRepository _authRepository;
    private readonly IMapper _mapper;

    public TokenService (IOptions<Settings.Settings> settings, IAuthRepository authRepository, IMapper mapper)
    {
        _settings = settings.Value;
        _authRepository = authRepository;
        _mapper = mapper;
    }

    public string GenerateToken(UsuarioDto usuarioDto)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_settings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.Name, usuarioDto.Email.ToString()),
                    //new Claim(ClaimTypes.Role, user.Role.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<UsuarioDto> LoginAsync(string email, string senha)
    {
        var usuarioEntity = await _authRepository.GetUsuarioByEmailAsync(email);

        if (usuarioEntity == null)
            throw new KeyNotFoundException($"Email ou senha Incorreto!");

        if (senha != usuarioEntity.Senha)
            throw new ArgumentException("Email ou senha Incorreto!");

        var usuarioDto = _mapper.Map<UsuarioDto>(usuarioEntity);

        return usuarioDto;
    }
}
