
using AutoMapper;
using CarrinhoService.Application.DTOs;
using CarrinhoService.Application.Interfaces;
using CarrinhoService.Domain.Entities;
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
    private readonly IMapper _mapper;

    public RedisProductCache(string connectionString)
    {
        _redis = ConnectionMultiplexer.Connect(connectionString);
        _db = _redis.GetDatabase();
    }

    public void UpdateProduto(int produtoId, string nome, double preco)
    {
        var produto = new Produto
        {
            ProdutoId = produtoId,
            Nome = nome,
            Preco = preco
        };

        var json = JsonConvert.SerializeObject(produto);

        var key = $"produto:{produtoId}";
        _db.StringSet(key, json);
    }

    public Produto GetProduto(int produtoId)
    {
        var key = $"produto:{produtoId}";
        var json = _db.StringGet(key);

        if (json.IsNullOrEmpty)
        {
            return null; // null indica que não existe no cache
        }

        return JsonConvert.DeserializeObject<Produto>(json);
    }

    public void Dispose()
    {
        _redis?.Dispose();
    }
}