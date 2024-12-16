using AutoMapper;
using Moq;
using AutoFixture;
using Shouldly;
using ProductCatalogService.Application.Dtos;
using ProductCatalogService.Application.Profiles;
using ProductCatalogService.Application.Services;
using ProductCatalogService.Domain.Contracts;
using ProductCatalogService.Domain.Entities;
using ProductCatalogService.Application.Exceptions;

namespace ProductCatalogService.Application.Tests
{
    public class CategoriaServiceTest
    {
        private readonly IMapper _mapper; 

        public CategoriaServiceTest()
        {
            MapperConfiguration configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CategoriaProfile>();
            });

            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public async Task CriaCategoria_QuandoDadosValidos_RetornaDetailsCategoriaDto()
        {
            //Arrange 
            CreateCategoriaDto createCategoriaDto = new Fixture().Create<CreateCategoriaDto>();

            Categoria categoria = _mapper.Map<Categoria>(createCategoriaDto);

            Mock<ICategoriaRepository> mock = new Mock<ICategoriaRepository>();
            mock.Setup(repo => repo.AddAsync(It.IsAny<Categoria>()))
                .ReturnsAsync((Categoria c) =>
                {
                    c.CategoriaID = 1;
                    return c;
                });

            ICategoriaRepository repository = mock.Object;

            CategoriaService service = new CategoriaService(repository, _mapper);

            //Act
            DetailsCategoriaDto? detailsCategoriaDto = await service.RegisterNewCategoriaAsync(createCategoriaDto);

            //Assert 
            detailsCategoriaDto.ShouldNotBeNull();

            detailsCategoriaDto.Nome.ShouldBe(categoria.Nome);
            detailsCategoriaDto.Descricao.ShouldBe(categoria.Descricao);

            mock.Verify(repo => repo.AddAsync(It.IsAny<Categoria>()), Times.Once);
        }

        [Fact]
        public async Task CriaCategoria_QuandoDadosInvalidos_RetornaArgumentException()
        {
            //Arrange
            ICategoriaRepository repository = new Mock<ICategoriaRepository>().Object;
            CategoriaService service = new CategoriaService(repository, _mapper);

            //Act & Assert
            await Should.ThrowAsync<CategoriaInvalidaException>(async () =>
                await service.RegisterNewCategoriaAsync(null));
        }

