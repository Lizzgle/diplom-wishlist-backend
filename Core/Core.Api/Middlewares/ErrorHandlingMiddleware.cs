using System.Text.Json;
using Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ArgumentException = Core.Exceptions.ArgumentException;

namespace Core.Api;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context,
                statusCode: StatusCodes.Status400BadRequest,
                type: "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                title: "Invalid Argument",
                detail: ex.Message);
        }
        catch (InvalidAuthException ex)
        {
            _logger.LogError(ex, "Unauthorized access");
            await HandleExceptionAsync(context,
                statusCode: StatusCodes.Status401Unauthorized,
                type: "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
                title: "Unauthorized",
                detail: ex.Message);
        }
        catch (InvalidTokenException ex)
        {
            _logger.LogError(ex, "Unauthorized access");
            await HandleExceptionAsync(context,
                statusCode: StatusCodes.Status401Unauthorized,
                type: "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
                title: "Unauthorized",
                detail: ex.Message);
        }
        catch (ForbiddenException ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context,
                statusCode: StatusCodes.Status403Forbidden,
                type: "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3",
                title: "Forbidden",
                detail: ex.Message);
        }
        catch (NotFoundException ex)
        {
            _logger.LogError(ex, "Resource not found");
            await HandleExceptionAsync(context,
                statusCode: StatusCodes.Status404NotFound,
                type: "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                title: "Not Found",
                detail: ex.Message);
        }
        catch (AlreadyExistException ex)
        {
            _logger.LogError(ex, "Resource already exists");
            await HandleExceptionAsync(context,
                statusCode: StatusCodes.Status409Conflict,
                type: "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8",
                title: "Already Exists",
                detail: ex.Message);
        }
        catch (AppException ex)
        {
            _logger.LogError(ex, "Application exception");
            await HandleExceptionAsync(context, 
                statusCode: StatusCodes.Status500InternalServerError,
                type: "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                title: "Application exception",
                detail: ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");
            await HandleExceptionAsync(context, 
                statusCode: StatusCodes.Status500InternalServerError,
                type: "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                title: "Error occured on server");
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, int statusCode, string type, string title, string? detail = null)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new
        {
            Status = statusCode,
            Type = type,
            Title = title,
            Detail = detail
        };

        var jsonResponse = JsonSerializer.Serialize(response);

        return context.Response.WriteAsync(jsonResponse);
        
    }
}