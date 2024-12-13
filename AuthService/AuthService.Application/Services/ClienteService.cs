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
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace AuthService.Application.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IMapper _mapper;

    public ClienteService(IClienteRepository clienteRepository, IMapper  mapper)
    {
        _clienteRepository = clienteRepository;
        _mapper = mapper;
    }

    public async Task<ClienteDto> CreateClienteAsync(ClienteCreateDto clienteCreateDto)
    {
        
        var clienteExistente = await _clienteRepository.GetClienteByEmailAsync(clienteCreateDto.Email);

        if (clienteExistente != null)
            throw new InvalidOperationException("Já existe um cliente com este email.");

        if (string.IsNullOrEmpty(clienteCreateDto.Nome))
            throw new ArgumentException("O nome do cliente é obrigatório.");

        if (clienteCreateDto.Nome.Length < 3) 
            throw new ArgumentException("O nome deve ter no mínimo 3 caracteres");
        
        if(!Regex.IsMatch(clienteCreateDto.Nome, @"^[A-Za-zÀ-ÖØ-öø-ÿ\s]+$"))
            throw new ArgumentException("Nome inválido. O nome não pode conter números ou caracteres especiais");

        if (string.IsNullOrEmpty(clienteCreateDto.Email))
            throw new ArgumentException("O Email do cliente é obrigatório.");

        if (!Regex.IsMatch(clienteCreateDto.Email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"))
            throw new ArgumentException("O email é invalido.");

        if (string.IsNullOrWhiteSpace(clienteCreateDto.Senha) || clienteCreateDto.Senha.Length < 8)
            throw new ArgumentException("A senha deve ter pelo menos 8 caracteres.");

        if (!Regex.IsMatch(clienteCreateDto.Senha, @"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$"))
            throw new ArgumentException("A senha fornecido não é válido.");

        var clienteEntity = _mapper.Map<Cliente>(clienteCreateDto);
        await _clienteRepository.CreateClienteAsync(clienteEntity);

        var clienteDto = _mapper.Map<ClienteDto>(clienteEntity);
   
        return clienteDto;
    }

    public async Task<IEnumerable<ClienteDto>> GetAllClientesAsync()
    {
        var clientesEntity = await _clienteRepository.GetAllClientesAsync();
        return _mapper.Map<IEnumerable<ClienteDto>>(clientesEntity);
    }

    public async Task<ClienteDto> GetClienteByIdAsync(int id)
    {
        var clienteEntity = await _clienteRepository.GetClienteByIdAsync(id);

        if (clienteEntity == null)
            throw new KeyNotFoundException($"Nenhum cliente encontrado com o id {id}.");

        var clienteDto = _mapper.Map<ClienteDto>(clienteEntity);

        return clienteDto;
    }

    public async Task<ClienteDto> GetClienteByEmailAsync(string email)
    {
        var clienteEntity = await _clienteRepository.GetClienteByEmailAsync(email);

        if (clienteEntity == null)
            throw new KeyNotFoundException($"Nenhum cliente encontrado com o Email {email}.");

        var clienteDto = _mapper.Map<ClienteDto>(clienteEntity);

        return clienteDto;
    }

    public async Task<ClienteDto> UpdateCliente(ClienteUpdateDto clienteUpdateDto, int id)
    {
        var clienteEntity = await _clienteRepository.GetClienteByIdAsync(id);

        if (clienteEntity == null)
            throw new KeyNotFoundException($"Nenhum cliente encontrado com o Id {id}.");

        if (string.IsNullOrEmpty(clienteUpdateDto.Nome))
            throw new ArgumentException("O nome do cliente é obrigatório.");

        if (clienteUpdateDto.Nome.Length < 3)
            throw new ArgumentException("O nome deve ter no mínimo 3 caracteres");

        if (!Regex.IsMatch(clienteUpdateDto.Nome, @"^[A-Za-zÀ-ÖØ-öø-ÿ\s]+$"))
            throw new ArgumentException("Nome inválido. O nome não pode conter números ou caracteres especiais");

        if (string.IsNullOrEmpty(clienteUpdateDto.Email))
            throw new ArgumentException("O Email do cliente é obrigatório.");

        if (!Regex.IsMatch(clienteUpdateDto.Email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"))
            throw new ArgumentException("O email é invalido.");

        var clienteAtualizado = await _clienteRepository.UpdateClienteAsync(clienteEntity, id);
        
        var clienteAtualizadoDto = _mapper.Map<ClienteDto>(clienteAtualizado);

        return clienteAtualizadoDto;
    }

    public async Task<bool> UpdateSenha(int id, string novaSenha)
    {
        var clienteEntity = await _clienteRepository.GetClienteByIdAsync(id);

        if (clienteEntity == null)
        {
            throw new KeyNotFoundException($"Cliente com ID {id} não encontrado.");
        }

        if (string.IsNullOrWhiteSpace(novaSenha) || novaSenha.Length < 8)
        {
            throw new ArgumentException("A senha deve ter pelo menos 8 caracteres.");
        }

        if (!Regex.IsMatch(novaSenha, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$"))
        {
            throw new ArgumentException("A senha deve conter pelo menos uma letra maiúscula, uma letra minúscula e um número.");
        }

        clienteEntity.Senha = novaSenha;

        await _clienteRepository.UpdateSenha(id, novaSenha);
        return true;
    }

    public async Task<ClienteDto> DeleteCliente(int id)
    {
        throw new NotImplementedException();
    }
}
