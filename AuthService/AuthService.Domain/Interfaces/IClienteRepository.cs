using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Interfaces;

public interface IClienteRepository
{
    Task<Cliente> CreateClienteAsync(Cliente cliente);
    Task<IEnumerable<Cliente>> GetAllClientesAsync();
    Task<Cliente> GetClienteByIdAsync(int id);
    Task<Cliente> GetClienteByEmailAsync(string email);
    Task<Cliente> UpdateCliente(Cliente cliente, int id);
    Task<bool> UpdateSenha(int id, string novaSenha);
    Task<Cliente> DeleteCliente(int id);
}