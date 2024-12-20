using Microsoft.EntityFrameworkCore;
using ProductCatalogService.Application.Contracts;
using ProductCatalogService.Application.Mapping;
using ProductCatalogService.Application.Services;
using ProductCatalogService.Domain.Contracts;
using ProductCatalogService.Infra.Data;
using ProductCatalogService.Infra.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IProdutoService,  ProdutoService>();

builder.Services.AddDbContextPool<ConfigDataBase>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnectionString")
));

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<DomainToDtoMappingProfile>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
