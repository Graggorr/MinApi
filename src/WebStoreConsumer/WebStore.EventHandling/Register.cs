using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WebStore.Extensions;
using WebStore.EventBus.Abstraction;
using System.Data;
using System.Data.SqlClient;

namespace WebStore.Consumer.RabbitMq
{
    public static class Register
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEventBusFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient<IDbConnection>(services => new SqlConnection());
            services.Configure<RabbitMqConfiguration>(configuration.GetRabbitMqConfiguration(nameof(RabbitMqConfiguration)));

            return services;
        }
    }
}
