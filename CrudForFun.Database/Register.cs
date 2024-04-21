using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebStore.Domain;
using WebStore.Infrastructure;
using WebStore.Infrastructure.Clients;
using WebStore.Infrastructure.Orders;

namespace WebStore.Application
{
    public static class Register
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IClientRepository, ClientRepository>()
                .AddScoped<IOrderRepository, OrderRepository>()
                .AddDbContext<WebStoreContext>(options => options.UseSqlServer(connectionString));

            return services;
        }
    }
}