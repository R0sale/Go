using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace Entities.Dtos
{
    public class TransportDto
    {
        public string? Id { get; set; }
        public virtual TransportType TransportType { get; init; }
        public string? ModelName { get; set; }
        public decimal Cost { get; set; }
        public Coordinates? Coordinates { get; set; }
    }
}
