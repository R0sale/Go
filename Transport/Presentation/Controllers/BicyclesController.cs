using Entities.Contracts;
using Entities.Contracts.Services;
using Entities.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/transport/bicycles")]
    public class BicyclesController(IBicycleService service) : ControllerBase
    {
        private readonly IBicycleService _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAllBicyclesAsync()
        {
            var bicycles = await _service.GetAllBicyclesAsync();

            return Ok(bicycles);
        }

        [HttpGet("{id}", Name = "BicycleById")]
        public async Task<IActionResult> GetBicycleByIdAsync(string id)
        {
            var bicycle = await _service.GetBicycleByIdAsync(id);

            return Ok(bicycle);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBicycleAsync([FromBody] CreateBicycleDto bicycleDto)
        {
            var bicycle = await _service.CreateBicycleAsync(bicycleDto);

            return CreatedAtRoute("BicycleById", new { id = bicycle.Id }, bicycle);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBicycleAsync(string id)
        {
            await _service.DeleteBicycleAsync(id);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBicycle([FromBody] BicycleDto bicycleDto, string id)
        {
            await _service.UpdateBicycleAsync(id, bicycleDto);

            return NoContent();
        }
    }
}
