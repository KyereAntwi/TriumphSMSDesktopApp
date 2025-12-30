using Serilog;

namespace Triumph.SMS.Remote.Core.Common.Extensions;

public static class RequestHandlerExtensions
{
    public static async Task<TResult> HandleWithErrorHandlingAsync<TResult>(
        this Func<Task<TResult>> handlerFunc,
        Func<Exception, TResult> errorHandler)
    {
        try
        {
            return await handlerFunc();
        }
        catch (Exception ex)
        {
            // Log the exception
            Log.Error(ex, "An error occurred while processing the request.");

            return errorHandler(ex);
        }
    }
}