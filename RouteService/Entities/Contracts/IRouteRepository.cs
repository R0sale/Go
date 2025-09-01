using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Contracts
{
    public interface IRouteRepository
    {
        Task<IEnumerable<Route>> GetAllRoutesAsync();
        Task<Route> GetRouteByIdAsync(string id);
        Task CreateRouteAsync(Route route);
        Task UpdateRouteAsync(Route route);
        Task DeleteRouteAsync(string id);
    }
}
