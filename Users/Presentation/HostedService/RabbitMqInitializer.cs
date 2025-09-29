using Entities.Contracts;
using Infrastructure.Rabbit;
using RabbitMQ.Client;

namespace Presentation.HostedService
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

            var rpcClient = _serviceProvider.GetRequiredService<IRpcClient>() as RpcClient;
            await rpcClient!.StartAsync(channel);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
