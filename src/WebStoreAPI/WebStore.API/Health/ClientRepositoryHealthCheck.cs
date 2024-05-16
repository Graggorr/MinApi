using Microsoft.Extensions.Diagnostics.HealthChecks;
using WebStore.API.Infrastructure.Clients;

namespace WebStore.API.Service.Health
{
    public class ClientRepositoryHealthCheck(IClientRepository clientRepository, ILogger<IHealthCheck> logger) : IHealthCheck
    {
        private readonly IClientRepository _clientRepository = clientRepository;
        private readonly ILogger _logger = logger;

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            HealthCheckResult result;

            try
            {
                await _clientRepository.IsPhoneNumberUniqueAsync("1");

                result = HealthCheckResult.Healthy();
            }
            catch (Exception exception) 
            {
                result = HealthCheckResult.Degraded(exception: exception);
            }

            _logger.Log(result.Status is HealthStatus.Healthy ? LogLevel.Debug : LogLevel.Warning, $"ClientRepository health status: {result.Status}");

            return result;
        }
    }
}
