using Microsoft.Extensions.DependencyInjection;
using WebStore.Application.Clients;

namespace WebStore.Application;

public static class Register
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IPostClientRequestHandler, PostClientRequestHandler>();
        services.AddSingleton<IGetClientRequestHandler, GetClientRequestHandler>();
        services.AddSingleton<IGetAllClientsRequestHandler, GetAllClientsRequestHandler>();
        services.AddSingleton<IDeleteClientRequestHandler, DeleteClientRequestHandler>();
        services.AddSingleton<IPutClientRequestHandler, PutClientRequestHandler>();
        services.AddSingleton<ClientMapper>();

        return services;
    }
}