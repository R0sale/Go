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
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin.Auth;
using System.Data;
using System.Security.Cryptography;

namespace Application
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
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

            var roles = await _userManager.GetRolesAsync(user);

            userDto.Roles = roles.ToList();

            return userDto;
        }

        public async Task CreateUserAsync(UserForCreationDto userDto)
        {
            var user = _mapper.Map<User>(userDto);

            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
                await _userManager.AddToRoleAsync(user, "user");
        }

        public async Task<UserDto> LoginUserAsync(string uid)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.FirebaseUid.Equals(uid));

            if (user is null)
                throw new UnauthorizedException($"There is no person with {uid}.");

            var userDto = _mapper.Map<UserDto>(user);

            var roles = await _userManager.GetRolesAsync(user);

            userDto.Roles = roles.ToList();

            await AddCustomClaims(user);

            return userDto;
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var user = await _userManager.Users.SingleAsync(u => u.Id.Equals(id.ToString()));

            await _userManager.DeleteAsync(user);
        }

        public async Task<UserDto> LoginUserViaGoogleAsync(UserForGoogleCreationDto userDto, string uid, string email)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.FirebaseUid.Equals(uid));

            if (user is null)
            {
                userDto.Email = email;
                userDto.FirebaseUid = uid;

                user = _mapper.Map<User>(userDto);

                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(user, "user");
            }

            await AddCustomClaims(user);

            var userResDto = _mapper.Map<UserDto>(user);

            return userResDto;
        }

        private async Task AddCustomClaims(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new Dictionary<string, object>()
            {
                { "roles", roles },
                { "firstName", user.FirstName },
                { "lastName", user.LastName },
                { "userName", user.UserName },
            };

            await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(user.FirebaseUid, claims);
        }
    }
}
