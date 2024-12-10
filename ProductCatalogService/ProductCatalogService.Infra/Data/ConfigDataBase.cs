using Microsoft.EntityFrameworkCore;
using ProductCatalogService.Domain.Entities;

namespace ProductCatalogService.Infra.Data
{
    public class ConfigDataBase : DbContext
    {
        public ConfigDataBase(DbContextOptions<ConfigDataBase> opt) : base(opt) { }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>().HasQueryFilter(p => p.Ativo);
            modelBuilder.Entity<Categoria>().HasQueryFilter(c => c.Ativo);
            base.OnModelCreating(modelBuilder);
        }

    }
}
