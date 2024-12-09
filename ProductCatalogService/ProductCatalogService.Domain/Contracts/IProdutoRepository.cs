using ProductCatalogService.Domain.Entities;

namespace ProductCatalogService.Domain.Contracts
{
    public interface IProdutoRepository
    {
        Task<Produto> Post(Produto produto);
        Task<Produto> GetById(int id);
        Task<List<Produto>> GetAll();
        Task<Produto> Put(Produto produto, int id);
        Task<bool> Delete(int id);
    }
}
