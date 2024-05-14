using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.EventBus.RabbitMq;

namespace WebStore.EventBus.BackgroundJobService
{
    public static class Register
    {
        public static IServiceCollection AddBackgroundJobs(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRabbitMq(configuration);
            services.AddHangfire(config => config.UseSqlServerStorage(configuration.GetConnectionString("WebstoreDb")));
            services.AddHangfireServer(config => config.SchedulePollingInterval = TimeSpan.FromSeconds(1));
            services.AddSingleton<IBackgroundJobProcesser, BackgroundJobProcesser>();

            return services;
        }

        public static IHost RunBackgroundJobs(this IHost app)
        {
            app.Services
                .GetRequiredService<IRecurringJobManager>()
                .AddOrUpdate<IBackgroundJobProcesser>("background-job", instance => instance.ProcessEvents(), "0/15 * * * * *");

            return app;
        }
    }
}
