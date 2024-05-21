using WebStore.Events.Clients;

namespace WebStore.Consumer.RabbitMq.Clients.ClientDeleted
{
    public class DeletedClientEvent: ClientEvent
    {
        public DeletedClientEvent(): base() { }

        public DeletedClientEvent(string id, string name, string phoneNumber, string email, string routeKey, string queueName)
            :base (id, name, phoneNumber, email, routeKey, queueName) { }
    }
}
