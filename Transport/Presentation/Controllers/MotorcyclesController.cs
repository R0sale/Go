using Entities.Contracts;
using Entities.Contracts.Services;
using Entities.Dto;
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
            try
            {
                var motos = await _service.GetAllMotorcyclesAsync();

                return Ok(motos);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}", Name = "MotorcycleById")]
        public async Task<IActionResult> GetMotorcycleByIdAsync(string id)
        {
            try
            {
                var moto = await _service.GetMotorcycleByIdAsync(id);

                return Ok(moto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMotorcycleAsync([FromBody] CreateMotorcycleDto motoDto)
        {
            try
            {
                var moto = await _service.CreateMotorcycleAsync(motoDto);

                return CreatedAtRoute("MotorcycleById", new { id = moto.Id }, moto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMotorcycleAsync(string id)
        {
            try
            {
                await _service.DeleteMotorcycleAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMotorcycle([FromBody] MotorcycleDto motoDto, string id)
        {
            try
            {
                await _service.UpdateMotorcycleAsync(id, motoDto);

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return BadRequest(ex.Message);
            }
        }
    }
}
