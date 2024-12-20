using AuthService.Application.DTOs;
using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Interfaces;

public interface IUsuarioService
{
    Task<UsuarioDto> CreateUsuarioAsync(UsuarioCreateDto usuarioCreateDto);
    Task<UsuarioDto> GetUsuarioByIdAsync(int id);
    Task<UsuarioDto> GetUsuarioByEmailAsync(string email);
}