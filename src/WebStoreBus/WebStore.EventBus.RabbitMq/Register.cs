using Microsoft.Extensions.DependencyInjection;
using WebStore.EventBus.Abstraction;

namespace WebStore.EventBus.RabbitMq
{
    public static class Register
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services)
        {
            services.AddSingleton<IEventProcesser, EventProcesser>();
            services.AddSingleton<IEventBus, EventBusRabbitMq>();

            return services;
        }
    }
}
