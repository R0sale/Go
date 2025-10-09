using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Contracts
{
    public interface IRpcServer
    {
        Task StartAsync(IChannel channel);
        Task CreateConsumer();
    }
}
