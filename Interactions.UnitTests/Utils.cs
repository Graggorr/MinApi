using Interactions.Database.Core;
using Interactions.Database.Dtos;
using Interactions.Domain.Common;
using Interactions.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Interactions.UnitTests
{
    internal class Utils
    {
        private static readonly DbContextOptions _contextOptions;

        static Utils()
        {
            _contextOptions = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase("testdb").Options;
        }

        public static IDomain<BookDto> CreateBookDomain(bool clear = false)
        {
            var result = CreateDomainCore<BookDomain>(clear);

            return new BookDomain(result.Item1, result.Item2);
        }

        public static IDomain<AuthorDto> CreateAuthorDomain(bool clear = false)
        {
            var result = CreateDomainCore<AuthorDomain>(clear);

            return new AuthorDomain(result.Item1, result.Item2);
        }

        private static (ILogger<T>, IServiceScopeFactory) CreateDomainCore<T>(bool clear)
        {

            var context = new DatabaseContext(_contextOptions);
            var loggerMock = new Mock<ILogger<T>>();
            var serviceScope = new Mock<IServiceScope>();
            var serviceProvider = new Mock<IServiceProvider>();
            var serviceScopeFactory = new Mock<IServiceScopeFactory>();
            serviceScopeFactory.Setup(x => x.CreateScope()).Returns(serviceScope.Object);
            serviceScope.Setup(x => x.ServiceProvider).Returns(serviceProvider.Object);
            serviceProvider.Setup(x => x.GetService(typeof(DatabaseContext))).Returns(context);

            if (clear)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return (loggerMock.Object, serviceScopeFactory.Object);
        }
    }
}
