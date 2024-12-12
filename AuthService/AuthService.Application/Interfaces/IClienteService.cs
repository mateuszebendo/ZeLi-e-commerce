using AuthService.Application.DTOs;
using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Interfaces;

public interface IClienteService
{
    Task<ClienteDto> CreateClienteAsync(ClienteCreateDto clienteCreateDto);
    Task<IEnumerable<ClienteDto>> GetAllClientesAsync();
    Task<ClienteDto> GetClienteByIdAsync(int id);
    Task<Cliente> GetClienteByEmailAsync(string email);
    Task<Cliente> UpdateCliente(Cliente cliente, int id);
    Task<bool> UpdateSenha(int id, string novaSenha);
    Task<Cliente> DeleteCliente(int id);
}