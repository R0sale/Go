using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dto
{
    public class CreateScooterDto
    {
        public TransportType Type { get; } = TransportType.Scooter;
        public string? ModelName { get; set; }
        public decimal Cost { get; set; }
        public Coordinates? Coordinates { get; set; }
    }
}
