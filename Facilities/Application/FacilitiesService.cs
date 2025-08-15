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
using Entities.Exceptions;

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

            if (facilities is null)
                throw new NotFoundException("There is no facilities in the collection.");

            var facilitiesDto = _mapper.Map<IList<FacilityDto>>(facilities);

            Console.WriteLine(facilitiesDto[0].Id);

            return facilitiesDto;
        }

        public async Task<IEnumerable<FacilityDto>> GetUsersFacilitiesAsync(string uid)
        {
            var facilities = await _facilityRepository.GetUsersFacilitiesAsync(uid);

            var facilitiesDto = _mapper.Map<IEnumerable<FacilityDto>>(facilities);

            return facilitiesDto;
        }

        public async Task<FacilityDto> GetFacilityAsync(string id)
        {
            var facility = await _facilityRepository.GetFacilityAsync(id);

            if (facility is null)
                throw new FacilityNotFoundException(id);

            var facilityDto = _mapper.Map<FacilityDto>(facility);

            return facilityDto;
        }

        public async Task<Facility> CreateFacilityAsync(FacilityDto newFacility, string uid)
        {
            var facility = _mapper.Map<Facility>(newFacility);

            facility.UserUid = uid;

            var fasc = await _facilityRepository.CreateFacilityAsync(facility);

            Log.Information("Facility with ID: {FacilityId} was created", fasc.Id);

            return fasc;
        }

        public async Task UpdateFacilityAsync(string id, FacilityDto updatedFacility)
        {
            await CheckIfFacilityExists(id);

            var facility = _mapper.Map<Facility>(updatedFacility);

            facility.Id = id;

            await _facilityRepository.UpdateFacilityAsync(id, facility);
        }

        public async Task RemoveFacilityAsync(string id)
        {
            await CheckIfFacilityExists(id);

            Log.Information("Facility with ID: {FacilityId} was removed", id);

            await _facilityRepository.RemoveFacilityAsync(id);
        }

        public async Task<IList<FacilityDto>> GetFacilitiesNearByAsync(double latitude, double longitude, double radiusKm)
        {
            var nearbyFacilities = await _facilityRepository.GetFacilitiesNearByAsync(longitude, latitude, radiusKm);

            return _mapper.Map<IList<FacilityDto>>(nearbyFacilities);
        }

        private async Task CheckIfFacilityExists(string id)
        {
            var facility = await _facilityRepository.GetFacilityAsync(id);

            if (facility == null)
                throw new FacilityNotFoundException(id);
        }
    }
}
