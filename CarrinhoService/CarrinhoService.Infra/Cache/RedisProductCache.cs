using CarrinhoService.Application.DTOs;
using CarrinhoService.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrinhoService.Infra.Cache;

public class RedisProductCache : IProdutoCache, IDisposable
{
    private readonly ConnectionMultiplexer _redis;
    private readonly StackExchange.Redis.IDatabase _db;

    public RedisProductCache(string connectionString)
    {
        // Exemplo: "localhost:6379"
        _redis = ConnectionMultiplexer.Connect(connectionString);
        _db = _redis.GetDatabase();
    }

    public void UpdateProduto(int produtoId, string nome, decimal preco)
    {
        // Converter o objeto em JSON para armazenar no Redis
        var produtoInfo = new ProdutoInfoDto
        {
            Id = produtoId,
            Nome = nome,
            Preco = preco
        };

        var json = JsonConvert.SerializeObject(produtoInfo);

        // Armazenar em Redis usando, por exemplo, a Key "produto:{id}"
        var key = $"produto:{produtoId}";
        _db.StringSet(key, json);
    }

    public ProdutoInfoDto? GetProduto(int produtoId)
    {
        var key = $"produto:{produtoId}";
        var json = _db.StringGet(key);

        if (json.IsNullOrEmpty)
        {
            return null; // ou null indica que não existe no cache
        }

        return JsonConvert.DeserializeObject<ProdutoInfoDto>(json);
    }

    public void Dispose()
    {
        _redis?.Dispose();
    }
}
