using FluentResults;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WebStore.Application.Clients;
using WebStore.Domain;

namespace WebStore.Application;

public static class Register
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IRequestHandler<PostClientHandlingRequest, Result<Client>>, PostClientRequestHandler>();
        services.AddSingleton<IRequestHandler<GetClientHandlingRequest, Result<Client>>, GetClientRequestHandler>();
        services.AddSingleton<IRequestHandler<GetAllClientsHandlingRequest, Result<IEnumerable<Client>>>, GetAllClientsRequestHandler>();
        services.AddSingleton<IRequestHandler<DeleteClientHandlingRequest, Result>, DeleteClientRequestHandler>();
        services.AddSingleton<IRequestHandler<PutClientHandlingRequest, Result<Client>>, PutClientRequestHandler>();
        //services.AddSingleton<ClientMapper>();

        return services;
    }
}