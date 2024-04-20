using Application.Phones;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class Register
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IPhonesService, PhoneService>();
        services.AddSingleton<PhoneMapper>();

        return services;
    }
}