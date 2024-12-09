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
                _context.Add(produto);
                await _context.SaveChangesAsync();
                return produto;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL error: " + ex.Message);
            }
        }
        public async Task<List<Produto>> GetAll()
        {
            try
            {
                return await _context.Produtos.ToListAsync<Produto>();
            }
            catch (Exception ex)
            {
                throw new Exception("SQL error: " + ex.Message);
            }
        }

        public async  Task<Produto> GetById(int id)
        {
            try
            {
                return await _context.Produtos.
                    FirstOrDefaultAsync<Produto>(p => p.ProdutoID == id);
            }
            catch (Exception ex)
            {
                throw new Exception("SQL error: " + ex.Message);
            }
        }

        public async Task<Produto> Put(Produto produtoAtualizado, int id)
        {
            try
            {
                var produtoExistente = await _context.Produtos
                    .Include(p => p.Categoria) // garante que a categoria seja carregada
                    .FirstOrDefaultAsync(p => p.ProdutoID == id);

                // Ajusta a chave primária do produto
                produtoAtualizado.ProdutoID = produtoExistente.ProdutoID;

                // Ajusta a chave estrangeira da categoria
                // Somente se o Produto tiver a propriedade CategoriaID:
                produtoAtualizado.Categoria.CategoriaID = produtoExistente.Categoria.CategoriaID;

                // Caso não tenha a propriedade CategoriaID, então ajuste diretamente no objeto:
                // produtoAtualizado.Categoria.CategoriaID = produtoExistente.Categoria.CategoriaID;

                _context.Entry(produtoExistente).CurrentValues.SetValues(produtoAtualizado);

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

                _context.Produtos.Remove(produto);

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
