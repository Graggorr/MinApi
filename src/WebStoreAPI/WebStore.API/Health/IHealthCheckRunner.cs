namespace WebStore.API.Service.Health
{
    public interface IHealthCheckRunner
    {
        public Task Run(int millisecondsDelay, CancellationToken cancellationToken);
    }
}
