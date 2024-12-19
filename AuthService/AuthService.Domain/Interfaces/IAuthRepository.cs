using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Interfaces;

public interface IAuthRepository
{
    Task<Usuario> CreateUsuarioAsync(Usuario usuario);
    Task<Usuario> GetUsuarioByIdAsync(int id);
    Task<Usuario> GetUsuarioByEmailAsync(string email);
    Task<Usuario> GetUsuarioLogin(string email, string senha);
}