using Entities.Contracts;
using Infrastructure.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Net.Sockets;

namespace Presentation.HostedServices
{
    public class RabbitMqInitializer : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        public RabbitMqInitializer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                UserName = "kalo",
                Password = "kalo"
            };

            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            var rpcServer = _serviceProvider.GetRequiredService<IRpcServer>() as RpcServer;
            await rpcServer!.StartAsync(channel);
            await rpcServer!.CreateConsumer();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
