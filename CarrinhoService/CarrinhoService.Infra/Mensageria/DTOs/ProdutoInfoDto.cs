using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrinhoService.Application.DTOs;

public class ProdutoInfoDto
{
    public int ProdutoId { get; set; }
    public String Nome { get; set; }
    public String Descricao { get;set; }
    public Double Preco { get; set; }
    public Double Estoque { get; set; }
    public int CategoriaId { get; set; }
    public String ImagemURL { get; set; }
    public Boolean Ativo { get; set; } = true;

    public ProdutoInfoDto(int produtoId, string nome, double preco)
    {
        ProdutoId = produtoId;
        Nome = nome;
        Preco = preco;
    }
}