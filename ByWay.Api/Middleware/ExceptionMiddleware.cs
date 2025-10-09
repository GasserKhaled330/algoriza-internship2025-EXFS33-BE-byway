using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ByWay.Api.Middleware;

public class ExceptionMiddleware
{
    private static readonly Action<ILogger, Exception, string, Exception?> LogUnhandledException =
        LoggerMessage.Define<Exception, string>(
            LogLevel.Error,
            new EventId(1, "UnhandledException"),
            "Unhandled exception occurred: {Exception}. Message: {Message}");

    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            LogUnhandledException(_logger, ex, ex.Message, ex);

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occurred.",
                Detail = _env.IsDevelopment() ? ex.ToString() : ""
            };

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var json = JsonSerializer.Serialize(problemDetails);
            await context.Response.WriteAsync(json).ConfigureAwait(false);
        }
    }
}
