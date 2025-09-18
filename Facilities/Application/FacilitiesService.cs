using Entities.Contracts;
using Entities.Dtos;
using AutoMapper;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;
using Entities.Exceptions;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using Azure.Storage.Blobs.Models;

namespace Application
{
    public class FacilitiesService : IFacilityService
    {
        private readonly IFacilityRepository _facilityRepository;
        private readonly IMapper _mapper;
        private readonly BlobContainerClient _container;
        private readonly IConfiguration _config;

        public FacilitiesService(IFacilityRepository facilityRepository, IMapper mapper, IConfiguration config)
        {
            _facilityRepository = facilityRepository;
            _mapper = mapper;
            _config = config;

            var service = new BlobServiceClient(config["Azure:ConnectionString"]);

            _container = service.GetBlobContainerClient(config["Azure:Container"]);

            _container.CreateIfNotExists();
            _container.SetAccessPolicy(PublicAccessType.Blob);
        }

        public async Task<IList<FacilityDto>> GetFacilitiesAsync()
        {
            var facilities = await _facilityRepository.GetFacilitiesAsync();

            if (facilities is null)
                throw new NotFoundException("There is no facilities in the collection.");

            var facilitiesDto = _mapper.Map<IList<FacilityDto>>(facilities);

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

        public async Task AddImageAsync(IFormFile file, string id)
        {
            if (file is null || file.Length == 0)
                throw new InvalidFileException("Image can't be null or of length 0.");

            var facility = await _facilityRepository.GetFacilityAsync(id);

            if (facility is null)
                throw new FacilityNotFoundException(id);

            var blob = _container.GetBlobClient(id);

            var blobHttpHeader = new BlobHttpHeaders()
            {
                ContentType = file.ContentType
            };

            var blobHttpOptions = new BlobUploadOptions()
            {
                HttpHeaders = blobHttpHeader
            };

            using (var stream = file.OpenReadStream())
            {
                await blob.UploadAsync(stream, blobHttpOptions);
            }

            facility.ImageUrl = blob.Uri.ToString();

            await _facilityRepository.UpdateFacilityAsync(id, facility);
        }

        public async Task<Facility> CreateFacilityAsync(CreateFacilityDto newFacility, string uid)
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
