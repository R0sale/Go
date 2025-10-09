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
        Task<IEnumerable<KeyValueDtoObject>> GetSelectedBicycleById(string id);
        Task<Bicycle> CreateBicycleAsync(CreateBicycleDto createBicycleDto, string uid);
        Task DeleteBicycleAsync(string id, string uid);
        Task UpdateBicycleAsync(string id, BicycleDto updateBicycleDto, string uid);
    }
}
