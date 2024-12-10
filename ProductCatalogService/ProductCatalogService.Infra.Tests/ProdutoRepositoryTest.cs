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
            var produtoCriado = new Produto()
            {
                Nome = "Notebook Gamer",
                Descricao = "Notebook potente para jogos e aplicações pesadas.",
                Preco = 5499.99,
                Estoque = 20,
                Categoria = new Categoria()
                {
                    Nome = "Informática",
                    Descricao = "Descricao"
                },
                ImagemURL = "https://example.com/imagens/notebook-gamer.png"
            };

            ProdutoRepository produtoRepository = new ProdutoRepository(_context);

            //Act
            var resultadoPost = await produtoRepository.Post(produtoCriado);

            //Assert
            Assert.NotNull(resultadoPost);

            Assert.Equal(produtoCriado.Nome, resultadoPost.Nome);
            Assert.Equal(produtoCriado.Descricao, resultadoPost.Descricao);
            Assert.Equal(produtoCriado.Estoque, resultadoPost.Estoque);
            Assert.Equal(produtoCriado.Categoria.Nome, resultadoPost.Categoria.Nome);
            Assert.Equal(produtoCriado.Categoria.Descricao, resultadoPost.Categoria.Descricao);
            Assert.Equal(produtoCriado.ImagemURL, resultadoPost.ImagemURL);
        }


        [Fact]
        public async Task ObterProduto_QuandoIdValido_DeveRetornarProduto()
        {
            //Arrange
            var produtoOriginal = new Produto()
            {
                Nome = "Notebook Gamer",
                Descricao = "Notebook potente para jogos e aplicações pesadas.",
                Preco = 5499.99,
                Estoque = 20,
                Categoria = new Categoria()
                {
                    Nome = "Informática",
                    Descricao = "Descricao"
                },
                ImagemURL = "https://example.com/imagens/notebook-gamer.png"
            };

            _context.Add(produtoOriginal);
            await _context.SaveChangesAsync();

            ProdutoRepository produtoRepository = new ProdutoRepository( _context);

            //Act
            var resultadoGetById = await produtoRepository.GetById(1);

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
            List<Produto> listaProdutoOriginal = new List<Produto>()
            {
                new Produto()
                {
                    Nome = "Notebook Gamer",
                    Descricao = "Notebook potente para jogos e aplicações pesadas.",
                    Preco = 5499.99,
                    Estoque = 20,
                    Categoria = new Categoria()
                    {
                        Nome = "Informática",
                        Descricao = "Descricao"
                    },
                    ImagemURL = "https://example.com/imagens/notebook-gamer.png"
                },
                new Produto()
                {
                    Nome = "Smartphone X",
                    Descricao = "Smartphone com câmera de alta resolução e bateria de longa duração.",
                    Preco = 1999.99,
                    Estoque = 50,
                    Categoria = new Categoria()
                    {
                        Nome = "Eletrônicos",
                        Descricao = "Descricao"
                    },
                    ImagemURL = "https://example.com/imagens/smartphone-x.png"
                },
                new Produto()
                {
                    Nome = "Cafeteira Automática",
                    Descricao = "Cafeteira com temporizador e reservatório de água embutido.",
                    Preco = 299.90,
                    Estoque = 10,
                    Categoria = new Categoria()
                    {
                        Nome = "Eletrodomésticos",
                        Descricao = "Descricao"
                    },
                    ImagemURL = "https://example.com/imagens/cafeteira-automatica.png"
                }
            };

            foreach(var produto in listaProdutoOriginal)
            {
                _context.Add(produto);
            }

            await _context.SaveChangesAsync();

            ProdutoRepository produtoRepository = new ProdutoRepository(_context);

            //Act
            var resultadoGetAll = await produtoRepository.GetAll();

            //Assert
            Assert.NotNull(resultadoGetAll);
            Assert.Equal(listaProdutoOriginal.Count, resultadoGetAll.Count);

            for (int i = 0; i < listaProdutoOriginal.Count; i++)
            {
                Assert.Equal(listaProdutoOriginal[i].Nome, resultadoGetAll[i].Nome);
                Assert.Equal(listaProdutoOriginal[i].Descricao, resultadoGetAll[i].Descricao);
                Assert.Equal(listaProdutoOriginal[i].Preco, resultadoGetAll[i].Preco);
                Assert.Equal(listaProdutoOriginal[i].Estoque, resultadoGetAll[i].Estoque);
                Assert.Equal(listaProdutoOriginal[i].Categoria.Nome, resultadoGetAll[i].Categoria.Nome);
                Assert.Equal(listaProdutoOriginal[i].Categoria.Descricao, resultadoGetAll[i].Categoria.Descricao);
                Assert.Equal(listaProdutoOriginal[i].ImagemURL, resultadoGetAll[i].ImagemURL);
            }

        }

        [Fact]
        public async Task AtualizarProduto_QuandoDadosValidos_DeveRetornarProdutoAtualizadoIgualObjetoEnviado()
        {
            //Arrange
            var produtoOriginal = new Produto
            {
                Nome = "Notebook Gamer",
                Descricao = "Notebook potente para jogos e aplicações pesadas.",
                Preco = 5499.99,
                Estoque = 20,
                Categoria = new Categoria
                {
                    Nome = "Informática",
                    Descricao = "Descricao"
                },
                ImagemURL = "https://example.com/imagens/notebook-gamer.png"
            };

            var nomeProdutoOriginal = produtoOriginal.Nome;

            await _context.Produtos.AddAsync(produtoOriginal);
            await _context.SaveChangesAsync();

            var produtoAtualizado = new Produto
            {
                Nome = "Notebook Gamer Atualizado",
                Descricao = "Notebook mais potente para jogos e aplicações pesadas.",
                Preco = 6499.99,
                Estoque = 20,
                Categoria = new Categoria
                {
                    Nome = "Informática",
                    Descricao = "Descricao"
                },
                ImagemURL = "https://example.com/imagens/notebook-gamer.png"
            };

            var produtoRepository = new ProdutoRepository(_context);

            //Act
            var produtoPreAlteracoes = await produtoRepository.GetById(1);
            Assert.NotNull(produtoPreAlteracoes);
            Assert.Equal(produtoOriginal.Nome, produtoPreAlteracoes.Nome);
            Assert.Equal(produtoOriginal.Descricao, produtoPreAlteracoes.Descricao);
            Assert.Equal(produtoOriginal.Preco, produtoPreAlteracoes.Preco);
            Assert.Equal(produtoOriginal.Estoque, produtoPreAlteracoes.Estoque);
            Assert.Equal(produtoOriginal.Categoria.Nome, produtoPreAlteracoes.Categoria.Nome);
            Assert.Equal(produtoOriginal.Categoria.Descricao, produtoPreAlteracoes.Categoria.Descricao);
            Assert.Equal(produtoOriginal.ImagemURL, produtoPreAlteracoes.ImagemURL);

            var produtoPosAlteracoes = await produtoRepository.Put(produtoAtualizado, 1);

            var nomeProdutoAtualizado = produtoPosAlteracoes.Nome;

            //Assert 
            Assert.NotNull(produtoPosAlteracoes);

            Assert.Equal(produtoAtualizado.Nome, produtoPosAlteracoes.Nome);
            Assert.Equal(produtoAtualizado.Descricao, produtoPosAlteracoes.Descricao);
            Assert.Equal(produtoAtualizado.Preco, produtoPosAlteracoes.Preco);
            Assert.Equal(produtoAtualizado.Estoque, produtoPosAlteracoes.Estoque);
            Assert.Equal(produtoAtualizado.Categoria.Nome, produtoPosAlteracoes.Categoria.Nome);
            Assert.Equal(produtoAtualizado.Categoria.Descricao, produtoPosAlteracoes.Categoria.Descricao);
            Assert.Equal(produtoAtualizado.ImagemURL, produtoPosAlteracoes.ImagemURL);

            Assert.NotEqual(nomeProdutoOriginal, nomeProdutoAtualizado);
        }


        [Fact]
        public async Task DeletarProduto_QuandoProdutoExistente_DeveRetornarTrueEEvitarQueProdutoAindaExista

()
        {
            //Arrange
            var produtoOriginal = new Produto
            {
                Nome = "Notebook Gamer",
                Descricao = "Notebook potente para jogos e aplicações pesadas.",
                Preco = 5499.99,
                Estoque = 20,
                Categoria = new Categoria()
                {
                    Nome = "Informática",
                    Descricao = "Descricao"
                },
                ImagemURL = "https://example.com/imagens/notebook-gamer.png"
            };

            _context.Add(produtoOriginal);
            await _context.SaveChangesAsync();

            ProdutoRepository produtoRepository = new ProdutoRepository(_context);

            //Act 
            bool deletouComSucesso = await produtoRepository.Delete(1);

            //Assert 
            Assert.True(deletouComSucesso);
            Assert.ThrowsAsync<Exception>(() => produtoRepository.GetById(1));
        }
    }
}

