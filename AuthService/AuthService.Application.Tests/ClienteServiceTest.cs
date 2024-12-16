using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Application.Mappings;
using AuthService.Application.Services;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AutoMapper;
using Moq;

namespace AuthService.Application.Tests;

public class ClienteServiceTest
{
    private readonly IMapper _mapper;
    private readonly MapperConfiguration _configuration;

    public ClienteServiceTest()
    {
        _configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<DomainToDTOMappingProfile>();
        });

        _mapper = _configuration.CreateMapper();
    }

    [Fact]
    public async void Criar_Novo_Cliente_Dados_Corretos_Deve_Retornar_Cliente()
    {
        //Arrange
        var clienteCreateDto = new ClienteCreateDto("JAO Gomes", "jaozin123@gmail.com", "Password1!");

        var clienteEntity = _mapper.Map<Cliente>(clienteCreateDto);
        var mockRepository = new Moq.Mock<IClienteRepository>();

        mockRepository.Setup(s => s.CreateClienteAsync(clienteEntity)).ReturnsAsync(clienteEntity);
        var service = new ClienteService(mockRepository.Object, _mapper);

        //Act
        var clienteToCreate = await service.CreateClienteAsync(clienteCreateDto);

        //Assert
        Assert.NotNull(clienteToCreate);
        Assert.Equal(clienteCreateDto.Nome, clienteToCreate.Nome);
        Assert.Equal(clienteCreateDto.Email, clienteToCreate.Email);

        mockRepository.Verify(r => r.CreateClienteAsync(
            It.Is<Cliente>(c => c.Email == clienteEntity.Email && c.Nome == clienteEntity.Nome && c.Senha == clienteEntity.Senha)), Times.Once);
    }

    [Fact]
    public async Task Criar_Novo_Cliente_Com_Email_Existente_Deve_Falhar_Retornar_Excecao()
    {
        //Arrange
        var clienteDto = new ClienteCreateDto("Claudin Borracheiro", "claudin@gmail.com", "Password1!");
        var clienteEntity = _mapper.Map<Cliente>(clienteDto);

        var mockRepository = new Mock<IClienteRepository>();
        var mockMapper = new Mock<IMapper>();

        mockRepository.Setup(s => s.GetClienteByEmailAsync(clienteDto.Email)).ReturnsAsync(clienteEntity);

        var service = new ClienteService(mockRepository.Object, mockMapper.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateClienteAsync(clienteDto));
        Assert.Equal("Já existe um cliente com este email.", exception.Message);

        mockRepository.Verify(v => v.CreateClienteAsync(It.IsAny<Cliente>()), Times.Never);
    }

    [Fact]
    public async Task ObtemTodosOsClientesAtivos_QuandoHaClientesCadastrados_RetornaListaDeClientes()
    {
        //Arrange
        var clientes = new List<Cliente>
        {
            new ("Joao","joao@email.com","Password1!"),
            new ("Pedro","pedro@email.com","Password1!"),
            new ("Manel","manel@email.com","Password1!")
        };

        var mockRepository = new Moq.Mock<IClienteRepository>();
        mockRepository.Setup(s => s.GetAllClientesAsync()).ReturnsAsync(clientes);
        var service = new ClienteService(mockRepository.Object, _mapper);

        //Act
        List<ClienteDto>? clientesDto = (List<ClienteDto>?)await service.GetAllClientesAsync();

        // Assert
        Assert.NotNull(clientesDto);
        Assert.NotEmpty(clientesDto);
        Assert.Equal(clientes.Count(), clientesDto.Count());

        clientes = clientes.OrderBy(c => c.Nome).ToList();
        clientesDto = clientesDto.OrderBy(c => c.Nome).ToList();

        for (int i = 0; i < clientes.Count; i++)
        {
            Assert.Equal(clientes[i].Nome, clientesDto[i].Nome);
            Assert.Equal(clientes[i].Email, clientesDto[i].Email);
        }
    }

    [Fact]
    public async Task ObtemListaVaziaDeClientes_QuandoNaoHaClientesCadastrados_RetornaListaDeClientesVazia()
    {
        //Arrange
        var clientes = new List<Cliente> { };

        var mockRepository = new Moq.Mock<IClienteRepository>();
        mockRepository.Setup(s => s.GetAllClientesAsync()).ReturnsAsync(clientes);
        var service = new ClienteService(mockRepository.Object, _mapper);

        //Act
        List<ClienteDto>? clientesDto = (List<ClienteDto>?)await service.GetAllClientesAsync();

        // Assert
        Assert.NotNull(clientesDto);
        Assert.Equal(clientes.Count(), clientesDto.Count());
        Assert.Equal(clientes.Count, clientesDto.Count);
        Assert.Empty(clientesDto);

    }

    [Fact]
    public async Task ObtemClientePeloId_QuandoIdValido_RetornaCliente()
    {
        //Arrange
        var cliente = new Cliente("Bonafe", "bonafe@email.com", "Password1!");
        cliente.ClienteId = 1;

        var mockRepository = new Moq.Mock<IClienteRepository>();
        mockRepository.Setup(s => s.GetClienteByIdAsync(1)).ReturnsAsync(cliente);
        var service = new ClienteService(mockRepository.Object, _mapper);

        //Act
        var clienteDto = await service.GetClienteByIdAsync(1);

        //Assert
        Assert.NotNull(clienteDto);

        mockRepository.Verify(v => v.GetClienteByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task ObtemClientePeloId_QuandoIdNaoValido_RetornaExcecao()
    {
        //Arrange       
        var mockRepository = new Mock<IClienteRepository>();
        mockRepository.Setup(s => s.GetClienteByIdAsync(1)).ReturnsAsync((Cliente)null);
        var service = new ClienteService(mockRepository.Object, _mapper);

        //Act e Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetClienteByIdAsync(1));

        mockRepository.Verify(v => v.GetClienteByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task ObtemClientePeloEmail_QuandoEmailValido_RetornaCliente()
    {
        //Arrange
        var cliente = new Cliente("Bonafe", "bonafe@email.com", "Password1!");

        var mockRepository = new Moq.Mock<IClienteRepository>();
        mockRepository.Setup(s => s.GetClienteByEmailAsync(cliente.Email)).ReturnsAsync(cliente);
        var service = new ClienteService(mockRepository.Object, _mapper);

        //Act
        var clienteDto = await service.GetClienteByEmailAsync(cliente.Email);

        //Assert
        Assert.NotNull(clienteDto);

        mockRepository.Verify(v => v.GetClienteByEmailAsync(clienteDto.Email), Times.Once);
    }

    [Fact]
    public async Task ObtemClientePeloEmail_QuandoEmailNaoValido_RetornaExcecao()
    {
        //Arrange
        var emailQualquer = "qualquer@email.com";

        var mockRepository = new Mock<IClienteRepository>();
        mockRepository.Setup(s => s.GetClienteByEmailAsync(emailQualquer)).ReturnsAsync((Cliente)null);
        var service = new ClienteService(mockRepository.Object, _mapper);

        //Act e Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetClienteByEmailAsync(emailQualquer));

        mockRepository.Verify(v => v.GetClienteByEmailAsync(emailQualquer), Times.Once);
    }

    [Fact]
    public async Task AtualizaDadosCliente_QuandoDadosEIdValidos_RetornaClienteAtualizado()
    {
        // Arrange
        var cliente = new Cliente("Bonafe", "bonafe@email.com", "Password1!");
        cliente.ClienteId = 1;

        var clienteUpdateDto = new ClienteUpdateDto("Lucas Bonafe", "bonafe@email.com");
        var clienteAtualizado = new Cliente
        {
            ClienteId = cliente.ClienteId,
            Nome = clienteUpdateDto.Nome,
            Email = clienteUpdateDto.Email,
            Senha = cliente.Senha
        };

        var mockRepository = new Mock<IClienteRepository>();
        mockRepository.Setup(r => r.GetClienteByIdAsync(cliente.ClienteId)).ReturnsAsync(cliente);
        mockRepository.Setup(r => r.UpdateClienteAsync(It.IsAny<Cliente>(), cliente.ClienteId)).ReturnsAsync(clienteAtualizado);

        var service = new ClienteService(mockRepository.Object, _mapper);

        // Act
        var clienteDto = await service.UpdateCliente(clienteUpdateDto, cliente.ClienteId);

        // Assert
        Assert.NotNull(clienteDto);
        Assert.Equal(clienteAtualizado.Nome, clienteDto.Nome);
        Assert.Equal(clienteAtualizado.Email, clienteDto.Email);

        mockRepository.Verify(r => r.GetClienteByIdAsync(cliente.ClienteId), Times.Once);
        mockRepository.Verify(r => r.UpdateClienteAsync(It.IsAny<Cliente>(), cliente.ClienteId), Times.Once);
    }

    [Fact]
    public async Task AtualizaDadosCliente_QuandoIdNaoValido_retornaExcecao()
    {
        // Arrange
        var clienteUpdateDto = new ClienteUpdateDto("Lucas Bonafe", "bonafe@email.com");
        var idInvalido = 999;

        var mockRepository = new Mock<IClienteRepository>();
        mockRepository.Setup(s => s.GetClienteByIdAsync(idInvalido)).ReturnsAsync((Cliente)null);

        var mockMapper = new Mock<IMapper>();
        var service = new ClienteService(mockRepository.Object, mockMapper.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => service.UpdateCliente(clienteUpdateDto, idInvalido));
        Assert.Equal($"Nenhum cliente encontrado com o Id {idInvalido}.", exception.Message);

        // Verifica que GetClienteByIdAsync foi chamado
        mockRepository.Verify(s => s.GetClienteByIdAsync(idInvalido), Times.Once);

        // Verifica que UpdateClienteAsync nunca foi chamado
        mockRepository.Verify(s => s.UpdateClienteAsync(It.IsAny<Cliente>(), idInvalido), Times.Never);
    }

    [Fact]
    public async Task AtualizaSenhaCliente_QuandoSenhaEIdValidos_retornaTrue()
    {
        //Arrange
        var cliente = new Cliente("Bonafe", "bonafe@email.com", "Password1!");
        cliente.ClienteId = 1;

        var novaSenha = "NewPassword1!";

        var mockRepository = new Mock<IClienteRepository>();
        mockRepository.Setup(s => s.GetClienteByIdAsync(cliente.ClienteId)).ReturnsAsync(cliente);
        var service = new ClienteService(mockRepository.Object, _mapper);

        //Act
        var response = await service.UpdateSenha(cliente.ClienteId, novaSenha);

        //Assert
        Assert.NotNull(novaSenha);
        Assert.Equal(cliente.Senha, novaSenha);
        Assert.True(response);
    }

    [Fact]
    public async Task AtualizaCliente_QuandoIdInvalido_LancaExcecao()
    {
        // Arrange
        var clienteUpdateDto = new ClienteUpdateDto("Lucas Bonafe", "bonafe@email.com");
        var idInvalido = 999;

        var mockRepository = new Mock<IClienteRepository>();
        var mockMapper = new Mock<IMapper>();

        mockRepository.Setup(s => s.GetClienteByIdAsync(idInvalido)).ReturnsAsync((Cliente)null);

        var service = new ClienteService(mockRepository.Object, mockMapper.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => service.UpdateCliente(clienteUpdateDto, idInvalido));
        Assert.Equal($"Nenhum cliente encontrado com o Id {idInvalido}.", exception.Message);

        mockRepository.Verify(s => s.GetClienteByIdAsync(idInvalido), Times.Once);
        mockRepository.Verify(s => s.UpdateClienteAsync(It.IsAny<Cliente>(), idInvalido), Times.Never);
    }


    [Fact]
    public async Task UpdateSenha_QuandoSenhaValida_AtualizaComSucesso()
    {
        // Arrange
        var cliente = new Cliente("Bonafe", "bonafe@email.com", "Password1!");
        cliente.ClienteId = 1;

        var novaSenha = "NovaSenha1!";

        var mockRepository = new Mock<IClienteRepository>();
        mockRepository.Setup(r => r.GetClienteByIdAsync(cliente.ClienteId)).ReturnsAsync(cliente);

        var service = new ClienteService(mockRepository.Object, _mapper);

        // Act
        var result = await service.UpdateSenha(cliente.ClienteId, novaSenha);

        // Assert
        Assert.True(result, "O método deve retornar true para uma senha válida.");

        mockRepository.Verify(r => r.GetClienteByIdAsync(cliente.ClienteId), Times.Once);
    }

    [Fact]
    public async Task DesativaCliente_QuandoIdValido_RetornaClienteDesativado()
    {
        //Arrange
        var cliente = new Cliente("Bonafe", "bonafe@email.com", "Password1!");
        cliente.ClienteId = 1;

        var clienteId = 1;

        var mockRepository = new Mock<IClienteRepository>();
        mockRepository.Setup(r => r.GetClienteByIdAsync(cliente.ClienteId)).ReturnsAsync(cliente);

        var service = new ClienteService(mockRepository.Object, _mapper);

        //Act 
        var clienteDesativado = await service.DeleteCliente(clienteId);

        //Assert
        Assert.NotNull(cliente);
        Assert.Null(clienteDesativado);

        mockRepository.Verify(repo => repo.DeleteCliente(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task DesativaCliente_QuandoIdInvalido_RetornaExcecao()
    {
        //Arrange
        var idInvalido = 999;
        var mockRepository = new Mock<IClienteRepository>();
        var service = new ClienteService(mockRepository.Object, _mapper);

        //Act e Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteCliente(idInvalido));
        Assert.Equal($"Nenhum cliente encontrado com o Id {idInvalido}.", exception.Message);
    }
}