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
        public async void CriaProduto_Deve_Retornar_Produto_Com_Id()
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

            var obj1 = JsonConvert.SerializeObject(produtoCriado);
            var obj2 = JsonConvert.SerializeObject(resultadoPost);

            Assert.Equal(obj1, obj2);
        }


        [Fact]
        public async void ObterProduto_Deve_Retornar_Produto()
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
        public async void ObterProdutos_Deve_Retornar_ListaProdutos()
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

            var obj1 = JsonConvert.SerializeObject(listaProdutoOriginal);
            var obj2 = JsonConvert.SerializeObject(resultadoGetAll);

            Assert.Equal(obj1, obj2);
        }

        [Fact] 
        public async void AtualizarProduto_Deve_Ser_Igual_ao_Objeto_Atualizado()
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

            var produtoAtualizado = new Produto
            {
                Nome = "Notebook Gamer Atualizado",
                Descricao = "Notebook mais potente para jogos e aplicações pesadas.",
                Preco = 6499.99,
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
            var produtoPreAlterocoes = await produtoRepository.GetById(1);
            var produtoPosAlteracoes = await produtoRepository.Put(produtoAtualizado, 1);

            //Assert 
            Assert.NotNull(produtoPosAlteracoes);

            var obj1 = JsonConvert.SerializeObject(produtoPosAlteracoes);
            var obj2 = JsonConvert.SerializeObject(produtoAtualizado);
            var obj3 = JsonConvert.SerializeObject(produtoPreAlterocoes);
            var obj4 = JsonConvert.SerializeObject(produtoOriginal);

            Assert.Equal(obj1, obj2);
            Assert.Equal(obj3, obj4);
        }

        [Fact]
        public async void DeletaProduto_Deve_Retornar_True_E_Produto_Nao_Existir_Mais()
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
            Produto? produtoDeletado = await produtoRepository.GetById(1);

            //Assert 
            Assert.True(deletouComSucesso);
            Assert.Null(produtoDeletado);
        }
    }
}

