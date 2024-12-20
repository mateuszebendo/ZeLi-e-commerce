using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsuarioService.Domain.Entities;

namespace UsuarioService.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario> CreateUsuarioAsync(Usuario usuario);
    Task<Usuario> GetUsuarioByIdAsync(int id);
    Task<Usuario> GetUsuarioByEmailAsync(string email);
    Task<Usuario> UpdateUsuarioAsync(Usuario usuario, int id);
    Task<Boolean> UpdateSenhaAsync(int id, string novaSenha);
    Task<Usuario> DeleteUsuarioAsync(int id);
}
