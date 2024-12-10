using ProductCatalogService.Domain.Entities;

namespace ProductCatalogService.Domain.Contracts
{
    public interface ICategoriaRepository
    {
        Task<Categoria> Post(Categoria Categoria);
        Task<Categoria> GetById(int id);
        Task<List<Categoria>> GetAll();
        Task<Categoria> Put(Categoria Categoria, int id);
        Task<bool> Delete(int id);
    }
}
