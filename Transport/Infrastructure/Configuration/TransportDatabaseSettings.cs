using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    public class TransportDatabaseSettings
    {
        public string? ConnectionString { get; set; } = null;
        public string? DatabaseName { get; set; } = null;
        public string? TransportCollectionName { get; set; } = null;
    }
}
