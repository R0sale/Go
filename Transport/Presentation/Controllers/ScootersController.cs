using AutoMapper;
using Entities.Contracts;
using Entities.Contracts.Services;
using Entities.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/transport/scooters")]
    public class ScootersController(IScooterService service, IMapper mapper) : ControllerBase
    {
        private readonly IScooterService _service = service;
        private readonly IMapper _mapper = mapper;

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

        [HttpGet("selected/{id}")]
        public async Task<IActionResult> GetSelectedScooterByIdAsync(string id)
        {
            var scooter = await _service.GetScooterByIdAsync(id);

            var scooterKV = _mapper.Map<IEnumerable<KeyValueDtoObject>>(scooter);

            return Ok(scooterKV);
        }

        [HttpPost]
        [Authorize(Roles = "TransportManager,Admin")]
        public async Task<IActionResult> CreateScooterAsync([FromBody] CreateScooterDto scooterDto)
        {
            var uid = User.FindFirst("UserUid").Value;

            var scooter = await _service.CreateScooterAsync(scooterDto, uid);

            return CreatedAtRoute("ScooterById", new { id = scooter.Id }, scooter);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "TransportManager,Admin")]
        public async Task<IActionResult> DeleteScooterAsync(string id)
        {
            var uid = User.FindFirst("UserUid").Value;

            await _service.DeleteScooterAsync(id, uid);

            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "TransportManager,Admin")]
        public async Task<IActionResult> UpdateScooter([FromBody] ScooterDto scooterDto, string id)
        {
            var uid = User.FindFirst("UserUid").Value;

            await _service.UpdateScooterAsync(id, scooterDto, uid);

            return NoContent();
        }
    }
}
