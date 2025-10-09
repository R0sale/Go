using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public class DeletionIsNotAllowedException : ForbiddenException
    {
        public DeletionIsNotAllowedException(string uid) : base($"User with uid: {uid} can't delete this transport.")
        { }
    }
}
