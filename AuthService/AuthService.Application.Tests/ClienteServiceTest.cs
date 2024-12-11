using AuthService.Domain.Entities;
using AuthService.Infrastructure.Interfaces;
using Moq;

namespace AuthService.Application.Tests;

public class ClienteServiceTest
{
    private readonly IClienteRepository _clienteRepository;

    public ClienteServiceTest(IClienteRepository clienteRepository) 
        {
            _clienteRepository = clienteRepository;
        } 

    [Fact]
    public async void Criar_Novo_Cliente_Dados_Corretos_Deve_Retornar_Cliente200OK()
    {
        //Arrange
        var nome = "JAO";
        var email = "jaozin123@email.com";
        var senha = "123";

        var mockRepository = new Moq.Mock<IClienteRepository>();

        mockRepository.Setup(s => s.GetClienteByEmailAsync(email)).ReturnsAsync((Cliente)null);

        //Act

        var service = ClienteService(mockRepository.Object);
        var clienteToCreate = await service.CriarCliente(nome, email, senha);

        //Assert
        Assert.NotNull(clienteToCreate);
        Assert.Equal(nome, clienteToCreate.Nome);
        Assert.Equal(email, clienteToCreate.Email);

        mockRepository.Verify(r => r.CreateClienteAsync(
            It.Is<Cliente>(c => c.Email == email && c.Nome == nome && c.Senha == senha)), Times.Once); //verifica se o metodo funcionou
    }

    [Fact]
    public async Task Criar_Novo_Cliente_Com_Email_Existente_Deve_Falhar_Retornar_Excecao()
    {
        //Arrange
        var nome = "Claudin Borracheiro";
        var email = "claudin@email.com";
        var senha = "abc";

        var mockRepository = new Moq.Mock<IClienteRepository>();

        mockRepository.Setup(s => s.GetClienteByEmailAsync(email))
            .ReturnsAsync(new Cliente { Nome = "Zeze do gás", Email = email, Senha = "abc" });

        var service = new ClienteService(mockRepository.Object);

        //Act e Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.RegistrarNovoCliente(nome, email, senha));

        mockRepository.Verify(v => v.CreateClienteAsync(It.IsAny<Cliente>()), Times.Never); //verifica se o metodo nao foi chamado
    }
}