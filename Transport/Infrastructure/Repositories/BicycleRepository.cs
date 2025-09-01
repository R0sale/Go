using Entities.Contracts.Repositories;
using Entities.Models.Transports;
using Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BicycleRepository : RepositoryBase<Bicycle>, IBicycleRepository
    {
        public BicycleRepository(IOptions<TransportDatabaseSettings> opts) : base(opts)
        {
        }

        public async Task<IEnumerable<Bicycle>> GetAllBicyclesAsync() => await FindAllAsync();
        public async Task<Bicycle> GetBicycleByIdAsync(string id) => (await FindByConditionAsync(bike => bike.Id.Equals(id))).FirstOrDefault();
        public async Task CreateBicycleAsync(Bicycle bicycle) => await CreateAsync(bicycle);
        public async Task DeleteBicycleAsync(Bicycle bicycle) => await DeleteAsync(bicycle);
        public async Task UpdateBicycleAsync(Bicycle bicycle) => await UpdateAsync(bicycle);
    }
}
