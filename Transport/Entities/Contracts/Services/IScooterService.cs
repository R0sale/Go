using Entities.Dto;
using Entities.Models.Transports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Contracts.Services
{
    public interface IScooterService
    {
        Task<IEnumerable<ScooterDto>> GetAllScootersAsync();
        Task<ScooterDto> GetScooterByIdAsync(string id);
        Task<Scooter> CreateScooterAsync(CreateScooterDto createScooterDto);
        Task DeleteScooterAsync(string id);
        Task UpdateScooterAsync(string id, ScooterDto updateScooterDto);
    }
}
