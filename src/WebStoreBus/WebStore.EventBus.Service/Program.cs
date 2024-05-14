using Hangfire;
using WebStore.EventBus.BackgroundJobService;
using WebStore.EventBus.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddEndpointsApiExplorer();
services.AddLogging();

services.AddInfrastructure(builder.Configuration);
services.AddBackgroundJobs(builder.Configuration);

var app = builder.Build();

app.MapHangfireDashboard("/dashboard");
app.UseHttpsRedirection();
app.UseRouting();

app.RunBackgroundJobs();
app.Run();
