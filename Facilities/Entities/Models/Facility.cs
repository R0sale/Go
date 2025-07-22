using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Entities.Models
{
    public class Facility
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }
        public Dictionary<string, string> Schedule { get; set; }
        public string? WebsiteURL { get; set; }

        public bool IsOpened { 
            get
            {
                var currentDay = DateTime.Now.DayOfWeek.ToString();
                var currentTime = TimeOnly.FromDateTime(DateTime.Now);

                if (Schedule.TryGetValue(currentDay, out var hours))
                {
                    var timeParts = hours.Split('-');
                    var openingTime = new TimeOnly(int.Parse(timeParts[0].Substring(0, 2)), int.Parse(timeParts[0].Substring(3, 2)));
                    var closingTime = new TimeOnly(int.Parse(timeParts[1].Substring(0, 2)), int.Parse(timeParts[1].Substring(3, 2)));

                    return currentTime >= openingTime && currentTime <= closingTime;
                }

                return false;
            }
        }
    }
}
