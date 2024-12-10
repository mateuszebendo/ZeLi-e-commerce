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

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Categoria>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Categoria> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Categoria> Post(Categoria Categoria)
        {
            throw new NotImplementedException();
        }

        public Task<Categoria> Put(Categoria Categoria, int id)
        {
            throw new NotImplementedException();
        }
    }
}
