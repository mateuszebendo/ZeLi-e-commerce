using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UsuarioService.Api.Controllers;
using UsuarioService.Application.DTOs;
using UsuarioService.Application.Interfaces;

namespace UsuarioService.Presentation.Test;

[ApiController]
[Route("[controller]")]
public class UsuarioControllerTest : ControllerBase
{
    private readonly Mock<IUsuarioService> _mockService;
    private readonly UsuarioController _clienteController;
    private readonly IMapper _mapper;

    public UsuarioControllerTest()
    {
        _mockService = new Mock<IUsuarioService>();
        _clienteController = new UsuarioController(_mockService.Object, _mapper);
    }

    [Fact]
    public async Task RegistrarUsuario_QuandoDadosValidos_RetornaCreatedStatusCode()
    {
        //Arrange
        var clienteCreateDto = new UsuarioCreateDto("Zezin", "zezin@gmail.com", "Password1!");
        var clienteDto = new UsuarioDto { Id = 1, Nome = "Zezin", Email = "zezin@gmail.com" };

        _mockService.Setup(s => s.CreateUsuarioAsync(clienteCreateDto))
                   .ReturnsAsync(clienteDto);

        //Act
        var result = await _clienteController.RegistrarUsuario(clienteCreateDto);

        //Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(1, ((UsuarioDto)createdResult.Value).Id);
        _mockService.Verify(s => s.CreateUsuarioAsync(clienteCreateDto), Times.Once);
    }

    [Fact]
    public async Task RegistrarUsuario_QuandoDadosInvalidos_RetornaBadRequest()
    {
        //Arrange
        var clienteCreateDto = new UsuarioCreateDto("", "email_invalido", "123");
        _clienteController.ModelState.AddModelError("nome", "Nome é obrigatório");

        //Act
        var result = await _clienteController.RegistrarUsuario(clienteCreateDto);

        //Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.IsType<SerializableError>(badRequestResult.Value);
        _mockService.Verify(s => s.CreateUsuarioAsync(It.IsAny<UsuarioCreateDto>()), Times.Never);
    }

    [Fact]
    public async Task GetUsuarioById_QuandoIdValido_RetornaOkResult()
    {
        //Arrange
        var clienteId = 1;
        var clienteDto = new UsuarioDto { Id = clienteId, Nome = "Markin" };

        _mockService.Setup(s => s.GetUsuarioByIdAsync(clienteId))
                    .ReturnsAsync(clienteDto);

        //Act
        var result = await _clienteController.GetUsuarioById(clienteId);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var clienteRetornado = Assert.IsType<UsuarioDto>(okResult.Value);
        Assert.Equal(clienteId, clienteRetornado.Id);

        _mockService.Verify(s => s.GetUsuarioByIdAsync(clienteId), Times.Once);
    }

    [Fact]
    public async Task GetUsuarioById_QuandoIdInvalido_RetornaNotFound()
    {
        //Arrange
        var clienteIdInvalido = 999;

        _mockService.Setup(s => s.GetUsuarioByIdAsync(clienteIdInvalido))
                    .ReturnsAsync((UsuarioDto)null);

        //Act
        var result = await _clienteController.GetUsuarioById(clienteIdInvalido);

        //Assert
        Assert.IsType<NotFoundResult>(result);

        _mockService.Verify(s => s.GetUsuarioByIdAsync(clienteIdInvalido), Times.Once);
    }

    [Fact]
    public async Task UpdateUsuario_QuandoDadosEIdValidos_RetornaOk()
    {
        //Arrange
        var idValido = 1;
        var clienteUpdateDto = new UsuarioUpdateDto("Novo Nome", "novoemail@gmail.com");
        var clienteAtualizado = new UsuarioDto { Id = idValido, Nome = "Novo Nome", Email = "novoemail@gmail.com" };

        _mockService.Setup(s => s.UpdateUsuarioAsync(clienteUpdateDto, idValido)).ReturnsAsync(clienteAtualizado);

        //Act
        var result = await _clienteController.UpdateUsuario(idValido, clienteUpdateDto);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var clienteResult = Assert.IsType<UsuarioDto>(okResult.Value);

        Assert.Equal(clienteAtualizado.Nome, clienteResult.Nome);
        Assert.Equal(clienteAtualizado.Email, clienteResult.Email);

        _mockService.Verify(s => s.UpdateUsuarioAsync(clienteUpdateDto, idValido), Times.Once);
    }

    [Fact]
    public async Task UpdateUsuario_QuandoIdInvalido_RetornaNotFound()
    {
        //Arrange
        var idInvalido = 999;
        var clienteUpdateDto = new UsuarioUpdateDto("Marcos", "marcos@gmail.com");

        _mockService.Setup(s => s.UpdateUsuarioAsync(clienteUpdateDto, idInvalido))
                    .ThrowsAsync(new KeyNotFoundException($"Nenhum cliente encontrado com o Id {idInvalido}."));

        //Act
        var result = await _clienteController.UpdateUsuario(idInvalido, clienteUpdateDto);

        //Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal($"Nenhum cliente encontrado com o Id {idInvalido}.", notFoundResult.Value);

        _mockService.Verify(s => s.UpdateUsuarioAsync(clienteUpdateDto, idInvalido), Times.Once);
    }

