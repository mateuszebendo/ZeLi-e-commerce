using ProductCatalogService.Domain.Entities;
using ProductCatalogService.Domain.Pagination;

namespace ProductCatalogService.Domain.Contracts
{
    public interface ICategoriaRepository
    {
        Task<Categoria> AddAsync(Categoria Categoria);
        Task<Categoria> GetByIdAsync(int id);
        Task<IEnumerable<Categoria>> GetAllAsync();
        Task<PagedList<Categoria>> GetAllPagedAsync(CategoriaParameters categoriaParameters);
        Task<Categoria> UpdateAsync (Categoria Categoria, int id);
        Task<Categoria> RemoveAsync(int id);
    }
}
