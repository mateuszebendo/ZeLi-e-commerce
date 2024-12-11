using ProductCatalogService.Domain.Entities;

namespace ProductCatalogService.Domain.Contracts
{
    public interface IProdutoRepository
    {
        Task<Produto> AddAsync(Produto produto);
        Task<Produto> GetByIdAsync(int id);
        Task<List<Produto>> GetAllAsync();
        Task<Produto> UpdateAsync(Produto produto, int id);
        Task<bool> RemoveAsync(int id);
    }
}
