using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public class UpdateIsNotAllowedException : ForbiddenException
    {
        public UpdateIsNotAllowedException(string uid) : base($"User with uid: {uid} can't update this transport.")
        { }
    }
}
