using Infrastructured;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UsersContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"));
});

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
