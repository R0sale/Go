using Entities.Contracts.Repositories;
using Entities.Dto;
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
    public class ScooterRepository : RepositoryBase<Scooter>, IScooterRepository
    {
        public ScooterRepository(IOptions<TransportDatabaseSettings> opts) : base(opts)
        {
        }

        public async Task<IEnumerable<Scooter>> GetAllScootersAsync() => await FindAllAsync();
        public async Task<Scooter> GetScooterByIdAsync(string id) => (await FindByConditionAsync(scooter => scooter.Id.Equals(id))).FirstOrDefault();
        public async Task CreateScooterAsync(Scooter scooter) => await CreateAsync(scooter);
        public async Task DeleteScooterAsync(Scooter scooter) => await DeleteAsync(scooter);
        public async Task UpdateScooterAsync(Scooter scooter) => await UpdateAsync(scooter);
    }
}
