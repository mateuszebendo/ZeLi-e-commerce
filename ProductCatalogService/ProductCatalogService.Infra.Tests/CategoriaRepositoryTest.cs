using Microsoft.EntityFrameworkCore;
using ProductCatalogService.Domain.Entities;
using ProductCatalogService.Infra.Data;
using ProductCatalogService.Infra.Repositories;

namespace ProductCatalogService.Infra.Tests
{
    public class CategoriaRepositoryTest
    {
        private readonly ConfigDataBase _context;

        public CategoriaRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<ConfigDataBase>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;
            _context = new ConfigDataBase(options);
        }

        [Fact]
        public async Task CriaCategoria_QuandoDadosValidos_DeveRetornarCategoriaComId()
        {
            //Arrange 
            var categoria = new Categoria("Informática", "Descricao");

            var categoriaRepository = new CategoriaRepository(_context);

            //Act 
            Categoria? postResult = await categoriaRepository.AddAsync(categoria);

            //Assert 
            Assert.NotNull(postResult);
            Assert.Equal(categoria.Nome, postResult.Nome);
            Assert.Equal(categoria.Descricao, postResult.Descricao);
        }

        [Fact]
        public async Task ObterCategorias_Deve_Retornar_Todas_Categorias()
        {
            //Arrange
            var categorias = new List<Categoria>
            {
                new Categoria("Eletrônicos", "Descricao"),
                new Categoria("Eletrodomésticos", "Descricao"),
                new Categoria("Informática", "Descricao")
            };

            await _context.AddRangeAsync(categorias);
            await _context.SaveChangesAsync();

            var categoriaRepository = new CategoriaRepository(_context);

            //Act 
            List<Categoria>? getAllResult = await categoriaRepository.GetAllAsync();

            //Assert 
            Assert.NotNull(getAllResult);
            Assert.Equal(categorias.Count, getAllResult.Count);

            for (int i = 0; i < categorias.Count; i++)
            {
                Assert.Equal(categorias[i].Nome, getAllResult[i].Nome);
                Assert.Equal(categorias[i].Descricao, getAllResult[i].Descricao);
            }
        }

        [Fact]
        public async Task ObterCategoriaPorId_QuandoIdExistente_DeveRetornarCategoria()
        {
            //Arrange
            var categoria = new Categoria("Eletrônicos", "Descricao");

            _context.Add(categoria);
            await _context.SaveChangesAsync();

            var categoriaRepositorio = new CategoriaRepository(_context);

            //Act 
            Categoria? getByIdResult = await categoriaRepositorio.GetByIdAsync(categoria.CategoriaID);

            //Assert 
            Assert.NotNull(getByIdResult);
            Assert.Equal(categoria.Nome, getByIdResult.Nome);
            Assert.Equal(categoria.Descricao, getByIdResult.Descricao);
        }

        [Fact]
        public async Task AtualizaCategoria_QuandoDadosValidos_DeveRetornarCategoriaAtualizadaIgualObjetoEnviado()
        {
            //Arrange 
            var categoria = new Categoria("Eletrônicos", "Descricao");

            _context.Add(categoria);
            await _context.SaveChangesAsync();

            var categoriaRepository = new CategoriaRepository(_context);

            // Agora para atualizar, usamos o método Update da própria entidade
            categoria.Update("Chinelos", "Descricao");

            //Act
            Categoria? categoriaAtualizada = await categoriaRepository.UpdateAsync(categoria, categoria.CategoriaID);

            //Assert 
            Assert.NotNull(categoriaAtualizada);
            Assert.Equal("Chinelos", categoriaAtualizada.Nome);
            Assert.Equal("Descricao", categoriaAtualizada.Descricao);
        }

        [Fact]
        public async Task DeletaCategoria_QuandoDeletada_RetornaTrueENaoEncontraNoBanco()
        {
            //Arrange
            var categoria = new Categoria("Eletrônicos", "Descricao");

            _context.Add(categoria);
            await _context.SaveChangesAsync();

            var categoriaRepository = new CategoriaRepository(_context);

            //Act 
            bool deletouComSucesso = await categoriaRepository.RemoveAsync(categoria.CategoriaID);
            var ex = await Assert.ThrowsAsync<Exception>(() => categoriaRepository.GetByIdAsync(categoria.CategoriaID));

            //Assert
            Assert.True(deletouComSucesso);
            Assert.Equal(typeof(Exception), ex.GetType());
        }
    }
}
