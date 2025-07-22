using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Converters;
using Application.Dtos;
using AutoMapper;
using Entities.Models;

namespace Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FacilityDto, Facility>().ConvertUsing<FacilityConverter>();
            CreateMap<Facility, FacilityDto>();
            CreateMap<Facility, UpdatedFacilityDto>();
            CreateMap<UpdatedFacilityDto, Facility>().ConvertUsing<UpdatedFacilityConverter>();
        }
    }
}
