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
            var cars = await _service.GetAllCarsAsync();

            return Ok(cars);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCarAsync([FromBody] CreateCarDto carDto)
        {
            try
            {
                var car = await _service.CreateCarAsync(carDto);

                return CreatedAtRoute("CarById", new { id = car.Id }, car);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return Ok();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarAsync(string id)
        {
            try
            {
                await _service.DeleteCarAsync(id);

                return NoContent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return Ok();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar([FromBody] CarDto carDto, string id)
        {
            try
            {
                await _service.UpdateCarAsync(id, carDto);

                return NoContent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return Ok();
            }
        }
    }
}
