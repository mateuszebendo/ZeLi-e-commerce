using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
            var categoria = new Categoria()
            {
                Nome = "Informática",
                Descricao = "Descricao"
            };

            var categoriaRepository = new CategoriaRepository(_context);

            //Act 
            Categoria? postResult = await categoriaRepository.Post(categoria);

            //Assert 

            Assert.NotNull(postResult);

            Assert.Equal(categoria.Nome, postResult.Nome);
            Assert.Equal(categoria.Descricao, postResult.Descricao);
        }

        [Fact]
        public async Task RecuperaCategorias_Deve_Retornar_Todas_Categorias()
        {
            //Arrange
            var categorias = new List<Categoria>
            {
                new Categoria()
                {
                    Nome = "Eletrônicos",
                    Descricao = "Descricao"
                }, 
                new Categoria()
                {
                    Nome = "Eletrodomésticos",
                    Descricao = "Descricao" 
                },
                new Categoria()
                {
                    Nome = "Informática",
                    Descricao = "Descricao"
                }
            };

            foreach(var categoria in categorias)
            {
                _context.Add(categoria);
            }
            await _context.SaveChangesAsync();

            var categoriaRepository = new CategoriaRepository(_context);

            //Act 
            List<Categoria>? getAllResult = await categoriaRepository.GetAll();

            //Assert 
            Assert.NotNull(_context);

            for(int i = 0; i< categorias.Count; i++)
            {
                Assert.Equal(categorias[i].Nome, getAllResult[i].Nome);
                Assert.Equal(categorias[i].Descricao, getAllResult[i].Descricao);
            }
        }

        public async Task ObterCategoriaPorId_QuandoIdExistente_DeveRetornarCategoria()
        {
            //Arrange
            var categoria = new Categoria
            {
                Nome = "Eletrônicos",
                Descricao = "Descricao"
            };

            _context.Add(categoria); 
            await _context.SaveChangesAsync();

            var categoriaRepositorio = new CategoriaRepository(_context);

            //Act 
            Categoria? getByIdResult = await categoriaRepositorio.GetById(1);

            //Assert 
            Assert.NotNull(getByIdResult);
            Assert.Equal(categoria.Nome, getByIdResult.Nome);
            Assert.Equal(categoria.Descricao, getByIdResult.Descricao);
        }

        public async Task AtualizarCategoria_QuandoDadosValidos_DeveRetornarCategoriaAtualizadoIgualObjetoEnviado()
        {
            //Arrange 
            Categoria categoriaOriginal = new Categoria
            {
                Nome = "Eletrônicos",
                Descricao = "Descricao"
            };

            
        }
    }
}
