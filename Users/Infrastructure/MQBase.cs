using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class MQBase
    {
        private readonly IConnection _connection;
        protected readonly IModel _channel;

        public MQBase(ConnectionFactory factory)
        {
        }
    }
}
