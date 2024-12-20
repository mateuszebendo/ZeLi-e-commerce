using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UsuarioService.Application.DTOs;
using UsuarioService.Application.Interfaces;
using UsuarioService.Domain.Entities;
using UsuarioService.Domain.Interfaces;

namespace UsuarioService.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IMapper _mapper;

    public UsuarioService(IUsuarioRepository usuarioRepository, IMapper mapper)
    {
        _usuarioRepository = usuarioRepository;
        _mapper = mapper;
    }

    public async Task<UsuarioDto> CreateUsuarioAsync(UsuarioCreateDto usuarioCreateDto)
    {

        var usuarioExistente = await _usuarioRepository.GetUsuarioByEmailAsync(usuarioCreateDto.Email);

        if (usuarioExistente != null)
            throw new InvalidOperationException("Já existe um usuario com este email.");

        if (string.IsNullOrEmpty(usuarioCreateDto.Nome))
            throw new ArgumentException("O nome do usuario é obrigatório.");

        if (usuarioCreateDto.Nome.Length < 3)
            throw new ArgumentException("O nome deve ter no mínimo 3 caracteres");

        if (!Regex.IsMatch(usuarioCreateDto.Nome, @"^[A-Za-zÀ-ÖØ-öø-ÿ\s]+$"))
            throw new ArgumentException("Nome inválido. O nome não pode conter números ou caracteres especiais");

        if (string.IsNullOrEmpty(usuarioCreateDto.Email))
            throw new ArgumentException("O Email do usuario é obrigatório.");

        if (!Regex.IsMatch(usuarioCreateDto.Email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"))
            throw new ArgumentException("O email é invalido.");

        if (string.IsNullOrWhiteSpace(usuarioCreateDto.Senha) || usuarioCreateDto.Senha.Length < 8)
            throw new ArgumentException("A senha deve ter pelo menos 8 caracteres.");

        if (!Regex.IsMatch(usuarioCreateDto.Senha, @"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$"))
            throw new ArgumentException("A senha fornecido não é válido.");

        var usuarioEntity = _mapper.Map<Usuario>(usuarioCreateDto);
        await _usuarioRepository.CreateUsuarioAsync(usuarioEntity);

        var usuarioDto = _mapper.Map<UsuarioDto>(usuarioEntity);

        return usuarioDto;
    }

    public async Task<UsuarioDto> GetUsuarioByIdAsync(int id)
    {
        var usuarioEntity = await _usuarioRepository.GetUsuarioByIdAsync(id);

        if (usuarioEntity == null)
            throw new KeyNotFoundException($"Nenhum usuario encontrado com o id {id}.");

        var usuarioDto = _mapper.Map<UsuarioDto>(usuarioEntity);

        return usuarioDto;
    }

    public async Task<UsuarioDto> GetUsuarioByEmailAsync(string email)
    {
        var usuarioEntity = await _usuarioRepository.GetUsuarioByEmailAsync(email);

        if (usuarioEntity == null)
            throw new KeyNotFoundException($"Nenhum cliente encontrado com o Email {email}.");

        var usuarioDto = _mapper.Map<UsuarioDto>(usuarioEntity);

        return usuarioDto;
    }

    public async Task<UsuarioDto> UpdateUsuarioAsync(UsuarioUpdateDto usuarioUpdateDto, int id)
    {
        var usuarioEntity = await _usuarioRepository.GetUsuarioByIdAsync(id);

        if (usuarioEntity == null)
            throw new KeyNotFoundException($"Nenhum usuario encontrado com o Id {id}.");

        if (string.IsNullOrEmpty(usuarioUpdateDto.Nome))
            throw new ArgumentException("O nome do usuario é obrigatório.");

        if (usuarioUpdateDto.Nome.Length < 3)
            throw new ArgumentException("O nome deve ter no mínimo 3 caracteres");

        if (!Regex.IsMatch(usuarioUpdateDto.Nome, @"^[A-Za-zÀ-ÖØ-öø-ÿ\s]+$"))
            throw new ArgumentException("Nome inválido. O nome não pode conter números ou caracteres especiais");

        if (string.IsNullOrEmpty(usuarioUpdateDto.Email))
            throw new ArgumentException("O Email do usuario é obrigatório.");

        if (!Regex.IsMatch(usuarioUpdateDto.Email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"))
            throw new ArgumentException("O email é invalido.");

        var usuarioAtualizado = await _usuarioRepository.UpdateUsuarioAsync(usuarioEntity, id);

        var usuarioAtualizadoDto = _mapper.Map<UsuarioDto>(usuarioAtualizado);

        return usuarioAtualizadoDto;
    }

    public async Task<bool> UpdateSenhaAsync(int id, string novaSenha)
    {
        var usuarioEntity = await _usuarioRepository.GetUsuarioByIdAsync(id);

        if (usuarioEntity == null)
        {
            throw new KeyNotFoundException($"Usuario com ID {id} não encontrado.");
        }

        if (string.IsNullOrWhiteSpace(novaSenha) || novaSenha.Length < 8)
        {
            throw new ArgumentException("A senha deve ter pelo menos 8 caracteres.");
        }

        if (!Regex.IsMatch(novaSenha, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$"))
        {
            throw new ArgumentException("A senha deve conter pelo menos uma letra maiúscula, uma letra minúscula e um número.");
        }

        usuarioEntity.Senha = novaSenha;

        await _usuarioRepository.UpdateSenhaAsync(id, novaSenha);
        return true;
    }

    public async Task<UsuarioDto> DeleteUsuarioAsync(int id)
    {
        var usuarioEntity = await _usuarioRepository.GetUsuarioByIdAsync(id);

        if (usuarioEntity == null)
            throw new KeyNotFoundException($"Nenhum usuario encontrado com o Id {id}.");

        var usuarioDesativado = await _usuarioRepository.DeleteUsuarioAsync(id);

        var usuarioDto = _mapper.Map<UsuarioDto>(usuarioDesativado);
        return usuarioDto;
    }
}
