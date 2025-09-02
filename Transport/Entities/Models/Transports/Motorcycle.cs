using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models.Transports
{
    public class Motorcycle : Transport
    {
        public override TransportType Type { get; init; } = TransportType.Motorcycle;
        public string? Color { get; set; }
        public int MaxSpeed { get; set; }
        public FuelType FuelType { get; init; }
        public string? PlatesNumber { get; init; }
    }
}
