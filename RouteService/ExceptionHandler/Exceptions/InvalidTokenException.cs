using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionHandler.Exceptions
{
    public class InvalidTokenException : UnauthorizedException
    {
        public InvalidTokenException(string message) : base(message)
        {
        }
    }
}
