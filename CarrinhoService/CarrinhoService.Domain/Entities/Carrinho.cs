using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrinhoService.Domain.Entities;

public class Carrinho
{
    public CarrinhoHeader CarrinhoHeader { get; set; } = new CarrinhoHeader();
    public IEnumerable<ItemCarrinho> ItemsCarrinho { get; set; } = Enumerable.Empty<ItemCarrinho>();
}
