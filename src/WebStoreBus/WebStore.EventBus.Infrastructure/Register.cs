using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebStore.EventBus.Infrastructure
{
    public static class Register
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<WebStoreEventContext>(options => options.UseSqlServer(configuration.GetConnectionString("WebstoreDb")));


            return services;
        }
    }
}
