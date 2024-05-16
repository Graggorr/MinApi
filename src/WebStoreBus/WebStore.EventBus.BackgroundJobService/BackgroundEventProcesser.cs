using Dapper;
using System.Data;
using Microsoft.Extensions.Logging;
using WebStore.EventBus.Abstraction;
using WebStore.Events;

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
            FROM ClientEvent
            WHERE IsProcessed = 0";

        public async Task ProcessJob()
        {
            if (_dbConnection.State is not ConnectionState.Open)
            {
                _dbConnection.Open();
            }

            var events = await _dbConnection.QueryAsync<ClientEvent>(QUERY);

            foreach (var clientEvent in events)
            {
                try
                {
                    var result = await _eventBus.PublishAsync(clientEvent);

                    if (result.IsSuccess)
                    {
                        _logger.LogInformation($"{clientEvent.Id} is published to the event bus");
                        using var transaction = _dbConnection.BeginTransaction();
                        clientEvent.IsProcessed = true;
                        var operation = @"UPDATE ClientEvent SET IsProcessed = 1 WHERE Id = @Id";

                        var Commandresult = await _dbConnection.ExecuteAsync(operation, clientEvent, transaction);

                        if (Commandresult > 0)
                        {
                            _logger.LogInformation($"{clientEvent.Id} IsProcessed status has been updated");
                        }
                        else
                        {
                            _logger.LogWarning($"{clientEvent.Id} IsProcessed status has not been updated!");
                        }

                        transaction.Commit();
                    }
                }
                catch (Exception exception)
                {
                    _logger.LogError($"Message: {exception.Message}\nStack trace: {exception.StackTrace}");
                }
            }

            _dbConnection.Close();
        }
    }
}
