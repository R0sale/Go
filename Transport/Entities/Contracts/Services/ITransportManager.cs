using Entities.Dto;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Contracts.Services
{
    public interface ITransportManager
    {
        Task<IEnumerable<Transport>> GetAllTransportAsync(Filter filter);
        Task<IEnumerable<Transport>> GetUsersTransport(string uid);
    }
}
