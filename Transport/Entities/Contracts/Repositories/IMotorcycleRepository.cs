using Entities.Dto;
using Entities.Models.Transports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Contracts.Repositories
{
    public interface IMotorcycleRepository
    {
        Task<IEnumerable<Motorcycle>> GetAllMotorcyclesAsync();
        Task<Motorcycle> GetMotorcycleByIdAsync(string id);
        Task<IEnumerable<KeyValueDtoObject>> FindSelectedTransportByIdAsync(string id);
        Task CreateMotorcycleAsync(Motorcycle moto);
        Task DeleteMotorcycleAsync(Motorcycle moto);
        Task UpdateMotorcycleAsync(Motorcycle moto);
    }
}
