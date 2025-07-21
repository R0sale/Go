
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public enum CuisineType
    {
        Italian,
        Chinese,
        Indian,
        Mexican,
        American,
        French,
        Japanese,
        Mediterranean,
        Thai,
        Other
    }

    public class Restaurant : Facility
    {
        public List<CuisineType>? CuisineType { get; set; } 
        public int SeatingCapacity { get; set; } 
        public bool HasDeliveryService { get; set; } 
        public bool HasTakeoutService { get; set; } 
    }
}
