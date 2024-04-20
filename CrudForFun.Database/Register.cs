using Microsoft.Extensions.DependencyInjection;
using WebStore.Infrastructure.Phones;

namespace WebStore.Infrastructure;

public static class Register
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IPhonesRepository, PhonesRepository>();

        // services.AddDbContext<>()

        return services;
    }
}