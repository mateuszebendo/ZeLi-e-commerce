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
        var clienteDto = new ClienteCreateDto("JAO Gomes", "jaozin123@gmail.com", "Password1!");

        var clienteEntity = _mapper.Map<Cliente>(clienteDto);
        var mockRepository = new Moq.Mock<IClienteRepository>();

        mockRepository.Setup(s => s.CreateClienteAsync(clienteEntity)).ReturnsAsync(clienteEntity);
        var service = new ClienteService(mockRepository.Object, _mapper);

        //Act
        var clienteToCreate = await service.CreateClienteAsync(clienteDto);

        //Assert
        Assert.NotNull(clienteToCreate);
        Assert.Equal(clienteDto.Nome, clienteToCreate.Nome);
        Assert.Equal(clienteDto.Email, clienteToCreate.Email);

        mockRepository.Verify(r => r.CreateClienteAsync(
            It.Is<Cliente>(c => c.Email == clienteDto.Email && c.Nome == clienteDto.Nome && c.Senha == clienteDto.Senha)), Times.Once); //verifica se o metodo funcionou
    }

    [Fact]
    public async Task Criar_Novo_Cliente_Com_Email_Existente_Deve_Falhar_Retornar_Excecao()
    {
        //Arrange
        var clienteDto = new ClienteCreateDto("Claudin Borracheiro", "claudin@email.com", "Password1!");
        var clienteEntity = _mapper.Map<Cliente>(clienteDto);

        var mockRepository = new Moq.Mock<IClienteRepository>();

        mockRepository.Setup(s => s.CreateClienteAsync(It.IsAny<Cliente>())).ThrowsAsync(new InvalidOperationException());
        var service = new ClienteService(mockRepository.Object, _mapper);

        //Act e Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateClienteAsync(clienteDto));

        mockRepository.Verify(v => v.CreateClienteAsync(clienteEntity), Times.Never); //verifica se o metodo nao foi chamado
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
        Assert.NotEmpty(clientesDto);
        Assert.NotNull(clientesDto);
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

        var mockRepository = new Moq.Mock<IClienteRepository>();
        mockRepository.Setup(s => s.GetClienteByIdAsync(1)).ReturnsAsync(cliente);
        var service = new ClienteService(mockRepository.Object, _mapper);

        //Act
        var clienteDto = await service.GetClienteByIdAsync(1);

        //Assert
        Assert.NotNull(clienteDto);
        Assert.Equal(cliente.Nome, clienteDto.Nome);
        Assert.Equal(cliente.Email, clienteDto.Email);

        mockRepository.Verify(v => v.CreateClienteAsync(cliente),Times.Once);
    }
}