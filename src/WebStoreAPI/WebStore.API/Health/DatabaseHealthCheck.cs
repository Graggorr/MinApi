using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data.Entity.Infrastructure;
using Webstore.Extensions;

namespace WebStore.API.Service.Health
{
    public class DatabaseHealthCheck(IDbConnectionFactory factory,
        IConfiguration configuration, ILogger<IHealthCheck> logger) : IHealthCheck
    {
        private readonly IDbConnectionFactory _factory = factory;
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger _logger = logger;

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            HealthCheckResult result;

            try
            {
                var connection = _factory.CreateConnection(_configuration.GetSqlConnectionString("ASPNETCORE_ENVIRONMENT"));
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"SELECT 1";
                await command.ExecuteScalarAsync();

                result = HealthCheckResult.Healthy();
            }
            catch (Exception exception)
            {
                result = HealthCheckResult.Unhealthy(exception: exception);
            }

            _logger.Log(result.Status is HealthStatus.Healthy ? LogLevel.Debug : LogLevel.Warning, $"Database health status: {result.Status}");

            return result;
        }
    }
}
