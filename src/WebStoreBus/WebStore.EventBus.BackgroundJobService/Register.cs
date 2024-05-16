using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data;
using System.Data.SqlClient;
using Webstore.Extensions;
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
            services.AddScoped<IDbConnection>((sp) => new SqlConnection(connectionString));
            services.AddScoped<IBackgroundJobProcesser, BackgroundJobProcesser>();

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
