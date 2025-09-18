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
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Azure.Storage.Blobs.Models;

namespace Application
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly BlobContainerClient _container;
        private readonly IConfiguration _config;

        public UserService(UserManager<User> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _config = config;

            var service = new BlobServiceClient(config["ConnectionStrings:AzuriteConnection"]);

            _container = service.GetBlobContainerClient(config["Azurite:Container"]);

            _container.CreateIfNotExists();
            _container.SetAccessPolicy(PublicAccessType.Blob);
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

        public async Task<UserDto> GetUserByUidAsync(string uid)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.FirebaseUid.Equals(uid));

            if (user is null)
                throw new UserNotFoundException("There's no such a uid in a database.");

            var userDto = _mapper.Map<UserDto>(user);

            var roles = await _userManager.GetRolesAsync(user);

            userDto.Roles = roles.ToList();

            return userDto;
        }

        public async Task ChangeUsersRolesAsync(string uid, IEnumerable<string> roles)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.FirebaseUid.Equals(uid));

            if (user is null)
                throw new NotFoundException($"User with id: {uid} not found.");

            var userRoles = await _userManager.GetRolesAsync(user);

            var programRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            var res = await _userManager.RemoveFromRolesAsync(user, userRoles);

            if (!res.Succeeded)
                throw new BadRequestException($"Exception: {res.Errors} Succeeded: {res.Succeeded}");

            foreach (var role in roles)
            {
                if (!programRoles.Contains(role))
                    throw new NotFoundException("Role wasn't found.");
            }

            res = await _userManager.AddToRolesAsync(user, roles);

            if (!res.Succeeded)
                throw new BadRequestException($"Exception: {res.Errors} Succeeded: {res.Succeeded}");
        }

        public async Task CreateUserAsync(UserForCreationDto userDto)
        {
            var user = _mapper.Map<User>(userDto);

            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error.Description);
                }

                throw new BadRequestException($"Exception: {result.Errors} Succeeded: {result.Succeeded}");
            }
        }

        public async Task AddImageAsync(IFormFile file, string uid)
        {
            if (file.Length == 0 || file is null)
                throw new IncorrectFileException("Your file has 0 length, or is null.");

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.FirebaseUid.Equals(uid));

            if (user is null)
                throw new UserNotFoundException($"There is no user with uid {uid}");

            var blobClient = _container.GetBlobClient(uid);

            var headers = new BlobHttpHeaders
            {
                ContentType = $"{file.ContentType}"
            };

            var blobOptions = new BlobUploadOptions
            {
                HttpHeaders = headers,
            };

            using (var fileStream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(fileStream, blobOptions);
            }

            user.ImageUrl = _config["Azurite:ContainerUrl"] + uid;

            await _userManager.UpdateAsync(user);
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

        public async Task GiveUserRoleAsync(Guid id, string role)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id.Equals(id.ToString()));

            if (user is null)
                throw new NotFoundException($"User with Id: {id} doesn't exist");

            await _userManager.AddToRoleAsync(user, role);
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
