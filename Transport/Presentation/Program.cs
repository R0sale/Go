using Application;
using Application.Services;
using Entities.Contracts;
using Entities.Contracts.Repositories;
using Entities.Contracts.Services;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using Presentation.Extensions;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using Entities.Models;
using Entities.Models.Transports;

BsonClassMap.RegisterClassMap<Transport>(cm =>
{
    cm.AutoMap();
    cm.MapIdMember(c => c.Id)
        .SetSerializer(new StringSerializer(BsonType.ObjectId))
        .SetIdGenerator(StringObjectIdGenerator.Instance);

    cm.MapMember(c => c.Type)
    .SetSerializer(new EnumSerializer<TransportType>(BsonType.String));
});

BsonClassMap.RegisterClassMap<Car>(cm =>
{
    cm.AutoMap();

    cm.MapMember(c => c.FuelType)
    .SetSerializer(new EnumSerializer<FuelType>(BsonType.String));
});

BsonClassMap.RegisterClassMap<Motorcycle>(cm =>
{
    cm.AutoMap();

    cm.MapMember(c => c.FuelType)
    .SetSerializer(new EnumSerializer<FuelType>(BsonType.String));
});

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureDB(builder.Configuration);

builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<ICarService, CarService>();

builder.Services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
builder.Services.AddScoped<IMotorcycleService, MotorcycleService>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
}, typeof(AssemblyReference).Assembly);

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
