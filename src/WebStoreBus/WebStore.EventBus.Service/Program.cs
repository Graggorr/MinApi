using Hangfire;
using Hangfire.SqlServer;
using WebStore.EventBus.Infrastructure;
using WebStore.EventBus.RabbitMq;
using WebStore.EventBus.Service;

var builder = Host.CreateApplicationBuilder(args);
var services = builder.Services;
var sqlString = builder.Configuration.GetConnectionString("sqlString");

GlobalConfiguration.Configuration.UseSqlServerStorage(sqlString, new SqlServerStorageOptions
{
    TryAutoDetectSchemaDependentOptions = false
});

services.AddInfrastructure(builder.Configuration);
services.AddRabbitMq();
services.AddHostedService<BackGroundJobService>();


var host = builder.Build();
host.Run();
