using ProductCatalogService.Domain.Entities;
using ProductCatalogService.Infra.Repositories;
using Newtonsoft.Json;
using ProductCatalogService.Infra.Data;
using Microsoft.EntityFrameworkCore;
using AutoFixture;
using Shouldly;

namespace ProductCatalogService.Infra.Tests
{
    public class ProdutoRepositoryTest
    {
        private readonly ConfigDataBase _context;
        private readonly ProdutoRepository _repository;

        public ProdutoRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<ConfigDataBase>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;
            _context = new ConfigDataBase(options);

            _repository = new ProdutoRepository(_context);
        }

        [Fact]
        public async Task CriarProduto_QuandoDadosValidos_DeveRetornarProdutoComId()
        {
            //Arrange
            Categoria categoria = new Fixture().Create<Categoria>();
            await _context.Categorias.AddAsync(categoria);
            await _context.SaveChangesAsync();

            Produto produtoCriado = new Fixture().Build<Produto>().With(prod => prod.CategoriaId, categoria.CategoriaID).Create();

            //Act
            Produto resultado = await _repository.AddAsync(produtoCriado);

            //Assert
            resultado.ShouldNotBeNull();

            resultado.Nome.ShouldBe(produtoCriado.Nome);
            resultado.Descricao.ShouldBe(produtoCriado.Descricao);
            resultado.Estoque.ShouldBe(produtoCriado.Estoque);
            resultado.ImagemURL.ShouldBe(produtoCriado.ImagemURL);
            resultado.Preco.ShouldBe(produtoCriado.Preco);
            resultado.CategoriaId.ShouldBe(produtoCriado.CategoriaId);

            // Categoria deve estar carregada
            resultado.Categoria.Nome.ShouldBe(produtoCriado.Categoria.Nome);
            resultado.Categoria.Descricao.ShouldBe(produtoCriado.Categoria.Descricao);
        }


        [Fact]
        public async Task ObterProduto_QuandoIdValido_DeveRetornarProduto()
        {
            //Arrange
            Categoria categoria = new Fixture().Create<Categoria>();
            await _context.Categorias.AddAsync(categoria);
            await _context.SaveChangesAsync();

            Produto produtoOriginal = new Fixture().Build<Produto>().With(prod => prod.CategoriaId, categoria.CategoriaID).Create(); 

            _context.Add(produtoOriginal);
            await _context.SaveChangesAsync();

            //Act
            var resultado = await _repository.GetByIdAsync(produtoOriginal.ProdutoID);

            //Assert
            resultado.ShouldNotBeNull();
            resultado.Nome.ShouldBe(produtoOriginal.Nome);
            resultado.Descricao.ShouldBe(produtoOriginal.Descricao);
            resultado.Estoque.ShouldBe(produtoOriginal.Estoque);
            resultado.ImagemURL.ShouldBe(produtoOriginal.ImagemURL);
            resultado.Preco.ShouldBe(produtoOriginal.Preco);
            resultado.CategoriaId.ShouldBe(produtoOriginal.CategoriaId);

            // Categoria deve estar carregada
            resultado.Categoria.Nome.ShouldBe(produtoOriginal.Categoria.Nome);
            resultado.Categoria.Descricao.ShouldBe(produtoOriginal.Categoria.Descricao);

        }

        [Fact]
        public async Task ObterProdutos_QuandoExistemProdutos_DeveRetornarListaDeProdutos()
        {
            //Arrange
            List<Categoria> categorias = new List<Categoria>();
            new Fixture().AddManyTo(categorias);

            foreach(var categoria in categorias)
            {
                await _context.Categorias.AddAsync(categoria);
            }
            await _context.SaveChangesAsync();

            var categoriaIds = categorias.Select(c => c.CategoriaID).ToList();

            var fixture = new Fixture();

            var random = new Random();

            fixture.Customize<Produto>(composer => composer
                .FromFactory(() =>
                {
                    // Seleciona um CategoriaId aleatório da lista existente
                    int selectedCategoriaId = categoriaIds[random.Next(categoriaIds.Count)];

                    // Cria uma instância de Produto com os valores gerados e o CategoriaId selecionado
                    return new Produto(
                        nome: fixture.Create<string>(),
                        descricao: fixture.Create<string>(),
                        preco: fixture.Create<double>(),
                        estoque: fixture.Create<double>(),
                        categoriaId: selectedCategoriaId,
                        imagemURL: fixture.Create<string>()
                    );
                })
                .OmitAutoProperties() // Evita que o AutoFixture atribua automaticamente outras propriedades
              );

            List<Produto> listaProdutoOriginal = fixture.CreateMany<Produto>(10).ToList();

            foreach (var produto in listaProdutoOriginal)
            {
                await _context.Produtos.AddAsync(produto);
            }
            await _context.SaveChangesAsync();

            //Act
            var resultadoGetAll = await _repository.GetAllAsync();

            //Assert
            Assert.NotNull(resultadoGetAll);
            Assert.Equal(listaProdutoOriginal.Count, resultadoGetAll.Count);

            listaProdutoOriginal = listaProdutoOriginal.OrderBy(p => p.Nome).ToList();
            resultadoGetAll = resultadoGetAll.OrderBy(p => p.Nome).ToList();

            for (int i = 0; i < listaProdutoOriginal.Count; i++)
            {
                resultadoGetAll[i].Nome.ShouldBe(listaProdutoOriginal[i].Nome);
                resultadoGetAll[i].Descricao.ShouldBe(listaProdutoOriginal[i].Descricao);
                resultadoGetAll[i].Estoque.ShouldBe(listaProdutoOriginal[i].Estoque);
                resultadoGetAll[i].ImagemURL.ShouldBe(listaProdutoOriginal[i].ImagemURL);
                resultadoGetAll[i].Preco.ShouldBe(listaProdutoOriginal[i].Preco);
                resultadoGetAll[i].CategoriaId.ShouldBe(listaProdutoOriginal[i].CategoriaId);

                resultadoGetAll[i].Categoria.CategoriaID.ShouldBe(listaProdutoOriginal[i].Categoria.CategoriaID);
                resultadoGetAll[i].Categoria.Nome.ShouldBe(listaProdutoOriginal[i].Categoria.Nome);
                resultadoGetAll[i].Categoria.Descricao.ShouldBe(listaProdutoOriginal[i].Categoria.Descricao);
            }
        }

