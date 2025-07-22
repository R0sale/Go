using Application.Dtos;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IFacilityRepository
    {
        Task<IList<Facility>> GetFacilitiesAsync();
        Task<Facility> GetFacilityAsync(string id);
        Task CreateFacilityAsync(Facility newFacility);
        Task UpdateFacilityAsync(string id, Facility updatedFacility);
        Task RemoveFacilityAsync(string id);
        
    }
}
