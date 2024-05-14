using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Transactions;
using WebStore.EventBus.Abstraction;
using WebStore.EventBus.Infrastructure;

namespace WebStore.EventBus.BackgroundJobService
{
    public class BackgroundJobProcesser(IEventBus eventBus, ILogger<IBackgroundJobProcesser> logger, IServiceScopeFactory scopeFactory)
        : IBackgroundJobProcesser
    {
        private readonly IEventBus _eventBus = eventBus;
        private readonly ILogger _logger = logger;
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

        public async Task ProcessEvents()
        {
            using var scope = _scopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<WebStoreEventContext>();

            var events = context.ClientEvents.Where(x => !x.IsProcessed);

            foreach (var clientEvent in events)
            {
                try
                {
                    var result = await _eventBus.PublishAsync(clientEvent);

                    if (result.IsSuccess)
                    {
                        clientEvent.IsProcessed = true;

                        await context.SaveChangesAsync();
                    }
                }
                catch (Exception exception)
                {
                    _logger.LogError($"Message: {exception.Message}\nStack trace: {exception.StackTrace}");
                }
            }
        }
    }
}
