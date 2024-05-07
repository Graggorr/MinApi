using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace WebStore.EventBus
{
    public static class Register
    {
        public static IServiceCollection AddEventBusFromAssembly(this IServiceCollection services, Assembly assembly,
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            var eventBusType = typeof(IEventBus);
            var consumerType = typeof(IConsumer);
            var eventHandlerType = typeof(IIntegrationEventHandler<>);

            var eventHandlers = assembly.GetTypes().Where(x => x.GetInterfaces().Contains(eventHandlerType)).ToList();
            var integrationEvents = assembly.GetTypes().Where(x => x.GetInterfaces().Contains(typeof(IntegrationEvent))).ToList();
            var eventBuses = assembly.GetTypes().Where(x => x.GetInterfaces().Contains(eventBusType));
            var consumer = assembly.GetTypes().FirstOrDefault(x => x.GetInterfaces().Contains(consumerType));

            if (consumer != null)
            {
                var consumers = new Type[] { consumerType, consumer };

                for (var i = 0; i < integrationEvents.Count || i < eventHandlers.Count; i++)
                {
                    services.AddConsumerAndEventHandler(consumers, [eventHandlerType, eventHandlers[i]], integrationEvents[i], lifetime);
                }
            }

            foreach (var eventBus in eventBuses)
            {
                services.Add(new(eventBusType, eventBus, lifetime));
            }

            return services;
        }

        private static IServiceCollection AddConsumerAndEventHandler(this IServiceCollection services, Type[] consumerTypes,
            Type[] eventHandlerTypes, Type integrationEventType, ServiceLifetime lifetime)
        {
            var genericEventHandlerType = eventHandlerTypes[1].MakeGenericType(integrationEventType);
            var genericConsumerHandlerType = consumerTypes[1].MakeGenericType(integrationEventType);

            services.Add(new(eventHandlerTypes[0], genericEventHandlerType, lifetime));
            services.Add(new(consumerTypes[0], genericConsumerHandlerType, lifetime));

            return services;
        }
    }
}
