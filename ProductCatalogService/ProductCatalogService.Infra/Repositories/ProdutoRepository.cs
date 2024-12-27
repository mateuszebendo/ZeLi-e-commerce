using Microsoft.EntityFrameworkCore;
using ProductCatalogService.Domain.Contracts;
using ProductCatalogService.Domain.Entities;
using ProductCatalogService.Domain.Pagination;
using ProductCatalogService.Infra.Data;
using System.Linq.Expressions;

namespace ProductCatalogService.Infra.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context) 
        {
            _context = context;
        }
        public async Task<Produto> AddAsync(Produto produto)
        {
            try
            {
                if(produto == null)
                    throw new ArgumentNullException(nameof(produto));

                _context.Add(produto);
                await _context.SaveChangesAsync();
                return produto;
            }
            catch (DbUpdateException dbEx)
            {
                throw new DbUpdateException("Ocorreu um erro ao inserir o produto.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro interno ao salvar o produto.", ex);
            }
        }
        public async Task<IEnumerable<Produto>> GetAllAsync()
        {
            try
            {
                var produtos = await _context
                    .Produtos
                    .ToListAsync<Produto>();

                return produtos;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro interno ao recuperar os produtos.", ex);
            }
        }

        public async Task<List<Produto>> GetAllPagedAsync(ProdutoParameters produtoParameters)
        {
            try
            {
                var produtos = await _context
                    .Produtos
                    .OrderBy(p => p.Nome)
                    .Skip((produtoParameters.PageNumber - 1) * produtoParameters.PageSize)
                    .Take(produtoParameters.PageSize)
                    .ToListAsync<Produto>();

                return produtos;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro interno ao recuperar os produtos.", ex);
            }
        }

        public async  Task<Produto> GetByIdAsync(int id)
        {
            try
            {
                var produto = await _context.Produtos
                    .FirstOrDefaultAsync<Produto>(p => p.ProdutoID == id);

                if (produto == null)
                    throw new ArgumentException("Produto não existente");

                return produto;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro interno ao recuperar o produto.", ex);
            }
        }

        public async Task<Produto> UpdateAsync(Produto produtoAtualizado, int id)
        {
            try
            {
                var produtoExistente = await _context.Produtos
                    .Include(p => p.Categoria)
                    .FirstOrDefaultAsync(p => p.ProdutoID == id);

                if (produtoExistente == null)
                {
                    throw new KeyNotFoundException("Produto não encontrado.");
                }

                produtoExistente.Update(
                produtoAtualizado.Nome,
                produtoAtualizado.Descricao,
                produtoAtualizado.Preco,
                produtoAtualizado.Estoque,
                produtoAtualizado.CategoriaId,
                produtoAtualizado.ImagemURL
                );

                produtoExistente.Categoria = produtoAtualizado.Categoria;

                await _context.SaveChangesAsync();

                return produtoExistente;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL error: " + ex.Message);
            }
        }

        public async Task<Produto> RemoveAsync(int id)
        {
            try
            {
                var produto = await _context.Produtos
                    .FirstOrDefaultAsync<Produto>(p => p.ProdutoID == id);

                produto.Ativo = false;
                await _context.SaveChangesAsync();

                return produto;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL error: " + ex.Message);
            }
        }

    }
}
