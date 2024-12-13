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
    Task<ClienteDto> GetClienteByEmailAsync(string email);
    Task<ClienteDto> UpdateCliente(ClienteUpdateDto clienteUpdateDto, int id);
    Task<bool> UpdateSenha(int id, string novaSenha);
    Task<ClienteDto> DeleteCliente(int id);
}