using Microsoft.AspNetCore.Mvc;
using Application.Contracts;
using Application.Dtos;
using Entities.Models;
using AutoMapper;

namespace Facilities.Controllers
{
    [ApiController]
    [Route("api/facility")]
    public class FacilityController : ControllerBase
    {
        private readonly IFacilityService _service;

        public FacilityController(IFacilityService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetFacilities()
        {
            return Ok(await _service.GetFacilitiesAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFacility(string id)
        {
            return Ok(await _service.GetFacilityAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateFacility([FromBody] FacilityDto facilityDto)
        {
            await _service.CreateFacilityAsync(facilityDto);

            return Created();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFacility(string id)
        {
            await _service.RemoveFacilityAsync(id);

            return NoContent();
        }
    }
}
