using AuthService.Domain.Entities;
using AuthService.Infrastructure.Data;
using AuthService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AuthService.Infrastructure.Tests;

public class ClienteRepositoryTest
{
    private readonly Context _context;

    public ClienteRepositoryTest()
    {
        var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        _context = new Context(options);
    }


    [Fact]
    public async void Deve_Criar_Cliente_No_Banco()
    {
        // Arrange
        var cliente = new Cliente("zeze Botijas","zeze@botijao.com","Password1!");
        var repository = new ClienteRepository(_context);

        // Act
        var clienteSalvo = await repository.CreateClienteAsync(cliente);
        await _context.SaveChangesAsync(); // tirar e colocafr dentro do metodo createCliente.

        // Assert 
        Assert.NotNull(clienteSalvo);

        //PERGUNTAR PARA O BERNARD 1:
        var obj1 = JsonConvert.SerializeObject(cliente);
        var obj2 = JsonConvert.SerializeObject(clienteSalvo);
        Assert.Equal(obj1, obj2);
    }

    [Fact]
    public async void Deve_Retornar_Todos_Os_Clientes()
    {
        // Arrange
        var cliente1 = new Cliente ("JAO", "cliente@botijao.com", "Password1!");
        var cliente2 = new Cliente("zeze", "zezin@botijao.com", "Password1!"); 

        var repository = new ClienteRepository(_context);
        await repository.CreateClienteAsync(cliente1);
        await repository.CreateClienteAsync(cliente2);

        // Act
        var clientes = await repository.GetAllClientesAsync();

        // Assert
        Assert.NotNull(clientes);
        var lista = clientes.ToList();
        Assert.Equal(2, lista.Count);

       
        var esperados = new List<Cliente> { cliente1, cliente2 }.OrderBy(c => c.ClienteId).ToList();
        var retornados = lista.OrderBy(c => c.ClienteId).ToList();

        var objEsperado = JsonConvert.SerializeObject(esperados);
        var objRetornado = JsonConvert.SerializeObject(retornados);
        Assert.Equal(objEsperado, objRetornado);
    }

    [Fact]
    public async Task GetClienteById_DeveRetornarClienteExistente()
    {
        // Arrange
        var cliente = new Cliente ("Ana","zeze@botijao.com", "Password1!");
        var repository = new ClienteRepository(_context);
        var clienteSalvo = await repository.CreateClienteAsync(cliente);
        

        // Act
        var clienteBuscado = await repository.GetClienteByIdAsync(cliente.ClienteId);

        // Assert
        Assert.NotNull(clienteBuscado);

        var obj1 = JsonConvert.SerializeObject(clienteSalvo);
        var obj2 = JsonConvert.SerializeObject(clienteBuscado);
        Assert.Equal(obj1, obj2);
    }

    [Fact]
    public async void Deve_Atualizar_Cliente()
    {
        //Arrange
        var cliente = new Cliente ("Antigo Nome", "email@email.com", "Password1!");
        var repository = new ClienteRepository(_context);
        await repository.CreateClienteAsync(cliente);

        var clienteAtualizado = new Cliente ("Novo Nome", "email@aaaemail.com", "Password1!");

        //Act
        await repository.UpdateCliente(clienteAtualizado, cliente.ClienteId);
        clienteAtualizado.ClienteId = cliente.ClienteId;

        //Assert
        Assert.NotNull(clienteAtualizado);

        Assert.Equal(cliente.Nome, clienteAtualizado.Nome);
        Assert.Equal(cliente.Email, clienteAtualizado.Email);
    }

    [Fact]
    public async Task Deve_Excluir_Cliente()
    {
        //Arrange
        var cliente = new Cliente("shaulin", "zeze@botijao.com", "Password1!");
        var repository = new ClienteRepository(_context);
        await repository.CreateClienteAsync(cliente);

        //Act
        var clienteDeletado = await repository.DeleteCliente(cliente.ClienteId);

        // Assert
        Assert.False(clienteDeletado.Ativo, "O campo Ativo deveria estar como false após a exclusão.");
        var clienteInativo = await repository.GetClienteByIdAsync(cliente.ClienteId);
        Assert.Null(clienteInativo);
    }
}