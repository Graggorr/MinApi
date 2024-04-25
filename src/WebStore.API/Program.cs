using WebStore.API.Clients;
using WebStore.Application;
using WebStore.Infrastructure;
using WebStore.EventBusRabbitMq;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(configuration.GetConnectionString("sqlString"));
builder.Services.AddRabbitMqEventBus(JsonSerializer.Deserialize<RabbitMqConfiguration>(configuration["RabbitMqConfiguration"]));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var group = app.MapGroup("/api/webstore")
    .WithOpenApi();

group.MapClients();

app.Run();