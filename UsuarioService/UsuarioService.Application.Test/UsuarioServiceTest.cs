using AutoMapper;
using Moq;
using UsuarioService.Application.DTOs;
using UsuarioService.Application.Mappings;
using UsuarioService.Domain.Entities;
using UsuarioService.Domain.Interfaces;
using UsuarioService.Application.Services;

namespace UsuarioService.Application.Test;

public class UsuarioServiceTest
{
    private readonly IMapper _mapper;
    private readonly MapperConfiguration _configuration;

    public UsuarioServiceTest()
    {
        _configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<DomainToDTOMappingProfile>();
        });

        _mapper = _configuration.CreateMapper();
    }

    [Fact]
    public async void Criar_Novo_Usuario_Dados_Corretos_Deve_Retornar_Usuario()
    {
        //Arrange
        var usuarioCreateDto = new UsuarioCreateDto("JAO Gomes", "jaozin123@gmail.com", "Password1!");

        var usuarioEntity = _mapper.Map<Usuario>(usuarioCreateDto);
        var mockRepository = new Moq.Mock<IUsuarioRepository>();

        mockRepository.Setup(s => s.CreateUsuarioAsync(usuarioEntity)).ReturnsAsync(usuarioEntity);
        var service = new Services.UsuarioService(mockRepository.Object, _mapper);

        //Act
        var usuarioToCreate = await service.CreateUsuarioAsync(usuarioCreateDto);

        //Assert
        Assert.NotNull(usuarioToCreate);
        Assert.Equal(usuarioCreateDto.Nome, usuarioToCreate.Nome);
        Assert.Equal(usuarioCreateDto.Email, usuarioToCreate.Email);

        mockRepository.Verify(r => r.CreateUsuarioAsync(
            It.Is<Usuario>(c => c.Email == usuarioEntity.Email && c.Nome == usuarioEntity.Nome && c.Senha == usuarioEntity.Senha)), Times.Once);
    }

    [Fact]
    public async Task Criar_Novo_Usuario_Com_Email_Existente_Deve_Falhar_Retornar_Excecao()
    {
        //Arrange
        var usuarioDto = new UsuarioCreateDto("Claudin Borracheiro", "claudin@gmail.com", "Password1!");
        var usuarioEntity = _mapper.Map<Usuario>(usuarioDto);

        var mockRepository = new Mock<IUsuarioRepository>();
        var mockMapper = new Mock<IMapper>();

        mockRepository.Setup(s => s.GetUsuarioByEmailAsync(usuarioDto.Email)).ReturnsAsync(usuarioEntity);

        var service = new Services.UsuarioService(mockRepository.Object, mockMapper.Object);

        //Act e Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateUsuarioAsync(usuarioDto));
        Assert.Equal("Já existe um usuario com este email.", exception.Message);

        mockRepository.Verify(v => v.CreateUsuarioAsync(It.IsAny<Usuario>()), Times.Never);
    }

    

    [Fact]
    public async Task ObtemUsuarioPeloId_QuandoIdValido_RetornaUsuario()
    {
        //Arrange
        var usuario = new Usuario("Bonafe", "bonafe@email.com", "Password1!");
        usuario.Id = 1;

        var mockRepository = new Moq.Mock<IUsuarioRepository>();
        mockRepository.Setup(s => s.GetUsuarioByIdAsync(1)).ReturnsAsync(usuario);
        var service = new Services.UsuarioService(mockRepository.Object, _mapper);

        //Act
        var usuarioDto = await service.GetUsuarioByIdAsync(1);

        //Assert
        Assert.NotNull(usuarioDto);

        mockRepository.Verify(v => v.GetUsuarioByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task ObtemUsuarioPeloId_QuandoIdNaoValido_RetornaExcecao()
    {
        //Arrange       
        var mockRepository = new Mock<IUsuarioRepository>();
        mockRepository.Setup(s => s.GetUsuarioByIdAsync(1)).ReturnsAsync((Usuario)null);
        var service = new Services.UsuarioService(mockRepository.Object, _mapper);

        //Act e Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetUsuarioByIdAsync(1));

        mockRepository.Verify(v => v.GetUsuarioByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task ObtemUsuarioPeloEmail_QuandoEmailValido_RetornaUsuario()
    {
        //Arrange
        var usuario = new Usuario("Bonafe", "bonafe@email.com", "Password1!");

        var mockRepository = new Moq.Mock<IUsuarioRepository>();
        mockRepository.Setup(s => s.GetUsuarioByEmailAsync(usuario.Email)).ReturnsAsync(usuario);
        var service = new Services.UsuarioService(mockRepository.Object, _mapper);

        //Act
        var usuarioDto = await service.GetUsuarioByEmailAsync(usuario.Email);

        //Assert
        Assert.NotNull(usuarioDto);

        mockRepository.Verify(v => v.GetUsuarioByEmailAsync(usuarioDto.Email), Times.Once);
    }

    [Fact]
    public async Task ObtemUsuarioPeloEmail_QuandoEmailNaoValido_RetornaExcecao()
    {
        //Arrange
        var emailQualquer = "qualquer@email.com";

        var mockRepository = new Mock<IUsuarioRepository>();
        mockRepository.Setup(s => s.GetUsuarioByEmailAsync(emailQualquer)).ReturnsAsync((Usuario)null);
        var service = new Services.UsuarioService(mockRepository.Object, _mapper);

        //Act e Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetUsuarioByEmailAsync(emailQualquer));

        mockRepository.Verify(v => v.GetUsuarioByEmailAsync(emailQualquer), Times.Once);
    }

    [Fact]
    public async Task AtualizaDadosUsuario_QuandoDadosEIdValidos_RetornaUsuarioAtualizado()
    {
        // Arrange
        var usuario = new Usuario("Bonafe", "bonafe@email.com", "Password1!");
        usuario.Id = 1;

        var usuarioUpdateDto = new UsuarioUpdateDto("Lucas Bonafe", "bonafe@email.com");
        var usuarioAtualizado = new Usuario
        {
            Id = usuario.Id,
            Nome = usuarioUpdateDto.Nome,
            Email = usuarioUpdateDto.Email,
            Senha = usuario.Senha
        };

        var mockRepository = new Mock<IUsuarioRepository>();
        mockRepository.Setup(r => r.GetUsuarioByIdAsync(usuario.Id)).ReturnsAsync(usuario);
        mockRepository.Setup(r => r.UpdateUsuarioAsync(It.IsAny<Usuario>(), usuario.Id)).ReturnsAsync(usuarioAtualizado);

        var service = new Services.UsuarioService(mockRepository.Object, _mapper);

        // Act
        var usuarioDto = await service.UpdateUsuarioAsync(usuarioUpdateDto, usuario.Id);

        // Assert
        Assert.NotNull(usuarioDto);
        Assert.Equal(usuarioAtualizado.Nome, usuarioDto.Nome);
        Assert.Equal(usuarioAtualizado.Email, usuarioDto.Email);

        mockRepository.Verify(r => r.GetUsuarioByIdAsync(usuario.Id), Times.Once);
        mockRepository.Verify(r => r.UpdateUsuarioAsync(It.IsAny<Usuario>(), usuario.Id), Times.Once);
    }

    [Fact]
    public async Task AtualizaDadosUsuario_QuandoIdNaoValido_retornaExcecao()
    {
        // Arrange
        var usuarioUpdateDto = new UsuarioUpdateDto("Lucas Bonafe", "bonafe@email.com");
        var idInvalido = 999;

        var mockRepository = new Mock<IUsuarioRepository>();
        mockRepository.Setup(s => s.GetUsuarioByIdAsync(idInvalido)).ReturnsAsync((Usuario)null);

        var mockMapper = new Mock<IMapper>();
        var service = new Services.UsuarioService(mockRepository.Object, mockMapper.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => service.UpdateUsuarioAsync(usuarioUpdateDto, idInvalido));
        Assert.Equal($"Nenhum usuario encontrado com o Id {idInvalido}.", exception.Message);

        // Verifica que GetUsuarioByIdAsync foi chamado
        mockRepository.Verify(s => s.GetUsuarioByIdAsync(idInvalido), Times.Once);

        // Verifica que UpdateUsuarioAsync nunca foi chamado
        mockRepository.Verify(s => s.UpdateUsuarioAsync(It.IsAny<Usuario>(), idInvalido), Times.Never);
    }

    [Fact]
    public async Task AtualizaSenhaUsuario_QuandoSenhaEIdValidos_retornaTrue()
    {
        //Arrange
        var usuario = new Usuario("Bonafe", "bonafe@email.com", "Password1!");
        usuario.Id = 1;

        var novaSenha = "NewPassword1!";

        var mockRepository = new Mock<IUsuarioRepository>();
        mockRepository.Setup(s => s.GetUsuarioByIdAsync(usuario.Id)).ReturnsAsync(usuario);
        var service = new Services.UsuarioService(mockRepository.Object, _mapper);

        //Act
        var response = await service.UpdateSenhaAsync(usuario.Id, novaSenha);

        //Assert
        Assert.NotNull(novaSenha);
        Assert.Equal(usuario.Senha, novaSenha);
        Assert.True(response);
    }

    [Fact]
    public async Task AtualizaUsuario_QuandoIdInvalido_LancaExcecao()
    {
        // Arrange
        var usuarioUpdateDto = new UsuarioUpdateDto("Lucas Bonafe", "bonafe@email.com");
        var idInvalido = 999;

        var mockRepository = new Mock<IUsuarioRepository>();
        var mockMapper = new Mock<IMapper>();

        mockRepository.Setup(s => s.GetUsuarioByIdAsync(idInvalido)).ReturnsAsync((Usuario)null);

        var service = new Services.UsuarioService(mockRepository.Object, mockMapper.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => service.UpdateUsuarioAsync(usuarioUpdateDto, idInvalido));
        Assert.Equal($"Nenhum usuario encontrado com o Id {idInvalido}.", exception.Message);

        mockRepository.Verify(s => s.GetUsuarioByIdAsync(idInvalido), Times.Once);
        mockRepository.Verify(s => s.UpdateUsuarioAsync(It.IsAny<Usuario>(), idInvalido), Times.Never);
    }


    [Fact]
    public async Task UpdateSenha_QuandoSenhaValida_AtualizaComSucesso()
    {
        // Arrange
        var usuario = new Usuario("Bonafe", "bonafe@email.com", "Password1!");
        usuario.Id = 1;

        var novaSenha = "NovaSenha1!";

        var mockRepository = new Mock<IUsuarioRepository>();
        mockRepository.Setup(r => r.GetUsuarioByIdAsync(usuario.Id)).ReturnsAsync(usuario);

        var service = new Services.UsuarioService(mockRepository.Object, _mapper);

        // Act
        var result = await service.UpdateSenhaAsync(usuario.Id, novaSenha);

        // Assert
        Assert.True(result, "O método deve retornar true para uma senha válida.");

        mockRepository.Verify(r => r.GetUsuarioByIdAsync(usuario.Id), Times.Once);
    }

    [Fact]
    public async Task DesativaUsuario_QuandoIdValido_RetornaUsuarioDesativado()
    {
        //Arrange
        var usuario = new Usuario("Bonafe", "bonafe@email.com", "Password1!");
        usuario.Id = 1;

        var usuarioId = 1;

        var mockRepository = new Mock<IUsuarioRepository>();
        mockRepository.Setup(r => r.GetUsuarioByIdAsync(usuario.Id)).ReturnsAsync(usuario);

        var service = new Services.UsuarioService(mockRepository.Object, _mapper);

        //Act 
        var usuarioDesativado = await service.DeleteUsuarioAsync(usuarioId);

        //Assert
        Assert.NotNull(usuario);
        Assert.Null(usuarioDesativado);

        mockRepository.Verify(repo => repo.DeleteUsuarioAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task DesativaUsuario_QuandoIdInvalido_RetornaExcecao()
    {
        //Arrange
        var idInvalido = 999;
        var mockRepository = new Mock<IUsuarioRepository>();
        var service = new Services.UsuarioService(mockRepository.Object, _mapper);

        //Act e Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteUsuarioAsync(idInvalido));
        Assert.Equal($"Nenhum usuario encontrado com o Id {idInvalido}.", exception.Message);
    }
}