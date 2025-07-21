using Application.Contracts;
using Facilities.Extensions;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);



builder.Services.ConfigureDB(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Application.AssemblyReference).Assembly);
builder.Services.AddControllers();
builder.Services.AddScoped<IFacilityRepository, FacilitiesRepository>();

var app = builder.Build();

app.MapControllers();

app.Run();
