using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace FitnesTracker;

public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
    
        var statusCode = exception switch
        {
            UnauthorizedAccessException => 401,
            InvalidOperationException => 409,
            NotImplementedException => 501,
            KeyNotFoundException => 404,
            ArgumentException => 400,
            _  => 500
        };

        context.ExceptionHandled = true;
        context.Result = new ObjectResult(exception.Message) { StatusCode = statusCode};
    }
}
