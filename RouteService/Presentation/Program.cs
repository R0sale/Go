using Microsoft.AspNetCore.SignalR;
using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.ConfigureDB(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteDev",
        policy =>
        {
            policy.WithOrigins(builder.Configuration.GetSection("FrontService").ToString())
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.ConfigureServices();

var app = builder.Build();

app.UseCors("AllowViteDev");

app.MapControllers();

app.Run();
