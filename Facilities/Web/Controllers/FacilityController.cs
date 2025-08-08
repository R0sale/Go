using Microsoft.AspNetCore.Mvc;
using Entities.Contracts;
using Entities.Dtos;
using Microsoft.AspNetCore.JsonPatch;   
using Entities.Models;
using AutoMapper;
using Infrastructure.Models;

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

        [HttpGet("{id}", Name = "FacilityById")]
        public async Task<IActionResult> GetFacility(string id)
        {
            return Ok(await _service.GetFacilityAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateFacility([FromBody] FacilityDto facilityDto)
        {
            var facility = await _service.CreateFacilityAsync(facilityDto);

            return CreatedAtRoute("FacilityById", new { id = facility.Id }, facility);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFacility(string id)
        {
            await _service.RemoveFacilityAsync(id);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFacility([FromBody] FacilityDto facilityDto, string id)
        {
            await _service.UpdateFacilityAsync(id, facilityDto);
            return NoContent();
        }

        [HttpGet("near")]
<<<<<<< HEAD
        public async Task<IActionResult> GetFacilitiesNearBy([FromBody] GeoModel geoModel)
=======
        public async Task<IActionResult> GetFacilitiesNearBy([FromQuery] GeoModel geoModel)
>>>>>>> 3fa1a59384835f46066db57be6b80867c9c90520
        {
            var list = await _service.GetFacilitiesNearByAsync(geoModel.Latitude, geoModel.Longitude, geoModel.RadiusKm);

            return Ok(list);
        }
    }
}
