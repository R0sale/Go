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

        [HttpGet("{id}", Name = "CarById")]
        public async Task<IActionResult> GetAllCarsAsync(string id)
        {
            var car = await _service.GetCarByIdAsync(id);

            return Ok(car);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCarAsync([FromBody] CreateCarDto carDto)
        {
            var car = await _service.CreateCarAsync(carDto);

            return CreatedAtRoute("CarById", new { id = car.Id }, car);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarAsync(string id)
        {
            await _service.DeleteCarAsync(id);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar([FromBody] CarDto carDto, string id)
        {
            await _service.UpdateCarAsync(id, carDto);

            return NoContent();
        }
    }
}
