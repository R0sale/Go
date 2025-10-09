using Microsoft.AspNetCore.SignalR;
using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.ConfigureDB(builder.Configuration);

builder.Services.CreateFirebaseApp(builder.Configuration);
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteDev",
        policy =>
        {
            policy.WithOrigins(builder.Configuration["FrontService"], builder.Configuration["GatewayService"])
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.ConfigureServices();

var app = builder.Build();

app.UseCors("AllowViteDev");

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandler.ExceptionHandler.ExceptionHandler>();

app.MapControllers();

app.Run();
