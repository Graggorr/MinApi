using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;
using WebStore.Events.Clients;

namespace WebStore.EventBus.BackgroundJobService
{
    public class BackgroundCleanupProcesser(IDbConnection dbConnection, ILogger<IBackgroundJobProcesser> logger)
        : IBackgroundJobProcesser
    {
        private readonly ILogger _logger = logger;
        private readonly IDbConnection _dbConnection = dbConnection;

        private const string QUERY = @"SELECT * FROM ClientEvent WHERE IsProcessed = 1";
        private const string FUNCTION = @"DELETE FROM ClientEvent WHERE Id = @Id";

        public async Task ProcessJob()
        {
            if (_dbConnection.State is not ConnectionState.Open)
            {
                _dbConnection.Open();
            }

            try
            {
                var events = await _dbConnection.QueryAsync<ClientEvent>(QUERY);
                var eventsToDelete = GetEventsToDelete(events);

                foreach (var clientEvent in eventsToDelete)
                {
                    using var transaction = _dbConnection.BeginTransaction();
                    var result = await _dbConnection.ExecuteAsync(FUNCTION, clientEvent, transaction: transaction);

                    if (result is 0)
                    {
                        _logger.LogWarning($"Cannot delete event with ID: {clientEvent.Id}");

                        continue;
                    }

                    transaction.Commit();

                    _logger.LogDebug($"Event with ID: {clientEvent.Id} has been deleted due to event storage timeout.");
                }

                _logger.LogInformation("Cleanup for ClientEvents has been performed");
            }
            catch (Exception exception)
            {
                _logger.LogError($"Message: {exception.Message}\nStack trace: {exception.StackTrace}");
            }

            _dbConnection.Close();
        }

        private static IEnumerable<ClientEvent> GetEventsToDelete(IEnumerable<ClientEvent> events)
        {
            var list = new List<ClientEvent>();

            foreach (var clientEvent in events)
            {
                var result = DateTime.UtcNow.Subtract(clientEvent.CreationTimeUtc);

                if (result.TotalDays > 30)
                {
                    list.Add(clientEvent);
                }
            }

            return list;
        }
    }
}
