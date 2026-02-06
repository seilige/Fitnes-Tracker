using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace FitnesTracker;

[ApiController]
[Route("api/[controller]")]
public class ErorController : ControllerBase
{
    [HttpGet("test-notfound")]
    public IActionResult TestNotFound()
    {
        throw new KeyNotFoundException("User not found");
    }

    [HttpGet("test-validation")]
    public IActionResult TestValidation()
    {
        throw new ValidationException("Invalid input data");
    }

    [HttpGet("test-server-error")]
    public IActionResult TestServerError()
    {
        throw new Exception("Something went wrong");
    }
}