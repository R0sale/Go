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
        private readonly IFacilityRepository _facilityRepository;
        private readonly IMapper _mapper;

        public FacilityController(IFacilityRepository facilityRepository, IMapper mapper)
        {
            _facilityRepository = facilityRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetFacilities()
        {
            return Ok(await _facilityRepository.GetFacilities());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFacility(string id)
        {
            return Ok(await _facilityRepository.GetFacility(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateFacility([FromBody] FacilityDto facilityDto)
        {
            var facility = _mapper.Map<Facility>(facilityDto);

            await _facilityRepository.CreateFacility(facility);

            return Created();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFacility(string id)
        {
            await _facilityRepository.RemoveFacility(id);

            return NoContent();
        }
    }
}
