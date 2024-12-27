using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CarrinhoService.Domain.Entities;

public class ItemCarrinho
{
    public int Id { get; set; }
    public int Quantidade { get; set; }
    public int ProdutoId { get; set; }
    public Produto Produto { get; set; } = new Produto();
    public int CarrinhoHeaderId { get; set; } 
    public CarrinhoHeader CarrinhoHeader { get; set; } = new CarrinhoHeader();
}
