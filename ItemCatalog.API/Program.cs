using System;
using System.Net.Mime;
using System.Security.AccessControl;
using System.Text.Json;
using ItemCatalog.API.Repositories;
using ItemCatalog.API.Settings;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Serialize the GUID as string in the database.
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

//Serialize the DateTimeOffset as string in the database.
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();

builder.Services.AddSingleton<IMongoClient>(serviceProvider => new MongoClient(mongoDbSettings.ConnectionString));
builder.Services.AddSingleton<IItemsRepository, MongoDbItemsRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks().AddMongoDb(mongoDbSettings.ConnectionString, 
                                                name: "mongodb",
                                                timeout: TimeSpan.FromSeconds(3),
                                                tags: new [] {"ready" });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("health/ready", new HealthCheckOptions {
    Predicate = (check) => check.Tags.Contains("ready"),
    ResponseWriter = async (context, report) => {
        var result = JsonSerializer.Serialize(new {
            status = report.Status.ToString(),
            checks = report.Entries.Select(entry => new {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                description = entry.Value.Description,
                exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                duration = entry.Value.Duration
            })
        });
        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(result);
    }
});

app.MapHealthChecks("health/live", new HealthCheckOptions {
    Predicate = (_) => false
});

app.Run();
