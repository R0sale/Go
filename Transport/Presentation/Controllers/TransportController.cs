using Entities.Dto;
using Microsoft.AspNetCore.Mvc;
using Entities.Models;
using Application.Services;
using Entities.Contracts.Services;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/transport")]
    public class TransportController(ITransportManager manager) : ControllerBase
    {
        private readonly ITransportManager _manager = manager;

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> FindTransportAsync([FromBody] Filter filter)
        {
            var transport = await _manager.GetAllTransportAsync(filter);

            return Ok(transport);
        }
    }
}
