using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dto
{
    public class CreateBicycleDto
    {
        public TransportType Type { get; } = TransportType.Bicycle;
        public string? MoodelName { get; set; }
        public decimal Cost { get; set; }
        public Coordinates? Coordinates { get; set; }
        public bool IsElectric { get; set; }
    }
}
