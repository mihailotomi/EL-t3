using System.Net;
using Microsoft.AspNetCore.Http;

using System.Text.Json;

namespace EL_t3.Core.Exceptions.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    private readonly JsonSerializerOptions serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

    };

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            response.StatusCode = error switch
            {
                ApiException e => e.StatusCode,
                EntityNotFoundException e => (int)HttpStatusCode.BadRequest,
                ArgumentNullException e => (int)HttpStatusCode.BadRequest,
                FluentValidation.ValidationException e => (int)HttpStatusCode.BadRequest,
                ValidationException e => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError,// unhandled error
            };

            var result = JsonSerializer.Serialize(new { message = error?.Message, data = error?.Data }, serializerOptions);
            await response.WriteAsync(result);
        }
    }
}
