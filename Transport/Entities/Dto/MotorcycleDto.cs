using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dto
{
    public class MotorcycleDto
    {
        public TransportType Type { get; set; }
        public string? MoodelName { get; set; }
        public decimal Cost { get; set; }
        public Coordinates? Coordinates { get; set; }
        public string? Color { get; set; }
        public int MaxSpeed { get; set; }
        public FuelType FuelType { get; init; }
        public string? PlatesNumber { get; init; }
    }
}
