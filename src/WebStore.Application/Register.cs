using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using WebStore.Application.Clients;
using WebStore.Application.Clients.Commands;
using WebStore.Application.Clients.Commands.CreateClient;
using WebStore.Application.Clients.Commands.UpdateClient;

namespace WebStore.Application;

public static class Register
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //services.AddScoped<IRequestHandler<RegisterClientRequest, Result<Client>>, CreateClientRequestHandler>();
        //services.AddScoped<IRequestHandler<GetClientHandlingRequest, Result<Client>>, GetClientRequestHandler>();
        //services.AddScoped<IRequestHandler<GetAllClientsHandlingRequest, Result<IEnumerable<Client>>>, GetAllClientsRequestHandler>();
        //services.AddScoped<IRequestHandler<DeleteClientHandlingRequest, Result<Client>>, DeleteClientRequestHandler>();
        //services.AddScoped<IRequestHandler<UpdateClientRequest, Result<Client>>, UpdateClientRequestHandler>();
        services.AddScoped<IValidator<RegisterClientRequest>, CreateClientValidator>();
        services.AddScoped<IValidator<UpdateClientRequest>, UpdateClientValidator>();
        services.AddMediatR(config =>
        {
            config.Lifetime = ServiceLifetime.Scoped;
            config.RegisterServicesFromAssemblyContaining<ClientBusinessValidator>();
            config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });
        //services.AddSingleton<ClientMapper>();

        return services;
    }
}