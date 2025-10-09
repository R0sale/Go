using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class OpeningTime
    {
        public TimeOnly Opening { get; set; }
        public TimeOnly Closing { get; set; }

        public override string ToString()
        {
            return $"{Opening.Hour.ToString("D2")}:{Opening.Minute.ToString("D2")}-{Closing.Hour.ToString("D2")}:{Closing.Minute.ToString("D2")}";
        }
    }
}
