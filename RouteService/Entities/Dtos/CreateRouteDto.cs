using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class CreateRouteDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? City { get; set; }
        public List<Node>? Nodes { get; set; }
    }
}
