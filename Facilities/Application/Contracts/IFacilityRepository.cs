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
        Task<IList<Facility>> GetFacilities();
        Task<Facility> GetFacility(string id);
        Task CreateFacility(Facility newFacility);
        Task UpdateFacility(string id, Facility updatedFacility);
        Task RemoveFacility(string id);
    }
}
