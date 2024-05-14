using WebStore.API.Application;
using WebStore.API.Infrastructure;
using WebStore.API.Service.Clients;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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