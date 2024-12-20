using AuthService.Domain.Entities;
using AuthService.Infrastructure.Data;
using AuthService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private AppDbContext _context;

    public AuthRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Usuario> CreateUsuarioAsync(Usuario cliente)
    {
        await _context.Usuarios.AddAsync(cliente);
        await _context.SaveChangesAsync();
        return cliente;

    }

    public async Task<Usuario> GetUsuarioByIdAsync(int id)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(c => c.Id == id);
        return usuario;
    }

    public async Task<Usuario> GetUsuarioByEmailAsync(string email)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(c => c.Email == email);
        return usuario;
    }

    public async Task<Usuario?> GetUsuarioLogin(string email, string senha) 
    { 
        return await _context.Usuarios.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower() && x.Senha == senha);
    }
}
