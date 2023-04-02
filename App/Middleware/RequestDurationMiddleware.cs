// using System.Diagnostics;
//
// namespace App.Middleware;
//
// /// <summary>
// /// Time how long a request takes
// /// </summary>
// public class RequestDurationMiddleware
// {
//     private readonly RequestDelegate _next;
//     private readonly ILogger<RequestDurationMiddleware> _logger;
//
//     /// <summary>
//     /// RequestDurationMiddleware constructor
//     /// </summary>
//     public RequestDurationMiddleware(RequestDelegate next, ILogger<RequestDurationMiddleware> logger)
//     {
//         _next = next;
//         _logger = logger;
//     }
//
//     /// <summary>
//     /// Invoke RequestDurationMiddleware
//     /// </summary>
//     public async Task Invoke(HttpContext context)
//     {
//         var watch = Stopwatch.StartNew();
//         await _next(context);
//         watch.Stop();
//
//         context.Response.Headers.Add("X-Elapsed-Milliseconds", watch.ElapsedMilliseconds.ToString());
//         _logger.LogTrace("{duration}ms", watch.ElapsedMilliseconds);
//     }
// }

using System.Diagnostics;

namespace App.Middleware;

/// <summary>
/// Time how long a request takes
/// </summary>
public class RequestDurationMiddleware
{
    private const string ResponseDurationHeader = "X-Response-Time-ms";

    private readonly RequestDelegate _next;

    /// <summary>
    ///
    /// </summary>
    public RequestDurationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

     /// <summary>
     ///
     /// </summary>
     public async Task Invoke(HttpContext context)
    {
        var watch = new Stopwatch();
        watch.Start();

        context.Response.OnStarting(() =>
        {
            watch.Stop();
            long responseTimeForCompleteRequest = watch.ElapsedMilliseconds;
            context.Response.Headers[ResponseDurationHeader] = responseTimeForCompleteRequest.ToString();
            return Task.CompletedTask;
        });

        // Call the next delegate/middleware in the pipeline
        await _next(context);
    }
}
