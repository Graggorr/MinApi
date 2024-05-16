using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebStore.Extensions;
using WebStore.EventBus.Abstraction;

namespace WebStore.EventBus.RabbitMq
{
    public static class Register
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEventBus, EventBusRabbitMq>();
            services.Configure<RabbitMqConfiguration>(configuration.GetRabbitMqConfiguration(nameof(RabbitMqConfiguration)));

            return services;
        }
    }
}
