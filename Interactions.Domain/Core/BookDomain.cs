using Interactions.Database.Core;
using Interactions.Database.Dtos;
using Interactions.Database.Entities;
using Interactions.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;
using Mapster;

namespace Interactions.Domain.Core
{
    public class BookDomain : IBookDomain
    {
        private readonly ILogger<BookDomain> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IAdapterBuilder<BookDto> _builder;

        public BookDomain(ILogger<BookDomain> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task<HttpResponseMessage> Add(BookDto[] items)
        {
            var methodName = nameof(Add);
            _logger.Log(LogLevel.Trace, $"{methodName} - Called.");

            using var databaseContext = _scopeFactory.CreateScope().ServiceProvider.GetService<DatabaseContext>();
            {
                foreach (var item in items)
                {
                    if (!await AddOrUpdateAuthorCore(item, databaseContext))
                    {
                        _logger.Log(LogLevel.Warning, $"{methodName} - Cannot add a new book {item.Name} into the database in case of invalid Author");

                        return new HttpResponseMessage(HttpStatusCode.BadRequest);
                    }
                }

                await databaseContext.SaveChangesAsync();
                _logger.Log(LogLevel.Debug, $"{methodName} - A new book has been added into the database");

                return new HttpResponseMessage(HttpStatusCode.Created);
            }
        }

        public async Task<BookDto> Get(int id)
        {
            var methodName = nameof(Get);
            _logger.Log(LogLevel.Trace, $"{methodName} - Called");

            using var databaseContext = _scopeFactory.CreateScope().ServiceProvider.GetService<DatabaseContext>();
            {
                var entity = await databaseContext.Books.FindAsync(id);

                if (entity == null)
                {
                    _logger.Log(LogLevel.Debug, $"{methodName} - There is no any book in database with chosen ID: {id}");

                    return null;
                }

                return entity.Map();
            }
        }

        public async Task<IEnumerable<BookDto>> GetAll()
        {
            var methodName = nameof(GetAll);
            _logger.Log(LogLevel.Trace, $"{methodName} - Called");

            using var databaseContext = _scopeFactory.CreateScope().ServiceProvider.GetService<DatabaseContext>();
            {
                var result = await databaseContext.Books.ToListAsync();
                var collection = new List<BookDto>();

                foreach (var book in result)
                {
                    var dto = book.Map();
                    collection.Add(dto);
                }

                return collection;
            }
        }

        public async Task<HttpResponseMessage> Remove(BookDto item)
        {
            var methodName = nameof(Remove);
            _logger.Log(LogLevel.Trace, $"{methodName} - Called");
            HttpResponseMessage httpResponseMessage;
            using var databaseContext = _scopeFactory.CreateScope().ServiceProvider.GetService<DatabaseContext>();
            {
                try
                {
                    var entity = item.Adapt<BookEntity>();
                    var result = databaseContext.Books.Remove(entity);

                    if (result.State == EntityState.Deleted)
                    {
                        _logger.Log(LogLevel.Debug, $"{methodName} - Book with ID: {result.Entity.Id} has been deleted successfully");
                        httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
                    }
                    else
                    {
                        throw new Exception($"Cannot delete book with ID: {entity.Id} from the database");
                    }

                    await databaseContext.SaveChangesAsync();
                }
                catch (Exception exception)
                {
                    _logger.Log(LogLevel.Error, $"{methodName} - ERROR: {exception.Message}");
                    httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                }
            }

            return httpResponseMessage;
        }

        public async Task<HttpResponseMessage> Update(BookDto item)
        {
            var methodName = nameof(Remove);
            _logger.Log(LogLevel.Trace, $"{methodName} - Called");

            using var databaseContext = _scopeFactory.CreateScope().ServiceProvider.GetService<DatabaseContext>();
            {
                var entity = item.Adapt<BookEntity>();
                var result = databaseContext.Books.Update(entity);

                if (result.State == EntityState.Modified)
                {
                    _logger.Log(LogLevel.Debug, $"{methodName} - Book with ID: {result.Entity.Id} has been modified");
                    await databaseContext.SaveChangesAsync();

                    return new HttpResponseMessage(HttpStatusCode.OK);
                }

                _logger.Log(LogLevel.Debug, $"{methodName} - cannot update chosen book");
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        private async Task<bool> AddOrUpdateAuthorCore(BookDto dto, DatabaseContext databaseContext)
        {
            var methodName = nameof(AddOrUpdateAuthorCore);
            var authors = new List<AuthorEntity>();
            _logger.Log(LogLevel.Trace, $"{methodName} - called");
            var authorsSet = databaseContext.Authors;

            foreach (var author in dto.Authors)
            {
                var result = authorsSet.Local.FirstOrDefault(x => x.Name.Equals(author, StringComparison.CurrentCultureIgnoreCase));

                if (result != null)
                {
                    authors.Add(result);

                    continue;
                }

                var addResult = await authorsSet.AddAsync(new AuthorEntity
                {
                    Name = author,
                    Books = new List<BookEntity>()
                    {
                       dto.Adapt<BookEntity>()
                    }
                });

                if (addResult.State == EntityState.Added)
                {
                    _logger.Log(LogLevel.Debug, $"{methodName} - A new author {author} has been added into the database");

                    continue;
                }

                _logger.Log(LogLevel.Warning, $"{methodName} - Cannot add a new author {author} into the database");

                return false;
            }

            foreach (var author in authors)
            {
                var entity = dto.Adapt<BookEntity>();
                author.Books.Add(entity);
                var result = authorsSet.Update(author);

                if (result.State == EntityState.Modified)
                {
                    _logger.Log(LogLevel.Debug, $"{methodName} - Author {author.Name} has been updated");

                    continue;
                }

                _logger.Log(LogLevel.Warning, $"{methodName} - Cannot update author {author.Name}");

                return false;
            }

            return true;
        }
    }
}
