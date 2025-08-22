using Entities.Exceptions;
using Entities.Models;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.Text.Json;

namespace Facilities.Extensions
{
    public static class ExtensionService
    {
        public static void ConfigureDB(this IServiceCollection service, IConfiguration config)
        {
            service.Configure<FacilitiesDatabaseSettings>(config.GetSection("FacilitiesDatabase"));
        }

        public static void CreateFirebaseApp(this IServiceCollection service, IConfiguration config)
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(config["Firebase:KeyPath"])
            });
        }

        public static void ConfigureAuthentication(this IServiceCollection service, IConfiguration config)
        {
            var projectId = config["Firebase:projectId"];

            service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                                    throw new NotValidTokenException("Your firebase id token isn't valid.");

                                var token = authHeaders.Substring("Bearer ".Length).Trim();

                                var decoded = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);

                                var roles = decoded.Claims["roles"].ToString();

                                var rolesList = JsonSerializer.Deserialize<List<string>>(roles);

                                if (rolesList is null)
                                    throw new NotValidTokenException("Your firebase id token doesn't have roles.");

                                var identity = (ClaimsIdentity)context.Principal.Identity;
                                identity.AddClaim(new Claim("UserUid", decoded.Uid));

                                foreach (var role in rolesList)
                                    identity.AddClaim(new Claim(ClaimTypes.Role, role));
                        }
                    };
                });
        }
    }
}
