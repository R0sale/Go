using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public class FacilityNotFoundException : NotFoundException
    {
        public FacilityNotFoundException(string id) : base($"Facility with id: {id} not found.")
        {
        }
    }
}
