using MediatR;
using Microsoft.Data.Sqlite;
using Triumph.SMS.Remote.Core.Common.CQRS;

namespace Triumph.SMS.Remote.Core.Common.Behaviors;

public class RetryDecorator<TRequest, TResponse>(IPublisher publisher) : IPipelineBehavior<TRequest, TResponse> where TRequest : ICommand<TResponse>
{
    private readonly IPublisher _publisher = publisher;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        var maxRetries = 3;
        var baseDelay = TimeSpan.FromMilliseconds(100);

        for (var attempt = 1; attempt <= maxRetries + 1; attempt++)
        {
            try
            {
                var response = await next();

                if (attempt > 1)
                {
                    //TODO - send event to logging system with message $"{requestName} succeeded on attempt {attempt}"
                }

                return response;
            }
            catch (Exception ex) when (ShouldRetry(ex, attempt, maxRetries))
            {
                var delay = TimeSpan.FromMilliseconds(baseDelay.TotalMilliseconds * Math.Pow(2, attempt - 1));

                // TODO - send event to logging system with message $"{requestName} failed on attempt {attempt}/{maxRetries + 1}. Retrying in {delay.TotalMilliseconds}ms"

                await Task.Delay(delay, cancellationToken);
            }
        }

        throw new InvalidOperationException($"Exceeded maximum retry attempts for {requestName}");
    }

    private static bool ShouldRetry(Exception ex, int attempt, int maxRetries)
    {
        if (attempt > maxRetries) return false;

        return ex is TimeoutException
                or TaskCanceledException
                or HttpRequestException
               || (ex is SqliteException sqlEx && IsTransientSqlError(sqlEx));
    }

    private static bool IsTransientSqlError(SqliteException ex)
    {
        // SQLite transient error codes
        int[] transientErrorCodes =
        {
            5,    // SQLITE_BUSY
            6,    // SQLITE_LOCKED
            261,  // SQLITE_BUSY_SNAPSHOT
            517   // SQLITE_LOCKED_SHAREDCACHE
        };

        return transientErrorCodes.Contains(ex.SqliteErrorCode)
               || transientErrorCodes.Contains(ex.SqliteExtendedErrorCode);
    }
}
