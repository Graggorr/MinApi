using Hangfire;
using WebStore.EventBus.BackgroundJobService;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddEndpointsApiExplorer();
services.AddLogging();

services.AddBackgroundJobs(builder.Configuration);

var app = builder.Build();

app.MapHangfireDashboard("/dashboard");
app.UseHttpsRedirection();
app.UseRouting();

app.RunBackgroundJobs();
app.Run();
