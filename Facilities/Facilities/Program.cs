using Application.Contracts;
using Facilities.Extensions;
using Infrastructure;
using Application;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);



builder.Services.ConfigureDB(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Application.AssemblyReference).Assembly);
builder.Services.AddControllers();
builder.Services.AddScoped<IFacilityRepository, FacilitiesRepository>();
builder.Services.AddScoped<IFacilityService, FacilitiesService>();

var app = builder.Build();

app.MapControllers();

app.Run();
