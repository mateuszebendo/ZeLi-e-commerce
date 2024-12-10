using Microsoft.EntityFrameworkCore;
using ProductCatalogService.Domain.Contracts;
using ProductCatalogService.Domain.Entities;
using ProductCatalogService.Infra.Data;

namespace ProductCatalogService.Infra.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly ConfigDataBase _context;

        public ProdutoRepository(ConfigDataBase context) 
        {
            _context = context;
        }
        public async Task<Produto> Post(Produto produto)
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
        public async Task<List<Produto>> GetAll()
        {
            try
            {
                var produtos = await _context.Produtos.ToListAsync<Produto>();

                return produtos;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro interno ao recuperar os produtos.", ex);
            }
        }

        public async  Task<Produto> GetById(int id)
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

        public async Task<Produto> Put(Produto produtoAtualizado, int id)
        {
            try
            {
                produtoAtualizado.ProdutoID = id;

                var produtoExistente = await _context.Produtos
                    .Include(p => p.Categoria)
                    .FirstOrDefaultAsync(p => p.ProdutoID == produtoAtualizado.ProdutoID);

                if (produtoExistente == null)
                {
                    throw new KeyNotFoundException("Produto não encontrado.");
                }

                produtoExistente.Nome = produtoAtualizado.Nome;
                produtoExistente.Descricao = produtoAtualizado.Descricao;
                produtoExistente.Preco = produtoAtualizado.Preco;
                produtoExistente.ImagemURL = produtoAtualizado.ImagemURL;
                produtoExistente.Categoria = produtoAtualizado.Categoria;
                produtoExistente.Estoque = produtoAtualizado.Estoque;

                await _context.SaveChangesAsync();

                return produtoExistente;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL error: " + ex.Message);
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var produto = await _context.Produtos
                    .FirstOrDefaultAsync<Produto>(p => p.ProdutoID == id);

                produto.Ativo = false;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL error: " + ex.Message);
            }
        }

    }
}
