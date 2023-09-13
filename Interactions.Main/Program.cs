using Interactions.Database.Core;
using Interactions.Database.Dtos;
using Interactions.Domain.Common;
using Interactions.Domain.Core;
using Microsoft.EntityFrameworkCore;

var pattern = "/api/v1";
var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services
    .AddScoped<IBookDomain, BookDomain>()
    .AddLogging()
    .AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("sqlString"), x => x.MigrationsAssembly("Interactions.Database")))
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var app = builder.Build();
var scopeFactory = app.Services.GetService<IServiceScopeFactory>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region Requests

app.MapPost($"{pattern}/books", async (BookDto[] dtos) =>
{
    return await scopeFactory.CreateScope().ServiceProvider.GetService<IBookDomain>().Add(dtos);
});

app.MapGet($"{pattern}/books", async () =>
{
    return await scopeFactory.CreateScope().ServiceProvider.GetService<IBookDomain>().GetAll();
});

app.MapGet($"{pattern}/books/[id]", async (int id) =>
{
    return await scopeFactory.CreateScope().ServiceProvider.GetService<IBookDomain>().Get(id);
});

#endregion

app.Run();

