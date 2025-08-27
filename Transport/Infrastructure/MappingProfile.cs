using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entities.Dto;
using Entities.Models.Transports;

namespace Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Car, CarDto>().ReverseMap();
            CreateMap<Motorcycle, MotorcycleDto>().ReverseMap();
            CreateMap<Bicycle, BicycleDto>().ReverseMap();
            CreateMap<Scooter, ScooterDto>().ReverseMap();
            CreateMap<CreateCarDto, Car>();
            CreateMap<CreateMotorcycleDto, Motorcycle>();
            CreateMap<CreateScooterDto, Scooter>();
            CreateMap<CreateBicycleDto, Bicycle>();
        }
    }
}
