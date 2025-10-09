using Entities.Dto;
using Entities.Models.Transports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Contracts.Repositories
{
    public interface ICarRepository
    {
        Task<IEnumerable<Car>> GetAllCarsAsync();
        Task<Car> GetCarByIdAsync(string id);
        Task CreateCarAsync(Car car);
        Task DeleteCarAsync(Car car);
        Task UpdateCarAsync(Car car);
    }
}
