using ProductCatalogService.Domain.Contracts;
using ProductCatalogService.Domain.Entities;
using ProductCatalogService.Infra.Data;

namespace ProductCatalogService.Infra.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly ConfigDataBase _config;

        public ProdutoRepository(ConfigDataBase config) 
        {
            _config = config;
        }
        public async Task<Produto> Post(Produto produto)
        {
            try
            {
                _config.Add(produto);
                await _config.SaveChangesAsync();
                return produto;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL error: " + ex.Message);
            }
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Produto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Produto> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Produto> Put(Produto produto, int id)
        {
            throw new NotImplementedException();
        }

    }
}
