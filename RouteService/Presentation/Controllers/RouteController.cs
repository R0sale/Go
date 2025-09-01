using Entities.Contracts;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/routes")]
    public class RouteController(IRouteService service) : ControllerBase
    {
        private readonly IRouteService _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAllRoutes()
        {
            var routes = await _service.GetAllRoutesAsync();

            return Ok(routes);
        }

        [HttpGet("{id}")]   
        public async Task<IActionResult> GetRouteById(string id)
        {
            var route = await _service.GetRouteByIdAsync(id);

            return Ok(route);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoute([FromBody] CreateRouteDto routeToCreate)
        {
            var route = await _service.CreateRouteAsync(routeToCreate);

            return CreatedAtAction(nameof(GetRouteById), new { id = route.Id }, route);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> DeleteRouteAsync(string id, RouteDto updatedRoute)
        {
            await _service.UpdateRouteAsync(updatedRoute, id);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRouteAsync(string id)
        {
            await _service.DeleteRouteAsync(id);

            return NoContent();
        }   
    }
}
