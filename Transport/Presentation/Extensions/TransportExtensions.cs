using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Extensions
{
    public static class TransportExtensions
    {
        public static void ConfigureDB(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<TransportContext>(opts =>
            {
                opts.UseSqlServer(config.GetConnectionString("sqlConnection"));
            });
        }
    }
}
