using Entities.Dtos;
using AutoMapper;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Converters
{
    public class FacilityConverter : ITypeConverter<FacilityDto, Facility>
    {
        public Facility Convert(FacilityDto source, Facility destination, ResolutionContext context)
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
            destination.Coordinates = source.Coordinates;
            destination.WebsiteURL = source.WebsiteURL;

            destination.Schedule = source.Schedule;
            

            return destination;
        }
    }
}
