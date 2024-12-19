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
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IAuthRepository _authRepository;
    private readonly IMapper _mapper;

    public UsuarioService(IAuthRepository authRepository, IMapper  mapper)
    {
        _authRepository = authRepository;
        _mapper = mapper;
    }

    public async Task<UsuarioDto> CreateUsuarioAsync(UsuarioCreateDto usuarioCreateDto)
    {
        
        var usuarioExistente = await _authRepository.GetUsuarioByEmailAsync(usuarioCreateDto.Email);

        if (usuarioExistente != null)
            throw new InvalidOperationException("Já existe um cliente com este email.");

        if (string.IsNullOrEmpty(usuarioCreateDto.Nome))
            throw new ArgumentException("O nome do cliente é obrigatório.");

        if (usuarioCreateDto.Nome.Length < 3) 
            throw new ArgumentException("O nome deve ter no mínimo 3 caracteres");
        
        if(!Regex.IsMatch(usuarioCreateDto.Nome, @"^[A-Za-zÀ-ÖØ-öø-ÿ\s]+$"))
            throw new ArgumentException("Nome inválido. O nome não pode conter números ou caracteres especiais");

        if (string.IsNullOrEmpty(usuarioCreateDto.Email))
            throw new ArgumentException("O Email do cliente é obrigatório.");

        if (!Regex.IsMatch(usuarioCreateDto.Email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"))
            throw new ArgumentException("O email é invalido.");

        if (string.IsNullOrWhiteSpace(usuarioCreateDto.Senha) || usuarioCreateDto.Senha.Length < 8)
            throw new ArgumentException("A senha deve ter pelo menos 8 caracteres.");

        if (!Regex.IsMatch(usuarioCreateDto.Senha, @"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$"))
            throw new ArgumentException("A senha fornecido não é válido.");

        var usuarioEntity = _mapper.Map<Usuario>(usuarioCreateDto);
        await _authRepository.CreateUsuarioAsync(usuarioEntity);

        var usuarioDto = _mapper.Map<UsuarioDto>(usuarioEntity);
   
        return usuarioDto;
    }

    public async Task<UsuarioDto> GetUsuarioByIdAsync(int id) 
    {
        var usuarioEntity = await _authRepository.GetUsuarioByIdAsync(id);

        if (usuarioEntity == null)
            throw new KeyNotFoundException($"Nenhum usuario encontrado com o id {id}.");

        var usuarioDto = _mapper.Map<UsuarioDto>(usuarioEntity);

        return usuarioDto;
    }

    public async Task<UsuarioDto> GetUsuarioByEmailAsync(string email)
    {
        var usuarioEntity = await _authRepository.GetUsuarioByEmailAsync(email);

        if (usuarioEntity == null)
            throw new KeyNotFoundException($"Nenhum usuario encontrado com o email {email}.");

        var usuarioDto = _mapper.Map<UsuarioDto>(usuarioEntity);

        return usuarioDto;
    }

   
}