    [Fact]
    public async Task UpdateUsuario_QuandoDadosInvalidos_RetornaBadRequest()
    {
        //Arrange
        var idValido = 1;
        var clienteUpdateDto = new UsuarioUpdateDto("", "email_invalido");

        _mockService.Setup(s => s.UpdateUsuarioAsync(clienteUpdateDto, idValido))
                    .ThrowsAsync(new ArgumentException("Dados inválidos para atualização do cliente."));

        //Act
        var result = await _clienteController.UpdateUsuario(idValido, clienteUpdateDto);

        //Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Dados inválidos para atualização do cliente.", badRequestResult.Value);

        _mockService.Verify(s => s.UpdateUsuarioAsync(clienteUpdateDto, idValido), Times.Once);
    }

    [Fact]
    public async Task UpdateUsuario_QuandoModelStateInvalido_RetornaBadRequest()
    {
        //Arrange
        var idValido = 1;
        var clienteUpdateDto = new UsuarioUpdateDto("Nome", "email@example.com");


        _clienteController.ModelState.AddModelError("Nome", "Nome é obrigatório");

        //Act
        var result = await _clienteController.UpdateUsuario(idValido, clienteUpdateDto);

        //Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.IsType<SerializableError>(badRequestResult.Value);

        _mockService.Verify(s => s.UpdateUsuarioAsync(It.IsAny<UsuarioUpdateDto>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task UpdateSenha_QuandoRetornaFalse_RetornaBadRequest()
    {
        //Arrange
        var idValido = 1;
        var novaSenha = "NovaSenha1!";
        _mockService.Setup(s => s.UpdateSenhaAsync(idValido, novaSenha)).ReturnsAsync(false);

        //Act
        var result = await _clienteController.UpdateSenha(idValido, novaSenha);

        //Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Falha ao atualizar a senha.", badRequestResult.Value);
        _mockService.Verify(s => s.UpdateSenhaAsync(idValido, novaSenha), Times.Once);
    }

    [Fact]
    public async Task UpdateSenha_QuandoIdInvalido_RetornaNotFound()
    {
        //Arrange
        var idInvalido = 999;
        var novaSenha = "SenhaValida1!";
        _mockService.Setup(s => s.UpdateSenhaAsync(idInvalido, novaSenha))
                    .ThrowsAsync(new KeyNotFoundException($"Nenhum cliente encontrado com o Id {idInvalido}."));

        //Act
        var result = await _clienteController.UpdateSenha(idInvalido, novaSenha);

        //Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal($"Nenhum cliente encontrado com o Id {idInvalido}.", notFoundResult.Value);
        _mockService.Verify(s => s.UpdateSenhaAsync(idInvalido, novaSenha), Times.Once);
    }

    [Fact]
    public async Task UpdateSenha_QuandoDadosInvalidos_RetornaBadRequest()
    {
        //Arrange
        var idValido = 1;
        var novaSenha = "invalida";
        _mockService.Setup(s => s.UpdateSenhaAsync(idValido, novaSenha))
                    .ThrowsAsync(new ArgumentException("A senha é inválida."));

        //Act
        var result = await _clienteController.UpdateSenha(idValido, novaSenha);

        //Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("A senha é inválida.", badRequestResult.Value);
        _mockService.Verify(s => s.UpdateSenhaAsync(idValido, novaSenha), Times.Once);
    }

    [Fact]
    public async Task DeleteUsuario_QuandoIdValido_RetornaOkComUsuarioDesativado()
    {
        // Arrange
        var idValido = 1;
        var clienteDesativado = new UsuarioDto { Id = idValido, Nome = "Fulano", Email = "fulano@example.com" };

        _mockService.Setup(s => s.DeleteUsuarioAsync(idValido)).ReturnsAsync(clienteDesativado);

        // Act
        var result = await _clienteController.DeleteUsuario(idValido);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var clienteResult = Assert.IsType<UsuarioDto>(okResult.Value);
        Assert.Equal(clienteDesativado.Id, clienteResult.Id);
        Assert.Equal(clienteDesativado.Nome, clienteResult.Nome);

        _mockService.Verify(s => s.DeleteUsuarioAsync(idValido), Times.Once);
    }

    [Fact]
    public async Task DeleteUsuario_QuandoIdInvalido_RetornaNotFound()
    {
        // Arrange
        var idInvalido = 999;
        _mockService.Setup(s => s.DeleteUsuarioAsync(idInvalido))
                    .ThrowsAsync(new KeyNotFoundException($"Nenhum cliente encontrado com o Id {idInvalido}."));

        // Act
        var result = await _clienteController.DeleteUsuario(idInvalido);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal($"Nenhum cliente encontrado com o Id {idInvalido}.", notFoundResult.Value);

        _mockService.Verify(s => s.DeleteUsuarioAsync(idInvalido), Times.Once);
    }
}