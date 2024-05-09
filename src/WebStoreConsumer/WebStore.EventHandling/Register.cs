using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WebStore.EventBus.Abstraction;

namespace WebStore.Consumer.RabbitMq
{
    public static class Register
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services)
        {
            services.AddEventBusFromAssembly(Assembly.GetExecutingAssembly());
            services.AddOptions<RabbitMqConfiguration>();

            return services;
        }
    }
}
