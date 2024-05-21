using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data;
using System.Data.SqlClient;
using WebStore.Extensions;
using WebStore.EventBus.RabbitMq;

namespace WebStore.EventBus.BackgroundJobService
{
    public static class Register
    {
        public static IServiceCollection AddBackgroundJobs(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetSqlConnectionString("DOTNET_ENVIRONMENT");

            services.AddRabbitMq(configuration);
            services.AddHangfire(config => config.UseSqlServerStorage(connectionString));
            services.AddHangfireServer(config => config.SchedulePollingInterval = TimeSpan.FromSeconds(1));
            services.AddSingleton<IDbConnection>(sp => new SqlConnection(connectionString));
            services.AddSingleton<IBackgroundJobProcesser, BackgroundEventProcesser>();
            services.AddSingleton<IBackgroundJobProcesser, BackgroundCleanupProcesser>();

            return services;
        }

        public static IHost RunBackgroundJobs(this IHost app)
        {
            var jobManager = app.Services.GetRequiredService<IRecurringJobManager>();

            jobManager.AddOrUpdate<BackgroundEventProcesser>("background-event-processer",
                instance => instance.ProcessJob(), "0/15 * * * * *");
            jobManager.AddOrUpdate<BackgroundCleanupProcesser>("background-cleanup-processer",
                instance => instance.ProcessJob(), Cron.Monthly());

            return app;
        }
    }
}
