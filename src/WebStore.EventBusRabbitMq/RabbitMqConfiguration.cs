namespace WebStore.EventBusRabbitMq
{
    public class RabbitMqConfiguration
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string QueueName { get; set; }
    }
}
