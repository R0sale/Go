using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class FacilitiesDatabaseSettings
    {
        public string? ConnectionString { get; set; } = null;
        public string? DatabaseName { get; set; } = null;
        public string? FacilitiesCollectionName { get; set; } = null;
    }
}
