using Dapper;
using System.Data;
using Microsoft.Extensions.Logging;
using WebStore.EventBus.Abstraction;
using WebStore.Events.Clients;
using WebStore.Events.Orders;

namespace WebStore.EventBus.BackgroundJobService
{
    public class BackgroundEventProcesser(IEventBus eventBus, IDbConnection dbConnection, ILogger<IBackgroundJobProcesser> logger)
        : IBackgroundJobProcesser
    {
        private readonly IEventBus _eventBus = eventBus;
        private readonly ILogger _logger = logger;
        private readonly IDbConnection _dbConnection = dbConnection;
        private const string QUERY = @"
            SELECT *
            FROM {0}
            WHERE IsProcessed = 0";

        public Task ProcessJob()
        {
            if (_dbConnection.State is not ConnectionState.Open)
            {
                _dbConnection.Open();
            }

            var clientEventsTask = ProceedEvents<ClientEvent>();
            var orderEventsTask = ProceedEvents<OrderEvent>();

            Task.WaitAll(clientEventsTask, orderEventsTask);

            _dbConnection.Close();

            return Task.CompletedTask;
        }

        private async Task ProceedEvents<T>() where T : IntegrationEvent
        {
            var type = typeof(T);
            var integrationEvents = (await _dbConnection.QueryAsync(type, string.Format(QUERY, type.Name))) as IEnumerable<T>;

            foreach (var integrationEvent in integrationEvents)
            {
                try
                {
                    var result = await _eventBus.PublishAsync(integrationEvent);

                    if (result.IsSuccess)
                    {
                        _logger.LogInformation($"{integrationEvent.Id} is published to the event bus");
                        integrationEvent.IsProcessed = true;
                        var operation = $@"UPDATE {type.Name} SET IsProcessed = 1 WHERE Id = @Id";

                        var commandResult = await _dbConnection.ExecuteAsync(operation, integrationEvent);

                        if (commandResult > 0)
                        {
                            _logger.LogInformation($"{integrationEvent.Id} IsProcessed status has been updated");
                        }
                        else
                        {
                            _logger.LogWarning($"{integrationEvent.Id} IsProcessed status has not been updated!");
                        }
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
