using Microsoft.EntityFrameworkCore;
using ProductCatalogService.Domain.Entities;
using ProductCatalogService.Infra.Data;
using ProductCatalogService.Infra.Repositories;
using AutoFixture;
using Shouldly;

namespace ProductCatalogService.Infra.Tests
{
    public class CategoriaRepositoryTest
    {
        private readonly ConfigDataBase _context;
        private readonly CategoriaRepository _repository;

        public CategoriaRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<ConfigDataBase>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;
            _context = new ConfigDataBase(options);

            _repository = new CategoriaRepository(_context);
        }

        [Fact]
        public async Task CriaCategoria_QuandoDadosValidos_DeveRetornarCategoriaComId()
        {
            //Arrange 
            var categoria = new Fixture().Create<Categoria>();

            //Act 
            Categoria? postResult = await _repository.AddAsync(categoria);

            //Assert 
            Assert.NotNull(postResult);
            Assert.Equal(categoria.Nome, postResult.Nome);
            Assert.Equal(categoria.Descricao, postResult.Descricao);
        }

        [Fact]
        public async Task ObterCategorias_Deve_Retornar_Todas_Categorias()
        {
            // Arrange
            List<Categoria> categorias = new Fixture().CreateMany<Categoria>(3).ToList();
            categorias.Count.ShouldBe(3);

            foreach (var categoria in categorias)
            {
                await _context.Categorias.AddAsync(categoria);
            }
            await _context.SaveChangesAsync();

            // Act 
            List<Categoria>? getAllResult = await _repository.GetAllAsync();

            // Assert 
            getAllResult.ShouldNotBeNull();
            getAllResult.Count.ShouldBe(categorias.Count);

            categorias = categorias.OrderBy(c => c.Nome).ToList();
            getAllResult = getAllResult.OrderBy(c => c.Nome).ToList();

            for (int i = 0; i < categorias.Count; i++)
            {
                getAllResult[i].Nome.ShouldBe(categorias[i].Nome);
                getAllResult[i].Descricao.ShouldBe(categorias[i].Descricao);
            }
        }

        [Fact]
        public async Task ObterCategoriaPorId_QuandoIdExistente_DeveRetornarCategoria()
        {
            //Arrange
            var categoria = new Fixture().Create<Categoria>();

            _context.Add(categoria);
            await _context.SaveChangesAsync();

            //Act 
            Categoria? getByIdResult = await _repository.GetByIdAsync(categoria.CategoriaID);

            //Assert 
            getByIdResult.ShouldNotBeNull();
            getByIdResult.Nome.ShouldBe(categoria.Nome);
            getByIdResult.Descricao.ShouldBe(categoria.Descricao);
        }

        [Fact]
        public async Task AtualizaCategoria_QuandoDadosValidos_DeveRetornarCategoriaAtualizadaIgualObjetoEnviado()
        {
            //Arrange 
            var categoria = new Fixture().Create<Categoria>();

            _context.Add(categoria);
            await _context.SaveChangesAsync();

            categoria.Update("Chinelos", "Descricao");

            //Act
            Categoria? categoriaAtualizada = await _repository.UpdateAsync(categoria, categoria.CategoriaID);

            //Assert 
            categoriaAtualizada.ShouldNotBeNull();
            categoriaAtualizada.Nome.ShouldBe("Chinelos");
            categoriaAtualizada.Descricao.ShouldBe("Descricao");
        }

        [Fact]
        public async Task DeletaCategoria_QuandoDeletada_RetornaTrueENaoEncontraNoBanco()
        {
            //Arrange
            var categoria = new Fixture().Create<Categoria>();

            _context.Add(categoria);
            await _context.SaveChangesAsync();

            //Act 
            bool deletouComSucesso = await _repository.RemoveAsync(categoria.CategoriaID);
            var ex = await Assert.ThrowsAsync<Exception>(() => _repository.GetByIdAsync(categoria.CategoriaID));

            //Assert
            deletouComSucesso.ShouldBeTrue();
            ex.GetType().ShouldBe(typeof(Exception));
        }
    }
}
