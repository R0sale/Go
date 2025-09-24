using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Contracts.Repositories
{
    public interface ITransportRepository
    {
        Task<IEnumerable<Transport>> GetAllTransportAsync();
        Task<IEnumerable<Transport>> FindUsersTransportAsync(string uid);
    }
}
