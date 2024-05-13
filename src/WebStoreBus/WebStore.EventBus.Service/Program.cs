using WebStore.EventBus.BackgroundJobService;

var builder = Host.CreateApplicationBuilder(args);
var services = builder.Services;

services.AddBackgroundJobs(builder.Configuration);

var host = builder.Build();

host.RunBackgroundJobs();
host.Run();
