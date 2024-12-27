using CarrinhoService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrinhoService.Domain.Interfaces;

public interface IProdutoRepository
{
    Task<Produto> CreateOrUpdateAsync(Produto produto);
    Task<Produto> GetByIdAsync(int produtoId);
    Task<List<Produto>> GetAllAsync();
    Task DeleteAsync(int produtoId);
}
