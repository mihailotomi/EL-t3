using EL_t3.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EL_t3.API.Infrastructure;

public class CustomExceptionHandler : IExceptionHandler
{
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path,
            Title = "An error occurred while processing your request.",
            Detail = exception.Message,
            Type = $"https://httpstatuses.com/{httpContext.Response.StatusCode}"
        };

        (problemDetails.Status, problemDetails.Title) = exception switch
        {
            ApiException e => (e.StatusCode, "API Error"),
            EntityNotFoundException e => (StatusCodes.Status404NotFound, "Entity Not Found"),
            ArgumentNullException e => (StatusCodes.Status400BadRequest, "Invalid Arguments"),
            FluentValidation.ValidationException e => (StatusCodes.Status400BadRequest, "Validation Error"),
            ValidationException e => (StatusCodes.Status400BadRequest, "Validation Error"),
            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
        };

        problemDetails.Type = $"https://httpstatuses.com/{problemDetails.Status.Value}";

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        httpContext.Response.ContentType = "application/problem+json";

        var result = JsonSerializer.Serialize(problemDetails, _serializerOptions);
        await httpContext.Response.WriteAsync(result, cancellationToken);

        return true;
    }
}