using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models.Transports
{
    public class Scooter : Transport
    {
        public override TransportType TransportType { get; init; } = TransportType.Scooter;
    }
}
