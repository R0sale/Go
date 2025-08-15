using Microsoft.AspNetCore.Mvc;
using Entities.Contracts;
using Entities.Dtos;
using Microsoft.AspNetCore.JsonPatch;   
using Entities.Models;
using AutoMapper;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;

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

        [Authorize(Roles = "Admin")]
        [HttpGet("myfacilities")]
        public async Task<IActionResult> GetMyFacilities()
        {
            var uid = User.FindFirst("UserUid");

            return Ok(await _service.GetUsersFacilitiesAsync(uid.Value));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateFacility([FromBody] FacilityDto facilityDto)
        {
            var uid = User.FindFirst("UserUid");

            var facility = await _service.CreateFacilityAsync(facilityDto, uid.Value);

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
        public async Task<IActionResult> GetFacilitiesNearBy([FromQuery] GeoModel geoModel)
        {
            var list = await _service.GetFacilitiesNearByAsync(geoModel.Latitude, geoModel.Longitude, geoModel.RadiusKm);

            return Ok(list);
        }
    }
}
