using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Dtos;

namespace Entities.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(Guid id);
        Task CreateUserAsync(UserForCreationDto user);
        Task DeleteUserAsync(Guid id);
    }
}
