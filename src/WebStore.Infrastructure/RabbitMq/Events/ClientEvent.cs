using WebStore.Domain;
using WebStore.EventBus;
using System.Reflection;

namespace WebStore.Infrastructure.RabbitMq.Events
{
    public abstract class ClientEvent(string clientId, string name, string phoneNumber, string email, List<string> orders) : IntegrationEvent
    {
        public string ClientId => clientId;
        public string Name => name;
        public string PhoneNumber => phoneNumber;
        public string Email => email;
        public List<string> Orders => orders;

        public static T CreateIntegrationEvent<T>(Client client) where T : ClientEvent
        {
            var constructor = typeof(T).GetConstructor(BindingFlags.Public, [typeof(string), typeof(string), typeof(string), typeof(string), typeof(List<string>)]);
            return constructor.Invoke([client.Id.ToString(), client.Name, client.PhoneNumber, client.Email, client.Orders.Select(x => x.ToStringWithoutClients()).ToList()]) as T;
        }
    }
}
