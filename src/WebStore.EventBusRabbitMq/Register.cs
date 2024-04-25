using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.EventBusRabbitMq
{
    public static class Register
    {
        public static IServiceCollection AddRabbitMqEventBus(this IServiceCollection services, RabbitMqConfiguration configuration)
        {
            services.AddOptions<RabbitMqConfiguration>();
            services.AddHostedService<ClientEventBusRabbitMq>();

            return services;
        }
    }
}
