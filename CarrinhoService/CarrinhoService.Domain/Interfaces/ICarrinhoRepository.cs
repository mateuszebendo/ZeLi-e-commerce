using CarrinhoService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrinhoService.Domain.Interfaces;

public interface ICarrinhoRepository
{
    Task<Carrinho> CreateCarrinhoAsync(Carrinho carrinho);
    Task AddItemAsync(int carrinhoId, ItemCarrinho item);
    Task<Carrinho> GetCarrinhoByIdAsync(int carrinhoId);
    Task<Carrinho> UpdateCarrinhoAsync(Carrinho carrinho);
}