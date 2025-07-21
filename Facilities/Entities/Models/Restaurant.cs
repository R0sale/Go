
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    class Restaurant : Facility
    {
        public string? CuisineType { get; set; } 
        public int SeatingCapacity { get; set; } 
        public bool HasDeliveryService { get; set; } 
        public bool HasTakeoutService { get; set; } 
    }
}
