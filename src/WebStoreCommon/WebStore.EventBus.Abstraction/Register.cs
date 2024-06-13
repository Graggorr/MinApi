using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace WebStore.EventBus.Abstraction
{
    public static class Register
    {
        public static IServiceCollection AddEventBusFromAssembly(this IServiceCollection services, Assembly assembly,
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            var tasks = new Task<IServiceCollection>[] { AddConsumers(assembly, lifetime), AddEventBuses(assembly, lifetime), AddEventHandlers(assembly, lifetime) };
            Task.WaitAll(tasks);

            var faultedTask = tasks.FirstOrDefault(x => x.Exception is not null);

            if (faultedTask is not null)
            {
                throw faultedTask.Exception;
            }

            foreach (var task in tasks)
            {
                services.AddRange(task.Result);
            }

            return services;
        }

        private static Task<IServiceCollection> AddEventBuses(Assembly assembly, ServiceLifetime lifetime)
        {
            return Task.Factory.StartNew(() =>
            {
                IServiceCollection services = new ServiceCollection();
                var eventBusService = typeof(IEventBus);
                var assemblyTypes = assembly.GetTypes();
                var eventBuses = assemblyTypes.Where(x => x.GetInterface(eventBusService.Name) is not null).ToList();

                foreach (var eventBus in eventBuses)
                {
                    services.Add(new(eventBusService, eventBus, lifetime));
                }

                return services;
            });
        }

        private static Task<IServiceCollection> AddConsumers(Assembly assembly, ServiceLifetime lifetime)
        {
            return Task.Factory.StartNew(() =>
            {
                IServiceCollection services = new ServiceCollection();
                var consumerService = typeof(IConsumer);
                var integrationEventType = typeof(IntegrationEvent);
                var assemblyTypes = assembly.GetTypes();

                var integrationEvents = assemblyTypes.Where(x => x.GetBaseType(integrationEventType.Name) is not null).ToList();
                var consumers = assemblyTypes.Where(x => x.GetInterface(consumerService.Name) is not null).ToList();

                if (consumers.Count is not 0)
                {
                    consumers.ForEach(consumer =>
                    {
                        integrationEvents.ForEach(integrationEvent =>
                        {
                            var consumerImplementation = consumer.MakeGenericType(integrationEvent);
                            services.Add(new(consumerService, consumerImplementation, lifetime));
                        });
                    });
                }

                return services;
            });
        }

        private static Task<IServiceCollection> AddEventHandlers(Assembly assembly, ServiceLifetime lifetime)
        {
            return Task.Factory.StartNew(() =>
            {
                IServiceCollection services = new ServiceCollection();
                var eventHandlerService = typeof(IIntegrationEventHandler<>);
                var integrationEventType = typeof(IntegrationEvent);
                var assemblyTypes = assembly.GetTypes();

                var eventHandlers = assemblyTypes.Where(x => x.GetInterface(eventHandlerService.Name) is not null && !x.IsAbstract).ToList();
                var integrationEvents = assemblyTypes.Where(x => x.GetBaseType(integrationEventType.Name) is not null).ToList();

                if (eventHandlers.Count is not 0)
                {
                    integrationEvents.ForEach(integrationEvent =>
                    {
                        var eventHandlerImplementation = eventHandlers.First(x => x.GetInterface(eventHandlerService.Name).GetGenericArguments().Contains(integrationEvent));
                        var eventHandlerServiceGeneric = eventHandlerService.MakeGenericType(integrationEvent);
                        services.Add(new(eventHandlerServiceGeneric, eventHandlerImplementation, lifetime));
                    });
                }

                return services;
            });
        }

        private static Type? GetBaseType(this Type type, string name)
        {
            Type? typeToReturn = null;
            var tempType = type;

            while (typeToReturn is null && tempType is not null)
            {
                tempType = tempType.BaseType;

                if (tempType is not null && tempType.Name.Equals(name))
                {
                    typeToReturn = tempType;
                }
            }

            return typeToReturn;
        }

        private static void AddRange(this IServiceCollection services, IEnumerable<ServiceDescriptor> values)
        {
            foreach (var value in values)
            {
                services.Add(value);
            }
        }
    }
}
