using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Facility
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public double[]? Coordinates { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }
        public string? WebsiteURL { get; set; }
        public Dictionary<DayOfWeek, OpeningTime> Schedule { get; set; }

        public bool IsOpened { 
            get
            {
                var currentDay = DateTime.Now.DayOfWeek;
                var currentTime = TimeOnly.FromDateTime(DateTime.Now);

                if (Schedule.TryGetValue(currentDay, out var openingTime))
                {
                    return currentTime >= openingTime.Opening && currentTime <= openingTime.Closing;
                }

                return false;
            }
        }
    }
}
