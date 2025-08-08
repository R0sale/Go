using Entities.Contracts;
using Entities.Dtos;
using Microsoft.AspNetCore.Identity;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ExceptionHandler.Exceptions;

namespace Application
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users).ToList();

            foreach (var (user, userDto) in users.Zip(usersDto))
            {
                var roles = await _userManager.GetRolesAsync(user);

                userDto.Roles = roles.ToList();
            }

            return usersDto;
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id.Equals(id.ToString()));

            if (user is null)
                throw new NotFoundException($"User with id: {id}");

            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }

        public async Task CreateUserAsync(UserForCreationDto userDto)
        {
            var user = _mapper.Map<User>(userDto);

            await _userManager.CreateAsync(user);
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var user = await _userManager.Users.SingleAsync(u => u.Id.Equals(id.ToString()));

            await _userManager.DeleteAsync(user);
        }
    }
}
