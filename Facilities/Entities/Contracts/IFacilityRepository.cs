using Entities.Dtos;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Contracts
{
    public interface IFacilityRepository
    {
        Task<IList<Facility>> GetFacilitiesAsync();
        Task<Facility> GetFacilityAsync(string id);
        Task<Facility> CreateFacilityAsync(Facility newFacility);
        Task UpdateFacilityAsync(string id, Facility updatedFacility);
        Task RemoveFacilityAsync(string id);
        Task<IList<Facility>> GetFacilitiesNearByAsync(double longitude, double latitude, double radius);
    }
}
