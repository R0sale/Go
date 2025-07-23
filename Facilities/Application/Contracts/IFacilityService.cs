using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Entities.Models;

namespace Application.Contracts
{
    public interface IFacilityService
    {
        Task<IList<FacilityDto>> GetFacilitiesAsync();
        Task<FacilityDto> GetFacilityAsync(string id);
        Task<Facility> CreateFacilityAsync(FacilityDto newFacility);
        Task UpdateFacilityAsync(string id, FacilityDto updatedFacility);
        Task RemoveFacilityAsync(string id);
        Task<IList<FacilityDto>> GetFacilitiesNearby(float latitude, float longitude, float radiusKm);
    }
}
