using Entities.Models;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class DictionarySerializer : IBsonSerializer<Dictionary<DayOfWeek, OpeningTime>>
    {
        public Type ValueType => typeof(Dictionary<DayOfWeek, OpeningTime>);
        public Dictionary<DayOfWeek, OpeningTime> Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var reader = context.Reader;
            var dictionary = new Dictionary<DayOfWeek, OpeningTime>();

            reader.ReadStartDocument();

            while (true)
            {
                if (reader.ReadBsonType() == BsonType.EndOfDocument)
                    break;

                var key = reader.ReadName();
                if (Enum.TryParse<DayOfWeek>(key, out var dayOfWeek))
                {
                    var time = reader.ReadString();
                    var times = time.Split('-');
                    var openingTimes = times[0].Split(':');
                    var closingTimes = times[1].Split(':');

                    var openingTime = new TimeOnly(int.Parse(openingTimes[0]), int.Parse(openingTimes[1]));
                    var closingTime = new TimeOnly(int.Parse(closingTimes[0]), int.Parse(closingTimes[1]));

                    dictionary[dayOfWeek] = new OpeningTime
                    {
                        Opening = openingTime,
                        Closing = closingTime
                    };
                }
            }

            reader.ReadEndDocument();

            return dictionary;
        }
        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Dictionary<DayOfWeek, OpeningTime> value)
        {
            var writer = context.Writer;
            writer.WriteStartDocument();
            foreach (var val in value)
            {
                writer.WriteName(val.Key.ToString());
                writer.WriteString(val.Value.ToString());
            }
            writer.WriteEndDocument();
        }

        object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        => Deserialize(context, args);

        void IBsonSerializer.Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
            => Serialize(context, args, (Dictionary<DayOfWeek, OpeningTime>)value);
    }
}
