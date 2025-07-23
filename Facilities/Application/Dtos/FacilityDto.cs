using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace Application.Dtos
{
    public class FacilityDto
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public double[]? Coordinates { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }
        public string? WebsiteURL { get; set; }
        public Dictionary<DayOfWeek, OpeningTime>? Schedule { get; set; }
    }
}
