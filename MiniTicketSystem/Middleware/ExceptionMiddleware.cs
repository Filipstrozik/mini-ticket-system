
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
namespace MiniTicketSystem.Middleware;

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
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {

        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Unexpected error occurred.",
            Detail = exception.Message,
            Instance = context.Request.Path
        };

        switch (exception)
        {
            case ArgumentNullException or ArgumentException:
                problem.Status = StatusCodes.Status400BadRequest;
                problem.Title = "Bad Request";
                break;

            case KeyNotFoundException:
                problem.Status = StatusCodes.Status404NotFound;
                problem.Title = "Not Found";
                break;
        }

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = problem.Status ?? 500;

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(problem, options);

        return context.Response.WriteAsync(json);
    }
}
