using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Transport
    {
        public string? UserId { get; set; }
        public string? Id { get; set; }
        public virtual TransportType TransportType { get; init; }
        public string? ModelName { get; set; }
        public decimal Cost { get; set; }
        public Coordinates? Coordinates { get; set; }
    }
}
