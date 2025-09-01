using Entities.Dtos;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Contracts
{
    public interface IRouteService
    {
        Task<RouteDto> GetRouteByIdAsync(string id);
        Task<IEnumerable<RouteDto>> GetAllRoutesAsync();
        Task<Route> CreateRouteAsync(CreateRouteDto route);
        Task UpdateRouteAsync(RouteDto updatedRoute, string id);
        Task DeleteRouteAsync(string id);
    }
}
