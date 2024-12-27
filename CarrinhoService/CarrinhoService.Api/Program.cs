using CarrinhoService.Application.Interfaces;
using CarrinhoService.Domain.Interfaces;
using CarrinhoService.Infra.Cache;
using CarrinhoService.Infra.Data;
using CarrinhoService.Infra.Mensageria;
using CarrinhoService.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var redisConnectionString = builder.Configuration["Redis:ConnectionString"]; 
builder.Services.AddSingleton<IProdutoCache>(provider => new RedisProductCache(redisConnectionString));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<ICarrinhoRepository, CarrinhoRepository>();
//builder.Services.AddScoped<IItemCarrinhoRepository, ItemCarrinhoRepository>();

builder.Services.AddHostedService<ProdutoEventsConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
