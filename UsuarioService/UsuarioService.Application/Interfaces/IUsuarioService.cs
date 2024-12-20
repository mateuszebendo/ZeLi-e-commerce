using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsuarioService.Application.DTOs;

namespace UsuarioService.Application.Interfaces;

public interface IUsuarioService
{
    Task<UsuarioDto> CreateUsuarioAsync(UsuarioCreateDto usuarioCreateDto);
    Task<UsuarioDto> GetUsuarioByIdAsync(int id);
    Task<UsuarioDto> GetUsuarioByEmailAsync(string email);
    Task<UsuarioDto> UpdateUsuarioAsync(UsuarioUpdateDto usuarioUpdateDto, int id);
    Task<bool> UpdateSenhaAsync(int id, string novaSenha);
    Task<UsuarioDto> DeleteUsuarioAsync(int id);
}
