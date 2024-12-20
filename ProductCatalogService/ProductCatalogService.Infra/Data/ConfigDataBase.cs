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

            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(e => e.CategoriaID);
                entity.Property(e => e.CategoriaID).ValueGeneratedOnAdd();
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Descricao).IsRequired().HasMaxLength(250);
            });

            modelBuilder.Entity<Produto>(entity =>
            {
                entity.HasKey(e => e.ProdutoID);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Descricao).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Preco).IsRequired();
                entity.Property(e => e.Estoque).IsRequired();
                entity.Property(e => e.ImagemURL).HasMaxLength(250);

                entity.HasOne(e => e.Categoria)
                      .WithMany()
                      .HasForeignKey(e => e.CategoriaId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

    }
}
