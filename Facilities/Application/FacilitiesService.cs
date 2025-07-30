using Entities.Contracts;
using Entities.Dtos;
using AutoMapper;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

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

        public async Task<Facility> CreateFacilityAsync(FacilityDto newFacility)
        {
            var facility = _mapper.Map<Facility>(newFacility);

            var fasc = await _facilityRepository.CreateFacilityAsync(facility);

            Log.Information("Facility with ID: {FacilityId} was created", fasc.Id);

            return fasc;
        }

        public async Task UpdateFacilityAsync(string id, FacilityDto updatedFacility)
        {
            var facility = _mapper.Map<Facility>(updatedFacility);

            facility.Id = id;

            await _facilityRepository.UpdateFacilityAsync(id, facility);
        }

        public async Task RemoveFacilityAsync(string id)
        {
            Log.Information("Facility with ID: {FacilityId} was removed", id);

            await _facilityRepository.RemoveFacilityAsync(id);
        }

        public async Task<IList<FacilityDto>> GetFacilitiesNearByAsync(double latitude, double longitude, double radiusKm)
        {
            var nearbyFacilities = await _facilityRepository.GetFacilitiesNearByAsync(longitude, latitude, radiusKm);

            return _mapper.Map<IList<FacilityDto>>(nearbyFacilities);
        }
    }
}
