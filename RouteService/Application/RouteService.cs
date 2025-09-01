using AutoMapper;
using Entities.Contracts;
using Entities.Dtos;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class RouteService(IRouteRepository repository, IMapper mapper) : IRouteService
    {
        private readonly IRouteRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<RouteDto>> GetAllRoutesAsync()
        {
            var routes = await _repository.GetAllRoutesAsync();

            var routesDto = _mapper.Map<IEnumerable<RouteDto>>(routes);

            return routesDto;
        }

        public async Task<RouteDto> GetRouteByIdAsync(string id)
        {
            var route = await _repository.GetRouteByIdAsync(id);

            var routeDto = _mapper.Map<RouteDto>(route);

            return routeDto;
        }   

        public async Task<Route> CreateRouteAsync(CreateRouteDto route)
        {
            var routeEntity = _mapper.Map<Route>(route);

            await _repository.CreateRouteAsync(routeEntity);

            return routeEntity;
        }

        public async Task UpdateRouteAsync(RouteDto updatedRoute, string id)
        {
            var route = _mapper.Map<Route>(updatedRoute);

            await _repository.UpdateRouteAsync(route);   
        }

        public async Task DeleteRouteAsync(string id) =>
            await _repository.DeleteRouteAsync(id);
    }
}
