using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace CollegeApp.Infrastructure
{
    public class GlobalExceptionHandle : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandle> _logger;

        public GlobalExceptionHandle(ILogger<GlobalExceptionHandle> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // Log the exception
            _logger.LogError(exception, exception.Message);

            // Initialize default values
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string title = "Internal Server Error";
            string detail = exception.Message;

            // Check the type of exception and map to corresponding status code
            if (exception is ArgumentException)
            {
                statusCode = HttpStatusCode.BadRequest;
                title = "Bad Request";
            }
            else if (exception is KeyNotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
                title = "Resource Not Found";
            }
            else if (exception is UnauthorizedAccessException)
            {
                statusCode = HttpStatusCode.Unauthorized;
                title = "Unauthorized";
            }
            else if (exception is NotImplementedException)
            {
                statusCode = HttpStatusCode.NotImplemented;
                title = "Not Implemented";
            }

            // Create a ProblemDetails object to return error information
            var details = new ProblemDetails
            {
                Detail = detail,
                Instance = httpContext.Request.Path,
                Status = (int)statusCode,
                Title = title,
                Type = "Error"
            };

            // Serialize the error details to JSON
            var response = JsonSerializer.Serialize(details);
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)statusCode;

            // Write the response
            await httpContext.Response.WriteAsync(response, cancellationToken);

            return true;
        }
    }
}
