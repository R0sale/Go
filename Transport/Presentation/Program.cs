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
using RabbitMQ.Client;
using Infrastructure.RabbitMq;
using Presentation.HostedServices;

BsonClassMap.RegisterClassMap<Transport>(cm =>
{
    cm.AutoMap();
    cm.MapIdMember(c => c.Id)
        .SetSerializer(new StringSerializer(BsonType.ObjectId))
        .SetIdGenerator(StringObjectIdGenerator.Instance);

    cm.SetDiscriminator(nameof(Transport.TransportType));
    cm.GetMemberMap(c => c.TransportType).SetSerializer(new EnumSerializer<TransportType>(BsonType.String));

    cm.SetDiscriminatorIsRequired(true);

    cm.AddKnownType(typeof(Car));
    cm.AddKnownType(typeof(Motorcycle));
});

BsonClassMap.RegisterClassMap<Car>(cm =>
{
    cm.AutoMap();

    cm.MapMember(c => c.FuelType)
    .SetSerializer(new EnumSerializer<FuelType>(BsonType.String));

    cm.SetDiscriminator(nameof(Car));
});

BsonClassMap.RegisterClassMap<Motorcycle>(cm =>
{
    cm.AutoMap();

    cm.MapMember(c => c.FuelType)
    .SetSerializer(new EnumSerializer<FuelType>(BsonType.String));

    cm.SetDiscriminator(nameof(Motorcycle));
});

BsonClassMap.RegisterClassMap<Bicycle>(cm =>
{
    cm.AutoMap();

    cm.SetDiscriminator(nameof(Bicycle));
});

BsonClassMap.RegisterClassMap<Scooter>(cm =>
{
    cm.AutoMap();

    cm.SetDiscriminator(nameof(Scooter));
});

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IRpcServer, RpcServer>();
builder.Services.AddHostedService<RabbitMqInitializer>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:Host"];
    options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
    {
        AbortOnConnectFail = true,
        EndPoints = { options.Configuration }
    };
});

builder.Services.ConfigureDB(builder.Configuration);

builder.Services.CreateFirebaseApp(builder.Configuration);
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.ConfigureServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteDev", policy =>
    {
        policy.WithOrigins(builder.Configuration["FrontService"], builder.Configuration["GatewayService"])
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

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

app.ConfigureExceptionHandler();

app.UseCors("AllowViteDev");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
