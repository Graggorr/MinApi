using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebStore.API.Infrastructure.Clients;
using WebStore.API.Infrastructure.Orders;

namespace WebStore.API.Infrastructure
{
    public static class Register
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddDbContext<WebStoreContext>(options => options.UseSqlServer(configuration.GetConnectionString("sqlString")));

            return services;
        }
    }
}