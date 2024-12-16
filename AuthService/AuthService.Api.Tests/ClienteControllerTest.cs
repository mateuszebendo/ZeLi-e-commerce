using AuthService.Api.Controllers;
using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Api.Tests;

public class ClienteControllerTest
{
    private readonly Mock<IClienteService> _mockService;
    private readonly ClienteController _clienteController;
    private readonly IMapper _mapper;

    public ClienteControllerTest()
    {
        _mockService = new Mock<IClienteService>();
        _clienteController = new ClienteController(_mockService.Object, _mapper);
    }
    
    [Fact]
    public async Task RegistrarCliente_QuandoDadosValidos_RetornaCreatedStatusCode()
    {
        //Arrange
        var clienteCreateDto = new ClienteCreateDto("Zezin", "zezin@gmail.com", "Password1!");
        var clienteDto = new ClienteDto { ClienteId = 1, Nome = "Zezin", Email = "zezin@gmail.com" };

        _mockService.Setup(s => s.CreateClienteAsync(clienteCreateDto))
                   .ReturnsAsync(clienteDto);

        //Act
        var result = await _clienteController.RegistrarCliente(clienteCreateDto);

        //Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(1, ((ClienteDto)createdResult.Value).ClienteId);
        _mockService.Verify(s => s.CreateClienteAsync(clienteCreateDto), Times.Once);
    }

    [Fact]
    public async Task RegistrarCliente_QuandoDadosInvalidos_RetornaBadRequest()
    {
        //Arrange
        var clienteCreateDto = new ClienteCreateDto ("","email_invalido","123" );
        _clienteController.ModelState.AddModelError("nome", "Nome é obrigatório");

        //Act
        var result = await _clienteController.RegistrarCliente(clienteCreateDto);

        //Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.IsType<SerializableError>(badRequestResult.Value);
        _mockService.Verify(s => s.CreateClienteAsync(It.IsAny<ClienteCreateDto>()), Times.Never);
    }

