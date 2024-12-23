using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrinhoService.Domain.Entities;

public class Item
{
    public int ItemId { get; set; }

    public Carrinho? Carrinho { get; set; }

    public int ProdutoId { get; set; }

    public string? NomeProduto { get; set; }

    public int Quantidade { get; set; }

    public decimal PrecoUnitario { get; set; }
}
