using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsuarioService.Domain.Entities;
using UsuarioService.Domain.Interfaces;
using UsuarioService.Infra.Data;

namespace UsuarioService.Infra.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<Usuario> CreateUsuarioAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task<Usuario> GetUsuarioByIdAsync(int id)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(c => c.Id == id);
        return usuario;
    }

    public async Task<bool> UpdateSenhaAsync(int id, string novaSenha)
    {
        var usuarioToUpdate = await _context.Usuarios.FirstOrDefaultAsync(c => c.Id == id);

        if (usuarioToUpdate == null)
            return false;

        usuarioToUpdate.Senha = novaSenha;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Usuario> UpdateUsuarioAsync(Usuario usuario, int id)
    {
        var usuarioToUpdate = await _context.Usuarios.FirstOrDefaultAsync(c => c.Id == id);

        if (usuarioToUpdate == null)
            return null;

        usuarioToUpdate.Nome = usuario.Nome;
        usuarioToUpdate.Email = usuario.Email;

        await _context.SaveChangesAsync();
        return usuarioToUpdate;
    }

    public async Task<Usuario> DeleteUsuarioAsync(int id)
    {
        var usuarioToDelete = await _context.Usuarios.FirstOrDefaultAsync(c => c.Id == id);

        if (usuarioToDelete == null)
            return null;

        usuarioToDelete.Ativo = false;
        await _context.SaveChangesAsync();
        return usuarioToDelete;
    }

    public async Task<Usuario> GetUsuarioByEmailAsync(string email)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(c => c.Email == email);
        return usuario;
    }
}
