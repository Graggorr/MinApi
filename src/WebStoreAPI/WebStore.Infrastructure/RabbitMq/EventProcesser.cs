using WebStore.EventBus;

namespace WebStore.Infrastructure.Clients
{
    public class EventProcesser<T>(WebStoreContext context, IEventBus eventBus) : IEventProcesser where T : IntegrationEvent
    {
        private readonly WebStoreContext _context = context;
        private readonly IEventBus _eventBus = eventBus;

        public async Task CallProcessing()
        {
            var set = _context.Set<T>();

            var integrationEvents = set.Where(x => !x.IsProcessed);

            foreach (var integrationEvent in integrationEvents)
            {
                var result = await _eventBus.PublishAsync(integrationEvent);

                if (result.IsSuccess)
                {
                    integrationEvent.IsProcessed = true;

                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
