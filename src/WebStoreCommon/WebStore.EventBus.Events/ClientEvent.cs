﻿using WebStore.EventBus.Abstraction;

namespace WebStore.EventBus.Events
{
    public class ClientEvent : IntegrationEvent
    {
        public ClientEvent()
        {
            
        }
        public ClientEvent(string clientId, string name, string phoneNumber, string email, string routeKey, string queueName, string orders)
        {
            ClientId = clientId;
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
            Orders = orders;
            RouteKey = routeKey;
            QueueName = queueName;
            CreationTimeUtc = DateTime.UtcNow;
        }

        public string ClientId { get; init; }
        public string Name { get; init; }
        public string PhoneNumber { get; init; }
        public string Email { get; init; }
        public string Orders { get; init; }
        public DateTime CreationTimeUtc { get; init; }
    }
}
