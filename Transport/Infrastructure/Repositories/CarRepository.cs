using Entities.Models.Transports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Entities.Contracts.Repositories;
using Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace Infrastructure.Repositories
{
    public class CarRepository : RepositoryBase<Car>, ICarRepository
    {
        public CarRepository(IOptions<TransportDatabaseSettings> settings) : base(settings)
        {
        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync() => await FindAllAsync();
        public async Task<Car> GetCarByIdAsync(string id) => (await FindByConditionAsync(car => car.Id.Equals(id))).FirstOrDefault();
        public async Task CreateCarAsync(Car car) => await CreateAsync(car);
        public async Task DeleteCarAsync(Car car) => await DeleteAsync(car);
        public async Task UpdateCarAsync(Car car) => await UpdateAsync(car);
    }
}
