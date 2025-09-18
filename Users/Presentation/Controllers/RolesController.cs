using Application;
using Entities.Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/users/roles")]
    public class RolesController : ControllerBase
    {
        private readonly IUserService _userService;

        public RolesController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut("{uid}")]
        [Authorize]
        public async Task<IActionResult> ChangeUserRole([FromBody] IEnumerable<string> roles, string uid)
        {
            await _userService.ChangeUsersRolesAsync(uid, roles);

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("admin")]
        public async Task<IActionResult> GiveAdminRole([FromBody] Guid id)
        {
            await _userService.GiveUserRoleAsync(id, "Admin");

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("owner")]
        public async Task<IActionResult> GiveOwnerRole([FromBody] Guid id)
        {
            await _userService.GiveUserRoleAsync(id, "Owner");

            return Ok();
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost("transportmanager")]
        public async Task<IActionResult> GiveTransportManagerRole([FromBody] Guid id)
        {
            await _userService.GiveUserRoleAsync(id, "TransportManager");

            return Ok();
        }
    }
}
