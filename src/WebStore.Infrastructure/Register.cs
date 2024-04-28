using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebStore.Domain;
using WebStore.EventBus;
using WebStore.Infrastructure.Clients;
using WebStore.Infrastructure.Orders;
using WebStore.Infrastructure.RabbitMq;

namespace WebStore.Infrastructure
{
    public static class Register
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddSingleton<IEventBus, EventBusRabbitMq>();
            services.AddDbContext<WebStoreContext>(options => options.UseSqlServer(connectionString));

            return services;
        }
    }
}