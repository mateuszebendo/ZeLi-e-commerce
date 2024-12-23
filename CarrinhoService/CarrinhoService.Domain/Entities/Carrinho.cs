using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrinhoService.Domain.Entities;

public class Carrinho
{
    public int CarrinhoId { get; set; }

    public int ClienteId { get; set; }

    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    public string Status { get; set; } = "Aberto";

    public ICollection<Item> Itens { get; set; } = new List<Item>();
}
