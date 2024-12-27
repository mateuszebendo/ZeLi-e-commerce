using CarrinhoService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CarrinhoService.Infra.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Carrinho> Carrinhos { get; set; }
    public DbSet<CarrinhoHeader> CarrinhoHeaders { get; set; }
    public DbSet<ItemCarrinho> ItensCarrinho { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*
         modelBuilder.Entity<Carrinho>()
             .HasMany(c => c.ItemsCarrinho)
             .WithOne(i => i.CarrinhoH)
             .HasForeignKey(i => i.CarrinhoHeader.Id);

         base.OnModelCreating(modelBuilder);
         */
    }
}
