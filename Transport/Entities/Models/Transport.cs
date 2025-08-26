using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    abstract public class Transport
    {
        public string? Id { get; set; }
        public abstract TransportType Type { get; init; }
        public string? ModelName { get; set; }
        public decimal Cost { get; set; }
        public Coordinates? Coordinates { get; set; }
    }
}
