using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Converters;
using Entities.Dtos;
using AutoMapper;
using Entities.Models;

namespace Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FacilityDto, Facility>().ConvertUsing<FacilityConverter>();
            CreateMap<Facility, FacilityDto>();
        }
    }
}
