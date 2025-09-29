using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Contracts
{
    public interface IRpcClient
    {
        Task<string> CallAsync(string routingKey, string uid, CancellationToken cancellationToken = default);
        Task StartAsync(IChannel channel);
    }
}
