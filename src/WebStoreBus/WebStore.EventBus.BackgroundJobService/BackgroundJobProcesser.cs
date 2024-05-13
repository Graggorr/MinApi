using Microsoft.Extensions.Logging;
using System.Transactions;
using WebStore.EventBus.Abstraction;
using WebStore.EventBus.Infrastructure;

namespace WebStore.EventBus.BackgroundJobService
{
    public class BackgroundJobProcesser(IEventBus eventBus, ILogger<IBackgroundJobProcesser> logger) : IBackgroundJobProcesser
    {
        private readonly IEventBus _eventBus = eventBus;
        private readonly ILogger _logger = logger;

        public async Task ProcessEvents()
        {
            using var context = new WebStoreEventContext();

            var events = context.ClientEvents.Where(x => !x.IsProcessed);

            foreach (var clientEvent in events)
            {
                try
                {
                    using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

                    var result = await _eventBus.PublishAsync(clientEvent);

                    if (result.IsSuccess)
                    {
                        clientEvent.IsProcessed = true;

                        await context.SaveChangesAsync();
                    }

                    transaction.Complete();
                }
                catch (Exception exception)
                {
                    _logger.LogError($"Message: {exception.Message}\nStack trace: {exception.StackTrace}");
                }
            }
        }
    }
}
