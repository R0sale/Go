using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class UserForCreationDto
    {
        public string? FirstName { get; init; }

        public string? LastName { get; init; }

        public string? UserName { get; init; }

        public string? Password { get; init; }

        public string? Email { get; init; }

        public ICollection<string>? Roles { get; init; }
    }
}
