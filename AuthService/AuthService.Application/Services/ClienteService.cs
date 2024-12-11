using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Data;
using AuthService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace AuthService.Application.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly Context _context;
    private readonly IMapper _mapper;

    public ClienteService(IClienteRepository clienteRepository, Context context, IMapper  mapper)
    {
        _clienteRepository = clienteRepository;
        _context = context;
        _mapper = mapper;
    }

    public async Task<ClienteDto> CreateClienteAsync(ClienteCreateDto clienteCreateDto)
    {
        
        var clienteExistente = await _clienteRepository.GetClienteByEmailAsync(clienteCreateDto.Email);
        if (clienteExistente != null)
            throw new InvalidOperationException("Já existe um cliente com este email.");

        var clienteEntity = _mapper.Map<Cliente>(clienteCreateDto);
        await _clienteRepository.CreateClienteAsync(clienteEntity);

        var clienteDto = _mapper.Map<ClienteDto>(clienteEntity);
   
        return clienteDto;
    }

    public async Task<IEnumerable<Cliente>> GetAllClientesAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Cliente> GetClienteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Cliente> GetClienteByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public async Task<Cliente> UpdateCliente(Cliente cliente, int id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateSenha(int id, string novaSenha)
    {
        throw new NotImplementedException();
    }

    public async Task<Cliente> DeleteCliente(int id)
    {
        throw new NotImplementedException();
    }
}
