using CarrinhoService.Domain.Entities;
using CarrinhoService.Domain.Interfaces;
using CarrinhoService.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrinhoService.Infra.Repositories;

public class CarrinhoRepository : ICarrinhoRepository
{

    private readonly AppDbContext _context;

    public CarrinhoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Carrinho> CreateCarrinhoAsync(Carrinho carrinho)
    {
        _context.Carrinhos.Add(carrinho);
        await _context.SaveChangesAsync();
        return carrinho;
    }

    public async Task AddItemAsync(int carrinhoId, ItemCarrinho itemCarrinho)
    {
        itemCarrinho.CarrinhoHeaderId = carrinhoId;

        _context.ItensCarrinho.Add(itemCarrinho);
        await _context.SaveChangesAsync();
    }
    

    public async Task<Carrinho> GetCarrinhoByIdAsync(int carrinhoId)
    {
        return await _context.Carrinhos
            .Include(c => c.ItemsCarrinho)
            .SingleOrDefaultAsync(c => c.CarrinhoHeader.Id == carrinhoId);
    }

    public async Task<Carrinho> UpdateCarrinhoAsync(Carrinho carrinho)
    {
        _context.Carrinhos.Update(carrinho);
        await _context.SaveChangesAsync();
        return carrinho;
    }
}
