using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Entities.Models;

namespace Infrastructure.Converters
{
    public class OpeningTimeToJson : JsonConverter<OpeningTime>
    {
        public override void Write(Utf8JsonWriter writer, OpeningTime value, JsonSerializerOptions options)
        {
            Console.WriteLine("Control flow");
            writer.WriteStringValue(value.ToString());
        }

        public override OpeningTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var time = reader.GetString();

            var times = time.Split('-');
            var openingTimes = times[0].Split(':');
            var closingTimes = times[1].Split(':');

            var openingTime = new TimeOnly(int.Parse(openingTimes[0]), int.Parse(openingTimes[1]));
            var closingTime = new TimeOnly(int.Parse(closingTimes[0]), int.Parse(closingTimes[1]));

            return new OpeningTime()
            {
                Opening = openingTime,
                Closing = closingTime
            };
        }
    }
}
