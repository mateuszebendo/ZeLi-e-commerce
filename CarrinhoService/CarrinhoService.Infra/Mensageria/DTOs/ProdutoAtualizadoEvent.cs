using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrinhoService.Application.DTOs;

public class ProdutoAtualizadoEvent
{
    public int ProdutoId { get; set; }
    public String Nome { get; private set; }
    public String Descricao { get; private set; }
    public Double Preco { get; private set; }
    public Double Estoque { get; private set; }
    public int CategoriaId { get; set; }
    public String ImagemURL { get; private set; }
    public Boolean Ativo { get; set; } = true;
}
