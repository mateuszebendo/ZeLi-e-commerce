using AutoMapper;
using CarrinhoService.Application.DTOs;
using CarrinhoService.Domain.Entities;
using CarrinhoService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CarrinhoService.Infra.Mensageria;

public class ProdutoEventsConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMapper _mapper;
    private IConnection _connection;
    private RabbitMQ.Client.IModel _channel;
    private const string EXCHANGE_NAME = "products-exchange";

    public ProdutoEventsConsumer(IServiceProvider serviceProvider, IMapper mapper)
    {
        _serviceProvider = serviceProvider;
        _mapper = mapper;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        // Declara a mesma exchange do tipo fanout
        _channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Fanout, durable: true);

        // Declara fila para o CarrinhoService
        var queueName = _channel.QueueDeclare(queue: "CarrinhoService_ProductQueue",
                                             durable: true, exclusive: false, autoDelete: false).QueueName;

        // Faz bind
        _channel.QueueBind(queueName, EXCHANGE_NAME, routingKey: "");

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            var evt = JsonConvert.DeserializeObject<ProdutoAtualizadoEvent>(json);

            // Salvar/atualizar no banco local (ProdutoRepository)
            using (var scope = _serviceProvider.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IProdutoRepository>();
                
                var produto = _mapper.Map<Produto>(evt);

                await repo.CreateOrUpdateAsync(produto);
            }
        };

        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}
