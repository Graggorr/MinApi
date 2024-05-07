using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WebStore.EventBus;

namespace WebStore.RabbitMqEventHandling
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
