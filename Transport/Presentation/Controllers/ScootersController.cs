using Entities.Contracts;
using Entities.Contracts.Services;
using Entities.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/transport/scooters")]
    public class ScootersController(IScooterService service) : ControllerBase
    {
        private readonly IScooterService _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAllScootersAsync()
        {
            var scooters = await _service.GetAllScootersAsync();

            return Ok(scooters);
        }

        [HttpGet("{id}", Name = "ScooterById")]
        public async Task<IActionResult> GetScooterByIdAsync(string id)
        {
            var scooter = await _service.GetScooterByIdAsync(id);

            return Ok(scooter);
        }

        [HttpPost]
        public async Task<IActionResult> CreateScooterAsync([FromBody] CreateScooterDto scooterDto)
        {
            var scooter = await _service.CreateScooterAsync(scooterDto);

            return CreatedAtRoute("ScooterById", new { id = scooter.Id }, scooter);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScooterAsync(string id)
        {
            await _service.DeleteScooterAsync(id);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateScooter([FromBody] ScooterDto scooterDto, string id)
        {
            await _service.UpdateScooterAsync(id, scooterDto);

            return NoContent();
        }
    }
}