        [Fact]
        public async Task AtualizarProduto_QuandoDadosValidos_DeveRetornarProdutoAtualizadoIgualObjetoEnviado()
        {
            //Arrange
            var categoria = new Fixture().Create<Categoria>();
            await _context.Categorias.AddAsync(categoria);
            await _context.SaveChangesAsync();

            var produtoOriginal = new Fixture().Build<Produto>().With(x => x.CategoriaId, categoria.CategoriaID).Create();

            await _context.Produtos.AddAsync(produtoOriginal);
            await _context.SaveChangesAsync();

            var produtoAtualizado = new Fixture().Build<Produto>()
                .With(x => x.CategoriaId, categoria.CategoriaID)
                .With(x => x.ProdutoID, produtoOriginal.ProdutoID)
                .Create();


            //Act
            var produtoPreAlteracoes = await _repository.GetByIdAsync(produtoOriginal.ProdutoID);
            produtoPreAlteracoes.ShouldNotBeNull();
            produtoPreAlteracoes.Nome.ShouldBe(produtoOriginal.Nome);
            produtoPreAlteracoes.Descricao.ShouldBe(produtoOriginal.Descricao);
            produtoPreAlteracoes.Preco.ShouldBe(produtoOriginal.Preco);
            produtoPreAlteracoes.Estoque.ShouldBe(produtoOriginal.Estoque);
            produtoPreAlteracoes.Categoria.Nome.ShouldBe(produtoOriginal.Categoria.Nome);
            produtoPreAlteracoes.Categoria.Descricao.ShouldBe(produtoOriginal.Categoria.Descricao);
            produtoPreAlteracoes.ImagemURL.ShouldBe(produtoOriginal.ImagemURL);

            var produtoPosAlteracoes = await _repository.UpdateAsync(produtoAtualizado, produtoOriginal.ProdutoID);

            //Assert
            produtoPosAlteracoes.ShouldNotBeNull();
            produtoPosAlteracoes.Nome.ShouldBe(produtoAtualizado.Nome);
            produtoPosAlteracoes.Descricao.ShouldBe(produtoAtualizado.Descricao);
            produtoPosAlteracoes.Preco.ShouldBe(produtoAtualizado.Preco);
            produtoPosAlteracoes.Estoque.ShouldBe(produtoAtualizado.Estoque);
            produtoPosAlteracoes.Categoria.Nome.ShouldBe(produtoAtualizado.Categoria.Nome);
            produtoPosAlteracoes.Categoria.Descricao.ShouldBe(produtoAtualizado.Categoria.Descricao);
            produtoPosAlteracoes.ImagemURL.ShouldBe(produtoAtualizado.ImagemURL);
        }

        [Fact]
        public async Task DeletarProduto_QuandoProdutoExistente_DeveRetornarTrueEEvitarQueProdutoAindaExista()
        {
            //Arrange
            var categoria = new Fixture().Create<Categoria>();
            await _context.Categorias.AddAsync(categoria);
            await _context.SaveChangesAsync();

            var produtoOriginal = new Fixture().Build<Produto>().With(prod => prod.CategoriaId, categoria.CategoriaID).Create();

            _context.Add(produtoOriginal);
            await _context.SaveChangesAsync();

            //Act 
            bool deletouComSucesso = await _repository.RemoveAsync(produtoOriginal.ProdutoID);

            //Assert 
            Assert.True(deletouComSucesso);
            await Assert.ThrowsAsync<Exception>(() => _repository.GetByIdAsync(produtoOriginal.ProdutoID));
        }
    }
}
