using FluentValidation;
using System.Text.Json;

namespace FitnesTracker;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "An error occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        int statusCode;
        string message;
        
        switch (ex)
        {
            case KeyNotFoundException:
                statusCode = 404;
                message = "Resource not found";
                break;
            case ValidationException:
                statusCode = 400;
                message = "Validation failed";
                break;
            case ArgumentException:
                statusCode = 400;
                message = ex.Message;
                break;
            default:
                statusCode = 500;
                message = "An internal server error occurred";
                break;
        }
        
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse 
        { 
            StatusCode = statusCode,
            Message = message,
            Details = ex.Message // или ex.StackTrace для dev окружения
        };

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}
