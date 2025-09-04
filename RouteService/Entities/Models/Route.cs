using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Route
    {
        public string? Id { get; set; }
        public string? OwnerUid { get; set; }
        public string? City { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<Node>? Nodes { get; set; }
    }
}
