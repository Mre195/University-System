//using System.Net;
//using System.Text.Json;
//using UniversitySystem.Services.Common;

//namespace UniversitySystem.Middleware;

//public class ExceptionHandlingMiddleware
//{
//    private readonly RequestDelegate _next;
//    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
//    private readonly IHostEnvironment _env;

//    public ExceptionHandlingMiddleware(
//        RequestDelegate next,
//        ILogger<ExceptionHandlingMiddleware> logger,
//        IHostEnvironment env)
//    {
//        _next = next;
//        _logger = logger;
//        _env = env;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        try
//        {
//            await _next(context);
//        }
//        catch (Exception ex)
//        {
//            await HandleExceptionAsync(context, ex);
//        }
//    }

//    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
//    {
//        var (statusCode, message) = MapException(exception);

//        _logger.LogError(
//            exception,
//            "Unhandled exception on {Method} {Path} -> {StatusCode}",
//            context.Request.Method,
//            context.Request.Path,
//            statusCode);

//        context.Response.ContentType = "application/json";
//        context.Response.StatusCode = (int)statusCode;

//        // In dev, surface the real exception message; in prod, keep it generic
//        var errorMessage = _env.IsDevelopment() ? exception.Message : message;

//        var result = Result<object>.Failure(errorMessage);

//        var json = JsonSerializer.Serialize(result, new JsonSerializerOptions
//        {
//            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//        });

//        await context.Response.WriteAsync(json);
//    }

//    private static (HttpStatusCode StatusCode, string Message) MapException(Exception exception) =>
//        exception switch
//        {
//            KeyNotFoundException => (HttpStatusCode.NotFound, "The requested resource was not found."),
//            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "You are not authorized to perform this action."),
//            ArgumentException => (HttpStatusCode.BadRequest, "Invalid request."),
//            InvalidOperationException => (HttpStatusCode.BadRequest, exception.Message),
//            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred. Please try again later.")
//        };
//}

//public static class ExceptionHandlingMiddlewareExtensions
//{
//    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
//        => app.UseMiddleware<ExceptionHandlingMiddleware>();
//}