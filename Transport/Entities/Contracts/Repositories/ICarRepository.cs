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
        Task<IEnumerable<Car>> GetAllCarsAsync(bool trackChanges);
        Task<Car> GetCarByIdAsync(string id, bool trackChanges);
        void CreateCar(Car car);
        void DeleteCar(Car car);
        void UpdateCar(Car car);
        Task SaveAsync();
    }
}
