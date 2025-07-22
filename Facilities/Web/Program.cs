using Application.Contracts;
using Facilities.Extensions;
using Infrastructure;
using Application;
using FluentValidation.AspNetCore;
using Application.Validators;
using FluentValidation;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
var builder = WebApplication.CreateBuilder(args);

BsonSerializer.RegisterSerializer(new DictionarySerializer());

builder.Services.ConfigureDB(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Application.AssemblyReference).Assembly);
builder.Services.AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddScoped<IFacilityRepository, FacilitiesRepository>();
builder.Services.AddScoped<IFacilityService, FacilitiesService>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<FacilityDtoValidator>();

var app = builder.Build();

app.MapControllers();

app.Run();
