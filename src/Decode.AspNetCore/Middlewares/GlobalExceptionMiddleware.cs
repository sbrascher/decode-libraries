using Decode.AspNetCore.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;

namespace Decode.AspNetCore.Middlewares;

/// <summary>
/// Global exception handler middleware that captures unhandled exceptions and returns a standardized JSON response.
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger,
        IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            string errorId = Activity.Current?.Id ?? context.TraceIdentifier;

            _logger.LogError(ex, "[{ErrorId}] An unhandled exception occurred: {Message}", errorId, ex.Message);

            await HandleExceptionAsync(context, ex, errorId);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception, string errorId)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        List<string> messages = ["An internal server error occurred. Please try again later."];

        if (_env.IsDevelopment())
        {
            messages.Clear();
            AddExceptionMessages(exception, messages);

            if (!string.IsNullOrEmpty(exception.StackTrace))
            {
                messages.Add($"StackTrace: {exception.StackTrace}");
            }
        }

        ApiResponse response = new(
            success: false,
            errors: messages,
            errorId: errorId
        );

        await context.Response.WriteAsJsonAsync(response);
    }

    private static void AddExceptionMessages(Exception? ex, List<string> messages)
    {
        while (ex != null)
        {
            messages.Add($"{ex.GetType().Name}: {ex.Message}");
            ex = ex.InnerException;
        }
    }
}