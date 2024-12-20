using ProductCatalogService.Domain.Entities;

namespace ProductCatalogService.Domain.Contracts
{
    public interface ICategoriaRepository
    {
        Task<Categoria> AddAsync(Categoria Categoria);
        Task<Categoria> GetByIdAsync(int id);
        Task<List<Categoria>> GetAllAsync();
        Task<Categoria> UpdateAsync (Categoria Categoria, int id);
        Task<Categoria> RemoveAsync(int id);
    }
}
