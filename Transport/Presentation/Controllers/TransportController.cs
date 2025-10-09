using Entities.Dto;
using Microsoft.AspNetCore.Mvc;
using Entities.Models;
using Application.Services;
using Entities.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Infrastructure.Redis;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/transport")]
    public class TransportController(ITransportManager manager, IDistributedCache cache) : ControllerBase
    {
        private readonly ITransportManager _manager = manager;
        private readonly IDistributedCache _cache = cache;

        [HttpPost]
        public async Task<IActionResult> FindTransportAsync([FromBody] Filter filter)
        {
            string cacheKey = "all_transport";

            var transport = await _cache.GetOrSetAsync(cacheKey, async () =>
            {
                return await _manager.GetAllTransportAsync(filter);
            });

            return Ok(transport);
        }

        [HttpGet]
        [Authorize(Roles = "TransportManager,Admin")]
        public async Task<IActionResult> GetUsersTransport()
        {
            var uid = User.FindFirst("UserUid").Value;

            var cacheKey = $"user_transport_{uid}";

            var transport = await _cache.GetOrSetAsync(cacheKey, async () =>
            {
                return await _manager.GetUsersTransport(uid);
            });

            return Ok(transport);
        }
    }
}
