using Microsoft.EntityFrameworkCore;
using WebStore.API.Infrastructure;

namespace WebStore.API.Service
{
    public class DatabaseInitializer(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task InitializeDatabase()
        {
            var options = new DbContextOptionsBuilder<WebStoreContext>().UseSqlServer(_configuration.GetConnectionString("WebstoreDb")).Options;
            var context = new WebStoreContext(options);

            await context.Database.EnsureCreatedAsync();
        }
    }
}
