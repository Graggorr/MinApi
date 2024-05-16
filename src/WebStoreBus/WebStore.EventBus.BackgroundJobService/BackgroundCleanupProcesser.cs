using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace WebStore.EventBus.BackgroundJobService
{
    public class BackgroundCleanupProcesser(IDbConnection dbConnection, ILogger<IBackgroundJobProcesser> logger)
        : IBackgroundJobProcesser
    {
        private readonly ILogger _logger = logger;
        private readonly IDbConnection _dbConnection = dbConnection;
        private const string QUERY = @"DELETE FROM ClientEvent WHERE IsProcessed = 1";

        public async Task ProcessJob()
        {
            if (_dbConnection.State is not ConnectionState.Open)
            {
                _dbConnection.Open();
            }
            try
            {
                using var transaction = _dbConnection.BeginTransaction();
                await _dbConnection.ExecuteAsync(QUERY, transaction: transaction);
                transaction.Commit();

                _logger.LogInformation("Cleanup for ClientEvents has been performed");
            }
            catch (Exception exception)
            {
                _logger.LogError($"Message: {exception.Message}\nStack trace: {exception.StackTrace}");
            }

            _dbConnection.Close();
        }
    }
}
