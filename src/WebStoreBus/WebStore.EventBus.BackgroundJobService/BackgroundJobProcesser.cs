using Dapper;
using System.Data;
using Microsoft.Extensions.Logging;
using WebStore.EventBus.Abstraction;
using WebStore.EventBus.Events;

namespace WebStore.EventBus.BackgroundJobService
{
    public class BackgroundJobProcesser(IEventBus eventBus, IDbConnection dbConnection, ILogger<IBackgroundJobProcesser> logger)
        : IBackgroundJobProcesser
    {
        private readonly IEventBus _eventBus = eventBus;
        private readonly ILogger _logger = logger;
        private readonly IDbConnection _dbConnection = dbConnection;
        private const string QUERY = @"
            SELECT *
            FROM ClientEvent
            WHERE IsProcessed = 0";

        public async Task ProcessEvents()
        {
            var events = await _dbConnection.QueryAsync<ClientEvent>(QUERY);

            foreach (var clientEvent in events)
            {
                try
                {
                    var result = await _eventBus.PublishAsync(clientEvent);

                    if (result.IsSuccess)
                    {
                        using var transaction = _dbConnection.BeginTransaction();
                        clientEvent.IsProcessed = true;
                        var operation = @"UPDATE ClientEvent SET IsProcessed = @IsProcessed WHERE Id = @Id";

                        await _dbConnection.ExecuteAsync(operation, clientEvent, transaction);

                        transaction.Commit();
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
