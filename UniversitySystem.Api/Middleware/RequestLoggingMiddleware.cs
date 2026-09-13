using System.Diagnostics;

namespace UniversitySystem.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var request = context.Request;

        _logger.LogInformation(
            "--> {Method} {Path}{QueryString}",
            request.Method,
            request.Path,
            request.QueryString);

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            var response = context.Response;

            var logLevel = response.StatusCode >= 500
                ? LogLevel.Error
                : response.StatusCode >= 400
                    ? LogLevel.Warning
                    : LogLevel.Information;

            _logger.Log(
                logLevel,
                "<-- {Method} {Path} responded {StatusCode} in {ElapsedMs}ms",
                request.Method,
                request.Path,
                response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
    }
}

public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
        => app.UseMiddleware<RequestLoggingMiddleware>();
}