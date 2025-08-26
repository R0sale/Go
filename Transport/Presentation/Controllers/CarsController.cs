using Entities.Contracts;
using Entities.Contracts.Services;
using Entities.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/transport/cars")]
    public class CarsController(ICarService service) : ControllerBase
    {
        private readonly ICarService _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAllCarsAsync()
        {
            var cars = await _service.GetAllCarsAsync();

            return Ok(cars);
        }

        [HttpGet("{id:guid}", Name = "CarById")]
        public async Task<IActionResult> GetAllCarsAsync(Guid id)
        {
            var cars = await _service.GetAllCarsAsync();

            return Ok(cars);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCarAsync([FromBody] CreateCarDto carDto)
        {
            var car = await _service.CreateCarAsync(carDto);

            return CreatedAtRoute("CarById", new { id = car.Id}, car);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCarAsync(Guid id)
        {
            await _service.DeleteCarAsync(id.ToString());

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCar([FromBody] CarDto carDto, Guid id)
        {
            await _service.UpdateCarAsync(id.ToString(), carDto);

            return NoContent();
        }
    }
}
