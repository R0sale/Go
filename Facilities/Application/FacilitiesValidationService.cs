using Entities.Contracts;
using Entities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class FacilitiesValidationService : IFacilitiesValidationService
    {
        private readonly IFacilityRepository _facilityRepository;

        public FacilitiesValidationService(IFacilityRepository facilityRepository)
        {
            _facilityRepository = facilityRepository;
        }

        public async Task<bool> CheckIfFacilityExists(string id)
        {
            var facility = await _facilityRepository.GetFacilityAsync(id);

            if (facility == null)
                throw new FacilityNotFoundException(id);

            return true;
        }
    }
}
