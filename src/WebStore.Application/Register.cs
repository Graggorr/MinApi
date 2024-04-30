using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WebStore.Application.Clients.Commands;
using WebStore.Application.Clients.Commands.CreateClient;
using WebStore.Application.Clients.Commands.DeleteClient;
using WebStore.Application.Clients.Commands.UpdateClient;
using WebStore.Application.Clients.Queries;
using WebStore.Domain;

namespace WebStore.Application;

public static class Register
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //services.AddScoped<IRequestHandler<RegisterClientRequest, Result<Client>>, CreateClientRequestHandler>();
        //services.AddScoped<IRequestHandler<GetClientHandlingRequest, Result<Client>>, GetClientRequestHandler>();
        //services.AddScoped<IRequestHandler<GetAllClientsHandlingRequest, Result<IEnumerable<Client>>>, GetAllClientsRequestHandler>();
        //services.AddScoped<IRequestHandler<DeleteClientHandlingRequest, Result<Client>>, DeleteClientRequestHandler>();
        //services.AddScoped<IRequestHandler<PutClientHandlingRequest, Result<Client>>, UpdateClientRequestHandler>();
        services.AddScoped<IValidator<RegisterClientRequest>, CreateClientValidator>();
        services.AddScoped<IValidator<PutClientHandlingRequest>, UpdateClientValidator>();
        services.AddMediatR(config =>
        {
            config.Lifetime = ServiceLifetime.Scoped;
            config.RegisterServicesFromAssemblyContaining<CreateClientPipelineBehavior>();
            config.AddBehavior<IPipelineBehavior<RegisterClientRequest, Result<Client>>, CreateClientPipelineBehavior>();
        });
        //services.AddSingleton<ClientMapper>();

        return services;
    }
}