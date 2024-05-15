using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebStore.API.Service.Health
{
    internal class HealthCheckRunner() : IHealthCheckRunner
    {


        public async Task Run(int millisecondsDelay, CancellationToken cancellationToken)
        {
           
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var httpContext = new HttpClient
                    {
                        BaseAddress = new Uri("http://127.0.0.1:8080/_health")
                    };
                    var response = await httpContext.SendAsync(new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        //RequestUri = new Uri("/_health")
                    });

                    await Task.Delay(millisecondsDelay, cancellationToken);
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
