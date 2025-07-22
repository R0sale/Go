using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Entities.Models
{
    public class OpeningTime
    {
        public TimeOnly Opening { get; set; }
        public TimeOnly Closing { get; set; }

        public override string ToString()
        {
            return $"{Opening.Hour.ToString("D2")}:{Opening.Minute.ToString("D2")}-{Closing.Hour.ToString("D2")}:{Closing.Minute.ToString("D2")}";
        }
    }

    public class Facility
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string? Name { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
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
