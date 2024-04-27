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
        services.AddScoped<IRequestHandler<PostClientHandlingRequest, Result<Client>>, PostClientRequestHandler>();
        services.AddScoped<IRequestHandler<GetClientHandlingRequest, Result<Client>>, GetClientRequestHandler>();
        services.AddScoped<IRequestHandler<GetAllClientsHandlingRequest, Result<IEnumerable<Client>>>, GetAllClientsRequestHandler>();
        services.AddScoped<IRequestHandler<DeleteClientHandlingRequest, Result>, DeleteClientRequestHandler>();
        services.AddScoped<IRequestHandler<PutClientHandlingRequest, Result<Client>>, PutClientRequestHandler>();
        //services.AddSingleton<ClientMapper>();

        return services;
    }
}