using Microsoft.EntityFrameworkCore;
using ProductCatalogService.Domain.Entities;

namespace ProductCatalogService.Infra.Data
{
    public class ConfigDataBase : DbContext
    {
        public ConfigDataBase(DbContextOptions<ConfigDataBase> opt) : base(opt) { }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
    }
}
