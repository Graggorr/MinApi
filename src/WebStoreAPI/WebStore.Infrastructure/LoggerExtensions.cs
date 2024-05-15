using Microsoft.Extensions.Logging;

namespace WebStore.API.Infrastructure
{
    public static class LoggerExtensions
    {
        public static void LogException(this ILogger logger, Exception exception)
            => logger.LogError($"Exception: {exception.Message}\nStack trace:\n{exception.StackTrace}");
    }
}