        [Fact]
        public async Task ObtemCategoriaPorId_QuandoIdValido_RetornaReadCategoriaDto()
        {
            //Arrange
            Categoria categoria = new Fixture().Create<Categoria>();

            Mock<ICategoriaRepository> mock = new Mock<ICategoriaRepository>();
            mock.Setup(c => c.GetByIdAsync(categoria.CategoriaID)).ReturnsAsync(categoria);
            ICategoriaRepository repository = mock.Object;

            CategoriaService service = new CategoriaService(repository, _mapper);

            //Act
            DetailsCategoriaDto? detailsCategoriaDto = await service.GetCategoriaByIdAsync(categoria.CategoriaID);

            //Assert 
            detailsCategoriaDto.ShouldNotBeNull();

            detailsCategoriaDto.Nome.ShouldBe(categoria.Nome);
            detailsCategoriaDto.Descricao.ShouldBe(categoria.Descricao);

            mock.Verify(repo => repo.GetByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public async Task ObtemCategoriaPorId_QuandoIdInvalido_RetornaArgumentException()
        {
            //Arrange
            ICategoriaRepository repository = new Mock<ICategoriaRepository>().Object;

            CategoriaService service = new CategoriaService(repository, _mapper);

            //Act & Assert 
            await Should.ThrowAsync<ArgumentException>(async () =>
                await service.GetCategoriaByIdAsync(-1));
        }

        [Fact]
        public async Task ObtemTodasAsCategoriasAtivas_QuandoBemSucedido_RetornaTodasCategoriasAtivas()
        {
            //Arrange
            List<Categoria> categorias = new Fixture().CreateMany<Categoria>().ToList();

            Mock<ICategoriaRepository> mock = new Mock<ICategoriaRepository>();
            mock.Setup(r => r.GetAllAsync()).ReturnsAsync(categorias);
            ICategoriaRepository repository = mock.Object;

            CategoriaService service = new CategoriaService(repository, _mapper);

            //Act 
            List<DetailsCategoriaDto>? categoriasDtos = await service.GetAllCategoriasAtivasAsync();

            //Assert
            categoriasDtos.ShouldNotBeNull();
            categoriasDtos.Count.ShouldBe(categorias.Count);

            categoriasDtos = categoriasDtos.OrderBy(c => c.Nome).ToList();
            categorias = categorias.OrderBy(c => c.Nome).ToList();

            for(int i = 0; i < categorias.Count; i++)
            {
                categoriasDtos[i].Nome.ShouldBe(categorias[i].Nome);
                categoriasDtos[i].Descricao.ShouldBe(categorias[i].Descricao);
            }

            mock.Verify(repo => repo.GetAllAsync(), Times.Once());
        }

        [Fact] 
        public async Task AtualizaCategoria_QuandoDadosValidos_RetornaDetailsCategoriaDtoComDadosAtualizados()
        {
            //Arrange
            UpdateCategoriaDto updateCategoriaDto = new Fixture().Create<UpdateCategoriaDto>();

            Categoria categoria = _mapper.Map<Categoria>(updateCategoriaDto);
            categoria.CategoriaID = 1;

            Mock<ICategoriaRepository> mock = new Mock<ICategoriaRepository>();
            mock.Setup(r => r.UpdateAsync(It.IsAny<Categoria>(), categoria.CategoriaID)).ReturnsAsync(categoria);
            ICategoriaRepository repository = mock.Object;

            CategoriaService service = new CategoriaService(repository, _mapper);

            //Act 
            DetailsCategoriaDto? detailsCategoriaDto = await service.UpdateCategoriaAsync(updateCategoriaDto, categoria.CategoriaID);

            //Assert 
            detailsCategoriaDto.ShouldNotBeNull();

            detailsCategoriaDto.Id.ShouldBe(categoria.CategoriaID);
            detailsCategoriaDto.Nome.ShouldBe(categoria.Nome);
            detailsCategoriaDto.Descricao.ShouldBe(categoria.Descricao);

            mock.Verify(repo => repo.UpdateAsync(It.IsAny<Categoria>(), It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public async Task AtualizaCategoria_QuandoCategoriaInvalida_RetornaCategoriaInvalidaException()
        {
            //Arrange
            Mock<ICategoriaRepository> mock = new Mock<ICategoriaRepository>();
            CategoriaService service = new CategoriaService(mock.Object, _mapper);

            //Act & Arrange 
            await Should.ThrowAsync<CategoriaInvalidaException>(async () =>
            {
                await service.UpdateCategoriaAsync(null, It.IsAny<int>());
            });
        }

        [Fact]
        public async Task AtualizaCategoria_QuandoIdInvalido_RetornaArgumentException()
        {
            //Arrange
            Mock<ICategoriaRepository> mock = new Mock<ICategoriaRepository>();
            UpdateCategoriaDto categoria = new Fixture().Create<UpdateCategoriaDto>();
            CategoriaService service = new CategoriaService(mock.Object, _mapper);

            //Act & Arrange 
            await Should.ThrowAsync<ArgumentException>(async () =>
            {
                await service.UpdateCategoriaAsync(categoria, 0);
            });
        }

        [Fact]
        public async Task DesativaCategoria_QuandoIdValido_RetornaCategoriaDesativada()
        {
            //Arrange
            Categoria categoria = new Fixture().Create<Categoria>();

            Mock<ICategoriaRepository> mock = new Mock<ICategoriaRepository>();
            mock.Setup(r => r.RemoveAsync(categoria.CategoriaID)).ReturnsAsync(() =>
            {
                categoria.Ativo = false;
                return categoria;
            });
            ICategoriaRepository repository = mock.Object;

            CategoriaService service = new CategoriaService(repository, _mapper);

            //Act
            ReadCategoriaDto? readCategoriaDto = await service.DisableCategoriaByIdAsync(categoria.CategoriaID);

            //Arrange 
            readCategoriaDto.ShouldNotBeNull();

            readCategoriaDto.Nome.ShouldBe(categoria.Nome);
            readCategoriaDto.Descricao.ShouldBe(categoria.Descricao);

            mock.Verify(repo => repo.RemoveAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DesativaCategoria_QuandoIdInvalido_RetornaArgumentException()
        {
            //Arrange
            CategoriaService service = new CategoriaService(new Mock<ICategoriaRepository>().Object, _mapper);

            //Act & Arrange
            await Should.ThrowAsync<ArgumentException>(async () =>
            {
                await service.DisableCategoriaByIdAsync(-1);
            });
        }
    }
}
