using Application.Contracts;
using Application.Dtos;
using AutoMapper;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class FacilitiesService : IFacilityService
    {
        private readonly IFacilityRepository _facilityRepository;
        private readonly IMapper _mapper;

        public FacilitiesService(IFacilityRepository facilityRepository, IMapper mapper)
        {
            _facilityRepository = facilityRepository;
            _mapper = mapper;
        }

        public async Task<IList<FacilityDto>> GetFacilitiesAsync()
        {
            var facilities = await _facilityRepository.GetFacilitiesAsync();

            var facilitiesDto = _mapper.Map<IList<FacilityDto>>(facilities);

            return facilitiesDto;
        }

        public async Task<FacilityDto> GetFacilityAsync(string id)
        {
            var facility = await _facilityRepository.GetFacilityAsync(id);

            var facilityDto = _mapper.Map<FacilityDto>(facility);
            return facilityDto;
        }

        public async Task CreateFacilityAsync(FacilityDto newFacility)
        {
            var facility = _mapper.Map<Facility>(newFacility);

            await _facilityRepository.CreateFacilityAsync(facility);
        }

        public async Task UpdateFacilityAsync(string id, FacilityDto updatedFacility)
        {
            var facility = _mapper.Map<Facility>(updatedFacility);

            facility.Id = id;

            await _facilityRepository.UpdateFacilityAsync(id, facility);
        }

        public async Task RemoveFacilityAsync(string id)
        {
            await _facilityRepository.RemoveFacilityAsync(id);
        }

        public async Task<(UpdatedFacilityDto updatedFacility, Facility facility)> GetFacilityToUpdateAsync(string id)
        {
            var facility = await _facilityRepository.GetFacilityAsync(id);

            var updatedFacility = _mapper.Map<UpdatedFacilityDto>(facility);

            return (updatedFacility, facility);
        }

        public async Task PartiallyUpdateFacilityAsync(string id, UpdatedFacilityDto updatedFacility)
        {
            var facility = _mapper.Map<Facility>(updatedFacility);

            facility.Id = id;

            await _facilityRepository.UpdateFacilityAsync(id, facility);
        }
    }
}
