using CarrinhoService.Domain.Entities;
using CarrinhoService.Domain.Interfaces;
using CarrinhoService.Infra.Data;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrinhoService.Infra.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly ConnectionMultiplexer _redis;
    private readonly IDatabase _db;

    public ProdutoRepository(string connectionString)
    {
        _redis = ConnectionMultiplexer.Connect(connectionString);
        _db = _redis.GetDatabase();
    }

    public async Task<Produto> CreateOrUpdateAsync(Produto produto)
    {
        // Serializa em JSON
        var json = JsonConvert.SerializeObject(produto);

        // Chave do Redis
        var key = $"produto:{produto.ProdutoId}";

        // Salva a string no Redis
        // Se quiser definir expiração, adicione TimeSpan
        await _db.StringSetAsync(key, json);

        return produto;
    }

    public async Task<Produto> GetByIdAsync(int produtoId)
    {
        var key = $"produto:{produtoId}";
        var json = await _db.StringGetAsync(key);

        if (json.IsNullOrEmpty)
            return null; // ou lança exceção se preferir

        return JsonConvert.DeserializeObject<Produto>(json);
    }

    public async Task<List<Produto>> GetAllAsync()
    {
        // Em Redis, não há "listar tudo" nativo para chaves.
        // Precisamos usar SCAN ou KEYS para buscar as chaves do padrão produto:*
        // Em produção, SCAN é preferível a KEYS (por performance).

        var result = new List<Produto>();

        var server = GetServer();
        // Padrão de busca
        var pattern = "produto:*";

        // SCAN retorna um cursor que percorre as chaves de forma incremental
        var keys = server.Keys(pattern: pattern);

        // Para cada chave, lemos do DB e desserializamos
        foreach (var key in keys)
        {
            var json = await _db.StringGetAsync(key);
            if (!json.IsNullOrEmpty)
            {
                var dto = JsonConvert.DeserializeObject<Produto>(json);
                if (dto != null)
                    result.Add(dto);
            }
        }

        return result;
    }

    public async Task DeleteAsync(int produtoId)
    {
        var key = $"produto:{produtoId}";
        await _db.KeyDeleteAsync(key);
    }

    // Helper para obter um "Server" do ConnectionMultiplexer para chamar SCAN
    private IServer GetServer()
    {
        // Pega o endpoint primário
        var endpoints = _redis.GetEndPoints();
        // Você pode precisar ajustar se tiver cluster ou replicação
        return _redis.GetServer(endpoints.First());
    }

    public void Dispose()
    {
        _redis?.Dispose();
    }
}