using Entities.Models.Transports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Contracts.Repositories
{
    public interface IScooterRepository
    {
        Task<IEnumerable<Scooter>> GetAllScootersAsync();
        Task<Scooter> GetScooterByIdAsync(string id);
        Task CreateScooterAsync(Scooter scooter);
        Task DeleteScooterAsync(Scooter scooter);
        Task UpdateScooterAsync(Scooter scooter);
    }
}
