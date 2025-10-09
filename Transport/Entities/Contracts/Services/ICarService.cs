using Entities.Dto;
using Entities.Models.Transports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Contracts.Services
{
    public interface ICarService
    {
        Task<IEnumerable<CarDto>> GetAllCarsAsync();
        Task<CarDto> GetCarByIdAsync(string id);
        Task<Car> CreateCarAsync(CreateCarDto createCarDto, string uid);
        Task DeleteCarAsync(string id, string uid);
        Task UpdateCarAsync(string id, CarDto updateCarDto, string uid);
    }
}
