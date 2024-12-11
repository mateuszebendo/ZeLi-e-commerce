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
        var cliente = new Cliente()
        {
            Nome = "zeze Botijas",
            Email = "zeze@botijao.com",
            Senha = "zeze"
        };
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
        var cliente1 = new Cliente { 
            Nome = "Cliente1", 
            Email = "cliente@botijao.com", 
            Senha = "123" 
        };
        var cliente2 = new Cliente { 
            Nome = "Cliente2", 
            Email = "zezin@botijao.com", 
            Senha = "abc" 
        };

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
        var cliente = new Cliente {
            Nome = "Ana", 
            Email = "zeze@botijao.com", 
            Senha = "123" 
        };
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
        // Arrange
        var cliente = new Cliente { 
            Nome = "Antigo Nome", 
            Email = "email@email.com", 
            Senha = "123" 
        };
        var repository = new ClienteRepository(_context);
        await repository.CreateClienteAsync(cliente);

        var clienteAtualizado = new Cliente { 
            Nome = "Novo Nome", 
            Email = "email@aaaemail.com", 
            Senha = "123" 
        };
        // Act
        await repository.UpdateCliente(clienteAtualizado, cliente.ClienteId);
        clienteAtualizado.ClienteId = cliente.ClienteId;

        // Assert
        Assert.NotNull(clienteAtualizado);

        Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAA  1" + cliente.Nome);
        Console.WriteLine("BBBBBBBBBBBBBBBBBBBBBBBBBBBB  2" + clienteAtualizado.Nome);
        Console.WriteLine("cccccccccccccccccccccccccc  2" + cliente.Email);
        Console.WriteLine("ddddddddddddddddddddddd  2" + clienteAtualizado.Email);
        Assert.Equal(cliente.Nome, clienteAtualizado.Nome);
        Assert.Equal(cliente.Email, clienteAtualizado.Email);
    }

    [Fact]
    public async void Deve_Excluir_Cliente()
    {
        // Arrange
        var cliente = new Cliente { 
            Nome = "shaulin", 
            Email = "zeze@botijao.com", 
            Senha = "matadordeporco" 
        };
        var repository = new ClienteRepository(_context);
        await repository.CreateClienteAsync(cliente);

        // Act
        var clienteDeletado = await repository.DeleteCliente(cliente.ClienteId);

        // Assert
        var obj1 = JsonConvert.SerializeObject(cliente);
        var obj2 = JsonConvert.SerializeObject(clienteDeletado);
        Assert.Equal(obj1, obj2);

        var clienteNoBanco = await repository.GetClienteByIdAsync(cliente.ClienteId);
        await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await repository.GetClienteByIdAsync(cliente.ClienteId)
        );
    }
}