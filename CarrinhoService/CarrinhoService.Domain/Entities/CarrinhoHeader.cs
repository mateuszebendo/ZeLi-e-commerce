using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrinhoService.Domain.Entities;

public class CarrinhoHeader
{
    public int Id { get; set; }
    public int UsuarioId { get; set; } = 0;
    public string Cupom { get; set; } = string.Empty;
}
