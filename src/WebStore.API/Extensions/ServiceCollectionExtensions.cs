using WebStore.Infrastructure.RabbitMq;

namespace WebStore.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebStoreOptions(this IServiceCollection services)
        {
            services.AddOptions<RabbitMqConfiguration>().BindConfiguration(nameof(RabbitMqConfiguration));

            return services;
        }
    }
}
