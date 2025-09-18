using Entities.Contracts;
using Entities.Models;
using Infrastructure;
using Application;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
using Infrastructure.Configuration;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ExceptionHandler.Exceptions;
using FirebaseAdmin.Auth;
using System.Security.Claims;
using System.Text.Json;

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

        public static void CreateFirebaseApp(this IServiceCollection services, IConfiguration config)
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(config["Firebase:KeyPath"])
            });
        }

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration config)
        {
            var projectId = config["Firebase:ProjectId"];

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = $"https://securetoken.google.com/{projectId}";
                    options.Audience = projectId;

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            var headers = context.HttpContext.Request.Headers["Authorization"].ToString();

                            if (!headers.StartsWith("Bearer "))
                                throw new InvalidTokenException("Your token is not valid (Doesn't start with Bearer )");

                            var token = headers.Substring("Bearer ".Length).Trim();

                            var decoded = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);

                            if (decoded == null)
                                throw new InvalidTokenException("Your token is not valid (Can't be decoded)");

                            var roles = decoded.Claims["roles"].ToString();

                            if (roles is null)
                                throw new UserDoesntHaveRolesException("Your token is not valid (No roles found)");    

                            var rolesList = JsonSerializer.Deserialize<List<string>>(roles);

                            if (rolesList is null || rolesList.Count == 0)
                                throw new UserDoesntHaveRolesException("Your token is not valid (No roles found)");

                            var uid = decoded.Uid;

                            var identity = context.Principal.Identity as ClaimsIdentity;

                            if (identity is null)
                                throw new NullIdentityException("Context doesn't have your identity.");

                            foreach (var role in rolesList)
                                identity.AddClaim(new Claim(ClaimTypes.Role, role));

                            identity.AddClaim(new Claim("UserUid", uid));
                        }
                    };

                });
        }
    }
}
