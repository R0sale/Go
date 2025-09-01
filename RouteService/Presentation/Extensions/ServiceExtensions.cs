using Entities.Contracts;
using Entities.Models;
using Infrastructure;
using Application;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
using Infrastructure.Configuration;

namespace Presentation.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            BsonClassMap.RegisterClassMap<Entities.Models.Route>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId))
                    .SetIdGenerator(StringObjectIdGenerator.Instance);
            });

            services.AddScoped<IRouteRepository, RouteRepository>();
            services.AddScoped<IRouteService, RouteService>();

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            }, typeof(AssemblyReference).Assembly);
        }

        public static void ConfigureDB(this IServiceCollection service, IConfiguration config)
        {
            service.Configure<RoutesDatabaseSettings>(config.GetSection("RoutesDatabase"));
        }
    }
}
