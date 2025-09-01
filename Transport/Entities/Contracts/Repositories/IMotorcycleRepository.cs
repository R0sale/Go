using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models.Transports;

namespace Entities.Contracts.Repositories
{
    public interface IMotorcycleRepository
    {
        Task<IEnumerable<Motorcycle>> GetAllMotorcyclesAsync();
        Task<Motorcycle> GetMotorcycleByIdAsync(string id);
        Task CreateMotorcycleAsync(Motorcycle moto);
        Task DeleteMotorcycleAsync(Motorcycle moto);
        Task UpdateMotorcycleAsync(Motorcycle moto);
    }
}
