using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Webstore.Extensions;
using WebStore.EventBus.Abstraction;

namespace WebStore.Consumer.RabbitMq
{
    public static class Register
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEventBusFromAssembly(Assembly.GetExecutingAssembly());
            services.Configure<RabbitMqConfiguration>(configuration.GetRabbitMqConfiguration(nameof(RabbitMqConfiguration)));

            return services;
        }
    }
}
