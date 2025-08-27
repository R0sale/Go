using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models.Transports
{
    public class Car : Transport
    {
        public override TransportType Type { get; init; } = TransportType.Car;
        public int MaxSpeed { get; set; }
        public FuelType FuelType { get; init; }
        public string? Color { get; set; }
        public string? PlatesNumber { get; init; }
    }
}
