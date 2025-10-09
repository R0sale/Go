using Entities.Dto;
using Entities.Models.Transports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Contracts.Repositories
{
    public interface IBicycleRepository
    {
        Task<IEnumerable<Bicycle>> GetAllBicyclesAsync();
        Task<Bicycle> GetBicycleByIdAsync(string id);
        Task CreateBicycleAsync(Bicycle bicycle);
        Task DeleteBicycleAsync(Bicycle bicycle);
        Task UpdateBicycleAsync(Bicycle bicycle);
    }
}
