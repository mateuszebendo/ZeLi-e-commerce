using ProductCatalogService.Domain.Entities;
using ProductCatalogService.Domain.Pagination;

namespace ProductCatalogService.Domain.Contracts
{
    public interface IProdutoRepository
    {
        Task<Produto> AddAsync(Produto produto);
        Task<Produto> GetByIdAsync(int id);
        Task<IEnumerable<Produto>> GetAllAsync();
        Task<List<Produto>> GetAllPagedAsync(ProdutoParameters produtoParameters);
        Task<Produto> UpdateAsync(Produto produto, int id);
        Task<Produto> RemoveAsync(int id);
    }
}
