using AutoFixture;
using AutoMapper;
using ProductCatalogService.Application.Contracts;
using Moq;
using ProductCatalogService.Application.Dtos;
using ProductCatalogService.Application.Mapping;
using ProductCatalogService.Api.Controllers;
using ProductCatalogService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Shouldly;

namespace ProductCatalogService.Api.Test
{
    public class CategoriaControllerTest
    {
        private readonly Mock<ICategoriaService> _mock;
        private readonly IMapper _mapper;

        public CategoriaControllerTest()
        {
            _mock = new Mock<ICategoriaService>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DomainToDtoMappingProfile>();
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task CriaCategoria_QuandoDadosValidos_Retorna201CreatedComDetailsCategoriaDto()
        {
            //Arrange
            DetailsCategoriaDto categoriaDetails = new Fixture().Create<DetailsCategoriaDto>();
            CreateCategoriaDto categoriaDto = _mapper.Map<CreateCategoriaDto>(categoriaDetails);

            _mock.Setup(s => s.RegisterNewCategoriaAsync(It.IsAny<CreateCategoriaDto>())).ReturnsAsync(categoriaDetails);

            CategoriasController controller = new(_mock.Object);

            //Act 
            ActionResult<DetailsCategoriaDto> result = await controller.PostCategoria(categoriaDto);

            //Assert 
            result.Result.ShouldBeOfType<CreatedAtActionResult>();

            var createdResult = result.Result as CreatedAtActionResult;

            createdResult.StatusCode.ShouldBe(201);

            createdResult.Value.ShouldBeEquivalentTo(categoriaDetails);

            _mock.Verify(s => s.RegisterNewCategoriaAsync(It.IsAny<CreateCategoriaDto>()), Times.Once);
        }

        [Fact]
        public async Task ObtemCategoriaPorId_QuandoIdValido_Retorna200OkComCategoria()
        {
            //Arrange
            DetailsCategoriaDto categoriaDto = new Fixture().Create<DetailsCategoriaDto>();

            _mock.Setup(s => s.GetCategoriaByIdAsync(It.IsAny<int>())).ReturnsAsync(categoriaDto);

            CategoriasController controller = new(_mock.Object);

            //Act 
            ActionResult<DetailsCategoriaDto> result = await controller.GetCategoriaById(categoriaDto.Id);

            //Assert
            result.Result.ShouldBeOfType<OkObjectResult>();

            var okResult = result.Result as OkObjectResult;

            okResult.StatusCode.ShouldBe(200);

            okResult.Value.ShouldBeEquivalentTo(categoriaDto);

            _mock.Verify(s => s.GetCategoriaByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task ObtemTodasAsCategoriasAtivas_QuandoBemSucedido_Retorna200OkComCategorias()
        {
            //Arrange
            List<DetailsCategoriaDto> categorias = new Fixture().CreateMany<DetailsCategoriaDto>().ToList();

            _mock.Setup(s => s.GetAllCategoriasAtivasAsync()).ReturnsAsync(categorias);

            CategoriasController controller = new(_mock.Object);

            //Act
            ActionResult<List<DetailsCategoriaDto>> result = await controller.GetAllCategorias();

            //Assert 
            result.Result.ShouldBeOfType<OkObjectResult>();

            var okResult = result.Result as OkObjectResult;

            okResult.StatusCode.ShouldBe(200);

            okResult.Value.ShouldBeEquivalentTo(categorias);

            _mock.Verify(s => s.GetAllCategoriasAtivasAsync(), Times.Once);
        }

        [Fact]
        public async Task AtualizaCategoria_QuandoDadosValidos_Retorna200OKComDetailsCategoriaAtualizada()
        {
            //Arrange
            Categoria categoria = new Fixture().Create<Categoria>();
            DetailsCategoriaDto detailsCategoria = _mapper.Map<DetailsCategoriaDto>(categoria);
            UpdateCategoriaDto categoriaDto = _mapper.Map<UpdateCategoriaDto>(categoria);

            _mock.Setup(s => s.UpdateCategoriaAsync(It.IsAny<UpdateCategoriaDto>(), It.IsAny<int>())).ReturnsAsync(detailsCategoria);

            CategoriasController controller = new(_mock.Object);

            //Act 
            ActionResult<DetailsCategoriaDto> result = await controller.PutCategoria(categoriaDto, categoria.CategoriaID);

            //Assert
            result.Result.ShouldBeOfType<OkObjectResult>();

            var okResult = result.Result as OkObjectResult;

            okResult.StatusCode.ShouldBe(200);

            okResult.Value.ShouldBeEquivalentTo(detailsCategoria);

            _mock.Verify(s => s.UpdateCategoriaAsync(It.IsAny<UpdateCategoriaDto>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DesativaCategoria_QuandoIdValido_Retorna200OkComCategoriaDesativada()
        {
            //Arrange
            Categoria categoria = new Fixture().Create<Categoria>();
            ReadCategoriaDto readCategoria = _mapper.Map<ReadCategoriaDto>(categoria);

            _mock.Setup(s => s.DisableCategoriaByIdAsync(It.IsAny<int>())).ReturnsAsync(readCategoria);

            CategoriasController controller = new(_mock.Object);

            //Act 
            ActionResult<ReadCategoriaDto> result = await controller.RemoveCategoria(categoria.CategoriaID);

            //Assert
            result.Result.ShouldBeOfType<OkObjectResult>();

            var okResult = result.Result as OkObjectResult;

            okResult.StatusCode.ShouldBe(200);

            okResult.Value.ShouldBeEquivalentTo(readCategoria);

            _mock.Verify(s => s.DisableCategoriaByIdAsync(It.IsAny<int>()), Times.Once);
        }
    }
}
