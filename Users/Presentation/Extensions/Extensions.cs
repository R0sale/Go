using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin.Auth;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Cryptography.X509Certificates;
using System.IdentityModel.Tokens.Jwt;
using ExceptionHandler.Exceptions;
using System.Security.Claims;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Builder.Extensions;

namespace Presentation.Extensions
{
    public static class Extensions
    {
        public static void AddFirebaseAuth(this IServiceCollection services, IConfiguration config)
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(config["Firebase:KeyPath"])
            });
        }

        public static void ConfigureDb(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<UsersContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("sqlConnection"));
            });
        }

        public static void AddAppAuthentication(this IServiceCollection services, IConfiguration config)
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
                            var db = context.HttpContext.RequestServices.GetRequiredService<UsersContext>();
                            var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();

                            if (!authHeader.StartsWith("Bearer "))
                                throw new NotValidTokenException("Your auth token is unacceptable.");

                            var token = authHeader.Substring("Bearer ".Length).Trim();

                            var decoded = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
                            var uid = decoded.Uid;

                            var user = await db.Users
                                .FirstOrDefaultAsync(u => u.FirebaseUid == uid);

                            var roles = await (from ur in db.UserRoles
                                               join r in db.Roles on ur.RoleId equals r.Id
                                               where ur.UserId == uid
                                               select r.Name).ToListAsync();

                            if (user is null)
                            {
                                await ModifyBodyAsync(context.HttpContext, uid);
                            }

                            var rolesClaims = JsonSerializer.Serialize(roles);

                            var identity = (ClaimsIdentity)context.Principal.Identity;
                            identity.AddClaim(new Claim("UserUid", uid));
                            identity.AddClaim(new Claim("Email", decoded.Claims["email"].ToString()));

                            foreach (var role in roles)
                                identity.AddClaim(new Claim(ClaimTypes.Role, role));
                        },

                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine($"Auth failed: {context.Exception}");
                            return Task.CompletedTask;
                        }
                    };

                });
        }

        private static async Task ModifyBodyAsync(HttpContext context, string uid)
        {
                context.Request.EnableBuffering();

                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
                var bodyString = await reader.ReadToEndAsync();

                context.Request.Body.Position = 0;

                if (string.IsNullOrWhiteSpace(bodyString))
                    throw new NotRegisteredException("You are trying to log in, however you don't have an account. Please sign up.");

                var bodyJson = JsonDocument.Parse(bodyString);

                var bodyObj = JsonSerializer.Deserialize<Dictionary<string, object>>(bodyJson);

                bodyObj["FirebaseUid"] = uid;

                var modifiedBody = JsonSerializer.Serialize(bodyObj);

                var body = Encoding.UTF8.GetBytes(modifiedBody);
                context.Request.Body = new MemoryStream(body);
                context.Request.ContentLength = body.Length;
        }
    }
}
