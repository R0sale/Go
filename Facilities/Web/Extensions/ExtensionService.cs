using Entities.Models;
using Infrastructure.Models;

namespace Facilities.Extensions
{
    public static class ExtensionService
    {
        public static void ConfigureDB(this IServiceCollection service, IConfiguration config)
        {
            service.Configure<FacilitiesDatabaseSettings>(config.GetSection("FacilitiesDatabase"));
        }
    }
}
