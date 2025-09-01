using Entities.Dto;
using Entities.Models.Transports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Contracts.Services
{
    public interface IBicycleService
    {
        Task<IEnumerable<BicycleDto>> GetAllBicyclesAsync();
        Task<BicycleDto> GetBicycleByIdAsync(string id);
        Task<Bicycle> CreateBicycleAsync(CreateBicycleDto createBicycleDto);
        Task DeleteBicycleAsync(string id);
        Task UpdateBicycleAsync(string id, BicycleDto updateBicycleDto);
    }
}
