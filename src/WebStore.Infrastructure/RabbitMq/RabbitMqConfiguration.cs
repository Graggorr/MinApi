using RabbitMQ.Client;

namespace WebStore.Infrastructure.RabbitMq
{
    public class RabbitMqConfiguration()
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
        public bool AutomaticRecoveryEnabled { get; set; }
        public SslOption SslOption { get; set; }
    }
}
