﻿using AutoMapper;
using Moq;
using AutoFixture;
using Shouldly;
using ProductCatalogService.Application.Dtos;
using ProductCatalogService.Application.Profiles;
using ProductCatalogService.Application.Services;
using ProductCatalogService.Domain.Contracts;
using ProductCatalogService.Domain.Entities;

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
            mock.Setup(c => c.AddAsync(categoria)).ReturnsAsync(categoria);
            ICategoriaRepository repository = mock.Object;

            CategoriaService service = new CategoriaService(repository, _mapper);

            //Act
            DetailsCategoriaDto? detailsCategoriaDto = await service.RegisterNewCategoriaAsync(createCategoriaDto);

            //Assert 
            detailsCategoriaDto.ShouldNotBeNull();

            detailsCategoriaDto.Nome.ShouldBe(categoria.Nome);
            detailsCategoriaDto.Descricao.ShouldBe(categoria.Descricao);

            mock.Verify(repo => repo.AddAsync(categoria), Times.Once);
        }

        [Fact]
        public async Task CriaCategoria_QuandoDadosInvalidos_RetornaArgumentException()
        {
            //Arrange
            ICategoriaRepository repository = new Mock<ICategoriaRepository>().Object;
            CategoriaService service = new CategoriaService(repository, _mapper);

            //Act & Assert
            await Should.ThrowAsync<ArgumentException>(async () =>
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
            Categoria categoria = new Fixture().Create<Categoria>();

            UpdateCategoriaDto updateCategoriaDto = new Fixture().Create<UpdateCategoriaDto>();

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
        public async Task AtualizaCategoria_QuandoDadosInvalidos_RetornaArgumentException()
        {
            //Arrange
            Mock<ICategoriaRepository> mock = new Mock<ICategoriaRepository>();
            CategoriaService service = new CategoriaService(mock.Object, _mapper);

            //Act & Arrange 
            await Should.ThrowAsync<ArgumentException>(async () =>
            {
                await service.UpdateCategoriaAsync(null, 0);
            });
        }

        [Fact]
        public async Task DesativaCategoria_QuandoIdValido_RetornaCategoriaDesativada()
        {
            //Arrange
            Categoria categoria = new Fixture().Create<Categoria>();

            Mock<ICategoriaRepository> mock = new Mock<ICategoriaRepository>();
            mock.Setup(r => r.RemoveAsync(categoria.CategoriaID)).ReturnsAsync(true);
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
