using ProductCatalogService.Domain.Entities;

namespace ProductCatalogService.Domain.Contracts
{
    public interface ICategoriaRepository
    {
        Categoria Post(Categoria Categoria);
        Categoria GetById(int id);
        Categoria GetAll();
        Categoria Put(Categoria Categoria, int id);
        bool Delete(int id);
    }
}
