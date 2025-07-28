using Entities.Contracts;
using Facilities.Extensions;
using Infrastructure.Converters;
using Infrastructure;
using Application.Validators;
using Application;
using Entities.Models;
using FluentValidation.AspNetCore;
using FluentValidation;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Entities.Dtos;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
var builder = WebApplication.CreateBuilder(args);

BsonSerializer.RegisterSerializer(new DictionarySerializer());

BsonClassMap.RegisterClassMap<Facility>(cm =>
{
    cm.AutoMap();
    cm.MapIdMember(c => c.Id)
        .SetSerializer(new StringSerializer(BsonType.ObjectId))
        .SetIdGenerator(StringObjectIdGenerator.Instance);
});


builder.Services.ConfigureDB(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Infrastructure.AssemblyReference).Assembly);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new OpeningTimeToJson());
    });

builder.Services.AddScoped<IFacilityRepository, FacilitiesRepository>();
builder.Services.AddScoped<IFacilityService, FacilitiesService>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<FacilityDtoValidator>();

var app = builder.Build();

app.MapControllers();

app.Run();
