using AuthService.Domain.Entities;
using AuthService.Infrastructure.Data;
using AuthService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AuthService.Infrastructure.Tests;

public class ClienteRepositoryTest
{
    private readonly AppDbContext _context;

    public ClienteRepositoryTest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        _context = new AppDbContext(options);
    }


    [Fact]
    public async void Deve_Criar_Cliente_No_Banco()
    {
        // Arrange
        var usuario = new Usuario("zeze Botijas","zeze@botijao.com","Password1!");
        var repository = new AuthRepository(_context);

        // Act
        var usuarioSalvo = await repository.CreateUsuarioAsync(usuario);
        await _context.SaveChangesAsync();

        // Assert 
        Assert.NotNull(usuarioSalvo);

        //PERGUNTAR PARA O BERNARD 1:
        var obj1 = JsonConvert.SerializeObject(usuario);
        var obj2 = JsonConvert.SerializeObject(usuarioSalvo);
        Assert.Equal(obj1, obj2);
    }
}