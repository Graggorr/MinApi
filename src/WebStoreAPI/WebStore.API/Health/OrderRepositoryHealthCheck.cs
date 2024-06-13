using Microsoft.Extensions.Diagnostics.HealthChecks;
using WebStore.API.Infrastructure.Orders;

namespace WebStore.API.Service.Health
{
    public class OrderRepositoryHealthCheck(IOrderRepository orderRepository, ILogger<IHealthCheck> logger) : IHealthCheck
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly ILogger _logger = logger;

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            HealthCheckResult result;

            try
            {
                await _orderRepository.GetOrderAsync(1);

                result = HealthCheckResult.Healthy();
            }
            catch (Exception exception)
            {
                result = HealthCheckResult.Degraded(exception: exception);
            }

            _logger.Log(result.Status is HealthStatus.Healthy ? LogLevel.Debug : LogLevel.Warning, $"Order repository health status: {result.Status}");

            return result;
        }
    }
}
