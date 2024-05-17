using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using WebStore.API.Application;
using WebStore.API.Infrastructure;
using WebStore.API.Service;
using WebStore.API.Service.Endpoints.Clients;
using WebStore.API.Service.Endpoints.Orders;
using WebStore.API.Service.Health;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddHealthChecks().AddWebstoreHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddSingleton<DatabaseInitializer>();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.Services.GetRequiredService<DatabaseInitializer>().InitializeDatabase();

app.MapHealthChecks("/_health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseRouting();

var group = app.MapGroup("/api/webstore")
    .WithOpenApi();

group.MapClients();
group.MapOrders();

app.RunHealthCheckBackground();
app.Run();