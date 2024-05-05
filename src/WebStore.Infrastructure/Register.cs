using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IEventProcesser, EventProcesser<ClientEvent>>();
            services.AddSingleton<IEventBus, EventBusRabbitMq>();
            services.AddDbContext<WebStoreContext>(options => options.UseSqlServer(configuration.GetConnectionString("sqlString")));

            services.Configure<RabbitMqConfiguration>(configuration.GetSection(nameof(RabbitMqConfiguration)));

            return services;
        }
    }
}