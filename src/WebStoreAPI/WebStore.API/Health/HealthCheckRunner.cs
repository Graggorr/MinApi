using WebStore.Extensions;

namespace WebStore.API.Service.Health
{
    internal class HealthCheckRunner : IHealthCheckRunner
    {
        private readonly HttpClient _client;
        private readonly ILogger _logger;

        public HealthCheckRunner(ILogger<IHealthCheckRunner> logger)
        {
            _logger = logger;

            var port = Environment.GetEnvironmentVariable("PORT");

            _client = new HttpClient
            { 
                BaseAddress = new Uri($"http://localhost:{Environment.GetEnvironmentVariable("PORT")}/_health") 
            };
        }

        public async Task Run(int millisecondsDelay, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await _client.SendAsync(new HttpRequestMessage { Method = HttpMethod.Get }, cancellationToken);

                    await Task.Delay(millisecondsDelay, cancellationToken);
                }
                catch (Exception exception)
                {
                    _logger.LogException(exception);
                }
            }
        }
    }
}
