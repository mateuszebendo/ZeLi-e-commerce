using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UsuarioService.Domain.Validation;

namespace UsuarioService.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public DateTime DataCadastro { get; set; }
    public bool Ativo { get; set; }


    public Usuario(string nome, string email, string senha)
    {
        ValidateDomain(nome, email, senha);
        DataCadastro = DateTime.UtcNow;
        Ativo = true;
    }

    public Usuario()
    {
    }

    private void ValidateDomain(string nome, string email, string senha)
    {
        bool nomeInvalido = !NomeRegex.IsMatch(nome);

        DomainExceptionValidation.When(string.IsNullOrEmpty(nome), "Nome inválido. O nome é obrigatório");
        DomainExceptionValidation.When(nome.Length < 3, "O nome deve ter no mínimo 3 caracteres");
        DomainExceptionValidation.When(nomeInvalido,
            "Nome inválido. O nome não pode conter números ou caracteres especiais");

        bool emailInvalido = !EmailRegex.IsMatch(email);

        DomainExceptionValidation.When(string.IsNullOrEmpty(email), "Email inválido. O Email é obrigatório");
        DomainExceptionValidation.When(emailInvalido,
            "Email invalido");

        bool senhaInvalido = !SenhaRegex.IsMatch(senha);

        DomainExceptionValidation.When(senhaInvalido,
            "Senha invalida. A senha deve contar no minimo 8 caracteres, um Letra Maiuscula, um numero e um caracter especial");

        Nome = nome;
        Email = email;
        Senha = senha;
    }

    private static readonly Regex NomeRegex = new Regex(@"^[A-Za-zÀ-ÖØ-öø-ÿ\s]+$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private static readonly Regex EmailRegex = new Regex(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private static readonly Regex SenhaRegex = new Regex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);
}
