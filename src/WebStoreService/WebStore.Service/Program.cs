using WebStore.RabbitMqEventHandling;
using WebStore.Service;

var builder = Host.CreateApplicationBuilder(args);
var services = builder.Services;

services.AddRabbitMq();
services.AddHostedService<HostedService>();

var host = builder.Build();
host.Run();
