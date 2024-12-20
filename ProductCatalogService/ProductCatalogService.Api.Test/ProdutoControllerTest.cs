using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using ProductCatalogService.Api.Controllers;
using ProductCatalogService.Application.Contracts;
using ProductCatalogService.Application.Dtos;
using ProductCatalogService.Application.Mapping;
using ProductCatalogService.Domain.Entities;

namespace ProductCatalogService.Api.Test
{
    public class ProdutoControllerTest
    {
        private readonly Mock<IProdutoService> _mock;
        private readonly IMapper _mapper;

        public ProdutoControllerTest()
        {
            _mock = new Mock<IProdutoService>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DomainToDtoMappingProfile>();
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task CriaProduto_QuandoDadosValidos_Retorna201CreatedComDetailsProdutoDto()
        {
            //Arrange
            DetailsProdutoDto produtoDetails = new Fixture().Create<DetailsProdutoDto>();
            CreateProdutoDto produtoDto = _mapper.Map<CreateProdutoDto>(produtoDetails);

            _mock.Setup(s => s.RegisterNewProdutoAsync(It.IsAny<CreateProdutoDto>())).ReturnsAsync(produtoDetails);

            ProdutoController controller = new(_mock.Object);

            //Act 
            ActionResult<DetailsProdutoDto> result = await controller.PostProduto(produtoDto);

            //Assert 
            result.Result.ShouldBeOfType<CreatedAtActionResult>();

            var createdResult = result.Result as CreatedAtActionResult;

            createdResult.StatusCode.ShouldBe(StatusCodes.Status201Created);

            createdResult.Value.ShouldBeEquivalentTo(produtoDetails);

            _mock.Verify(s => s.RegisterNewProdutoAsync(It.IsAny<CreateProdutoDto>()), Times.Once);
        }

        [Fact]
        public async Task CriaProduto_QuandoDadosInvalidos_Retorna500BadRequest()
        {
            //Arrange 
            ProdutoController controller = new(_mock.Object);

            //Act 
            ActionResult<DetailsProdutoDto> result = await controller.PostProduto(null);

            //Assert 
            result.Result.ShouldBeOfType<BadRequestObjectResult>();

            var badRequestResult = result.Result as BadRequestObjectResult;

            badRequestResult.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task ObtemProdutoPorId_QuandoIdValido_Retorna200OkComProduto()
        {
            //Arrange
            DetailsProdutoDto produtoDto = new Fixture().Create<DetailsProdutoDto>();

            _mock.Setup(s => s.GetProdutoByIdAsync(It.IsAny<int>())).ReturnsAsync(produtoDto);

            ProdutoController controller = new(_mock.Object);

            //Act 
            ActionResult<DetailsProdutoDto> result = await controller.GetProdutoById(produtoDto.Id);

            //Assert
            result.Result.ShouldBeOfType<OkObjectResult>();

            var okResult = result.Result as OkObjectResult;

            okResult.StatusCode.ShouldBe(StatusCodes.Status200OK);

            okResult.Value.ShouldBeEquivalentTo(produtoDto);

            _mock.Verify(s => s.GetProdutoByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task ObtemProdutoPorId_QuandoProdutoInexistente_Retorna404NotFound()
        {
            //Arrange
            int idInvalido = 555;

            _mock.Setup(s => s.GetProdutoByIdAsync(idInvalido)).ReturnsAsync((DetailsProdutoDto?)null);

            ProdutoController controller = new(_mock.Object);

            //Act
            ActionResult<DetailsProdutoDto> result = await controller.GetProdutoById(idInvalido);

            //Assert
            result.Result.ShouldBeOfType<NotFoundResult>();

            var notFoundResult = result.Result as NotFoundResult;

            notFoundResult.StatusCode.ShouldBe(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task ObtemProdutoPorId_QuandoIdInvalido_Retorna500BadRequest()
        {
            //Arrange
            int idInvalido = -1;

            ProdutoController controller = new(_mock.Object);

            //Act
            ActionResult<DetailsProdutoDto> result = await controller.GetProdutoById(idInvalido);

            //Assert
            result.Result.ShouldBeOfType<BadRequestResult>();

            var notFoundResult = result.Result as BadRequestResult;

            notFoundResult.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task ObtemTodasAsProdutosAtivas_QuandoBemSucedido_Retorna200OkComProdutos()
        {
            //Arrange
            List<DetailsProdutoDto> produtos = new Fixture().CreateMany<DetailsProdutoDto>().ToList();

            _mock.Setup(s => s.GetAllProdutosAtivosAsync()).ReturnsAsync(produtos);

            ProdutoController controller = new(_mock.Object);

            //Act
            ActionResult<List<DetailsProdutoDto>> result = await controller.GetAllProdutos();

            //Assert 
            result.Result.ShouldBeOfType<OkObjectResult>();

            var okResult = result.Result as OkObjectResult;

            okResult.StatusCode.ShouldBe(StatusCodes.Status200OK);

            okResult.Value.ShouldBeEquivalentTo(produtos);

            _mock.Verify(s => s.GetAllProdutosAtivosAsync(), Times.Once);
        }

        [Fact]
        public async Task AtualizaProduto_QuandoDadosValidos_Retorna200OKComDetailsProdutoAtualizada()
        {
            //Arrange
            Produto produto = new Fixture().Create<Produto>();
            DetailsProdutoDto detailsProduto = _mapper.Map<DetailsProdutoDto>(produto);
            UpdateProdutoDto produtoDto = _mapper.Map<UpdateProdutoDto>(produto);

            _mock.Setup(s => s.UpdateProdutoAsync(It.IsAny<UpdateProdutoDto>(), It.IsAny<int>())).ReturnsAsync(detailsProduto);

            ProdutoController controller = new(_mock.Object);

            //Act 
            ActionResult<DetailsProdutoDto> result = await controller.PutProduto(produtoDto, produto.ProdutoID);

            //Assert
            result.Result.ShouldBeOfType<OkObjectResult>();

            var okResult = result.Result as OkObjectResult;

            okResult.StatusCode.ShouldBe(StatusCodes.Status200OK);

            okResult.Value.ShouldBeEquivalentTo(detailsProduto);

            _mock.Verify(s => s.UpdateProdutoAsync(It.IsAny<UpdateProdutoDto>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task AtualizaProduto_QuandoProdutoInvalida_Retorna400BadRequest()
        {
            //Arrange
            ProdutoController controller = new(_mock.Object);

            //Act 
            ActionResult<DetailsProdutoDto> result = await controller.PutProduto(null, 1);

            //Assert
            result.Result.ShouldBeOfType<BadRequestResult>();

            var badRequestResult = result.Result as BadRequestResult;

            badRequestResult.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task AtualizaProduto_QuandoIdInvalido_Retorna400BadRequest()
        {
            //Arrange
            int idInvalido = -1;
            UpdateProdutoDto produtoDto = new Fixture().Create<UpdateProdutoDto>();
            ProdutoController controller = new(_mock.Object);

            //Act 
            ActionResult<DetailsProdutoDto> result = await controller.PutProduto(produtoDto, idInvalido);

            //Assert
            result.Result.ShouldBeOfType<BadRequestResult>();

            var badRequestResult = result.Result as BadRequestResult;

            badRequestResult.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task AtualizaProduto_QuandoProdutoInexistente_Retorna404NotFound()
        {
            //Arrange
            int idInvalido = 555;
            UpdateProdutoDto produtoDto = new Fixture().Create<UpdateProdutoDto>();

            _mock.Setup(s => s.UpdateProdutoAsync(produtoDto, idInvalido)).ReturnsAsync((DetailsProdutoDto?)null);

            ProdutoController controller = new(_mock.Object);

            //Act
            ActionResult<DetailsProdutoDto> result = await controller.PutProduto(produtoDto, idInvalido);

            //Assert
            result.Result.ShouldBeOfType<NotFoundResult>();

            var notFoundResult = result.Result as NotFoundResult;

            notFoundResult.StatusCode.ShouldBe(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task DesativaProduto_QuandoIdValido_Retorna200OkComProdutoDesativada()
        {
            //Arrange
            Produto produto = new Fixture().Create<Produto>();
            ReadProdutoDto readProduto = _mapper.Map<ReadProdutoDto>(produto);

            _mock.Setup(s => s.DisableProdutoByIdAsync(It.IsAny<int>())).ReturnsAsync(readProduto);

            ProdutoController controller = new(_mock.Object);

            //Act 
            ActionResult<ReadProdutoDto> result = await controller.RemoveProduto(produto.ProdutoID);

            //Assert
            result.Result.ShouldBeOfType<OkObjectResult>();

            var okResult = result.Result as OkObjectResult;

            okResult.StatusCode.ShouldBe(StatusCodes.Status200OK);

            okResult.Value.ShouldBeEquivalentTo(readProduto);

            _mock.Verify(s => s.DisableProdutoByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DesativaProduto_QuandoIdInvalido_Retorna400BadRequest()
        {
            //Arrange
            int idInvalido = -1;

            ProdutoController controller = new(_mock.Object);

            //Act 
            ActionResult<ReadProdutoDto> result = await controller.RemoveProduto(idInvalido);

            //Assert 
            result.Result.ShouldBeOfType<BadRequestResult>();

            var badRequestResult = result.Result as BadRequestResult;

            badRequestResult.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task DesativaProduto_QuandoProdutoInexistente_Retorna404NotFound()
        {
            //Arrange 
            int idInvalido = 555;
            _mock.Setup(s => s.DisableProdutoByIdAsync(idInvalido)).ReturnsAsync((ReadProdutoDto?)null);
            ProdutoController controller = new(_mock.Object);

            //Act
            ActionResult<ReadProdutoDto> result = await controller.RemoveProduto(idInvalido);

            //Assert 
            result.Result.ShouldBeOfType<NotFoundResult>();

            var notFoundResult = result.Result as NotFoundResult;

            notFoundResult.StatusCode.ShouldBe(StatusCodes.Status404NotFound);
        }
    }
}
