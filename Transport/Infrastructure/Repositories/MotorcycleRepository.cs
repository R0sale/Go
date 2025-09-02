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
    public class MotorcycleRepository : RepositoryBase<Motorcycle>, IMotorcycleRepository
    {
        public MotorcycleRepository(IOptions<TransportDatabaseSettings> opts) : base(opts)
        {
        }

        public async Task<IEnumerable<Motorcycle>> GetAllMotorcyclesAsync() => await FindAllAsync();
        public async Task<Motorcycle> GetMotorcycleByIdAsync(string id) => (await FindByConditionAsync(moto => moto.Id.Equals(id))).FirstOrDefault();
        public async Task CreateMotorcycleAsync(Motorcycle moto) => await CreateAsync(moto);
        public async Task DeleteMotorcycleAsync(Motorcycle moto) => await DeleteAsync(moto);
        public async Task UpdateMotorcycleAsync(Motorcycle moto) => await UpdateAsync(moto);
    }
}
