using Microsoft.EntityFrameworkCore;
using WebStore.Extensions;
using WebStore.API.Infrastructure;

namespace WebStore.API.Service
{
    public class DatabaseInitializer(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public void InitializeDatabase()
        {
            var options = new DbContextOptionsBuilder<WebStoreContext>()
                .UseSqlServer(_configuration.GetSqlConnectionString("ASPNETCORE_ENVIRONMENT")).Options;
            var context = new WebStoreContext(options);

            context.Database.EnsureCreated();
        }
    }
}
