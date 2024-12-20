using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.DTOs;

public class UsuarioLoginDto
{
    public string Email { get; set; }
    public string Senha { get; set; }
}