    [Fact]
    public async Task GetAllClientes_QuandoHaClientesCadastrados_RetornaOK200()
    {
        //Arrange
        var clientes = new List<ClienteDto>
        {
            new ClienteDto { ClienteId = 1, Nome = "Carlin", Email = "carlin@gmail.com" },
            new ClienteDto { ClienteId = 2, Nome = "Claudio", Email = "claudin@gmail.com" }
        };

        _mockService.Setup(s => s.GetAllClientesAsync()).ReturnsAsync(clientes);

        //Act

        var result = await _clienteController.GetAllClientes();

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var clientesRetornados = Assert.IsType<List<ClienteDto>>(okResult.Value);
        Assert.Equal(2, clientesRetornados.Count);

        _mockService.Verify(s => s.GetAllClientesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllClientes_QuandoNaoHaClientesCadastrados_RetornaNotFound404()
    {
        //Arrange
        _mockService.Setup(s => s.GetAllClientesAsync()).ReturnsAsync(new List<ClienteDto>());

        //Act
        var result = await _clienteController.GetAllClientes();

        //Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);

        _mockService.Verify(s => s.GetAllClientesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetClienteById_QuandoIdValido_RetornaOkResult()
    {
        //Arrange
        var clienteId = 1;
        var clienteDto = new ClienteDto { ClienteId = clienteId, Nome = "Markin" };

        _mockService.Setup(s => s.GetClienteByIdAsync(clienteId))
                    .ReturnsAsync(clienteDto);

        //Act
        var result = await _clienteController.GetClienteById(clienteId);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var clienteRetornado = Assert.IsType<ClienteDto>(okResult.Value);
        Assert.Equal(clienteId, clienteRetornado.ClienteId);

        _mockService.Verify(s => s.GetClienteByIdAsync(clienteId), Times.Once);
    }

    [Fact]
    public async Task GetClienteById_QuandoIdInvalido_RetornaNotFound()
    {
        //Arrange
        var clienteIdInvalido = 999;

        _mockService.Setup(s => s.GetClienteByIdAsync(clienteIdInvalido))
                    .ReturnsAsync((ClienteDto)null);

        //Act
        var result = await _clienteController.GetClienteById(clienteIdInvalido);

        //Assert
        Assert.IsType<NotFoundResult>(result);

        _mockService.Verify(s => s.GetClienteByIdAsync(clienteIdInvalido), Times.Once);
    }

    [Fact]
    public async Task UpdateCliente_QuandoDadosEIdValidos_RetornaOk()
    {
        //Arrange
        var idValido = 1;
        var clienteUpdateDto = new ClienteUpdateDto("Novo Nome", "novoemail@gmail.com");
        var clienteAtualizado = new ClienteDto { ClienteId = idValido, Nome = "Novo Nome", Email = "novoemail@gmail.com" };

        _mockService.Setup(s => s.UpdateCliente(clienteUpdateDto, idValido)).ReturnsAsync(clienteAtualizado);

        //Act
        var result = await _clienteController.UpdateCliente(idValido, clienteUpdateDto);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var clienteResult = Assert.IsType<ClienteDto>(okResult.Value);

        Assert.Equal(clienteAtualizado.Nome, clienteResult.Nome);
        Assert.Equal(clienteAtualizado.Email, clienteResult.Email);

        _mockService.Verify(s => s.UpdateCliente(clienteUpdateDto, idValido), Times.Once);
    }

    [Fact]
    public async Task UpdateCliente_QuandoIdInvalido_RetornaNotFound()
    {
        //Arrange
        var idInvalido = 999;
        var clienteUpdateDto = new ClienteUpdateDto("Marcos", "marcos@gmail.com");

        _mockService.Setup(s => s.UpdateCliente(clienteUpdateDto, idInvalido))
                    .ThrowsAsync(new KeyNotFoundException($"Nenhum cliente encontrado com o Id {idInvalido}."));

        //Act
        var result = await _clienteController.UpdateCliente(idInvalido, clienteUpdateDto);

        //Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal($"Nenhum cliente encontrado com o Id {idInvalido}.", notFoundResult.Value);

        _mockService.Verify(s => s.UpdateCliente(clienteUpdateDto, idInvalido), Times.Once);
    }

    [Fact]
    public async Task UpdateCliente_QuandoDadosInvalidos_RetornaBadRequest()
    {
        //Arrange
        var idValido = 1;
        var clienteUpdateDto = new ClienteUpdateDto("", "email_invalido");

        _mockService.Setup(s => s.UpdateCliente(clienteUpdateDto, idValido))
                    .ThrowsAsync(new ArgumentException("Dados inválidos para atualização do cliente."));

        //Act
        var result = await _clienteController.UpdateCliente(idValido, clienteUpdateDto);

        //Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Dados inválidos para atualização do cliente.", badRequestResult.Value);

        _mockService.Verify(s => s.UpdateCliente(clienteUpdateDto, idValido), Times.Once);
    }

    [Fact]
    public async Task UpdateCliente_QuandoModelStateInvalido_RetornaBadRequest()
    {
        //Arrange
        var idValido = 1;
        var clienteUpdateDto = new ClienteUpdateDto("Nome", "email@example.com");

  
        _clienteController.ModelState.AddModelError("Nome", "Nome é obrigatório");

        //Act
        var result = await _clienteController.UpdateCliente(idValido, clienteUpdateDto);

        //Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.IsType<SerializableError>(badRequestResult.Value);

        _mockService.Verify(s => s.UpdateCliente(It.IsAny<ClienteUpdateDto>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task UpdateSenha_QuandoRetornaFalse_RetornaBadRequest()
    {
        //Arrange
        var idValido = 1;
        var novaSenha = "NovaSenha1!";
        _mockService.Setup(s => s.UpdateSenha(idValido, novaSenha)).ReturnsAsync(false);

        //Act
        var result = await _clienteController.UpdateSenha(idValido, novaSenha);

        //Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Falha ao atualizar a senha.", badRequestResult.Value);
        _mockService.Verify(s => s.UpdateSenha(idValido, novaSenha), Times.Once);
    }

    [Fact]
    public async Task UpdateSenha_QuandoIdInvalido_RetornaNotFound()
    {
        //Arrange
        var idInvalido = 999;
        var novaSenha = "SenhaValida1!";
        _mockService.Setup(s => s.UpdateSenha(idInvalido, novaSenha))
                    .ThrowsAsync(new KeyNotFoundException($"Nenhum cliente encontrado com o Id {idInvalido}."));

        //Act
        var result = await _clienteController.UpdateSenha(idInvalido, novaSenha);

        //Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal($"Nenhum cliente encontrado com o Id {idInvalido}.", notFoundResult.Value);
        _mockService.Verify(s => s.UpdateSenha(idInvalido, novaSenha), Times.Once);
    }

    [Fact]
    public async Task UpdateSenha_QuandoDadosInvalidos_RetornaBadRequest()
    {
        //Arrange
        var idValido = 1;
        var novaSenha = "invalida";
        _mockService.Setup(s => s.UpdateSenha(idValido, novaSenha))
                    .ThrowsAsync(new ArgumentException("A senha é inválida."));

        //Act
        var result = await _clienteController.UpdateSenha(idValido, novaSenha);

        //Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("A senha é inválida.", badRequestResult.Value);
        _mockService.Verify(s => s.UpdateSenha(idValido, novaSenha), Times.Once);
    }

    [Fact]
    public async Task DeleteCliente_QuandoIdValido_RetornaOkComClienteDesativado()
    {
        // Arrange
        var idValido = 1;
        var clienteDesativado = new ClienteDto { ClienteId = idValido, Nome = "Fulano", Email = "fulano@example.com" };

        _mockService.Setup(s => s.DeleteCliente(idValido)).ReturnsAsync(clienteDesativado);

        // Act
        var result = await _clienteController.DeleteCliente(idValido);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var clienteResult = Assert.IsType<ClienteDto>(okResult.Value);
        Assert.Equal(clienteDesativado.ClienteId, clienteResult.ClienteId);
        Assert.Equal(clienteDesativado.Nome, clienteResult.Nome);

        _mockService.Verify(s => s.DeleteCliente(idValido), Times.Once);
    }

    [Fact]
    public async Task DeleteCliente_QuandoIdInvalido_RetornaNotFound()
    {
        // Arrange
        var idInvalido = 999;
        _mockService.Setup(s => s.DeleteCliente(idInvalido))
                    .ThrowsAsync(new KeyNotFoundException($"Nenhum cliente encontrado com o Id {idInvalido}."));

        // Act
        var result = await _clienteController.DeleteCliente(idInvalido);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal($"Nenhum cliente encontrado com o Id {idInvalido}.", notFoundResult.Value);

        _mockService.Verify(s => s.DeleteCliente(idInvalido), Times.Once);
    }
}


