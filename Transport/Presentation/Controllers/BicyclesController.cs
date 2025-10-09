using AutoMapper;
using Entities.Contracts;
using Entities.Contracts.Services;
using Entities.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/transport/bicycles")]
    public class BicyclesController(IBicycleService service, IMapper mapper) : ControllerBase
    {
        private readonly IBicycleService _service = service;
        private readonly IMapper _mapper = mapper;

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

        [HttpGet("selected/{id}")]
        public async Task<IActionResult> GetSelectedBicycleById(string id)
        {
            var bicycle = await _service.GetBicycleByIdAsync(id);

            var bicycleKV = _mapper.Map<IEnumerable<KeyValueDtoObject>>(bicycle);

            return Ok(bicycleKV);
        }

        [HttpPost]
        [Authorize(Roles = "TransportManager,Admin")]
        public async Task<IActionResult> CreateBicycleAsync([FromBody] CreateBicycleDto bicycleDto)
        {
            var uid = User.FindFirst("UserUid").Value;

            var bicycle = await _service.CreateBicycleAsync(bicycleDto, uid);

            return CreatedAtRoute("BicycleById", new { id = bicycle.Id }, bicycle);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "TransportManager,Admin")]
        public async Task<IActionResult> DeleteBicycleAsync(string id)
        {
            var uid = User.FindFirst("UserUid").Value;

            await _service.DeleteBicycleAsync(id, uid);

            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "TransportManager,Admin")]
        public async Task<IActionResult> UpdateBicycle([FromBody] BicycleDto bicycleDto, string id)
        {
            var uid = User.FindFirst("UserUid").Value;

            await _service.UpdateBicycleAsync(id, bicycleDto, uid);

            return NoContent();
        }
    }
}
