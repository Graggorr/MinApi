using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace WebStore.EventBus.Abstraction
{
    public static class Register
    {
        public static IServiceCollection AddEventBusFromAssembly(this IServiceCollection services, Assembly assembly,
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            var eventBusType = typeof(IEventBus);
            var consumerType = typeof(IConsumer);
            var eventHandlerType = typeof(IIntegrationEventHandler<>);
            var integrationEventType = typeof(IntegrationEvent);
            var assemblyTypes = assembly.GetTypes();

            var eventHandlers = assemblyTypes.Where(x => x.GetInterface(eventHandlerType.Name) is not null).ToList();
            var integrationEvents = assemblyTypes.Where(x => x.GetBaseType(integrationEventType.Name) is not null).ToList();
            var eventBuses = assemblyTypes.Where(x => x.GetInterface(eventBusType.Name) is not null).ToList();
            var consumer = assemblyTypes.FirstOrDefault(x => x.GetInterface(consumerType.Name) is not null);

            if (consumer != null)
            {
                var consumers = new TypeContainer(consumerType, consumer);

                for (var i = 0; i < integrationEvents.Count && i < eventHandlers.Count; i++)
                {
                    services.AddConsumerAndEventHandler(consumers, new(eventHandlerType, eventHandlers[i]), integrationEvents[i], lifetime);
                }
            }

            foreach (var eventBus in eventBuses)
            {
                services.Add(new(eventBusType, eventBus, lifetime));
            }

            return services;
        }

        private static IServiceCollection AddConsumerAndEventHandler(this IServiceCollection services, TypeContainer consumer,
            TypeContainer eventHandler, Type integrationEventType, ServiceLifetime lifetime)
        {
            eventHandler.Service = eventHandler.Service.MakeGenericType(integrationEventType);
            consumer.Implementation = consumer.Implementation.MakeGenericType(integrationEventType);

            services.Add(new(eventHandler.Service, eventHandler.Implementation, lifetime));
            services.Add(new(consumer.Service, consumer.Implementation, lifetime));

            return services;
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

        private class TypeContainer(Type service, Type implementation)
        {
            public Type Service { get; set; } = service;
            public Type Implementation { get; set; } = implementation;
        }
    }
}
