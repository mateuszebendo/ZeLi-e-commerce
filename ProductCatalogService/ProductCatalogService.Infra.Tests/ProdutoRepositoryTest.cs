using ProductCatalogService.Domain.Entities;
using ProductCatalogService.Infra.Repositories;
using Newtonsoft.Json;
using ProductCatalogService.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace ProductCatalogService.Infra.Tests
{
    public class ProdutoRepositoryTest
    {
        private readonly ConfigDataBase _context;

        public ProdutoRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<ConfigDataBase>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;
            _context = new ConfigDataBase(options);
        }

        [Fact]
        public async Task CriarProduto_QuandoDadosValidos_DeveRetornarProdutoComId()
        {
            //Arrange
            // Primeiro cria-se a categoria, obtém-se o CategoriaId.
            var categoria = new Categoria("Informática", "Descricao");
            await _context.Categorias.AddAsync(categoria);
            await _context.SaveChangesAsync();

            var produtoCriado = new Produto(
                nome: "Notebook Gamer",
                descricao: "Notebook potente para jogos e aplicações pesadas.",
                preco: 5499.99,
                estoque: 20,
                categoriaId: categoria.CategoriaID,
                imagemURL: "https://example.com/imagens/notebook-gamer.png");

            ProdutoRepository produtoRepository = new ProdutoRepository(_context);

            //Act
            var resultadoPost = await produtoRepository.AddAsync(produtoCriado);

            //Assert
            Assert.NotNull(resultadoPost);
            Assert.Equal(produtoCriado.Nome, resultadoPost.Nome);
            Assert.Equal(produtoCriado.Descricao, resultadoPost.Descricao);
            Assert.Equal(produtoCriado.Estoque, resultadoPost.Estoque);
            Assert.Equal(produtoCriado.ImagemURL, resultadoPost.ImagemURL);

            // Categoria deve estar carregada
            Assert.NotNull(resultadoPost.Categoria);
            Assert.Equal(categoria.Nome, resultadoPost.Categoria.Nome);
            Assert.Equal(categoria.Descricao, resultadoPost.Categoria.Descricao);
        }


        [Fact]
        public async Task ObterProduto_QuandoIdValido_DeveRetornarProduto()
        {
            //Arrange
            var categoria = new Categoria("Informática", "Descricao");
            await _context.Categorias.AddAsync(categoria);
            await _context.SaveChangesAsync();

            var produtoOriginal = new Produto(
                nome: "Notebook Gamer",
                descricao: "Notebook potente para jogos e aplicações pesadas.",
                preco: 5499.99,
                estoque: 20,
                categoriaId: categoria.CategoriaID,
                imagemURL: "https://example.com/imagens/notebook-gamer.png"
            );

            _context.Add(produtoOriginal);
            await _context.SaveChangesAsync();

            ProdutoRepository produtoRepository = new ProdutoRepository(_context);

            //Act
            var resultadoGetById = await produtoRepository.GetByIdAsync(produtoOriginal.ProdutoID);

            //Assert 
            Assert.NotNull(resultadoGetById);

            var obj1 = JsonConvert.SerializeObject(resultadoGetById);
            var obj2 = JsonConvert.SerializeObject(produtoOriginal);

            Assert.Equal(obj1, obj2);
        }

        [Fact]
        public async Task ObterProdutos_QuandoExistemProdutos_DeveRetornarListaDeProdutos()
        {
            //Arrange
            var categoriaInformatica = new Categoria("Informática", "Descricao");
            var categoriaEletronicos = new Categoria("Eletrônicos", "Descricao");
            var categoriaEletro = new Categoria("Eletrodomésticos", "Descricao");

            await _context.Categorias.AddRangeAsync(categoriaInformatica, categoriaEletronicos, categoriaEletro);
            await _context.SaveChangesAsync();

            List<Produto> listaProdutoOriginal = new List<Produto>()
            {
                new Produto(
                    "Notebook Gamer",
                    "Notebook potente para jogos e aplicações pesadas.",
                    5499.99,
                    20,
                    categoriaInformatica.CategoriaID,
                    "https://example.com/imagens/notebook-gamer.png"
                ),
                new Produto(
                    "Smartphone X",
                    "Smartphone com câmera de alta resolução e bateria de longa duração.",
                    1999.99,
                    50,
                    categoriaEletronicos.CategoriaID,
                    "https://example.com/imagens/smartphone-x.png"
                ),
                new Produto(
                    "Cafeteira Automática",
                    "Cafeteira com temporizador e reservatório de água embutido.",
                    299.90,
                    10,
                    categoriaEletro.CategoriaID,
                    "https://example.com/imagens/cafeteira-automatica.png"
                )
            };

            _context.AddRange(listaProdutoOriginal);
            await _context.SaveChangesAsync();

            ProdutoRepository produtoRepository = new ProdutoRepository(_context);

            //Act
            var resultadoGetAll = await produtoRepository.GetAllAsync();

            //Assert
            Assert.NotNull(resultadoGetAll);
            Assert.Equal(listaProdutoOriginal.Count, resultadoGetAll.Count);

            for (int i = 0; i < listaProdutoOriginal.Count; i++)
            {
                Assert.Equal(listaProdutoOriginal[i].Nome, resultadoGetAll[i].Nome);
                Assert.Equal(listaProdutoOriginal[i].Descricao, resultadoGetAll[i].Descricao);
                Assert.Equal(listaProdutoOriginal[i].Preco, resultadoGetAll[i].Preco);
                Assert.Equal(listaProdutoOriginal[i].Estoque, resultadoGetAll[i].Estoque);
                Assert.Equal(listaProdutoOriginal[i].ImagemURL, resultadoGetAll[i].ImagemURL);

                Assert.NotNull(resultadoGetAll[i].Categoria);
                Assert.Equal(listaProdutoOriginal[i].CategoriaId, resultadoGetAll[i].CategoriaId);
                Assert.Equal(listaProdutoOriginal[i].Categoria.Nome, resultadoGetAll[i].Categoria.Nome);
                Assert.Equal(listaProdutoOriginal[i].Categoria.Descricao, resultadoGetAll[i].Categoria.Descricao);
            }
        }

        [Fact]
        public async Task AtualizarProduto_QuandoDadosValidos_DeveRetornarProdutoAtualizadoIgualObjetoEnviado()
        {
            //Arrange
            var categoria = new Categoria("Informática", "Descricao");
            await _context.Categorias.AddAsync(categoria);
            await _context.SaveChangesAsync();

            var produtoOriginal = new Produto(
                nome: "Notebook Gamer",
                descricao: "Notebook potente para jogos e aplicações pesadas.",
                preco: 5499.99,
                estoque: 20,
                categoriaId: categoria.CategoriaID,
                imagemURL: "https://example.com/imagens/notebook-gamer.png"
            );

            await _context.Produtos.AddAsync(produtoOriginal);
            await _context.SaveChangesAsync();

            var produtoAtualizado = new Produto(
                nome: "Notebook Gamer Atualizado",
                descricao: "Notebook mais potente para jogos e aplicações pesadas.",
                preco: 6499.99,
                estoque: 20,
                categoriaId: categoria.CategoriaID,
                imagemURL: "https://example.com/imagens/notebook-gamer.png"
            );

            produtoAtualizado.Categoria = produtoOriginal.Categoria;

            var produtoRepository = new ProdutoRepository(_context);

            //Act
            var produtoPreAlteracoes = await produtoRepository.GetByIdAsync(produtoOriginal.ProdutoID);
            Assert.NotNull(produtoPreAlteracoes);
            Assert.Equal(produtoOriginal.Nome, produtoPreAlteracoes.Nome);
            Assert.Equal(produtoOriginal.Descricao, produtoPreAlteracoes.Descricao);
            Assert.Equal(produtoOriginal.Preco, produtoPreAlteracoes.Preco);
            Assert.Equal(produtoOriginal.Estoque, produtoPreAlteracoes.Estoque);
            Assert.Equal(produtoOriginal.Categoria.Nome, produtoPreAlteracoes.Categoria.Nome);
            Assert.Equal(produtoOriginal.Categoria.Descricao, produtoPreAlteracoes.Categoria.Descricao);
            Assert.Equal(produtoOriginal.ImagemURL, produtoPreAlteracoes.ImagemURL);

            var produtoPosAlteracoes = await produtoRepository.UpdateAsync(produtoAtualizado, produtoOriginal.ProdutoID);

            //Assert 
            Assert.NotNull(produtoPosAlteracoes);

            Assert.Equal(produtoAtualizado.Nome, produtoPosAlteracoes.Nome);
            Assert.Equal(produtoAtualizado.Descricao, produtoPosAlteracoes.Descricao);
            Assert.Equal(produtoAtualizado.Preco, produtoPosAlteracoes.Preco);
            Assert.Equal(produtoAtualizado.Estoque, produtoPosAlteracoes.Estoque);
            Assert.Equal(produtoAtualizado.Categoria.Nome, produtoPosAlteracoes.Categoria.Nome);
            Assert.Equal(produtoAtualizado.Categoria.Descricao, produtoPosAlteracoes.Categoria.Descricao);
            Assert.Equal(produtoAtualizado.ImagemURL, produtoPosAlteracoes.ImagemURL);
        }

        [Fact]
        public async Task DeletarProduto_QuandoProdutoExistente_DeveRetornarTrueEEvitarQueProdutoAindaExista()
        {
            //Arrange
            var categoria = new Categoria("Informática", "Descricao");
            await _context.Categorias.AddAsync(categoria);
            await _context.SaveChangesAsync();

            var produtoOriginal = new Produto(
                nome: "Notebook Gamer",
                descricao: "Notebook potente para jogos e aplicações pesadas.",
                preco: 5499.99,
                estoque: 20,
                categoriaId: categoria.CategoriaID,
                imagemURL: "https://example.com/imagens/notebook-gamer.png"
            );

            _context.Add(produtoOriginal);
            await _context.SaveChangesAsync();

            ProdutoRepository produtoRepository = new ProdutoRepository(_context);

            //Act 
            bool deletouComSucesso = await produtoRepository.RemoveAsync(produtoOriginal.ProdutoID);

            //Assert 
            Assert.True(deletouComSucesso);
            await Assert.ThrowsAsync<Exception>(() => produtoRepository.GetByIdAsync(produtoOriginal.ProdutoID));
        }
    }
}
