using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteDev", policy =>
    {
        policy.WithOrigins(builder.Configuration["FrontService"])
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.UseCors("AllowViteDev");

app.UseOcelot().Wait();
app.Run();
