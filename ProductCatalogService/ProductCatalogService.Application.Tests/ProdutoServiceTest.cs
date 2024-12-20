using AutoMapper;
using Moq;
using ProductCatalogService.Domain.Contracts;
using ProductCatalogService.Domain.Entities;
using AutoFixture;
using ProductCatalogService.Application.Services;
using ProductCatalogService.Application.Dtos;
using Shouldly;
using ProductCatalogService.Application.Exceptions;
using ProductCatalogService.Application.Mapping;

namespace ProductCatalogService.Application.Tests
{
    public class ProdutoServiceTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IProdutoRepository> _mock; 

        public ProdutoServiceTest()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DomainToDtoMappingProfile>();
            });
            _mapper = configuration.CreateMapper();

            _mock = new Mock<IProdutoRepository>();
        }

        [Fact]
        public async Task CriaProduto_QuandoDadosValidos_RetornaDetailsProdutoDto()
        {
            //Arrange
            Produto produto = new Fixture().Build<Produto>().With(p => p.ProdutoID, 1).Create();
            CreateProdutoDto createProdutoDto = _mapper.Map<CreateProdutoDto>(produto);
            _mock.Setup(repo => repo.AddAsync(It.IsAny<Produto>())).ReturnsAsync(produto);
            ProdutoService service = new(_mapper, _mock.Object);

            //Act 
            var produtoDto = await service.RegisterNewProdutoAsync(createProdutoDto);

            //Assert 
            produtoDto.ShouldNotBeNull();
            produtoDto.Id.ShouldBe(produto.ProdutoID);
            produtoDto.Nome.ShouldBe(produto.Nome);
            produtoDto.Descricao.ShouldBe(produto.Descricao);
            produtoDto.Preco.ShouldBe(produto.Preco);
            produtoDto.Estoque.ShouldBe(produto.Estoque);
            produtoDto.ImagemURL.ShouldBe(produto.ImagemURL);
            produtoDto.Categoria.Id.ShouldBe(produto.Categoria.CategoriaID);
            produtoDto.Categoria.Nome.ShouldBe(produto.Categoria.Nome);
            produtoDto.Categoria.Descricao.ShouldBe(produto.Categoria.Descricao);

            _mock.Verify(repo => repo.AddAsync(It.IsAny<Produto>()), Times.Once);
        }

        [Fact]
        public async Task CriaProduto_QuandoDadosInvalidos_RetornaProdutoInvalidoException()
        {
            //Arrange
            ProdutoService service = new(_mapper, _mock.Object);

            //Act & Arrange
            await Should.ThrowAsync<ProdutoInvalidoException>(async () =>
            {
                await service.RegisterNewProdutoAsync(null);
            });
        }

        [Fact]
        public async Task ObtemProdutoPorId_QuandoIdValido_RetornaProduto()
        {
            //Arrange
            int id = 1;
            Produto produto = new Fixture()
                .Build<Produto>()
                .With(p => p.ProdutoID, id)
                .With(p => p.Ativo, true)
                .Create();

            _mock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(produto);

            ProdutoService service = new(_mapper, _mock.Object);

            //Act 
            DetailsProdutoDto produtoDto = await service.GetProdutoByIdAsync(id);

            //Assert
            produtoDto.ShouldNotBeNull();
            produtoDto.Id.ShouldBe(produto.ProdutoID);
            produtoDto.Nome.ShouldBe(produto.Nome);
            produtoDto.Descricao.ShouldBe(produto.Descricao);
            produtoDto.Preco.ShouldBe(produto.Preco);
            produtoDto.Estoque.ShouldBe(produto.Estoque);
            produtoDto.ImagemURL.ShouldBe(produto.ImagemURL);
            produtoDto.Categoria.Id.ShouldBe(produto.Categoria.CategoriaID);
            produtoDto.Categoria.Nome.ShouldBe(produto.Categoria.Nome);
            produtoDto.Categoria.Descricao.ShouldBe(produto.Categoria.Descricao);

            _mock.Verify(repo => repo.GetByIdAsync(id), Times.Once());
        }

        [Fact]
        public async Task ObtemProdutoPorId_QuandoIdInvalido_RetornaArgumentException()
        {
            //Arrange
            ProdutoService service = new(_mapper, _mock.Object);

            //Act & Assert
            await Should.ThrowAsync<ArgumentException>(async () =>
            {
                await service.GetProdutoByIdAsync(0);
            });
        }

        [Fact]
        public async Task ObtemTodosOsProdutosAtivos_QuandoBemSucedido_RetornaTodosOsProdutosAtivos()
        {
            //Arrange
            List<Produto> produtos = new Fixture()
                .Build<Produto>()
                .With(p => p.Ativo, true)
                .CreateMany(4)
                .ToList();

            _mock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(produtos);

            ProdutoService service = new(_mapper, _mock.Object);

            //Act
            List<DetailsProdutoDto>? produtosDtos = await service.GetAllProdutosAtivosAsync();

            //Assert 
            produtosDtos.ShouldNotBeNull();
            produtosDtos.Count.ShouldBe(produtos.Count);

            for(int i = 0; i < produtosDtos.Count; i++)
            {
                produtosDtos[i].Id.ShouldBe(produtos[i].ProdutoID);
                produtosDtos[i].Nome.ShouldBe(produtos[i].Nome);
                produtosDtos[i].Descricao.ShouldBe(produtos[i].Descricao);
                produtosDtos[i].Preco.ShouldBe(produtos[i].Preco);
                produtosDtos[i].Estoque.ShouldBe(produtos[i].Estoque);
                produtosDtos[i].ImagemURL.ShouldBe(produtos[i].ImagemURL);
                produtosDtos[i].Categoria.Id.ShouldBe(produtos[i].Categoria.CategoriaID);
                produtosDtos[i].Categoria.Nome.ShouldBe(produtos[i].Categoria.Nome);
                produtosDtos[i].Categoria.Descricao.ShouldBe(produtos[i].Categoria.Descricao);
            }

            _mock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task AtualizaProduto_QuandoDadosValidos_RetornaDetailsProdutoDto()
        {
            //Arrange
            Produto produto = new Fixture()
                .Build<Produto>()
                .With(p => p.ProdutoID, 1)
                .Create();
            UpdateProdutoDto produtoUpdateDto = _mapper.Map<UpdateProdutoDto>(produto);

            _mock.Setup(repo => repo.UpdateAsync(It.IsAny<Produto>(), It.IsAny<int>())).ReturnsAsync(produto);

            ProdutoService service = new(_mapper, _mock.Object);

            //Act
            DetailsProdutoDto? produtoDto = await service.UpdateProdutoAsync(produtoUpdateDto, produto.ProdutoID);

            //Assert
            produtoDto.ShouldNotBeNull();
            produtoDto.Id.ShouldBe(produto.ProdutoID);
            produtoDto.Nome.ShouldBe(produto.Nome);
            produtoDto.Descricao.ShouldBe(produto.Descricao);
            produtoDto.Preco.ShouldBe(produto.Preco);
            produtoDto.Estoque.ShouldBe(produto.Estoque);
            produtoDto.ImagemURL.ShouldBe(produto.ImagemURL);
            produtoDto.Categoria.Id.ShouldBe(produto.Categoria.CategoriaID);
            produtoDto.Categoria.Nome.ShouldBe(produto.Categoria.Nome);
            produtoDto.Categoria.Descricao.ShouldBe(produto.Categoria.Descricao);

            _mock.Verify(repo => repo.UpdateAsync(It.IsAny<Produto>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task AtualizaProduto_QuandoProdutoInvalido_RetornaProdutoInvalidoException()
        {
            //Arrange
            ProdutoService service = new(_mapper, _mock.Object);

            //Act & Arrange
            await Should.ThrowAsync<ProdutoInvalidoException>(async () =>
            {
                await service.UpdateProdutoAsync(null, 1);
            });
        }

        [Fact]
        public async Task AtualizaProduto_QuandoIdInvalido_RetornaArgumentException()
        {
            //Arrange
            UpdateProdutoDto produtoDto = new Fixture().Create<UpdateProdutoDto>();
            ProdutoService service = new(_mapper, _mock.Object);

            //Act & Arrange
            await Should.ThrowAsync<ArgumentException>(async () =>
            {
                await service.UpdateProdutoAsync(produtoDto, 0);
            });
        }

        [Fact]
        public async Task DesativaProduto_QuandoIdValido_RetornaReadProdutoDto()
        {
            //Arrange 
            Produto produto = new Fixture().Create<Produto>();

            _mock.Setup(repo => repo.RemoveAsync(It.IsAny<int>())).ReturnsAsync(() =>
            {
                produto.Ativo = false;
                return produto;
            });
            ProdutoService service = new(_mapper, _mock.Object);

            //Act 
            ReadProdutoDto produtoDto = await service.DisableProdutoByIdAsync(1);

            //Assert
            produtoDto.ShouldNotBeNull();
            produtoDto.Nome.ShouldBe(produto.Nome);
            produtoDto.Descricao.ShouldBe(produto.Descricao);
            produtoDto.Preco.ShouldBe(produto.Preco);
            produtoDto.Estoque.ShouldBe(produto.Estoque);
            produtoDto.ImagemURL.ShouldBe(produto.ImagemURL);
            produtoDto.Categoria.Nome.ShouldBe(produto.Categoria.Nome);
            produtoDto.Categoria.Descricao.ShouldBe(produto.Categoria.Descricao);

            _mock.Verify(repo => repo.RemoveAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DesativaProduto_QuandoIdInvalido_RetornaArgumentException()
        {
            //Arrange
            ProdutoService service = new(_mapper, _mock.Object);

            //Act & Arrange
            await Should.ThrowAsync<ArgumentException>(async () =>
            {
                await service.DisableProdutoByIdAsync(0);
            });
        }
    }
}
