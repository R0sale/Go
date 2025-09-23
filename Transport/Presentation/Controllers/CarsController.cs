using Entities.Contracts;
using Entities.Contracts.Services;
using Entities.Dto;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> GetCarByIdAsync(string id)
        {
            var car = await _service.GetCarByIdAsync(id);

            return Ok(car);
        }

        [HttpGet("selected/{id}")]
        public async Task<IActionResult> GetSelectedCarById(string id)
        {
            var car = await _service.GetSelectedCarById(id);

            return Ok(car);
        }

        [HttpPost]
        [Authorize(Roles = "TransportManager,Admin")]
        public async Task<IActionResult> CreateCarAsync([FromBody] CreateCarDto carDto)
        {
            var uid = User.FindFirst("UserUid").Value;

            var car = await _service.CreateCarAsync(carDto, uid);

            return CreatedAtRoute("CarById", new { id = car.Id }, car);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "TransportManager,Admin")]
        public async Task<IActionResult> DeleteCarAsync(string id)
        {
            var uid = User.FindFirst("UserUid").Value;

            await _service.DeleteCarAsync(id, uid);

            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "TransportManager,Admin")]
        public async Task<IActionResult> UpdateCar([FromBody] CarDto carDto, string id)
        {
            var uid = User.FindFirst("UserUid").Value;

            await _service.UpdateCarAsync(id, carDto, uid);

            return NoContent();
        }
    }
}
