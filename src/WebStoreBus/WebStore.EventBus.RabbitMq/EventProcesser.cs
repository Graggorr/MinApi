using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebStore.EventBus.Abstraction;
using WebStore.EventBus.Infrastructure;

namespace WebStore.EventBus.RabbitMq
{
    public class EventProcesser(IOptions<DbContextOptions> options, IEventBus eventBus) : IEventProcesser
    {
        private readonly IEventBus _eventBus = eventBus;
        private readonly DbContextOptions _options = options.Value;

        public async Task<Result> ProceedEventsAsync()
        {
            try
            {
                using var context = new WebStoreEventContext(_options);
                var clientEvents = context.ClientEvents.Where(x => !x.IsProcessed);

                foreach (var clientEvent in clientEvents)
                {
                    var publishResult = await _eventBus.PublishAsync(clientEvent);

                    if (publishResult.IsSuccess)
                    {
                        clientEvent.IsProcessed = true;
                    }
                }

                await context.SaveChangesAsync();

                return Result.Ok();
            }
            catch(Exception exception)
            {
                return Result.Fail(exception.Message);
            }
        }
    }
}
