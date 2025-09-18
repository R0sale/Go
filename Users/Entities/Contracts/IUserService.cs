using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;

namespace Entities.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(Guid id);
        Task<UserDto> GetUserByUidAsync(string uid);
        Task AddImageAsync(IFormFile file, string uid);
        Task CreateUserAsync(UserForCreationDto userDto);
        Task DeleteUserAsync(Guid id);
        Task<UserDto> LoginUserAsync(string uid);
        Task<UserDto> LoginUserViaGoogleAsync(UserForGoogleCreationDto userDto, string uid, string email);
        Task GiveUserRoleAsync(Guid id, string role);
        Task ChangeUsersRolesAsync(string uid, IEnumerable<string> roles);
    }
}
