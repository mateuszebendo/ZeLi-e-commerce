using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.DTOs;

public class UsuarioCreateDto
{
    [Required(ErrorMessage = "Nome é obrigatório.")]
    [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ\s]+$",
    ErrorMessage = "Nome inválido. O nome não pode conter números ou caracteres especiais.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Email é obrigatório.")]
    [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Email inválido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Senha é obrigatória.")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
    ErrorMessage = "Senha inválida. A senha deve ter no mínimo 8 caracteres, contendo pelo menos uma letra maiúscula, um numero e um caractere especial.")]
    public string Senha { get; set; }

    public UsuarioCreateDto(string nome, string email, string senha)
    {
        Nome = nome;
        Email = email;
        Senha = senha;
    }
}