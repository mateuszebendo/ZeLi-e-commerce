using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.DTOs
{
    public class ClienteUpdateDto
    {
        public ClienteUpdateDto(string nome, string email)
        {
            Nome = nome;
            Email = email;
        }

        [Required(ErrorMessage = "Nome é obrigatório.")]
        [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ\s]+$",
        ErrorMessage = "Nome inválido. O nome não pode conter números ou caracteres especiais.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Email é obrigatório.")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Email inválido.")]
        public string Email { get; set; }
    }
}
