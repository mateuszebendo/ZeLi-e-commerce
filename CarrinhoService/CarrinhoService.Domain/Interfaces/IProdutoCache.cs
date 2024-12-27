using CarrinhoService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrinhoService.Application.Interfaces;

public interface IProdutoCache
{
    Produto GetProduto(int produtoId);
    void UpdateProduto(int produtoId, string nome, double preco);
}
