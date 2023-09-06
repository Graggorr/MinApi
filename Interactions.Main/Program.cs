using Interactions.Database.Core;
using Interactions.Database.Dtos;
using System.Net;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using Mapster;
using Interactions.Database.Entities;

var pattern = "localhost:5007/api/v1";
var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services
    .AddLogging()
    .AddDbContext<DatabaseContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("sqlString"), x => x.MigrationsAssembly("Interactions.Database")))
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var app = builder.Build();
var logger = app.Services.GetService<ILogger<DatabaseContext>>();
var scopeFactory = app.Services.GetService<IServiceScopeFactory>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

#region Requests

app.MapPost($"{pattern}/books", async (BookDto dto) =>
{
    logger.Log(LogLevel.Trace, "Add new book called");
    HttpResponseMessage httpResponseMessage;
    using var databaseContext = scopeFactory.CreateScope().ServiceProvider.GetService<DatabaseContext>();
    {
        try
        {
            var entity = dto.Adapt<BookEntity>();
            await databaseContext.Books.AddAsync(entity);
            httpResponseMessage = new HttpResponseMessage(HttpStatusCode.Created);
        }
        catch (Exception exception)
        {
            logger.Log(LogLevel.Error, $"Cannot add a new book into the database in case of ERROR: {exception.Message}");
            httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
    }

    return httpResponseMessage;
});

app.MapGet($"{pattern}/books", async () =>
{
    logger.Log(LogLevel.Debug, "Get all books called");
    using var databaseContext = scopeFactory.CreateScope().ServiceProvider.GetService<DatabaseContext>();
    {
        return await databaseContext.Books.ToListAsync();
    }
});

app.MapGet($"{pattern}/books/[id]", async (int id) =>
{
    logger.Log(LogLevel.Debug, "Get book by ID called");
    using var databaseContext = scopeFactory.CreateScope().ServiceProvider.GetService<DatabaseContext>();
    {
        return await databaseContext.Books.FindAsync(id);
    }
});

#endregion

app.Run();

