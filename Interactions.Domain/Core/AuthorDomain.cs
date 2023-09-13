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
    public class AuthorDomain : IAuthorDomain
    {
        private readonly ILogger<IAuthorDomain> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public AuthorDomain(ILogger<IAuthorDomain> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _scopeFactory = serviceScopeFactory;
        }

        public async Task<AuthorDto> Get(int id)
        {
            var methodName = nameof(Get);
            _logger.Log(LogLevel.Trace, $"{methodName} - Called.");
            using var databaseContext = _scopeFactory.CreateScope().ServiceProvider.GetService<DatabaseContext>();
            {
                var result = await databaseContext.Authors.FindAsync(id);

                if (result != null)
                {
                    return result.Adapt<AuthorDto>();
                }

                return null;
            }
        }

        public async Task<IEnumerable<AuthorDto>> GetAll()
        {
            var methodName = nameof(GetAll);
            _logger.Log(LogLevel.Trace, $"{methodName} - Called.");
            using var databaseContext = _scopeFactory.CreateScope().ServiceProvider.GetService<DatabaseContext>();
            {
                var result = await databaseContext.Authors.ToListAsync();
                var collection = new List<AuthorDto>();

                foreach(var author in result)
                {
                    var dto = author.Map();
                    collection.Add(dto);
                }

                return collection;
            }
        }

        public async Task<HttpResponseMessage> Remove(AuthorDto item)
        {
            var methodName = nameof(Remove);
            _logger.Log(LogLevel.Trace, $"{methodName} - Called.");
            using var databaseContext = _scopeFactory.CreateScope().ServiceProvider.GetService<DatabaseContext>();
            {
                var entity = item.Adapt<AuthorEntity>();
                var result = databaseContext.Authors.Remove(entity);

                if (result.State == EntityState.Deleted)
                {
                    _logger.Log(LogLevel.Debug, $"{methodName} - Author {entity.Name} with all related books has been deleted");
                    await databaseContext.SaveChangesAsync();

                    return new HttpResponseMessage(HttpStatusCode.OK);
                }

                _logger.Log(LogLevel.Warning, $"{methodName} - Cannot delete author {entity.Name} in case of validation");

                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        public async Task<HttpResponseMessage> Update(AuthorDto item)
        {
            var methodName = nameof(Update);
            _logger.Log(LogLevel.Trace, $"{methodName} - Called.");
            using var databaseContext = _scopeFactory.CreateScope().ServiceProvider.GetService<DatabaseContext>();
            {
                var entity = item.Adapt<AuthorEntity>();
                var result = databaseContext.Authors.Update(entity);

                if (result.State == EntityState.Modified)
                {
                    _logger.Log(LogLevel.Debug, $"{methodName} - Author {entity.Name} has been modified");
                    await databaseContext.SaveChangesAsync();

                    return new HttpResponseMessage(HttpStatusCode.OK);
                }

                _logger.Log(LogLevel.Warning, $"{methodName} - Cannot modify author {entity.Name} in case of validation");

                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        async Task<HttpResponseMessage> IDomain<AuthorDto>.Add(AuthorDto[] items)
        {
            return new HttpResponseMessage(HttpStatusCode.NotImplemented);
        }
    }
}
