using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionHandler.Exceptions
{
    public class NotRegisteredException : BadRequestException
    {
        public NotRegisteredException(string message) : base(message)
        { }
    }
}
