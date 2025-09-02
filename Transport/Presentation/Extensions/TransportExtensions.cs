using Application.Services;
using Entities.Contracts.Repositories;
using Entities.Contracts.Services;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Infrastructure;
using Infrastructure.Configuration;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace Presentation.Extensions
{
    public static class TransportExtensions
    {
        public static void ConfigureDB(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<TransportDatabaseSettings>(config.GetSection("TransportDatabase"));
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
                            var authHeaders = context.HttpContext.Request.Headers["Authorization"].ToString();

                            if (!authHeaders.StartsWith("Bearer "))
                                throw new ArgumentException("Your firebase id token isn't valid.");

                            var token = authHeaders.Substring("Bearer ".Length).Trim();

                            var decoded = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);

                            var roles = decoded.Claims["roles"].ToString();

                            var rolesList = JsonSerializer.Deserialize<List<string>>(roles);

                            if (rolesList is null)
                                throw new ArgumentException("Your firebase id token doesn't have roles.");

                            var identity = context.Principal.Identity as ClaimsIdentity;
                            identity.AddClaim(new Claim("UserUid", decoded.Uid));

                            foreach (var role in rolesList)
                                identity.AddClaim(new Claim(ClaimTypes.Role, role));
                        }
                    };
                });
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<ICarService, CarService>();

            services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
            services.AddScoped<IMotorcycleService, MotorcycleService>();

            services.AddScoped<IScooterRepository, ScooterRepository>();
            services.AddScoped<IScooterService, ScooterService>();

            services.AddScoped<IBicycleRepository, BicycleRepository>();
            services.AddScoped<IBicycleService, BicycleService>();

            services.AddScoped<ITransportManager, TransportManager>();
        }
    }
}
