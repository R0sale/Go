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

        public Task<IEnumerable<UserDto>> GetAllUsers()
        {
            var users = _userManager.Users;

            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);

            return usersDto;
        }
    }
}
