using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public class UserIdNotFoundException : NotFoundException
    {
        public UserIdNotFoundException(string message) : base(message)
        { }
    }
}
