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

public class ClienteRepository : IClienteRepository
{
    private Context _context;

    public ClienteRepository(Context context)
    {
        _context = context;
    }
    
    public async Task<Cliente> CreateClienteAsync(Cliente cliente)
    {
        await _context.Clientes.AddAsync(cliente);
        await _context.SaveChangesAsync();
        return cliente;

    }

    public async Task<IEnumerable<Cliente>> GetAllClientesAsync()
    {
        return await _context.Clientes.ToListAsync();
    }

    public async Task<ClienteDto> GetClienteByIdAsync(int id)
    {
        var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.ClienteId == id);

        if (cliente == null)
            throw new KeyNotFoundException($"Nenhum cliente encontrado com o Id {id}.");

        return cliente;
    }

    public async Task<Cliente> GetClienteByEmailAsync(string email)
    {
        var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Email == email);

        if (cliente == null)
            throw new KeyNotFoundException($"Nenhum cliente encontrado com o Id {email}.");

        return cliente;
    }

    public async Task<Cliente> UpdateCliente(Cliente cliente, int id)
    {
        var clienteToUpdate = await _context.Clientes.FirstOrDefaultAsync(c => c.ClienteId == id);

        if (clienteToUpdate == null)
            return null;

        clienteToUpdate.Nome = cliente.Nome;
        clienteToUpdate.Email = cliente.Email;
 
        await _context.SaveChangesAsync();
        return clienteToUpdate;
    }

    public async Task<bool> UpdateSenha(int clienteId, string novaSenha)
    {
        var clienteToUpdate = await _context.Clientes.FirstOrDefaultAsync(c => c.ClienteId == clienteId);

        if (clienteToUpdate == null)
            return false;

        clienteToUpdate.Senha = novaSenha;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Cliente> DeleteCliente(int id)
    {
        var clienteToDelete = await _context.Clientes.FirstOrDefaultAsync(c => c.ClienteId == id);

        if (clienteToDelete == null)
        {
            throw new KeyNotFoundException($"Nenhum cliente encontrado com o Id {id}.");
        }

        clienteToDelete.Ativo = false;
        await _context.SaveChangesAsync();
        return clienteToDelete;
    }
}
