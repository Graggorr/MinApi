using RabbitMQ.Client;

namespace WebStore.EventBus.Abstraction.Extensions
{
    public static class ModelExtensions
    {
        public static void InitializeQueue(this IModel channel, string exchangeName, string queueName, string routeKey)
        {
            channel.ExchangeDeclare(exchangeName, "direct", true, false, null);
            channel.QueueDeclare(queueName, true, false, false, null);
            channel.QueueBind(queueName, exchangeName, routeKey);
        }
    }
}
