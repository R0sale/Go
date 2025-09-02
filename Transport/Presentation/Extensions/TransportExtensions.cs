using Infrastructure;
using Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Extensions
{
    public static class TransportExtensions
    {
        public static void ConfigureDB(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<TransportDatabaseSettings>(config.GetSection("TransportDatabase"));
        }
    }
}
