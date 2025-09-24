using Entities.Contracts;
using Entities.Contracts.Services;
using Entities.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZstdSharp.Unsafe;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/transport/motorcycles")]
    public class MotorcyclesController(IMotorcycleService service) : ControllerBase
    {
        private readonly IMotorcycleService _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAllMotorcyclesAsync()
        {
            var motos = await _service.GetAllMotorcyclesAsync();

            return Ok(motos);
        }

        [HttpGet("{id}", Name = "MotorcycleById")]
        public async Task<IActionResult> GetMotorcycleByIdAsync(string id)
        {
            var moto = await _service.GetMotorcycleByIdAsync(id);

            return Ok(moto);
        }

        [HttpGet("selected/{id}")]
        public async Task<IActionResult> GetSelectedMotorcycleByIdAsync(string id)
        {
            var moto = await _service.GetSelectedMotorcycleById(id);

            return Ok(moto);
        }

        [HttpPost]
        [Authorize(Roles = "TransportManager,Admin")]
        public async Task<IActionResult> CreateMotorcycleAsync([FromBody] CreateMotorcycleDto motoDto)
        {
            var uid = User.FindFirst("UserUid").Value;

            var moto = await _service.CreateMotorcycleAsync(motoDto, uid);

            return CreatedAtRoute("MotorcycleById", new { id = moto.Id }, moto);
            
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "TransportManager,Admin")]
        public async Task<IActionResult> DeleteMotorcycleAsync(string id)
        {
            var uid = User.FindFirst("UserUid").Value;

            await _service.DeleteMotorcycleAsync(id, uid);

            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "TransportManager,Admin")]
        public async Task<IActionResult> UpdateMotorcycle([FromBody] MotorcycleDto motoDto, string id)
        {
            var uid = User.FindFirst("UserUid").Value;

            await _service.UpdateMotorcycleAsync(id, motoDto, uid);

            return NoContent();
        }
    }
}
