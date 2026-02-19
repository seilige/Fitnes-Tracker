using FluentValidation;
using System.Text.Json;
using System.Security.Claims;

namespace FitnesTracker;

public class EmailConfirmationMiddleware
{
    private readonly RequestDelegate _next;

    public EmailConfirmationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IUserRepository repo)
    {
        if(context.User.Identity.IsAuthenticated)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // измени
            
            if(userId != null)
            {
                var user = await repo.GetByIDAsync(int.Parse(userId));
                
                if(user != null && !user.IsEmailConfirmed)
                {
                    context.Response.StatusCode = 403;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync("Email not confirmed");
                    return;
                }
            }
        }
        
        await _next(context);
    }
}
