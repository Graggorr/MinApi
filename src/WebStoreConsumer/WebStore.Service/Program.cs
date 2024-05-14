using WebStore.Consumer.RabbitMq;
using WebStore.Service;

var builder = Host.CreateApplicationBuilder(args);
var services = builder.Services;

services.AddRabbitMq(builder.Configuration);
services.AddHostedService<HostedService>();

var host = builder.Build();
host.Run();
