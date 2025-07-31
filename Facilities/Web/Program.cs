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
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
using Serilog;
using Web.Extensions;

using var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

Log.Logger = logger;

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

app.ConfigureExceptionHandler(logger);

app.MapControllers();

app.Run();
