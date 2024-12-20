using AuthService.Application.DTOs;
using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(UsuarioDto usuarioDto);
    Task<UsuarioDto> LoginAsync(string email, string senha);
}
