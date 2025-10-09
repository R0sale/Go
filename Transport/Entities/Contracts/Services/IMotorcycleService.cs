using Entities.Dto;
using Entities.Models.Transports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Contracts.Services
{
    public interface IMotorcycleService
    {
        Task<IEnumerable<MotorcycleDto>> GetAllMotorcyclesAsync();
        Task<MotorcycleDto> GetMotorcycleByIdAsync(string id);
        Task<Motorcycle> CreateMotorcycleAsync(CreateMotorcycleDto moto, string uid);
        Task DeleteMotorcycleAsync(string id, string uid);
        Task UpdateMotorcycleAsync(string id, MotorcycleDto moto, string uid);
    }
}
