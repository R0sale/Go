using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Dtos;
using Entities.Models;

namespace Entities.Contracts
{
    public interface IFacilityService
    {
        Task<IList<FacilityDto>> GetFacilitiesAsync();
        Task<FacilityDto> GetFacilityAsync(string id);
        Task<Facility> CreateFacilityAsync(FacilityDto newFacility, string uid);
        Task UpdateFacilityAsync(string id, FacilityDto updatedFacility);
        Task RemoveFacilityAsync(string id);
        Task<IEnumerable<FacilityDto>> GetUsersFacilitiesAsync(string uid);
        Task<IList<FacilityDto>> GetFacilitiesNearByAsync(double latitude, double longitude, double radiusKm);
    }
}
