using Application.Dtos;
using AutoMapper;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Converters
{
    public class UpdatedFacilityConverter : ITypeConverter<UpdatedFacilityDto, Facility>
    {
        public Facility Convert(UpdatedFacilityDto source, Facility destination, ResolutionContext context)
        {
            if (destination == null)
            {
                destination = new Facility();
            }
            destination.Name = source.Name;
            destination.Address = source.Address;
            destination.PhoneNumber = source.PhoneNumber;
            destination.Email = source.Email;
            destination.Description = source.Description;
            destination.Latitude = source.Latitude;
            destination.Longitude = source.Longitude;
            destination.WebsiteURL = source.WebsiteURL;

            var schedule = new Dictionary<DayOfWeek, OpeningTime>();

            foreach (var item in source.Schedule)
            {
                var times = item.Value.Split('-');
                var openingTimes = times[0].Split(':');
                var closingTimes = times[1].Split(':');

                var openingTime = new TimeOnly(int.Parse(openingTimes[0]), int.Parse(openingTimes[1]));
                var closingTime = new TimeOnly(int.Parse(closingTimes[0]), int.Parse(closingTimes[1]));

                var openingTimeObj = new OpeningTime
                {
                    Opening = openingTime,
                    Closing = closingTime
                };

                schedule.Add(item.Key, openingTimeObj);
            }

            destination.Schedule = schedule;


            return destination;
        }
    }
}
