using System.Data.Entity.Infrastructure;

namespace WebStore.API.Service.Health
{
    internal static class Register
    {
        public static IHealthChecksBuilder AddWebstoreHealthChecks(this IHealthChecksBuilder builder)
        {
            builder.AddCheck<DatabaseHealthCheck>("Database");
            builder.AddCheck<ClientRepositoryHealthCheck>("ClientRepository");
            builder.Services.AddSingleton<IDbConnectionFactory>((sp) => new SqlConnectionFactory());
            builder.Services.AddSingleton<IHealthCheckRunner, HealthCheckRunner>();

            return builder;
        }

        public static IHost RunHealthCheckBackground(this IHost app)
        {
            var healthChecker = app.Services.GetRequiredService<IHealthCheckRunner>();
            var cancellationTokenSource = new CancellationTokenSource();
            healthChecker.Run(millisecondsDelay: 5000, cancellationTokenSource.Token);

            return app;
        }
    }
}
