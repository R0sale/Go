using Entities.Contracts;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var usersDto = await _userService.GetAllUsersAsync();

            return Ok(usersDto);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAllUsers(Guid id)
        {
            var userDto = await _userService.GetUserByIdAsync(id);

            return Ok(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserForCreationDto userDto)
        {
            await _userService.CreateUserAsync(userDto);

            return Created();
        }

        [Authorize]
        [HttpPost("google")]
        public async Task<IActionResult> CreateUserFromGoogleAsync([FromBody] UserForGoogleCreationDto userDto)
        {
            var fireBaseUid = User.FindFirst("UserUid");
            var email = User.FindFirst("Email");

            await _userService.LoginUserViaGoogleAsync(userDto, fireBaseUid.Value, email.Value);

            return Ok();
        }

        [Authorize]
        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync()
        {
            var firebaseUid = User.FindFirst("UserUid");

            var userDto = await _userService.LoginUserAsync(firebaseUid.Value);

            return Ok(userDto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUserAsync(Guid id)
        {
            await _userService.DeleteUserAsync(id);

            return NoContent();
        }
    }
}
