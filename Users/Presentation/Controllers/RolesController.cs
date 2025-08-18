using Application;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/roles")]
    public class RolesController : ControllerBase
    {
        private readonly UserService _userService;

        public RolesController(UserService userService)
        {
            _userService = userService;
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
    }
}
