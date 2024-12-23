using CarrinhoService.Application.Interfaces;
using CarrinhoService.Infra.Cache;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var redisConnectionString = builder.Configuration["Redis:ConnectionString"];

builder.Services.AddSingleton<IProdutoCache>(provider => new RedisProductCache(redisConnectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
