using Entities.Models.Transports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Entities.Contracts.Repositories;

namespace Infrastructure.Repositories
{
    public class CarRepository : RepositoryBase<Car>, ICarRepository
    {
        public CarRepository(TransportContext _context) : base(_context)
        {
        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync(bool trackChanges) => await FindAll(trackChanges: trackChanges).ToListAsync();
        public async Task<Car> GetCarByIdAsync(string id, bool trackChanges) => await FindByCondition(car => car.Id.Equals(id), trackChanges: trackChanges).FirstOrDefaultAsync();
        public void CreateCar(Car car) => Create(car);
        public void DeleteCar(Car car) => Delete(car);
        public void UpdateCar(Car car) => Update(car);
        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}
