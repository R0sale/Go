using Entities.Dto;
using Microsoft.AspNetCore.Mvc;
using Entities.Models;
using Application.Services;
using Entities.Contracts.Services;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/transport")]
    public class TransportController(ITransportManager manager) : ControllerBase
    {
        private readonly ITransportManager _manager = manager;

        [HttpPost]
        public async Task<IActionResult> FindTransportAsync([FromBody] Filter filter)
        {
            var transport = await _manager.GetAllTransportAsync(filter);

            return Ok(transport);
        }

        [HttpGet]
        [Authorize(Roles = "TransportManager,Admin")]
        public async Task<IActionResult> GetUsersTransport()
        {
            var uid = User.FindFirst("UserUid").Value;

            var transport = await _manager.GetUsersTransport(uid);

            return Ok(transport);
        }
    }
}
