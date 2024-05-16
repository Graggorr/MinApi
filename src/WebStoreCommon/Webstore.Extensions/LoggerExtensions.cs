using FluentResults;
using Microsoft.Extensions.Logging;

namespace WebStore.Extensions
{
    public static class LoggerExtensions
    {
        public static void LogException(this ILogger logger, Exception exception)
            => logger.LogError($"Exception: {exception.Message}\nStack trace:\n{exception.StackTrace}");

        public static Result LogSendAndFail(this ILogger logger, string message)
        {
            logger.LogDebug(message);

            return Result.Fail(message);
        }
    }
}
