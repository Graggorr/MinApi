using WebStore.API.Clients;
using WebStore.API.Extensions;
using WebStore.Application;
using WebStore.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddWebStoreOptions();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(configuration.GetConnectionString("sqlString"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebStore API");
        c.RoutePrefix = "";
    });
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();
app.UseRouting();


var group = app.MapGroup("/api/webstore")
    .WithOpenApi();

group.MapClients();

app.Run();