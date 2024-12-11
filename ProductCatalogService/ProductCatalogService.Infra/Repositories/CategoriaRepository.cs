using Microsoft.EntityFrameworkCore;
using ProductCatalogService.Domain.Contracts;
using ProductCatalogService.Domain.Entities;
using ProductCatalogService.Infra.Data;

namespace ProductCatalogService.Infra.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly ConfigDataBase _context;

        public CategoriaRepository(ConfigDataBase context)
        {
            _context = context;
        }

        public async Task<Categoria> AddAsync(Categoria categoria)
        {
            try
            {
                if ( categoria == null)
                    throw new ArgumentNullException(nameof( categoria));

                _context.Add( categoria);
                await _context.SaveChangesAsync();
                return  categoria;
            }
            catch (DbUpdateException dbEx)
            {
                throw new DbUpdateException("Ocorreu um erro ao inserir o  categoria.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro interno ao salvar o  categoria.", ex);
            }
        }

        public async Task<List<Categoria>> GetAllAsync()
        {
            try
            {
                var categorias = await _context.Categorias.ToListAsync<Categoria>();

                return categorias;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro interno ao recuperar os  categorias.", ex);
            }
        }

        public async Task<Categoria> GetByIdAsync(int id)
        {
            try
            {
                var  categoria = await _context. Categorias
                    .FirstOrDefaultAsync<Categoria>(c => c.CategoriaID == id);

                if ( categoria == null)
                    throw new ArgumentException(" categoria não existente");

                return  categoria;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro interno ao recuperar o  categoria.", ex);
            }
        }

        public async Task<Categoria> UpdateAsync(Categoria categoriaAtualizada, int id)
        {
            try
            {
                var categoriaExistente = await _context.Categorias
                    .FirstOrDefaultAsync(p => p.CategoriaID == categoriaAtualizada.CategoriaID);

                if (categoriaExistente == null)
                {
                    throw new KeyNotFoundException("Categoria não encontrado.");
                }

                categoriaExistente.Update(categoriaAtualizada.Nome, categoriaAtualizada.Descricao);

                await _context.SaveChangesAsync();

                return categoriaExistente;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL error: " + ex.Message);
            }
        }

        public async Task<bool> RemoveAsync(int id)
        {
            try
            {
                var categoria = await _context.Categorias
                    .FirstOrDefaultAsync<Categoria>(c => c.CategoriaID == id);

                categoria.Ativo = false;
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
